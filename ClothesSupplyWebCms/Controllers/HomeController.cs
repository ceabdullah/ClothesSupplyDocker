using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClothesSupplyWebCms.Models;
using ClothesSupplyWebCms.Services;

namespace ClothesSupplyWebCms.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Product");
            }

            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
