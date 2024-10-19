using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System.Text;
using System.Numerics;
using System.Diagnostics;

namespace TurtleSandbox
{
    internal partial class App
    {
        // Enums

        public enum State
        {
            splashHold,
            splashFade,
            play
        };

        // State

        static State state;
        static State nextState;

        // Music

        static Music music;

        // Turtle

        static Turtle turtle;
        static bool turtleVisible;

        // Play

        static int playIndex = 0;
        static int nextPlayIndex = 0;

        // Background

        static Sprite backgroundSprite;
        static Texture backgroundTexture;
        static Color sandColor;

        // Splash

        static float splashTimer;
        static float splashFadeTimer;

        // Shared important objects

        static StringBuilder textBuilder;
        static RenderWindow window;

        // Screenshot

        static bool takeScreenshot;

        // Time

        static float timeBoost;
        static float elapsedTime;
        static Clock updateClock;


        static void Main(string[] args)
        {
            InitBackground();

            
            InitTime();
            InitTextBuilder();
            InitWindow();
            Texts.Init();
            UI.Init(window);
            InitMusic();
            InitTurtle();
            InitPlay();
            TracePlayer.Init();

            if(Config.skipSplash) { state = State.play; nextState = State.play; }
            else { state = State.splashHold; nextState = State.splashHold; splashTimer = 0; }

            TracePlayer.Init();

            while (window.IsOpen)
            {
                window.DispatchEvents();

                // Update

                if(state != nextState)
                {
                    state = nextState;
                }

                if(state == State.splashHold)
                {
                    splashTimer += elapsedTime;
                    if(splashTimer >= AppConfig.splashDuration)
                    {
                        nextState = State.splashFade;
                        splashFadeTimer = 0;
                    }
                }
                else if(state == State.splashFade)
                {
                    splashFadeTimer += elapsedTime;
                    if(splashFadeTimer >= AppConfig.splashFadeDuration)
                    {
                        splashFadeTimer = AppConfig.splashFadeDuration;
                        nextState = State.play;
                    }
                }
                else if(state == State.play)
                {
                    UpdatePlay();

                    UpdateTime();

                    TracePlayer.Update(elapsedTime, timeBoost);
                    UI.Update(elapsedTime);
                }

                UpdateElapsedTime();

                // Draw

                DrawBackground(window);

                if(state == State.splashHold)
                {
                    UI.DrawSplash(window, 1.0f, false);
                }
                else if(state == State.splashFade)
                {
                    UI.DrawSplash(window, 1.0f - splashFadeTimer / AppConfig.splashFadeDuration, false);
                }
                else if(state == State.play)
                {
                    if (Config.showGrid && !takeScreenshot) { UI.DrawGrid(window); }
                    TracePlayer.Draw(window);
                    if(turtleVisible) { UI.DrawTurtle(window); }
                    if (takeScreenshot) { TakeScreenshot(window); }
                    UI.Draw(window);
                }

                window.Display();

                // Flags

                takeScreenshot = false;
            }
        }

        public static float GetTimeBoost()
        {
            return timeBoost;
        }

        public static StringBuilder GetTextBuilder()
        {
            return textBuilder;
        }

        public static bool GetTurtleVisible()
        {
            return turtleVisible;
        }

        public static bool GetGridVisible()
        {
            return Config.showGrid;
        }

        public static State GetState()
        {
            return state;
        }


        static void InitTime()
        {
            updateClock = new Clock();
            elapsedTime = 0;
            timeBoost = 1;

        }

        static void UpdateTime()
        {
            TracePlayer.PlayState playState = TracePlayer.GetPlayState();
            if (playState == TracePlayer.PlayState.fastForward ||
                playState == TracePlayer.PlayState.fastBackwards)
            { timeBoost = AppConfig.timeBoostFast; }
            else { timeBoost = 1; }

        }

        static void UpdateElapsedTime()
        {
            elapsedTime = updateClock.ElapsedTime.AsSeconds() * timeBoost;
            updateClock.Restart();

        }

        static void InitBackground()
        {
            backgroundSprite = new Sprite();
            backgroundTexture = new Texture("Assets/Background.png");
            backgroundSprite.Texture = backgroundTexture;
            sandColor = new Color((byte)Config.sandR, (byte)Config.sandG, (byte)Config.sandB);
            backgroundSprite.Color = sandColor;
        }

        static void DrawBackground(RenderWindow window)
        {
            window.Clear(sandColor);
            window.Draw(backgroundSprite);
        }

        static void InitWindow()
        {
            VideoMode mode = new VideoMode((uint)AppConfig.windowWidth, (uint)AppConfig.windowHeight);

            Styles style = Styles.Titlebar | Styles.Close;
            window = new RenderWindow(mode, Config.windowTitle + " " + AppConfig.appVersion, style);
            Image icon = new Image("Assets/Icon.png");
            window.SetIcon(icon.Size.X, icon.Size.Y, icon.Pixels);
            window.SetMouseCursorVisible(false);

        }

        static void InitTextBuilder()
        {
            textBuilder = new StringBuilder("", 200);
        }

        public static RenderWindow GetWindow()
        {
            return window;
        }

        static void InitMusic()
        {
            music = new Music("Assets/Music.wav");
            music.Loop = true;

            if (Config.playMusic)
            {
                music.Play();
            }

        }

        public static bool IsMusicPlaying()
        {
            return music.Status == SoundStatus.Playing;
        }

        public static void SwitchMusic()
        {
            if (music.Status == SoundStatus.Playing)
            {
                UI.AddInfoMessage(Texts.Get(Texts.Id.musicOff), UI.InfoMessagePosition.Toolbar);
                music.Stop();
            }
            else
            {
                UI.AddInfoMessage(Texts.Get(Texts.Id.musicOn), UI.InfoMessagePosition.Toolbar);
                music.Play();
            }

        }

        static void InitPlay()
        {
            playIndex = Config.play - 1;
            nextPlayIndex = Config.play - 1;

        }

        public static int GetPlayIndex()
        {
            return playIndex;
        }

        public static void SetPlayIndex(int index)
        {
            nextPlayIndex = index;
        }

        public static void NextPlayIndex()
        {
            if (nextPlayIndex + 1 >= AppConfig.playsCount) { nextPlayIndex = 0; }
            else { nextPlayIndex++; }

        }

        public static void PreviousPlayIndex()
        {
            if(nextPlayIndex - 1 < 0) { nextPlayIndex = AppConfig.playsCount - 1; }
            else { nextPlayIndex--; }
        }

        public static void TakeScreenshot()
        {
            takeScreenshot = true;
        }

        static void UpdatePlay()
        {
            if (playIndex != nextPlayIndex)
            {
                TracePlayer.SetStep(0);
                playIndex = nextPlayIndex;
                TracePlayer.Play();
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

            List<Turtle.Step> trace = turtle.GetTrace();
            TracePlayer.SetTrace(trace);

        }

        public static void SwitchGrid()
        {
            Config.showGrid = !Config.showGrid;

            UI.AddInfoMessage(Texts.Get(Config.showGrid ? Texts.Id.gridOn : Texts.Id.gridOff), UI.InfoMessagePosition.Toolbar);

        }

        static void InitTurtle()
        {
            turtle = new Turtle();
            turtleVisible = true;

        }

        public static void SwitchTurtle()
        {
            turtleVisible = !turtleVisible;
            UI.AddInfoMessage(Texts.Get(turtleVisible ? Texts.Id.turtleOn : Texts.Id.turtleOff), UI.InfoMessagePosition.Toolbar);
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
                textBuilder.AppendFormat(Texts.Get(Texts.Id.screenshotFilename), index);
                string fileName = textBuilder.ToString();

                if (!File.Exists(fileName))
                {
                    UI.AddInfoMessage(Texts.Get(Texts.Id.screenshotSaved), UI.InfoMessagePosition.Toolbar);
                    image.SaveToFile(fileName);
                    done = true;
                }
                else { index++; }


            }

        }

        public static void Quit()
        {
            window.Close();
        }

    }
}
