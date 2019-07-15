using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aether.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Aether.Controllers
{
    public class APIController : Controller
    {
        public static List<AQIs> GetListAQI(string zipCode = "49503")
        {
            // GR 49503 - KZOO 49001 - DETROIT 48127
            string key = APIKeys.AirNowAPI;

            string URL = $"http://www.airnowapi.org/aq/observation/zipCode/current/?format=application/json&zipCode={zipCode}&distance=10&API_KEY={key}";

            JToken jt = ParseAPI.APICall(URL);

            List<AQIs> ListOfAQIs = new List<AQIs>();

            for (int i = 0; i < jt.Count(); i++)
            {
                ListOfAQIs.Add(new AQIs(jt, i));
            }

            return ListOfAQIs;
        }


        public static List<AQIs> GetHistoricAQI(string zipCode = "49503", string dateTime = "2019-07-13T00-0000")
        {
            // GR 49503 - KZOO 49001 - DETROIT 48127
            string key = APIKeys.AirNowAPI;

            string URL = $"http://www.airnowapi.org/aq/observation/zipCode/historical/?format=application/json&zipCode={zipCode}&date={dateTime}&distance=25&API_KEY={key}";

            JToken jt = ParseAPI.APICall(URL);

            List<AQIs> ListOfAQIs = new List<AQIs>();

            for (int i = 0; i < jt.Count(); i++)
            {
                ListOfAQIs.Add(new AQIs(jt, i));
            }

            return ListOfAQIs;
        }



        public static List<WeatherDataFromAPI> GetWeatherForcast()
        {
            string key = APIKeys.WeatherAPI;
            string cityCode = "4994358"; // Grand Rapids
            string URL = $"http://api.openweathermap.org/data/2.5/forecast?id={cityCode}&APPID={key}";

            JToken jt = ParseAPI.APICall(URL);

            // Forecast readings are every 3h: 0=now, 8=1 day, 24=3days, 39=5days minus 3h
            List<int> indexes = new List<int>() { 0, 8, 24, 39 };

            List<WeatherDataFromAPI> weatherForecast = new List<WeatherDataFromAPI>();

            foreach (int index in indexes)
            {
                WeatherDataFromAPI wd = new WeatherDataFromAPI(jt, index);
                wd.TemperatureC = wd.TemperatureK - 273.15;
                wd.TemperatureF = (wd.TemperatureC) * 9 / 5 + 32;

                weatherForecast.Add(wd);
            }

            return weatherForecast;
        }

    }
}
