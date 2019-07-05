using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aether.Models
{
    public class PollutantData1Hr
    {
        public string Dev_id { get; set; }
        public DateTime Time { get; set; }
        public double? O3 { get; set; }
        public double? Pm25 { get; set; }
        public double? PM10 { get; set; }
        public double? CO { get; set; }
        public double? NO2 { get; set; }
        public double? SO2 { get; set; }
        public int Id { get; set; }
    }
}
