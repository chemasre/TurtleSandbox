using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System.Text;


namespace TurtlesBeach
{
    internal partial class Program
    {
        static int windowWidth = 1280;
        static int windowHeight = 720;        

        static float stepSize = 1; // Distancia en píxels que recorre la tortuga por cada unidad que avanza

        static Music music;

        static bool turtleVisible;
        static Turtle turtle;
        static int stepIndex = 0;
        static int playIndex = 0;
        static int nextPlayIndex = 0;

        static Font font;
        static Text text;
        static StringBuilder textBuilder;
        static int infoPosX;
        static int infoPosY;
        static int infoAngle;

        static Texture gridTexture;
        static Sprite gridSprite;
        
        static void Main(string[] args)
        {
            turtle = new Turtle();
            Sprite sandSprite = new Sprite();
            Texture sandTexture = new Texture("Assets/Background.png");
            sandSprite.Texture = sandTexture;
            Color sandColor = new Color((byte)sandR, (byte)sandG, (byte)sandB);
            sandSprite.Color = sandColor;
            Sprite turtleSprite = new Sprite();
            Texture turtleTexture = new Texture("Assets/Turtle.png");
            turtleSprite.Texture = turtleTexture;
            turtleSprite.Origin = new Vector2f(50, 50);
            turtleSprite.Scale = new Vector2f(0.5f,0.5f);
            var lineSprite = new Sprite();
            var lineTexture = new Texture("Assets/Line.png");
            lineSprite.Texture = lineTexture;
            lineSprite.Origin = new Vector2f(25.0f, 0);
            lineSprite.Position = new Vector2f(windowWidth, windowHeight);
            lineSprite.Scale = new Vector2f(1, 100);
            lineSprite.Rotation = 45;
            Color lineColor = new Color(255,255,255);
            lineSprite.Color = lineColor;

            music = new Music("Assets/Music.wav");
            music.Loop = true;

            text = new Text();
            text.Position = new Vector2f(20, 670);
            text.FillColor = new Color((byte)infoR, (byte)infoG, (byte)infoB);
            font = new Font("Assets/Font.ttf");
            textBuilder = new StringBuilder("", 200);
            text.DisplayedString = textBuilder.ToString();
            infoPosX = 0;
            infoPosY = 0;
            infoAngle = 0;

            gridTexture = new Texture("Assets/Grid.png");
            gridSprite = new Sprite();
            gridSprite.Texture = gridTexture;
            gridSprite.Position = new Vector2f(0, 0);
            gridSprite.Color = new Color((byte)gridR, (byte)gridG, (byte)gridB, (byte)gridOpacity);

            if (playMusic)
            {
                music.Play();
            }

            Clock stepClock = new Clock();

            VideoMode mode = new VideoMode((uint)windowWidth, (uint)windowHeight);
            RenderWindow window = new RenderWindow(mode, windowTitle);
            window.KeyPressed += OnKeyPressed;

            playIndex = play - 1;
            nextPlayIndex = play - 1;
            turtleVisible = true;

            text.Font = font;


            while (window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear(sandColor);

                window.Draw(sandSprite);

                turtle.Reset();

                if(playIndex != nextPlayIndex)
                {
                    stepIndex = 0;
                    stepClock.Restart();
                    playIndex = nextPlayIndex;
                }

                if (playIndex == 0) { Play1(); }
                else if (playIndex == 1) { Play2(); }
                else if (playIndex == 2) { Play3(); }
                else if (playIndex == 3) { Play4(); }
                else if (playIndex == 4) { Play5(); }
                else if (playIndex == 5) { Play6(); }
                else if (playIndex == 6) { Play7(); }
                else if (playIndex == 7) { Play8(); }
                else if (playIndex == 8) { Play9(); }


                List<Turtle.Step> trace = turtle.GetTrace();

                float stepBoost = 1;
                if(Keyboard.IsKeyPressed(Keyboard.Key.LShift)) { stepBoost = 5.0f; }
                else if(Keyboard.IsKeyPressed(Keyboard.Key.RShift)) { stepBoost = 0.3f; }

                if (stepClock.ElapsedTime.AsSeconds() > stepWait / stepBoost)
                {
                    if(stepIndex < trace.Count - 1)
                    {
                        stepIndex ++;
                    }

                    stepClock.Restart();
                }

                turtleSprite.Position = new Vector2f(windowWidth / 2, windowHeight / 2) + new Vector2f(turtle.posX, -turtle.posY) * stepSize;
                turtleSprite.Rotation = turtle.angle + 90;


                for (int i = 0; i < stepIndex; i++)
                {
                    Turtle.Step p1 = trace[i];
                    Turtle.Step p2 = trace[i + 1];
                    p1.x *= stepSize;
                    p1.y *= stepSize;
                    p2.x *= stepSize;
                    p2.y *= stepSize;
                    float aX = p2.x - p1.x;
                    float aY = -p2.y - (-p1.y);
                    lineSprite.Position = new Vector2f(windowWidth/2, windowHeight/2) + new Vector2f(p1.x, -p1.y);

                    float rotation = MathF.Atan2(aY, aX) * 180 / MathF.PI - 90;
                    float length = MathF.Sqrt(aX * aX + aY * aY);
                    lineSprite.Rotation = rotation;
                    turtleSprite.Position = new Vector2f(windowWidth / 2, windowHeight / 2) + new Vector2f(p2.x, -p2.y);
                    turtleSprite.Rotation = rotation;

                    lineSprite.Scale = new Vector2f(lineWidth / 50.0f, length / 600.0f);
                    lineSprite.Color = p2.color;

                    if(p2.draw)
                    {
                        window.Draw(lineSprite);
                    }

                    infoPosX = (int)p2.x;
                    infoPosY = (int)p2.y;
                    infoAngle = (int)p2.angle;
                }

                if (showGrid)
                {
                    window.Draw(gridSprite);
                }

                if (turtleVisible)
                {
                    window.Draw(turtleSprite);
                }


                if (showInfo)
                {
                    textBuilder.Clear();
                    textBuilder.AppendFormat("Play {0,1} Steps {1,4} X {2,3} Y {3,3} Angle {4,3}",
                                             playIndex + 1, stepIndex, infoPosX, infoPosY, infoAngle);

                    text.DisplayedString = textBuilder.ToString();
                    window.Draw(text);
                }

                window.Display();
            }
        }

        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            Window window = (Window)sender;
            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
            else if(e.Code == Keyboard.Key.Space)
            {
                stepIndex = 0;
            }
            else if (e.Code == Keyboard.Key.M)
            {
                if(music.Status == SoundStatus.Playing)
                {
                    music.Stop();
                }
                else
                {
                    music.Play();
                }
            }
            else if (e.Code == Keyboard.Key.H)
            {
                turtleVisible = !turtleVisible;
            }
            else if (e.Code == Keyboard.Key.I)
            {
                showInfo = !showInfo;
            }
            else if (e.Code == Keyboard.Key.G)
            {
                showGrid = !showGrid;
            }
            else if (e.Code == Keyboard.Key.Num1)
            {
                nextPlayIndex = 0;
            }
            else if (e.Code == Keyboard.Key.Num2)
            {
                nextPlayIndex = 1;
            }
            else if (e.Code == Keyboard.Key.Num3)
            {
                nextPlayIndex = 2;
            }
            else if (e.Code == Keyboard.Key.Num4)
            {
                nextPlayIndex = 3;
            }
            else if (e.Code == Keyboard.Key.Num5)
            {
                nextPlayIndex = 4;
            }
            else if (e.Code == Keyboard.Key.Num6)
            {
                nextPlayIndex = 5;
            }
            else if (e.Code == Keyboard.Key.Num7)
            {
                nextPlayIndex = 6;
            }
            else if (e.Code == Keyboard.Key.Num8)
            {
                nextPlayIndex = 7;
            }
            else if (e.Code == Keyboard.Key.Num9)
            {
                nextPlayIndex = 8;
            }
        }
    }
}
