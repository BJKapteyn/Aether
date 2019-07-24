using Newtonsoft.Json.Linq;

namespace Aether.Models
{
    public class WeatherDataFromAPI
    {
        public double TemperatureK { get; set; }
        public double TemperatureC { get; set; }
        public double TemperatureF { get; set; }
        public double Pressure { get; set; }
        public int Humidity { get; set; }
        public string Clouds { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public int IconCode { get; set; }


        public WeatherDataFromAPI()
        {
        }

        public WeatherDataFromAPI(JToken weather, int index)
        {
            TemperatureK = (double)weather["list"][index]["main"]["temp"];
            Pressure = (double)weather["list"][index]["main"]["grnd_level"]; // changed to ground level from sea level 19-07-07
            Humidity = (int)weather["list"][index]["main"]["humidity"];
            Clouds = weather["list"][index]["weather"][0]["description"].ToString(); // sky conditions
            WindSpeed = (double)weather["list"][index]["wind"]["speed"];
            WindDirection = (double)weather["list"][index]["wind"]["deg"];
            IconCode = (int)weather["list"][index]["weather"][0]["id"];
        }

    }

}