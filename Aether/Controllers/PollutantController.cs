using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Aether.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Aether.Controllers
{
    public class PollutantController : Controller
    {

        private readonly IConfiguration configuration;

        public PollutantController(IConfiguration config)
        {
            this.configuration = config;
        }
        public IActionResult Index()
        {
            string connectionstring = configuration.GetConnectionString("DefaultConnectionstring");
            SqlConnection connection = new SqlConnection(connectionstring);
            connection.Open();
            String sql = "EXEC SelectReadings @dev_id = graqm0107, @time = '2019-03-28 11:00', @endtime = '2019-03-28 13:00';";
            SqlCommand com = new SqlCommand(sql, connection);
            var model = new List<PollutantData>();
            SqlDataReader rdr = com.ExecuteReader();
              while (rdr.Read())
              {
                  var pollutant = new PollutantData
                  {
                        Dev_id = (string)rdr["dev_id"],
                        Time = (DateTime)rdr["time"],
                        O3 = (double)rdr["o3"],
                        Pm25 = (double)rdr["pm25"],
                        Id = (int)rdr["id"]
                  };
                  model.Add(pollutant);
              }
            connection.Close();
            return View(model);
        }
    }
}