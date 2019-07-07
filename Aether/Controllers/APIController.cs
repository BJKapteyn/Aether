using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aether.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Aether.Controllers
{
    public class APIController : Controller
    {
        public static List<AQIs> GetListAQI()
        {
            string zipCode = "49503";  // GR 49503 - KZOO 49001 - DETROIT 48127
            string key = APIKeys.AirNowAPI; // key hidden in APIKeys Model
            string dateTime = "2019-07-04T00-0000";

            // URL to get historic data via Zip Code
            //string URL = $"http://www.airnowapi.org/aq/observation/zipCode/historical/?format=application/json&zipCode={zipCode}&date={dateTime}&distance=25&API_KEY={key}";

            // URL to get data via Zip Code
            string URL = $"http://www.airnowapi.org/aq/observation/zipCode/current/?format=application/json&zipCode={zipCode}&distance=10&API_KEY={key}";

            JToken jt = ParseAPI.APICall(URL);

            List<AQIs> ListOfAQIs = new List<AQIs>();

            for (int i = 0; i < jt.Count(); i++)
            {
                ListOfAQIs.Add(new AQIs(jt, i));
            }

            return ListOfAQIs;

        }

    }
}
