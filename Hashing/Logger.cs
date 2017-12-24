using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;

namespace Hashing
{
    public class StepsLogger
    {
        public TextWriter Writer;

        private long steps = 0;
        public void AddStep()
        {
            steps++;
        }

        private long elements = 0;
        public void NewElementBoundary()
        {
            elements++;
        }

        public void FlushElementSegment(double fillFactor)
        {
            double average = steps / (double)elements;
            Writer.WriteLine($"{fillFactor.ToString(CultureInfo.GetCultureInfo("en-US"))} {average.ToString(CultureInfo.GetCultureInfo("en-US"))}");

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

        public void FlushElementSegment(double fillFactor)
        {
            stp.Stop();
            double average = stp.ElapsedMilliseconds / (double)elements;
            Writer.WriteLine($"{fillFactor.ToString(CultureInfo.GetCultureInfo("en-US"))} {average.ToString(CultureInfo.GetCultureInfo("en-US"))}");

            elements = 0;
            stp.Reset();
        }

    }
}
