using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Hashing
{
    public class StepsCounter
    {
        protected long steps = 0;
        public void AddStep()
        {
            steps++;
        }
    }

    public class AdvancedStepsLogger : StepsCounter
    {

        public StreamWriter wrMin;
        public StreamWriter wrMax;
        public StreamWriter wrAvg;
        public StreamWriter wrMed;
        public StreamWriter wrDec;

        public int currentXValue { get; set; } = 0;
        List<double> runsForOneSize = new List<double>();

        protected long elements = 0;
        public void NewElementBoundary()
        {
            elements++;
        }

        public void StartNewSegment()
        {
            steps = 0;
            elements = 0;
        }

        public void FlushElementSegment()
        { 
            double average = steps / (double)elements;
            runsForOneSize.Add(average);

            steps = 0;
            elements = 0;
        }

        public (double avg, double min, double max, double median, double decil) GetStatisticalSummary()
        {
            runsForOneSize.Sort();
            double avg = runsForOneSize.Average();

            double min = runsForOneSize[0];
            double max = runsForOneSize[runsForOneSize.Count - 1];

            double median = runsForOneSize[runsForOneSize.Count / 2];
            double decil = runsForOneSize[(int)(runsForOneSize.Count * 0.9)];

            return (avg, min, max, median, decil);
        }

        public void PrintStatisticalSummaryForOneSize()
        {
            var data = GetStatisticalSummary();

            wrAvg.WriteLine($"{currentXValue.ToString(CultureInfo.GetCultureInfo("en-US"))} {data.avg.ToString(CultureInfo.GetCultureInfo("en-US"))}");
            wrMed.WriteLine($"{currentXValue.ToString(CultureInfo.GetCultureInfo("en-US"))} {data.median.ToString(CultureInfo.GetCultureInfo("en-US"))}");
            wrMin.WriteLine($"{currentXValue.ToString(CultureInfo.GetCultureInfo("en-US"))} {data.min.ToString(CultureInfo.GetCultureInfo("en-US"))}");
            wrMax.WriteLine($"{currentXValue.ToString(CultureInfo.GetCultureInfo("en-US"))} {data.max.ToString(CultureInfo.GetCultureInfo("en-US"))}");
            wrDec.WriteLine($"{currentXValue.ToString(CultureInfo.GetCultureInfo("en-US"))} {data.decil.ToString(CultureInfo.GetCultureInfo("en-US"))}");
        }

        public void InitNewForOneSize(int newCurrentX)
        {
            elements = 0;
            steps = 0;

            runsForOneSize.Clear();
            currentXValue = newCurrentX;
        }

    }

    public class StepsLogger : StepsCounter
    {
        public TextWriter Writer;

        protected long elements = 0;
        public void NewElementBoundary()
        {
            elements++;
        }

        public void FlushElementSegment(double xValue)
        {
            double average = steps / (double)elements;
            Writer.WriteLine($"{xValue.ToString(CultureInfo.GetCultureInfo("en-US"))} {average.ToString(CultureInfo.GetCultureInfo("en-US"))}");

            elements = 0;
            steps = 0;
        }
    }

    public class TimeLogger
    {
        public TextWriter Writer;
        Stopwatch stp = new Stopwatch();

        public void StartElementSegment()
        {
            stp.Start();
        }

        private long elements = 0;
        public void NewElementBoundary()
        {
            elements++;
        }

        public void FlushElementSegment(double xValue)
        {
            stp.Stop();
            double average = stp.ElapsedMilliseconds / (double)elements;
            Writer.WriteLine($"{xValue.ToString(CultureInfo.GetCultureInfo("en-US"))} {average.ToString(CultureInfo.GetCultureInfo("en-US"))}");

            elements = 0;
            stp.Reset();
        }

    }
}
