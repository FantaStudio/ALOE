using System.Collections.Generic;
using System.Timers;

namespace ALOE.Helpers
{
    class AntispamHelper
    {
        static Dictionary<string, bool> AntispamStates = new Dictionary<string, bool>();

        public static void AddAntispam(string name, int interval = 1000)
        {
            AntispamStates[name] = true;
            Timer timer = new Timer() { Interval = interval };
            timer.Elapsed += (s, e) =>
            {
                AntispamStates[name] = false;
            };
            timer.Start();
        }

        public static bool IsBlocked(string name)
        {
            if (!AntispamStates.ContainsKey(name))
            {
                return false;
            }

            return AntispamStates[name];
        }
    }
}
