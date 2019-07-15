using System;
namespace Aether.Models
{
    public class FutureAQIs
    {
        public int O3AQI { get; set; }
        public int COAQI { get; set; }
        public int NO2AQI { get; set; }


        public FutureAQIs(int O3AQI, int COAQI, int NO2AQI)
        {
            this.O3AQI = O3AQI;
            this.COAQI = COAQI;
            this.NO2AQI = NO2AQI;
        }
    }
}
