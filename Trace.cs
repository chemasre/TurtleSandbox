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
    internal partial class Program
    {
        enum PlayState
        {
            playing,
            stopped,
            fastForward,
            fastBackwards
        };

        const float stepSize = 1; // Distancia en píxels que recorre la tortuga por cada unidad que avanza
        const float stepEpsilon = 0.001f;

        static List<Turtle.Step> trace;

        static bool turtleVisible;
        static Turtle turtle;
        static Sprite turtleSprite;
        static Texture turtleTexture;

        static Sprite lineSprite;
        static Texture lineTexture;

        static int stepIndex = 0;
        static int playIndex = 0;
        static int nextPlayIndex = 0;
        static PlayState playState;
        static bool stepForward;
        static bool stepBackward;

        static void InitTrace()
        {
            turtle = new Turtle();

            turtleSprite = new Sprite();
            turtleTexture = new Texture("Assets/Turtle.png");
            turtleSprite.Texture = turtleTexture;
            turtleSprite.Origin = new Vector2f(50, 50);
            turtleSprite.Scale = new Vector2f(0.5f, 0.5f);

            lineSprite = new Sprite();
            lineTexture = new Texture("Assets/Line.png");
            lineSprite.Texture = lineTexture;
            lineSprite.Origin = new Vector2f(25.0f, 0);
            lineSprite.Position = new Vector2f(windowWidth, windowHeight);
            lineSprite.Scale = new Vector2f(1, 100);
            lineSprite.Rotation = 45;
            Color lineColor = new Color(255, 255, 255);
            lineSprite.Color = lineColor;

            playIndex = play - 1;
            nextPlayIndex = play - 1;
            playState = PlayState.playing;
            turtleVisible = true;
        }

        static void UpdateTrace(float elapsedTime)
        {
            if (playIndex != nextPlayIndex)
            {
                stepIndex = 0;
                stepClock.Restart();
                playIndex = nextPlayIndex;
                playState = PlayState.playing;
            }

            // Generate trace

            turtle.Reset();

            if (playIndex == 0) { Play1(); }
            else if (playIndex == 1) { Play2(); }
            else if (playIndex == 2) { Play3(); }
            else if (playIndex == 3) { Play4(); }
            else if (playIndex == 4) { Play5(); }
            else if (playIndex == 5) { Play6(); }
            else if (playIndex == 6) { Play7(); }
            else if (playIndex == 7) { Play8(); }
            else if (playIndex == 8) { Play9(); }


            trace = turtle.GetTrace();

            // Update step

            stepChanged = false;
            
            bool stepWaitOver = stepClock.ElapsedTime.AsSeconds() > stepWait / timeBoost;
            bool isPlayingState = (playState == PlayState.playing);
            bool isFastForwardState = (playState == PlayState.fastForward);
            bool isFastBackwardsState = (playState == PlayState.fastBackwards);

            if(isFastForwardState || isFastBackwardsState) { timeBoost = timeBoostFast; }
            else { timeBoost = 1; }

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

            infoPosX = (int)p.x;
            infoPosY = (int)p.y;
            infoAngle = (int)p.angle;

            if (stepChanged)
            {
                string info = FormatOrderInfo(p.order);
                AddInfoMessage(info, TurtlePositionToScreen(p.x, p.y));

            }


        }

        static void DrawTrace(RenderWindow window)
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
                    float length = MathF.Sqrt(aX * aX + aY * aY) * stepSize;
                    lineSprite.Position = TurtlePositionToScreen(p1.x, p1.y);
                    lineSprite.Rotation = rotation;
                    lineSprite.Scale = new Vector2f(lineWidth / 50.0f, length / 600.0f);
                    lineSprite.Color = new Color((byte)p2.colorR, (byte)p2.colorG, (byte)p2.colorB, (byte)p2.opacity);
                    window.Draw(lineSprite);
                }



            }

            if (stepIndex >= 0 && stepIndex < trace.Count)
            {

                if (turtleVisible)
                {
                    Turtle.Step p = trace[stepIndex];
                    turtleSprite.Position = TurtlePositionToScreen(p.x, p.y);
                    turtleSprite.Rotation = TurtleAngleToScreenRotation(p.angle);
                    window.Draw(turtleSprite);

                }

            }
        }

        static void SwitchTurtle()
        {
            turtleVisible = !turtleVisible;

            AddInfoMessage(texts[turtleVisible ? TextId.turtleOn : TextId.turtleOff], new Vector2f(buttonTurtleSprite.Position.X, buttonBar1Y));

        }

        static string FormatOrderInfo(Turtle.Order order)
        {
            textBuilder.Clear();
            textBuilder.AppendFormat("{0}", orderIdToString[order.id]);

            Turtle.OrderId id = order.id;
            if (id == Turtle.OrderId.walk)
            {
                textBuilder.AppendFormat(" {0,-4}", (int)order.param1);
            }
            else if (id == Turtle.OrderId.turn)
            {
                textBuilder.AppendFormat(" {0,-4}", (int)order.param1);
            }
            else if (id == Turtle.OrderId.randTurn)
            {
                textBuilder.AppendFormat(" {0,-4} {1,-4}", (int)order.param1, (int)order.param2);
            }
            else if (id == Turtle.OrderId.teleport)
            {
                textBuilder.AppendFormat(" {0,-4} {1,-4} {2,-3}", (int)order.param1, (int)order.param2, (int)order.param3);
            }

            return textBuilder.ToString();
        }

        static Vector2f TurtlePositionToScreen(float x, float y)
        {
            return new Vector2f(windowWidth / 2, windowHeight / 2) + new Vector2f(x, -y) * stepSize;
        }

        static float TurtleAngleToScreenRotation(float a)
        {
            return -a - 90;
        }



    }
}
