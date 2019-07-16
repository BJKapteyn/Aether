using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aether.Controllers;

namespace Aether.Models
{
    public class PollutantData
    {

        public double? O3 { get; set; } = 0;

        public double? PM25 { get; set; } = 0;

        public double? PM10 { get; set; } = 0;

        public double? CO { get; set; } = 0;

        public double? NO2 { get; set; } = 0;

        public double? SO2 { get; set; } = 0;

        public PollutantData ()
        {

        }
    }
}
