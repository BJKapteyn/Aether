using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aether.Controllers
{
    public class DateController
    {
        public static List<string> PastSevenDates()
        {
            DateTime today = DateTime.Now;
            List<string> past7 = new List<string>();
            for (int i = 7; i > 0; i--)
            {
                string past = today.AddDays(-i).ToString("MM/dd");
                past7.Add(past);
            }
            return past7;
        }

    }
}
