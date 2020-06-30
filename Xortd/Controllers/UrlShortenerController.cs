using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xortd.Data;
using Xortd.Models;

namespace Xortd.Controllers
{
    public class UrlShortener : Controller
    {
        private UrlDbContext context;

        public UrlShortener(UrlDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("/{slug}")]
        public IActionResult Get([FromRoute] string slug)
        {
            var shortUrl = context.ShortUrls.FirstOrDefault(s => s.Slug == slug);

            if (shortUrl != null)
            {
                return RedirectPermanent(shortUrl.Url);
            }

            return LocalRedirect("/");
        }

        [HttpPost]
        [Route("/create")]
        public async Task<IActionResult> Create([FromBody] ShortUrl shortUrl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid input");
            }

            if (!IsUrlValid(shortUrl.Url))
            {
                return BadRequest("Invalid url");
            }

            shortUrl.Slug ??= RandomIdGenerator.GetBase62(8);

            try
            {
                await context.AddAsync(shortUrl);
                await context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                return BadRequest("Invalid input");
            }


            return Created(Url.Action("Get", "UrlShortener", new {slug = shortUrl.Slug}),
                shortUrl);
        }

        private static bool IsUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }

    public static class RandomIdGenerator
    {
        private static char[] _base62chars =
            "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz"
                .ToCharArray();

        private static Random _random = new Random();

        public static string GetBase62(int length)
        {
            var sb = new StringBuilder(length);

            for (var i = 0; i < length; i++)
                sb.Append(_base62chars[_random.Next(62)]);

            return sb.ToString();
        }
    }
}