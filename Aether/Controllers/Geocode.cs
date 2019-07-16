using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aether.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using Newtonsoft.Json;

namespace Aether.Controllers
{
    public class Geocode
    {
        public static double LatLongDistance(double lat1, double long1, double lat2, double long2)
        {
            //mean earth Radius in statute(normal) miles
            double earthRadius = 3958.8;
            //turn the difference between the two lats and longs into radians
            double degreeLat = degreesToRadians(lat1 - lat2);
            double degreeLong = degreesToRadians(long1 - long2);
            //turn it all into a haversine, no biggie
            double Haversine =
                Math.Pow(Math.Sin(degreeLat / 2), 2)
                + Math.Cos(degreesToRadians(lat1))
                * Math.Cos(degreesToRadians(lat2))
                * Math.Sin(degreeLong / 2)
                * Math.Sin(degreeLong / 2);
            //final calculation to complete the formula
            double c = 2 * Math.Atan2(Math.Sqrt(Haversine), Math.Sqrt(1 - Haversine));
            //trigonometry is fun for everyone
            double jeezFinally = earthRadius * c;

            //Now we have the great circle distance between the two coordinates
            return jeezFinally;
        }

        private static double degreesToRadians(double deg)
        {
            return deg * (Math.PI / 180);
        }

        public static List<Sensor> OrderedSensors(string address)
        {
            List<Sensor> sensors = Sensor.GetSensors();
            try
            {
                var userLocation = UserLocation(address).Result;
                
                for (int i = 0; i < sensors.Count; i++)
                {
                    sensors[i].Distance = LatLongDistance(userLocation.Lat, userLocation.Lng, sensors[i].Lat, sensors[i].Long);
                }
                List<Sensor> finalList = sensors.OrderBy(x => x.Distance).ToList();

                return finalList;
            }
            catch(AggregateException)
            {
                sensors.Clear();
                return sensors;
            }
        }

        public static async Task<UserLatLng> UserLocation(string address)
        {
            UserLatLng userLocation = new UserLatLng();

            var jsonAddress = await GeocodeingAPI.UserAddress(address);

            userLocation.Lat = double.Parse(jsonAddress["results"][0]["geometry"]["location"]["lat"].ToString());
            userLocation.Lng = double.Parse(jsonAddress["results"][0]["geometry"]["location"]["lng"].ToString());

            return userLocation;
            //List<Sensor> sensors = Sensor.GetSensors();
            //List<Sensor> shortSensors = new List<Sensor>();
            //Changes the Address to a longitude and latitude coordinate from the google geocode API
            //JToken jsonAddress = GoogleMapDAL.GoogleJson(address);

            //double addressLat = double.Parse(jsonAddress["results"][0]["geometry"]["location"]["lat"].ToString());
            //double addressLng = double.Parse(jsonAddress["results"][0]["geometry"]["location"]["lng"].ToString());

            //{ addressLat, addressLng };

        }


    }
}
