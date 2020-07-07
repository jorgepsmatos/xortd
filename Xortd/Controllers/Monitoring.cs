using System;
using Microsoft.AspNetCore.Mvc;

namespace Xortd.Controllers
{
    public class Monitoring : Controller
    {
        // GET
        public IActionResult Index()
        {
            return Ok(Environment.GetEnvironmentVariable("HOSTNAME"));
        }
    }
}