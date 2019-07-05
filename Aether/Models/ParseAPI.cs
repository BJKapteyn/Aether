using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Aether.Models
{
    public class ParseAPI
    {
        public static JToken APICall()
        {
            //DateTime nowHour = DateTime.Now.AddHours(4); // to add 4 hours to UTC for EDT
            //string currentTime = nowHour.ToString("yyyy-MM-ddTHH");

            // DETROIT 48127
            // KZOO 49001
            // GR 49503
            string zipCode = "48127";
            string key = APIKeys.AirNowAPI; // key hidden in APIKeys Model
            
            string URL = $"http://www.airnowapi.org/aq/observation/zipCode/current/?format=application/json&zipCode={zipCode}&distance=10&API_KEY={key}";

            HttpWebRequest request = WebRequest.CreateHttp(URL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string APIText = rd.ReadToEnd();

            return JToken.Parse(APIText);
        }
    }
}
