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
        const float splashDuration = 3.0f;
        const float splashFadeDuration = 0.5f;

        const int playsCount = 9;

        const float timeBoostFast = 5.0f;
        const float timeBoostSlow = 0.3f;

        enum AppState
        {
            splashHold,
            splashFade,
            play
        };

        static AppState state;
        static AppState nextState;

        static RenderWindow window;

        static Music music;

        static Sprite backgroundSprite;
        static Texture backgroundTexture;
        static Color sandColor;

        static float splashTimer;
        static float splashFadeTimer;

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


            stepClock = new Clock();
            updateClock = new Clock();
            elapsedTime = 0;
            timeBoost = 1;

            InitWindow();
            InitUI(window);
            InitMusic();
            InitTrace();

            if(skipSplash) { state = AppState.play; nextState = AppState.play; }
            else { state = AppState.splashHold; nextState = AppState.splashHold; splashTimer = 0; }

            InitTrace();

            while (window.IsOpen)
            {
                window.DispatchEvents();

                // Update

                if(state != nextState)
                {
                    state = nextState;
                }

                if(state == AppState.splashHold)
                {
                    splashTimer += elapsedTime;
                    if(splashTimer >= splashDuration)
                    {
                        nextState = AppState.splashFade;
                        splashFadeTimer = 0;
                    }
                }
                else if(state == AppState.splashFade)
                {
                    splashFadeTimer += elapsedTime;
                    if(splashFadeTimer >= splashFadeDuration)
                    {
                        splashFadeTimer = splashFadeDuration;
                        nextState = AppState.play;
                    }
                }
                else if(state == AppState.play)
                {
                    UpdateTrace(elapsedTime);
                    UpdateUI(window, elapsedTime);
                }

                elapsedTime = updateClock.ElapsedTime.AsSeconds() * timeBoost;
                updateClock.Restart();

                // Draw

                DrawBackground(window);

                if(state == AppState.splashHold)
                {
                    DrawSplash(window, 1.0f, false);
                }
                else if(state == AppState.splashFade)
                {
                    DrawSplash(window, 1.0f - splashFadeTimer / splashFadeDuration, false);
                }
                else if(state == AppState.play)
                {
                    if (showGrid) { DrawGrid(window); }
                    DrawTrace(window);
                    if (takeScreenshot) { TakeScreenshot(window); }
                    DrawUI(window);
                }

                window.Display();

                // Flags

                stepForward = false;
                stepBackward = false;
                takeScreenshot = false;
            }
        }

        static void InitBackground()
        {
            backgroundSprite = new Sprite();
            backgroundTexture = new Texture("Assets/Background.png");
            backgroundSprite.Texture = backgroundTexture;
            sandColor = new Color((byte)sandR, (byte)sandG, (byte)sandB);
            backgroundSprite.Color = sandColor;
        }

        static void DrawBackground(RenderWindow window)
        {
            window.Clear(sandColor);
            window.Draw(backgroundSprite);
        }

        static void InitWindow()
        {
            VideoMode mode = new VideoMode((uint)windowWidth, (uint)windowHeight);

            Styles style = Styles.Titlebar | Styles.Close;
            window = new RenderWindow(mode, windowTitle, style);
            Image icon = new Image("Assets/Icon.png");
            window.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);
            window.SetMouseCursorVisible(false);

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
