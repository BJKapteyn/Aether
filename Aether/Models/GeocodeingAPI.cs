using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Aether.Controllers;
using Aether.Models;
using Newtonsoft.Json.Linq;

namespace Aether.Models
{
    public class GeocodeingAPI
    {

        public static async Task<JObject> UserAddress(string address)
        {
            using(var client = new HttpClient())
            {
                try
                {
                    client.BaseAddress = new Uri("https://maps.googleapis.com");
                    string googleAddress = GoogleAddress(address);

                    //creates an asynchronous response to free up resources during API call
                    HttpResponseMessage response = await client.GetAsync($"/maps/api/geocode/json?address={googleAddress}&key={APIKeys.GoogleMapsAPI}");
                    response.EnsureSuccessStatusCode();

                    string result = await response.Content.ReadAsStringAsync();
                    JObject jsonResult = JObject.Parse(result);
                    return jsonResult;
                }
                catch (HttpRequestException)
                {
                    JObject j = new JObject();
                    return j;
                }
            }
        }

        private static string GoogleAddress(string streetAddress)
        {
            string[] addressArr = streetAddress.Split(' ');
            string googleAddress = "";

            for (int i = 0; i < addressArr.Length; i++)
            {
                googleAddress += addressArr[i] + "+";
            }
            googleAddress += ",+Grand+Rapids,+MI";

            return googleAddress;
        }
    }
}
