using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aether.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aether.Controllers
{
    public class CalculationController : Controller
    {
        public static List<BreakPointTable> breakPointTable = BreakPointTable.GetPollutantTypes();

        public static double ConvertPPBtoPPM(double PollutantPPB)
        {
            //1000 ppm = 1000 ppb
            double PollutantPPM = PollutantPPB / 1000;
            return PollutantPPM;
        }
        public static double ConvertPPMtoPPB(double PollutantPPM)
        {
            //1 ppm = 1000 ppb
            double PollutantPPB = PollutantPPM * 1000;
            return PollutantPPB;
        }

        public static double PPMConvertToUGM3(double PPM, double moleWeight)
        {
            //0.0409 is a conversion constant
            double UGM3 = (PPM * 1000) * 0.0409 * moleWeight;
            return UGM3;
        }

        // converts to PPM
        public static double UGM3ConvertToPPM(double UGM3, double moleWeight)
        {
            double PPM = (UGM3 / (0.0409 * moleWeight)) / 1000;
            return PPM;
        }

        //Which breakpoint to use for ozone - 8 hr vs. 1 hr
        public static int OzoneOneHourOrEightHour(double O3Reading)
        {
            if (O3Reading <= (double)0.125)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        //Calculate what breakpoint index to use
        public static int BreakpointIndex(double Pollutant, int pollutantIndex)
        {
            int breakPointIndex = 0;

            for (int i = 0; i < 6; i++)
            {
                double low = breakPointTable[pollutantIndex].Low[i];
                double high = breakPointTable[pollutantIndex].High[i];

                if (Pollutant >= low && Pollutant <= high)
                {
                    breakPointIndex = i;
                    break;
                }
            }

            return breakPointIndex;
        }

        public static double CalculateAQI(double pollutantReading, int breakpointIndex, int pollutantIndex)
        {
            //index 7 in breakpoint table is the AQI
            double Ihi = breakPointTable[7].High[breakpointIndex];
            double Ilo = breakPointTable[7].Low[breakpointIndex];
            double BPhi = breakPointTable[pollutantIndex].High[breakpointIndex];
            double BPlow = breakPointTable[pollutantIndex].Low[breakpointIndex];
            double Cp = pollutantReading;

            double AQIForPollutant = ((Ihi - Ilo) / (BPhi - BPlow)) * (Cp - BPlow) + Ilo;
            return AQIForPollutant;
        }
    }
}