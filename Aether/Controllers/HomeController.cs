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

            // Find highest AQI
            int highestAQI = 0;
            foreach(AQIs a in ListOfAQIs)
            {
                if (a.AQI > highestAQI)
                {
                    highestAQI = a.AQI;
                }
            }

            // Get AQI Color
            int AQIIndex;
            if (highestAQI > 200 && highestAQI <= 300)
            {
                AQIIndex = 4;
            }
            else if (highestAQI > 300)
            {
                AQIIndex = 5;
            }
            else
            {
                AQIIndex = (highestAQI + 1) / 50;
            }

            string colorAQI = returnHexColor(AQIIndex);

            ViewBag.highestAQI = highestAQI;
            ViewBag.AQIColor = colorAQI;
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



        public static string returnHexColor(int index)
        {
            string[] hexColors = { "00e400", "ffff00", "ff7e00", "ff0000", "8f3f97", "7e0023" };

            return hexColors[index];

            //if (highestAQI <= 50)
            //{
            //    return "limegreen"; // GREEN 00e400 0
            //}
            //else if (highestAQI > 50 && highestAQI <= 100)
            //{
            //    return "yellow"; // YELLOW ffff00 1
            //}
            //else if (highestAQI > 100 && highestAQI <= 150)
            //{
            //    return "orange"; // ORANGE ff7e00 2
            //}
            //else if (highestAQI > 150 && highestAQI <= 200)
            //{
            //    return "red"; // RED ff0000 3
            //}
            //else if (highestAQI > 200 && highestAQI <= 300)
            //{
            //    return "purple"; // PURPLE 8f3f97 4
            //}
            //else
            //{
            //    return "maroon"; // MAROON 7e0023 5
        }
    }
}
