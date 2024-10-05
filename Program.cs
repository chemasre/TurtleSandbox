using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System.Text;


namespace TurtlesBeach
{
    internal partial class Program
    {
        // Private config

        const int windowWidth = 1280;
        const int windowHeight = 720;

        const float stepSize = 1; // Distancia en píxels que recorre la tortuga por cada unidad que avanza

        const int infoMessagesCount = 50;
        const float infoMessageDuration = 0.5f;
        const float infoMessageDistance = 250;
        const float infoMessageOffset = 35;
        const float infoMessageScale = 0.8f;

        // End private config

        static Music music;

        static bool turtleVisible;
        static Turtle turtle;
        static int stepIndex = 0;
        static int playIndex = 0;
        static int nextPlayIndex = 0;
        static bool playing;
        static bool stepForward;
        static bool stepBackward;

        static Font font;
        static Text text;
        static StringBuilder textBuilder;
        static int infoPosX;
        static int infoPosY;
        static int infoAngle;

        static Text[] infoMessages;
        static bool[] infoMessagesFree;
        static float[] infoMessagesLifetime;
        static Vector2f[] infoMessagesPosition;
        static Dictionary<Turtle.OrderId, string> orderIdToString;

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
            orderIdToString = new Dictionary<Turtle.OrderId, string>();
            orderIdToString[Turtle.OrderId.origin] = "origin";
            orderIdToString[Turtle.OrderId.walk] = "walk";
            orderIdToString[Turtle.OrderId.turn] = "turn";
            orderIdToString[Turtle.OrderId.randTurn] = "randTurn";
            orderIdToString[Turtle.OrderId.randWalk] = "randWalk";
            orderIdToString[Turtle.OrderId.teleport] = "teleport";

            InitInfoMessages();

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
            Clock updateClock = new Clock();

            VideoMode mode = new VideoMode((uint)windowWidth, (uint)windowHeight);
            RenderWindow window = new RenderWindow(mode, windowTitle);
            Image icon = new Image("Assets/Icon.png");            
            window.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);
            window.KeyPressed += OnKeyPressed;

            playIndex = play - 1;
            nextPlayIndex = play - 1;
            playing = true;
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

                bool stepChanged = false;
                if (playing && stepClock.ElapsedTime.AsSeconds() > stepWait / stepBoost || stepForward || stepBackward)
                {
                    if((playing || stepForward) && stepIndex < trace.Count - 1)
                    {
                        stepChanged = true;
                        stepIndex ++;
                    }
                    else if(stepBackward && stepIndex > 0)
                    {
                        stepChanged = true;
                        stepIndex--;
                    }

                    if (playing)
                    {
                        stepClock.Restart();
                    }

                }

                turtleSprite.Position = new Vector2f(windowWidth / 2, windowHeight / 2) + new Vector2f(turtle.posX, -turtle.posY) * stepSize;
                turtleSprite.Rotation = -turtle.angle - 90;

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

                    if(aX != 0.0f || aY != 0.0f)
                    {
                        float rotation = MathF.Atan2(aY, aX) * 180 / MathF.PI - 90;
                        float length = MathF.Sqrt(aX * aX + aY * aY);
                        lineSprite.Rotation = rotation;
                        lineSprite.Scale = new Vector2f(lineWidth / 50.0f, length / 600.0f);
                        lineSprite.Color = new Color(p2.color.R, p2.color.G, p2.color.B, (byte)p2.opacity);
                    }

                    turtleSprite.Position = TurtlePositionToScreen(p2.x, p2.y);
                    turtleSprite.Rotation = TurtleAngleToScreenRotation(p2.angle);


                    if(p2.draw && (aX != 0.0f || aY != 0.0f))
                    {
                        window.Draw(lineSprite);
                    }

                    if(i == stepIndex - 1)
                    {
                        infoPosX = (int)p2.x;
                        infoPosY = (int)p2.y;
                        infoAngle = (int)p2.angle;

                        if(stepChanged)
                        {
                            string text;
                            textBuilder.Clear();
                            textBuilder.AppendFormat("{0}", orderIdToString[p2.order.id]);

                            Turtle.OrderId id = p2.order.id;
                            if (id == Turtle.OrderId.walk)
                            {
                                textBuilder.AppendFormat(" {0,-4}", (int)p2.order.param1);
                            }
                            else if (id == Turtle.OrderId.turn)
                            {
                                textBuilder.AppendFormat(" {0,-4}", (int)p2.order.param1);
                            }
                            else if (id == Turtle.OrderId.randTurn)
                            {
                                textBuilder.AppendFormat(" {0,-4} {1,-4}", (int)p2.order.param1, (int)p2.order.param2);
                            }
                            else if(id == Turtle.OrderId.teleport)
                            {
                                textBuilder.AppendFormat(" {0,-4} {1,-4} {2,-3}", (int)p2.order.param1, (int)p2.order.param2, (int)p2.order.param3);
                            }
                            text = textBuilder.ToString();
                            AddInfoMessage(text, TurtlePositionToScreen(p2.x, p2.y));

                        }


                    }
                }

                if (showGrid)
                {
                    window.Draw(gridSprite);
                }

                if (turtleVisible)
                {
                    window.Draw(turtleSprite);
                }

                UpdateInfoMessages(updateClock.ElapsedTime.AsSeconds() * stepBoost);


                if (showInfo)
                {
                    textBuilder.Clear();
                    textBuilder.AppendFormat("Play {0,1} Steps {1,4} X {2,3} Y {3,3} Angle {4,3}",
                                             playIndex + 1, stepIndex, infoPosX, infoPosY, infoAngle);

                    text.DisplayedString = textBuilder.ToString();
                    window.Draw(text);

                    DrawInfoMessages(window);


                }

                window.Display();

                updateClock.Restart();

                stepForward = false;
                stepBackward = false;
            }
        }

        static Vector2f TurtlePositionToScreen(float x, float y)
        {
            return new Vector2f(windowWidth / 2, windowHeight / 2) + new Vector2f(x, -y);
        }

        static float TurtleAngleToScreenRotation(float a)
        {
            return -a - 90;
        }

        static void InitInfoMessages()
        {
            infoMessages = new Text[infoMessagesCount];
            infoMessagesFree = new bool[infoMessagesCount];
            infoMessagesPosition = new Vector2f[infoMessagesCount];
            infoMessagesLifetime = new float[infoMessagesCount];
            for (int i = 0; i < infoMessagesCount; i++)
            {
                infoMessages[i] = new Text();
                infoMessages[i].Font = font;
                infoMessages[i].Scale = new Vector2f(infoMessageScale, infoMessageScale);
                infoMessagesFree[i] = true;
                infoMessagesPosition[i] = new Vector2f(0, 0);
                infoMessagesLifetime[i] = 0;
            }

        }

        static void AddInfoMessage(string text, Vector2f position)
        {
            int? free = null;
            int oldest = 0;
            float oldestLifeTime = 0;
            int j = 0;
            bool done = false;

            while (j < infoMessages.Length && !done)
            {
                if (infoMessagesFree[j]) { free = j; infoMessagesFree[j] = false; done = true; }
                else
                {
                    if (infoMessagesLifetime[j] > oldestLifeTime)
                    {
                        oldestLifeTime = infoMessagesLifetime[j];
                        oldest = j;
                    }

                    j++;
                }
            }

            int messageIndex = free ?? oldest;
            infoMessages[messageIndex].DisplayedString = text;
            infoMessagesPosition[messageIndex] = position;
            infoMessagesLifetime[messageIndex] = 0;
            infoMessagesFree[messageIndex] = false;

        }

        static void UpdateInfoMessages(float elapsedTime)
        {
            for (int i = 0; i < infoMessages.Length; i++)
            {
                if (!infoMessagesFree[i])
                {
                    infoMessagesLifetime[i] += elapsedTime;
                    if (infoMessagesLifetime[i] > infoMessageDuration) { infoMessagesFree[i] = true; }
                }
            }
        }

        static void DrawInfoMessages(RenderWindow window)
        {
            for (int i = 0; i < infoMessages.Length; i++)
            {
                if (!infoMessagesFree[i])
                {
                    float factor = infoMessagesLifetime[i] / infoMessageDuration;
                    float opacityFactor = MathF.Pow(1 - factor, 3);

                    Vector2f position = infoMessagesPosition[i];
                    infoMessages[i].Position = position + new Vector2f(0, - infoMessageOffset - infoMessageDistance * factor);
                    infoMessages[i].FillColor = new Color((byte)infoR, (byte)infoG, (byte)infoB, (byte)(255 * opacityFactor));
                    window.Draw(infoMessages[i]);

                }
            }
        }

        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;

            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }
            else if(e.Code == Keyboard.Key.Space)
            {
                stepIndex = 0;
            }
            else if (e.Code == Keyboard.Key.Right)
            {
                stepForward = true;
                playing = false;
            }
            else if (e.Code == Keyboard.Key.Left)
            {
                stepBackward = true;
                playing = false;
            }
            else if (e.Code == Keyboard.Key.Enter)
            {
                playing = !playing;
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
            else if(e.Code == Keyboard.Key.C)
            {
                Texture texture = new Texture(window.Size.X, window.Size.Y);
                texture.Update(window);
                Image image = texture.CopyToImage();

                bool done = false;
                StringBuilder builder = new StringBuilder("", 100);
                int index = 0;
                while(index < 1000 && !done)
                {
                    builder.Clear();
                    builder.AppendFormat("screenshot{0:000}.png", index);
                    string fileName = builder.ToString();

                    if (!File.Exists(fileName))
                    {   image.SaveToFile(fileName);
                        Console.WriteLine("Screenshot saved to " + fileName);
                        done = true;
                    }
                    else { index++; }

                    
                }
            }
        }
    }
}
