using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleSandbox
{
    internal partial class PlayMode
    {
        static int playIndex = 0;
        static int nextPlayIndex = 0;

        static Turtle turtle;

        public static void Init()
        {
            playIndex = Config.play - 1;
            nextPlayIndex = Config.play - 1;

            turtle = App.GetTurtle();

            turtle.Reset();

            if (nextPlayIndex == 0) { Play1(); }
            else if (nextPlayIndex == 1) { Play2(); }
            else if (nextPlayIndex == 2) { Play3(); }
            else if (nextPlayIndex == 3) { Play4(); }
            else if (nextPlayIndex == 4) { Play5(); }
            else if (nextPlayIndex == 5) { Play6(); }
            else if (nextPlayIndex == 6) { Play7(); }
            else if (nextPlayIndex == 7) { Play8(); }
            else if (nextPlayIndex == 8) { Play9(); }

            List<Turtle.Step> trace = turtle.GetTrace();
            TracePlayer.SetTrace(trace);

            TracePlayer.SetStep(0);
            TracePlayer.Play();
        }

        public static int GetPlayIndex()
        {
            return playIndex;
        }

        public static void SetPlayIndex(int index)
        {
            nextPlayIndex = index;
        }

        public static void NextPlayIndex()
        {
            if (nextPlayIndex + 1 >= AppConfig.playsCount) { nextPlayIndex = 0; }
            else { nextPlayIndex++; }

        }

        public static void PreviousPlayIndex()
        {
            if (nextPlayIndex - 1 < 0) { nextPlayIndex = AppConfig.playsCount - 1; }
            else { nextPlayIndex--; }
        }

        public static void Update()
        {
            if (playIndex != nextPlayIndex)
            {
                // Generate trace

                turtle.Reset();

                if (nextPlayIndex == 0) { Play1(); }
                else if (nextPlayIndex == 1) { Play2(); }
                else if (nextPlayIndex == 2) { Play3(); }
                else if (nextPlayIndex == 3) { Play4(); }
                else if (nextPlayIndex == 4) { Play5(); }
                else if (nextPlayIndex == 5) { Play6(); }
                else if (nextPlayIndex == 6) { Play7(); }
                else if (nextPlayIndex == 7) { Play8(); }
                else if (nextPlayIndex == 8) { Play9(); }

                List<Turtle.Step> trace = turtle.GetTrace();
                TracePlayer.SetTrace(trace);

                TracePlayer.SetStep(0);
                TracePlayer.Play();

                playIndex = nextPlayIndex;
            }


        }
    }
}
