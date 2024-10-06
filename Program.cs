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
            timeBoost = 1;

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


                UpdateTrace(elapsedTime);
                UpdateUI(window, elapsedTime);

                elapsedTime = updateClock.ElapsedTime.AsSeconds() * timeBoost;
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

    }
}
