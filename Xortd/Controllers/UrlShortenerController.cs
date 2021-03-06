﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Xortd.Data;
using Xortd.Helpers;
using Xortd.Models;

namespace Xortd.Controllers
{
    [EnableCors]
    public class UrlShortener : Controller
    {
        private readonly ApplicationDbContext context;

        public UrlShortener(ApplicationDbContext context)
        {
            this.context = context;
        }
        
        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("/{slug}")]
        public IActionResult Index([FromRoute] string slug)
        {
            var shortUrl = context.ShortUrls.FirstOrDefault(s => s.Slug == slug);

            if (shortUrl == null)
            {
                return View();
            }

            return RedirectPermanent(shortUrl.Url);
        }

        [HttpPost]
        [Route("/shorturl")]
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

            if (string.IsNullOrWhiteSpace(shortUrl.Slug))
            {
                shortUrl.Slug = RandomIdGenerator.GetBase62(8);
            }

            try
            {
                await context.ShortUrls.AddAsync(shortUrl);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest("Failed to create short url");
            }


            return Created(Url.Action("Index", "UrlShortener", new
                {
                    slug = shortUrl.Slug
                }, Request.Scheme),
                shortUrl);
        }

        private static bool IsUrlValid(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}