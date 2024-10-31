using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Terraira_v0._4_beta
{
    class DeltaTime
    {

        private static float nowTime;
        public static float deltaTime = 0;
        private static float lastDeltaTime;
        public static Thread deltaTimeThread;

        public static float getDeltaTime()
        {
            return deltaTime;
        }

        public static float getLastDeltaTime()
        {
            return deltaTime;
        }

        public static void startDeltaTime()
        {
            deltaTimeThread = new Thread(threadDeltaTime);
            deltaTimeThread.Start();
        }

        public static void threadDeltaTime()
        {
            while (true)
            {
                Thread.Sleep(2);
                nowTime += 1;
            }
        }

    }
}
