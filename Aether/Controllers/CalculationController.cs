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
        public static List<double> pollutantAverages = new List<double>();
        
        //sensor s
        public static void SumAndAveragePollutantReadings(Sensor s)
        {
            string sensorLocation = s.Name;

            if (sensorLocation.Contains("graq"))
            {
                double ostO3Sum = (double)PollutantController.pollutantData.Sum(x => x.O3);
                double ostO3Average = Math.Round(ostO3Sum / PollutantController.pollutantData.Count, 3);

                double ostPM25Sum = (double)PollutantController.pollutantData.Sum(x => x.Pm25);
                double ostPM25Average = (double)Math.Round(ostPM25Sum / PollutantController.pollutantData.Count, 1);

                double ostPM10Sum = (double)PollutantController.pollutantData.Sum(x => x.PM10);
                double ostPM10Average = (double)Math.Round(ostPM10Sum / PollutantController.pollutantData.Count, 0);

                pollutantAverages.Add(ostO3Average); //index[0] O3 ppm
                pollutantAverages.Add(ostPM25Average); //index[1] PM2.5 ug/m3
                pollutantAverages.Add(ostPM10Average); //index[2] PM10 ug/m3

            }
            else
            {
                double simmsO3Sum = (double)PollutantController.pollutantData.Sum(x => x.O3);
                double simmsO3Average = Math.Round(simmsO3Sum / PollutantController.pollutantData.Count, 3);

                double simmsPM25Sum = (double)PollutantController.pollutantData.Sum(x => x.Pm25);
                double simmsPM25Average = (double)Math.Round(simmsPM25Sum / PollutantController.pollutantData.Count, 1);

                double simmsCOSum = (double)PollutantController.pollutantData.Sum(x => x.CO);
                double simmsCOAverage = (double)Math.Round(simmsCOSum / PollutantController.pollutantData.Count, 1);

                double simmsNO2Sum = (double)PollutantController.pollutantData.Sum(x => x.NO2);
                double simmsNO2Average = (double)Math.Round(simmsNO2Sum / PollutantController.pollutantData.Count, 1);

                double simmsSO2Sum = (double)PollutantController.pollutantData.Sum(x => x.SO2);
                double simmsSO2Average = (double)Math.Round(simmsSO2Sum / PollutantController.pollutantData.Count, 1);

                pollutantAverages.Add(simmsO3Average); //index[0] O3 ppm
                pollutantAverages.Add(simmsPM25Average); //index[1] PM2.5 ug/m3
                pollutantAverages.Add(0); //index[2] 
                pollutantAverages.Add(simmsCOAverage); //index[3] 8 hr
                pollutantAverages.Add(simmsNO2Average);  //index[4] ppb 1 hr
                pollutantAverages.Add(simmsSO2Average);  //index[5] ppb 1 hr
            }
               
        }

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

        public static string ColorWarning(double reading)
        {
            if (reading >= 0 && reading < 51)
            {
                return "limegreen";
            }
            else if (reading >= 51 && reading < 101)
            {
                return "yellow";
            }
            else if (reading >= 101 && reading < 151)
            {
                return "orange";
            }
            else if (reading >= 151 && reading < 201)
            {
                return "red";
            }
            else if (reading >= 201 && reading < 301)
            {
                return "purple";
            }
            else
            {
                return "maroon";
            }

        }
        public static string ColorWarningEO(double reading)
        {
            if (reading >= 0 && reading < 0.18)
            {
                return "limegreen";
            }
            else if (reading >= 0.18 && reading <= 1)
            {
                return "yellow";
            }
            else
            {
                return "red";
            }
        }
    }
}