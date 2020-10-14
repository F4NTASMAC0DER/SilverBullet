using System;
using System.Diagnostics;

namespace RuriLib.Models
{
    /// <summary>
    /// CPU,RAM usage in percentage
    /// </summary>
    public class Usage
    {
        //application instance name
        private static string instance;
        //process Id
        private static int pid;

        private static DateTime lastTime;
        private static TimeSpan lastTotalProcessorTime;
        private static DateTime curTime;
        private static TimeSpan curTotalProcessorTime;


        private Usage() { }

        /// <summary>
        /// cpu usaged
        /// </summary>
        public double Cpu { get; private set; }

        /// <summary>
        /// ram usaged
        /// </summary>
        public double Ram { get; private set; }

        /// <summary>
        /// Get current CPU,RAM usage in percentage
        /// </summary>
        /// <returns></returns>
        public static Usage Get()
        {
            // Getting information about current process
            var process = Process.GetCurrentProcess();

            if (process.ProcessName != instance || pid != process.Id)
            {
                foreach (var inst in new PerformanceCounterCategory("Process").GetInstanceNames())
                {
                    if (inst == process.ProcessName)
                    {
                        using (var processId = new PerformanceCounter("Process", "ID Process", inst, true))
                        {
                            if ((pid = process.Id) == (int)processId.RawValue)
                            {
                                instance = inst;
                                break;
                            }
                        }
                    }
                    instance = string.Empty;
                }
            }

            if (instance == string.Empty) return null;

            double cpuUsage = 0;
            if (lastTime == null || lastTime == new DateTime())
            {
                lastTime = DateTime.Now;
                lastTotalProcessorTime = process.TotalProcessorTime;
            }
            else
            {
                curTime = DateTime.Now;
                curTotalProcessorTime = process.TotalProcessorTime;

                cpuUsage = (curTotalProcessorTime.TotalMilliseconds - lastTotalProcessorTime.TotalMilliseconds) / curTime.Subtract(lastTime).TotalMilliseconds / Convert.ToDouble(Environment.ProcessorCount);

                lastTime = curTime;
                lastTotalProcessorTime = curTotalProcessorTime;
            }

            return new Usage()
            {
                // If system has multiple cores, that should be taken into account
                Cpu = Math.Round(cpuUsage > 0 ? cpuUsage * 100 : 0, 2),
                // Returns number of MB consumed by application
                Ram = Math.Round((double)process.PagedMemorySize64 / 1024 / 1024, 2)
            };
        }

        public override string ToString()
        {
            return $"{Cpu}/{SizeExtensions.SizeSuffix((long)Ram, 2)}";
        }
    }
}
