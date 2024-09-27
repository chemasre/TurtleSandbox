using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;


namespace TurtlesBeach
{
    internal partial class Program
    {
        static Music music;

        static Turtle turtle;
        static int stepIndex = 0;

        static void Main(string[] args)
        {
            turtle = new Turtle(lineR, lineG, lineB);
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
            Color lineColor = new Color((byte)lineR, (byte)lineG, (byte)lineB);
            lineSprite.Color = lineColor;

            music = new Music("Assets/Music.wav");
            music.Loop = true;

            if(playMusic)
            {
                music.Play();
            }

            Clock stepClock = new Clock();

            VideoMode mode = new VideoMode((uint)windowWidth, (uint)windowHeight);
            RenderWindow window = new RenderWindow(mode, windowTitle);
            window.KeyPressed += OnKeyPressed;

            while (window.IsOpen)
            {
                window.DispatchEvents();

                window.Clear(sandColor);

                window.Draw(sandSprite);

                turtle.Reset();

                Play();

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
                }

                window.Draw(turtleSprite);

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
        }
    }
}
