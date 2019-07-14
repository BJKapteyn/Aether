using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aether.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet;

namespace Aether.Controllers
{
    //Lisa
    public class AQICalculations : Controller
    {
        //public static List<double> pollutantAverages = new List<double>();
        public static List<int> breakPointIndexes = new List<int>();
        public static List<BreakPointTable> breakPointTable = BreakPointTable.GetPollutantTypes();
        public static List<double> pollutantAQIs = new List<double>();

        //stopped here ---------------------------------------------------------------------------------------------------
        //enter lambda expression under pollutantLamda to designate the pollutant you want averaged ex. x => x.O3
        public static double PollutantAverage(List<PollutantData> PD, Func<PollutantData, IComparable> pollutantLamda)
        {
            double pollutantAverage;
            double pollutantSum;

            PD.RemoveAll(x => (double)pollutantLamda(x) == 0);

            if(PD.Count > 0)
            {
                pollutantSum = PD.Sum(x => (double)pollutantLamda(x));
                pollutantAverage = Math.Round((pollutantSum / PD.Count), 3);
                return pollutantAverage;
            }

            return 0;
        }

        //public static void SumAndAveragePollutantReadings(Sensor s)
        //{
        //    string sensorLocation = s.Name;

        //    if (sensorLocation.Contains("graq"))
        //    {
        //        double ostO38hrSum = (double)HomeController.pollutantData8Hr.Sum(x => x.O3);
        //        double ostO38hrAverage = Math.Round(ostO38hrSum / HomeController.pollutantData8Hr.Count, 3);

        //        double ostO31hrSum = (double)HomeController.pollutantData1Hr.Sum(x => x.O3);
        //        double ostO31hrAverage = Math.Round(ostO31hrSum / HomeController.pollutantData1Hr.Count, 3);

        //        double ostPM25Sum = (double)HomeController.pollutantData24Hr.Sum(x => x.Pm25);
        //        double ostPM25Average = (double)Math.Round(ostPM25Sum / HomeController.pollutantData24Hr.Count, 1);

        //        double ostPM10Sum = (double)HomeController.pollutantData24Hr.Sum(x => x.PM10);
        //        double ostPM10Average = (double)Math.Round(ostPM10Sum / HomeController.pollutantData24Hr.Count, 0);

        //        pollutantAverages.Add(ostO38hrAverage); //index[0] O3 8hr ppm
        //        pollutantAverages.Add(ostO31hrAverage); // index[1] O3 1hr ppm
        //        pollutantAverages.Add(ostPM10Average); //index[2] PM10 ug/m3
        //        pollutantAverages.Add(ostPM25Average); //index[3] PM2.5 ug/m3
        //        pollutantAverages.Add(0); // blank index
        //        pollutantAverages.Add(0); // blank index
        //        pollutantAverages.Add(0); // blank index

        //    }
        //    else
        //    {
        //        double simmsO38hrSum = (double)HomeController.pollutantData8Hr.Sum(x => x.O3);
        //        double simmsO38hrAverage = Math.Round(simmsO38hrSum / HomeController.pollutantData8Hr.Count, 3);

        //        double simmsO31hrSum = (double)HomeController.pollutantData8Hr.Sum(x => x.O3);
        //        double simmsO31hrAverage = Math.Round(simmsO31hrSum / HomeController.pollutantData8Hr.Count, 3);

        //        double simmsPM25Sum = (double)HomeController.pollutantData24Hr.Sum(x => x.Pm25);
        //        double simmsPM25Average = (double)Math.Round(simmsPM25Sum / HomeController.pollutantData24Hr.Count, 1);

        //        double simmsCOSum = (double)HomeController.pollutantData8Hr.Sum(x => x.CO);
        //        double simmsCOAverage = (double)Math.Round(simmsCOSum / HomeController.pollutantData8Hr.Count, 1);

        //        double simmsSO2Sum = (double)HomeController.pollutantData1Hr.Sum(x => x.SO2);
        //        double simmsSO2Average = (double)Math.Round(simmsSO2Sum / HomeController.pollutantData1Hr.Count, 1);

        //        double simmsNO2Sum = (double)HomeController.pollutantData1Hr.Sum(x => x.NO2);
        //        double simmsNO2Average = (double)Math.Round(simmsNO2Sum / HomeController.pollutantData1Hr.Count, 1);

        //        pollutantAverages.Add(simmsO38hrAverage); //index[0] O3 8hr ppm
        //        pollutantAverages.Add(simmsO31hrAverage); // index[1] O3 1hr ppm
        //        pollutantAverages.Add(0);                  // blank index
        //        pollutantAverages.Add(simmsPM25Average); //index[2] PM2.5 ug/m3
        //        pollutantAverages.Add(simmsCOAverage); //index[4] CO 8 hr
        //        pollutantAverages.Add(simmsSO2Average);  //index[5] SO2 ppb 1 hr
        //        pollutantAverages.Add(simmsNO2Average);  //index[6] NO2 ppb 1 hr
        //    }

        //}

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


        //Calculate what breakpoint index to use
        public static int BreakpointIndexCalculation(double Pollutant, BreakPointTable B)
        {
            int breakPointIndex = 0;
            if (Pollutant == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    double low = B.Low[i];
                    double high = B.High[i];

                    if (Pollutant >= low && Pollutant <= high)
                    {
                        breakPointIndex = i;
                        break;
                    }
                }
            }
            else
            {
                breakPointIndex = int.MaxValue;
            }

            return breakPointIndex;
        }

        //public static void BreakPointIndex()
        //{
        //    if (pollutantAverages[1] < 0.125)
        //    {
        //        breakPointIndexes.Add(BreakpointIndexCalculation(pollutantAverages[0], 0));
        //        breakPointIndexes.Add(0); //blank
        //    }
        //    else
        //    {
        //        breakPointIndexes.Add(0); //blank
        //        breakPointIndexes.Add(BreakpointIndexCalculation(pollutantAverages[1], 1));
        //    }

        //    breakPointIndexes.Add(BreakpointIndexCalculation(pollutantAverages[2], 2));
        //    breakPointIndexes.Add(BreakpointIndexCalculation(pollutantAverages[3], 3));
        //    breakPointIndexes.Add(BreakpointIndexCalculation(pollutantAverages[4], 4));
        //    breakPointIndexes.Add(BreakpointIndexCalculation(pollutantAverages[5], 5));
        //    breakPointIndexes.Add(BreakpointIndexCalculation(pollutantAverages[6], 6));

        //    //o3 8hr = 0, o3 1hr = 1, pm10 = 2, pm2.5 = 3, co = 4, so2 = 5, no2 = 6
        //}

        public static double AQIEquation(double pollutantReading, int breakpointIndex, int pollutantIndex)
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

        public static double CalculateAQI(double pollutantPPM, int breakpointIndex, int pollutantIndex)
        {
            // using 1h reading
            double airQuailtyIndex = AQIEquation(pollutantPPM, breakpointIndex, pollutantIndex);

            return airQuailtyIndex;
        }

        //public static void AQI()
        //{
        //    if (pollutantAverages[1] < 0.125)
        //    {
        //        double O3AQI = Math.Round(CalculateAQI(pollutantAverages[0], breakPointIndexes[0], 0), 0);
        //        pollutantAQIs.Add(O3AQI);
        //    }
        //    else
        //    {
        //        double O3AQI = Math.Round(CalculateAQI(pollutantAverages[1], breakPointIndexes[1], 1), 0);
        //        pollutantAQIs.Add(O3AQI);
        //    }

        //    double PM10AQI = Math.Round(CalculateAQI(pollutantAverages[2], breakPointIndexes[2], 2), 0);
        //    double PM25AQI = Math.Round(CalculateAQI(pollutantAverages[3], breakPointIndexes[3], 3), 0);
        //    double COAQI = Math.Round(CalculateAQI(pollutantAverages[4], breakPointIndexes[4], 4), 0);
        //    double SO2AQI = Math.Round(CalculateAQI(pollutantAverages[5], breakPointIndexes[5], 5), 0);
        //    double NO2AQI = Math.Round(CalculateAQI(pollutantAverages[6], breakPointIndexes[6], 6), 0);

        //    pollutantAQIs.Add(PM10AQI);
        //    pollutantAQIs.Add(PM25AQI);
        //    pollutantAQIs.Add(COAQI);
        //    pollutantAQIs.Add(SO2AQI);
        //    pollutantAQIs.Add(NO2AQI);

        //    //o3 8hr = 0, o3 1hr = 1, pm10 = 2, pm2.5 = 3, co = 4, so2 = 5, no2 = 6
        //}

        public static double MaxAQI()
        {
            double maxAQI = pollutantAQIs.Max();

            return maxAQI;
        }


        public static int WeatherForecastEquation(List<WeatherDataFromAPI> weatherTime, int index, double eightHourO3)
        {

            double FutureAQI = (double)(5.3 * weatherTime[index].WindSpeed) + (double)(0.4 * weatherTime[index].TemperatureC) +
                (double)(0.1 * weatherTime[index].Humidity) + ((double)0.7 * eightHourO3);

            return (int)Math.Round(FutureAQI);

        }

    }

}