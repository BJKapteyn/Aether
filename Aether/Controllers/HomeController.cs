using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aether.Models;
using Newtonsoft.Json.Linq;

namespace Aether.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            JToken jt = ParseAPI.APICall();
            List<AQIs> ListOfAQIs = new List<AQIs>();

            int count = jt.Count();

            if (jt == null)
            {
                ViewBag.Message = "Ooops. That didn't work.";
            }
            else
            {
                for (int i = 0; i < jt.Count(); i++)
                {
                    ListOfAQIs.Add(new AQIs(jt, i));
                }
            }

            ViewBag.AQIList = ListOfAQIs;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

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
