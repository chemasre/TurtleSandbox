using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleSandbox
{
    internal class TracePlayer
    {
        // Enums

        public enum PlayState
        {
            playing,
            stopped,
            fastForward,
            fastBackwards
        };

        // Constants

        const float stepEpsilon = 0.001f;

        // Line

        static Sprite lineSprite;
        static Texture lineTexture;

        // Trace

        static List<Turtle.Step> trace;

        static int stepIndex = 0;
        static PlayState playState;
        static bool stepForward;
        static bool stepBackward;

        static Clock stepClock;
        static bool stepChanged;

        public static void Init()
        {
            RenderWindow window = App.GetWindow();

            lineSprite = new Sprite();
            lineTexture = new Texture("Assets/Line.png");
            lineSprite.Texture = lineTexture;
            lineSprite.Origin = new Vector2f(25.0f, 0);
            lineSprite.Position = new Vector2f(window.Size.X, window.Size.Y);
            lineSprite.Scale = new Vector2f(1, 100);
            lineSprite.Rotation = 45;
            Color lineColor = new Color(255, 255, 255);
            lineSprite.Color = lineColor;

            playState = PlayState.playing;

            stepClock = new Clock();            

            trace = new List<Turtle.Step>();
        }

        public static void SetStep(int _step)
        {
            stepIndex = _step;
            stepClock.Restart();
        }

        public static void SetEndStep()
        {
            SetStep(trace.Count - 1);
        }

        public static void SetTrace(List<Turtle.Step> _trace)
        {
            trace = _trace;
        }

        public static void Play()
        {
            playState = PlayState.playing;
        }

        public static void FastForward()
        {
            playState = PlayState.fastForward;
        }

        public static void FastBackwards()
        {
            playState = PlayState.fastBackwards;
        }

        public static void Stop()
        {
            playState = PlayState.stopped;
        }

        public static PlayState GetPlayState()
        {
            return playState;
        }

        public static void StepForward()
        {
            stepForward = true;
        }

        public static void StepBackward()
        {
            stepBackward = true;
        }

        public static void Update(float elapsedTime, float timeBoost)
        {
            // Update step

            stepChanged = false;
            
            bool stepWaitOver = stepClock.ElapsedTime.AsSeconds() > AppConfig.stepWait / timeBoost;
            bool isPlayingState = (playState == PlayState.playing);
            bool isFastForwardState = (playState == PlayState.fastForward);
            bool isFastBackwardsState = (playState == PlayState.fastBackwards);

            if ((isPlayingState || isFastForwardState || isFastBackwardsState) && stepWaitOver ||
                stepForward || stepBackward)
            {
                if ((isPlayingState || isFastForwardState || stepForward) && stepIndex < trace.Count - 1)
                {
                    stepChanged = true;
                    stepIndex++;

                    if(stepIndex == trace.Count - 1) { playState = PlayState.stopped; }
                }
                else if ((isFastBackwardsState || stepBackward) && stepIndex > 0)
                {
                    stepChanged = true;
                    stepIndex--;

                    if (stepIndex == 0) { playState = PlayState.stopped; }
                }

                if (isPlayingState || isFastForwardState || isFastBackwardsState)
                {
                    stepClock.Restart();
                }
                

            }

            Turtle.Step p = trace[stepIndex];

            UI.SetStatus((int)p.x, (int)p.y, (int)p.angle);

            if (stepChanged)
            {
                string info = UI.FormatOrderInfo(p.order);
                UI.AddInfoMessage(info, UI.InfoMessagePosition.Turtle);

            }

            // Flags

            stepForward = false;
            stepBackward = false;



        }

        public static void Draw(RenderWindow window, float opacity)
        {
            for (int i = 0; i < stepIndex; i++)
            {
                Turtle.Step p1 = trace[i];
                Turtle.Step p2 = trace[i + 1];
                float aX = p2.x - p1.x;
                float aY = -p2.y - (-p1.y);

                bool hasMoved = MathF.Abs(aX) >= stepEpsilon || MathF.Abs(aY) >= stepEpsilon;

                if (hasMoved && p2.draw)
                {
                    float rotation = MathF.Atan2(aY, aX) * 180 / MathF.PI - 90;
                    float length = MathF.Sqrt(aX * aX + aY * aY) * AppConfig.pixelsPerStep;
                    lineSprite.Position = UI.TurtlePositionToScreen(p1.x, p1.y, window);
                    lineSprite.Rotation = rotation;
                    lineSprite.Scale = new Vector2f(Config.lineWidth / 50.0f, length / 600.0f);
                    lineSprite.Color = new Color((byte)p2.colorR, (byte)p2.colorG, (byte)p2.colorB, (byte)(p2.opacity * opacity));
                    window.Draw(lineSprite);
                }

            }

        }

    }
}
