using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MwoCWDropDeckBuilder.Infrastructure
{
    public static class PerformanceHelper
    {
        private static Dictionary<Guid, Stopwatch> _timers = new Dictionary<Guid, Stopwatch>();


        public static Guid Start()
        {
            var sw = new Stopwatch();
            var guid = Guid.NewGuid();
            sw.Start();
            _timers.Add(guid, sw);
            return guid;
        }

        public static long Stop(Guid guid)
        {
            long returnValue = 0;
            if (_timers.ContainsKey(guid))
            {
                var sw = _timers[guid];
                sw.Stop();
                returnValue = sw.ElapsedMilliseconds;
            }
            return returnValue;
        }

    }
}
