using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Aether.Models;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon.Util;

namespace Aether.Controllers
{
    public class HomeController : Controller
    {
        public static List<PollutantData1Hr> pollutantData1Hr = new List<PollutantData1Hr>();
        public static List<PollutantData8Hr> pollutantData8Hr = new List<PollutantData8Hr>();
        public static List<PollutantData24Hr> pollutantData24Hr = new List<PollutantData24Hr>();
        private readonly IConfiguration configuration;

        public HomeController(IConfiguration config)
        {
            this.configuration = config;
        }

        public void Pull8hrData()
        {
            var options = configuration.GetAWSOptions("AWS");
            IAmazonDynamoDB client = options.CreateServiceClient<IAmazonDynamoDB>();

            Dictionary<string, AttributeValue> key = new Dictionary<string, AttributeValue>
                {
                    { "dev_id", new AttributeValue { S = "graqm0106" }},
                    { "time", new AttributeValue { S = "2018-09-26T13:42:42.007939438Z" }},

                };
            var result = client.GetItemAsync("ost_data", key);

            List<string> smelly = new List<string>();

            // View response
            Dictionary<string, AttributeValue> item = result.Result.Item;
            foreach (var keyValuePair in item)
            {
                smelly.Add(keyValuePair.Value.S);
                smelly.Add(keyValuePair.Value.N);
            }

            ViewBag.OSTData = smelly;
        }

        public IActionResult Index()
        {
      
            //Pull8hrData();
            return View();
        }

        public IActionResult QueryTest()
        {
            var options = configuration.GetAWSOptions("AWS");
            IAmazonDynamoDB client = options.CreateServiceClient<IAmazonDynamoDB>();

            DateTime oneHourAgoDate = DateTime.UtcNow - TimeSpan.FromHours(1);

            string currentTime = DateTime.UtcNow.ToString(AWSSDKUtils.ISO8601DateFormat);
            string oneHourAgoString = oneHourAgoDate.ToString(AWSSDKUtils.ISO8601DateFormat);


            var request = new QueryRequest
            {
                TableName = "ost_data",
                KeyConditionExpression = "dev_id = :v_replyId and ReplyDateTime between :v_start and :v_end",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                {":v_replyId", new AttributeValue {
                     S = "graqm0106"
                 }},
                {":v_start", new AttributeValue {
                     S = "2018-09-26T13:42:42.007939438Z"
                 }
    },
                {":v_end", new AttributeValue {
                     S = "2018-09-26T13:42:42.007939438Z"
                 }}
            }
            };

            var response = client.QueryAsync(request).Result.Items ;



            //var request = new QueryRequest
            //{
            //    TableName = "ost_data",
                
            //    KeyConditionExpression = "dev_id",
            //    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
            //        { "v_sensor", new AttributeValue { S = "graqm0106" }},
            //        { "v_now", new AttributeValue { S = "2018-09-26T13:42:42.007939438Z" }}},

            //    ProjectionExpression = "dev_id, time, o3, pm25"
            //};

            //var response = client.QueryAsync(request).Result.Items;

            foreach (var keyValuePair in response)
            {
                var pollutant = new PollutantData8Hr
                {
                    Dev_id = Convert.ToString(keyValuePair.Values.ElementAt(0)),
                    Time = Convert.ToDateTime(keyValuePair.Values.ElementAt(1)),
                    O3 = Convert.ToDouble(keyValuePair.Values.ElementAt(2)),
                    Pm25 = Convert.ToDouble(keyValuePair.Values.ElementAt(3))
                };
                pollutantData8Hr.Add(pollutant);
            }

            return View(pollutantData8Hr);
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
