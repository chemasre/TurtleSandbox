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

        // Background

        static Sprite backgroundSprite;
        static Texture backgroundTexture;
        static Color backgroundColor;

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

        static RenderTexture cacheTexture;
        static Sprite cacheSprite;
        static int cachedSteps;

        public static void Init(Color _backgroundColor)
        {
            RenderWindow window = App.GetWindow();

            Color c = UI.GetBackgroundColor();
            backgroundColor = _backgroundColor;
            backgroundSprite = new Sprite();
            backgroundTexture = new Texture("Assets/Background.png");
            backgroundSprite.Texture = backgroundTexture;
            backgroundSprite.Color = c;

            cacheTexture = new RenderTexture(window.Size.X, window.Size.Y);
            cacheTexture.Clear(c);
            cacheTexture.Draw(backgroundSprite);
            cacheTexture.Display();
            cacheTexture.Smooth = true;
            cacheSprite = new Sprite(cacheTexture.Texture);
            cachedSteps = 0;

            lineSprite = new Sprite();
            lineTexture = new Texture("Assets/Line.png");
            lineTexture.Smooth = true;
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
            stepIndex = 0;
            trace = _trace;

            ClearCache();
        }

        public static void SetBackgroundColor(Color c)
        {
            backgroundColor = c;

            ClearCache();
        }

        public static void Reset()
        {
            stepIndex = 0;
            ClearCache();
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

        static void ClearCache()
        {
            DrawBackground(cacheTexture);
            cacheTexture.Display();
            cachedSteps = 0;
        }

        public static void Draw(RenderTarget target, float opacity)
        {
            if(AppConfig.cacheEnabled)
            {
                int blockSize = AppConfig.cachedStepsBlockSize;

                if (stepIndex < cachedSteps - 1)
                {
                    ClearCache();
                }

                bool cacheStepsAdded = false;

                while(stepIndex - cachedSteps >= blockSize)
                {
                    DrawSteps(cachedSteps, cachedSteps + blockSize - 1, cacheTexture, opacity);

                    cachedSteps += blockSize;

                    cacheStepsAdded = true;

                }

                if(cacheStepsAdded) { cacheTexture.Display(); }

                target.Draw(cacheSprite);
            }
            else
            {
                DrawBackground(target);
                cachedSteps = 0;
            }

            if(cachedSteps <= stepIndex)
            {                
                DrawSteps(cachedSteps, stepIndex, target, opacity);
            }

        }

        static void DrawSteps(int firstStep, int lastStep, RenderTarget target, float opacity)
        {
            if(firstStep > 0) { firstStep--; }

            for (int i = firstStep; i < lastStep; i++)
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
                    lineSprite.Position = UI.TurtlePositionToScreen(p1.x, p1.y, target);
                    lineSprite.Rotation = rotation;
                    lineSprite.Scale = new Vector2f(Config.lineWidth / 50.0f, length / 600.0f);
                    lineSprite.Color = new Color((byte)p2.colorR, (byte)p2.colorG, (byte)p2.colorB, (byte)(p2.opacity * opacity));
                    target.Draw(lineSprite);
                }

            }

        }

        public static void DrawBackground(RenderTarget target)
        {
            target.Clear(backgroundColor);
            backgroundSprite.Color = backgroundColor;
            target.Draw(backgroundSprite);
        }

    }
}
