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

        const float timeBoostFast = 5.0f;
        const float timeBoostSlow = 0.3f;

        static Music music;


        static Sprite sandSprite;
        static Texture sandTexture;
        static Color sandColor;

        static float holdSplashTimer;
        static bool holdSplash;

        static StringBuilder textBuilder;

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

            InitMusic();

            InitTrace();

            stepClock = new Clock();
            updateClock = new Clock();
            elapsedTime = 0;
            timeBoost = 1;

            VideoMode mode = new VideoMode((uint)windowWidth, (uint)windowHeight);

            Styles style = Styles.Titlebar | Styles.Close;
            RenderWindow window = new RenderWindow(mode, windowTitle, style);
            Image icon = new Image("Assets/Icon.png");            
            window.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);
            window.SetMouseCursorVisible(false);

            window.KeyPressed += OnKeyPressed;
            window.MouseButtonPressed += OnMouseButtonPressed;

            showSplash = true;
            holdSplash = true;
            holdSplashTimer = 0;

            InitTrace();


            while (window.IsOpen)
            {
                window.DispatchEvents();

                // Update

                if(holdSplash)
                {
                    showSplash = true;

                    holdSplashTimer += elapsedTime;
                    if(holdSplashTimer > holdSplashDuration)
                    {
                        holdSplash = false;
                        showSplash = false;
                    }
                }
                

                UpdateTrace(elapsedTime);
                UpdateUI(window, elapsedTime);

                elapsedTime = updateClock.ElapsedTime.AsSeconds() * timeBoost;
                updateClock.Restart();

                // Draw

                DrawBackground(window);


                if(showGrid) { DrawGrid(window); }
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
                AddInfoMessage(texts[TextId.musicOff], new Vector2f(buttonTurtleSprite.Position.X, buttonBar2Y));
                music.Stop();
            }
            else
            {
                AddInfoMessage(texts[TextId.musicOn], new Vector2f(buttonTurtleSprite.Position.X, buttonBar2Y));
                music.Play();
            }

        }

        static void SwitchGrid()
        {
            showGrid = !showGrid;

            AddInfoMessage(texts[showGrid ? TextId.gridOn : TextId.gridOff], new Vector2f(buttonTurtleSprite.Position.X, buttonBar2Y));

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
                    AddInfoMessage(texts[TextId.screenshotSaved], new Vector2f(buttonTurtleSprite.Position.X, buttonBar2Y));
                    image.SaveToFile(fileName);
                    done = true;
                }
                else { index++; }


            }

        }

    }
}
