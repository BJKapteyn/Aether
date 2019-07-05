using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Aether.Models
{
    public class ParseAPI
    {
        public static JToken APICall(string URL)
        {
            HttpWebRequest request = WebRequest.CreateHttp(URL);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader rd = new StreamReader(response.GetResponseStream());
            string APIText = rd.ReadToEnd();

            return JToken.Parse(APIText);
        }
    }
}
