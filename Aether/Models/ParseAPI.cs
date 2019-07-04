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

            DateTime nowHour = DateTime.Now.AddHours(4); // to add 4 hours to UTC for EDT
            string currentTime = nowHour.ToString("yyyy-MM-ddTHH");

            string key = APIKeys.AirNowAPI; // key hidden in APIKeys Model
            
            string URL = $"http://www.airnowapi.org/aq/data/?startDate={currentTime}&endDate={currentTime}&parameters=OZONE,PM25,PM10,CO,NO2,SO2&BBOX=-85.684031,42.960187,-85.640773,42.985307&dataType=A&format=application/json&verbose=0&nowcastonly=0&API_KEY={key}";

            HttpWebRequest request = WebRequest.CreateHttp(URL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string APIText = rd.ReadToEnd();

            return JToken.Parse(APIText);
        }

    }
}
