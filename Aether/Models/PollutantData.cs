using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aether.Models
{
    public class PollutantData
    {
        public string Dev_id { get; set; }
        public DateTime Time { get; set; }
        public double O3 { get; set; }
        public double Pm25 { get; set; }
        public int Id { get; set; }
    }
}
