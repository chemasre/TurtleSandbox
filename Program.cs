using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System.Text;
using System.Numerics;


namespace TurtleSandbox
{
    internal partial class Program
    {
        const int windowWidth = 1280;
        const int windowHeight = 720;

        const int playsCount = 9;

        static Music music;


        static Sprite sandSprite;
        static Texture sandTexture;
        static Color sandColor;


        static StringBuilder textBuilder;

        static Texture gridTexture;
        static Sprite gridSprite;

        static bool takeScreenshot;
        static bool stepChanged;

        static Clock stepClock;
        static float timeBoost;
        static float elapsedTime;
        static Clock updateClock;

        static void Main(string[] args)
        {
            InitBackground();

            textBuilder = new StringBuilder("", 200);

            InitUI();

            InitGrid();

            InitMusic();

            InitTrace();

            stepClock = new Clock();
            updateClock = new Clock();
            elapsedTime = 0;

            VideoMode mode = new VideoMode((uint)windowWidth, (uint)windowHeight);
            RenderWindow window = new RenderWindow(mode, windowTitle);
            Image icon = new Image("Assets/Icon.png");            
            window.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);
            window.KeyPressed += OnKeyPressed;
            window.MouseButtonPressed += OnMouseButtonPressed;

            InitTrace();


            while (window.IsOpen)
            {
                window.DispatchEvents();

                // Update

                timeBoost = 1;
                if (Keyboard.IsKeyPressed(Keyboard.Key.LShift)) { timeBoost = 5.0f; }
                else if (Keyboard.IsKeyPressed(Keyboard.Key.RShift)) { timeBoost = 0.3f; }

                UpdateTrace(elapsedTime * timeBoost);
                UpdateUI(elapsedTime * timeBoost);

                elapsedTime = updateClock.ElapsedTime.AsSeconds();
                updateClock.Restart();

                // Draw

                DrawBackground(window);
                DrawGrid(window);
                DrawTrace(window);

                if (takeScreenshot) { TakeScreenshot(window); }

                DrawUI(window);

                window.Display();

                // Flags

                stepForward = false;
                stepBackward = false;
                takeScreenshot = false;
            }
        }

        static void InitBackground()
        {
            sandSprite = new Sprite();
            sandTexture = new Texture("Assets/Background.png");
            sandSprite.Texture = sandTexture;
            sandColor = new Color((byte)sandR, (byte)sandG, (byte)sandB);
            sandSprite.Color = sandColor;
        }

        static void DrawBackground(RenderWindow window)
        {
            window.Clear(sandColor);
            window.Draw(sandSprite);
        }

        static void InitMusic()
        {
            music = new Music("Assets/Music.wav");
            music.Loop = true;

            if (playMusic)
            {
                music.Play();
            }

        }

        static bool IsMusicPlaying()
        {
            return music.Status == SoundStatus.Playing;
        }

        static void SwitchMusic()
        {
            if (music.Status == SoundStatus.Playing)
            {
                AddInfoMessage(texts[TextId.musicOff], new Vector2f(buttonTurtleSprite.Position.X, buttonBar1Y));
                music.Stop();
            }
            else
            {
                AddInfoMessage(texts[TextId.musicOn], new Vector2f(buttonTurtleSprite.Position.X, buttonBar1Y));
                music.Play();
            }

        }

        static void SwitchGrid()
        {
            showGrid = !showGrid;

            AddInfoMessage(texts[showGrid ? TextId.gridOn : TextId.gridOff], new Vector2f(buttonTurtleSprite.Position.X, buttonBar1Y));

        }

        static void InitGrid()
        {
            gridTexture = new Texture("Assets/Grid.png");
            gridSprite = new Sprite();
            gridSprite.Texture = gridTexture;
            gridSprite.Position = new Vector2f(0, 0);
            gridSprite.Color = new Color((byte)gridR, (byte)gridG, (byte)gridB, (byte)gridOpacity);

        }

        static void DrawGrid(RenderWindow window)
        {
            if (showGrid)
            {
                window.Draw(gridSprite);
            }
        }

        static void TakeScreenshot(RenderWindow window)
        {
            Texture texture = new Texture(window.Size.X, window.Size.Y);
            texture.Update(window);
            Image image = texture.CopyToImage();

            bool done = false;
            int index = 0;
            while (index < 1000 && !done)
            {
                textBuilder.Clear();
                textBuilder.AppendFormat(texts[TextId.screenshotFilename], index);
                string fileName = textBuilder.ToString();

                if (!File.Exists(fileName))
                {
                    AddInfoMessage(texts[TextId.screenshotSaved], new Vector2f(buttonTurtleSprite.Position.X, buttonBar1Y));
                    image.SaveToFile(fileName);
                    done = true;
                }
                else { index++; }


            }

        }

        static void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;

            if(e.Button == Mouse.Button.Left)
            {
                if(buttonPlaySprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    playing = !playing;
                }
                else if(buttonRestartSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    stepIndex = 0;
                }
                else if (buttonForwardSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    stepForward = true;
                    playing = false;
                }
                else if (buttonBackwardsSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    stepBackward = true;
                    playing = false;
                }
                else if (buttonScreenshotSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    takeScreenshot = true;
                }
                else if (buttonTurtleSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    SwitchTurtle();
                }
                else if (buttonGridSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    SwitchGrid();
                }
                else if (buttonMusicSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    SwitchMusic();
                }
                else
                {
                    for(int i = 0; i < playsCount; i++)
                    {
                        if(buttonPlaySprites[i].GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            nextPlayIndex = i;
                        }
                    }
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
                SwitchMusic();
            }
            else if (e.Code == Keyboard.Key.H)
            {
                SwitchTurtle();
            }
            else if (e.Code == Keyboard.Key.I)
            {
                showInfo = !showInfo;
            }
            else if (e.Code == Keyboard.Key.G)
            {
                SwitchGrid();
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
                takeScreenshot = true;
            }
        }
    }
}
