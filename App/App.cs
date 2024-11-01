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
            Splash,
            SelectMode,
            Play,
            Brush
        };

        // State

        static State state;
        static State nextState;

        // Music

        static Music music;

        // Turtle

        static Turtle turtle;

        // Background

        static Sprite backgroundSprite;
        static Texture backgroundTexture;
        static Color sandColor;

        // Splash

        static float splashTimer;

        // Shared important objects

        static StringBuilder textBuilder;
        static RenderWindow window;

        // Screenshot

        static bool takeScreenshot;

        // Select mode screen

        static bool modeSelected;
        static bool modeSelectedIsPlay;

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
            TracePlayer.Init();

            if(Config.skipSplash)
            {
                state = State.Play;
                nextState = State.Play;
                UI.GotoScreen(UI.ScreenId.PlayMode, true);
            }
            else
            {
                state = State.Splash;
                nextState = State.Splash;
                splashTimer = 0;
                UI.GotoScreen(UI.ScreenId.Splash, true);
            }

            TracePlayer.Init();

            while (window.IsOpen)
            {
                window.DispatchEvents();

                // Update

                if(state != nextState)
                {
                    if(nextState == State.Splash)
                    {
                        splashTimer = 0;
                    }
                    else if(nextState == State.SelectMode)
                    {
                    }
                    else if(nextState == State.Play)
                    {
                        PlayMode.Init();
                    }

                    state = nextState;
                }

                if(state == State.Splash)
                {
                    if(UI.GetCurrentScreen() == UI.ScreenId.Splash && !UI.IsTransitioning())
                    {
                        splashTimer += elapsedTime;
                        if(splashTimer >= AppConfig.splashDuration)
                        {
                            UI.GotoScreen(UI.ScreenId.SelectMode);
                        }
                    }
                    else if(UI.GetCurrentScreen() == UI.ScreenId.SelectMode && !UI.IsTransitioning())
                    {
                        nextState = State.SelectMode;
                    }
                }
                else if(state == State.SelectMode)
                {
                    if(UI.GetCurrentScreen() == UI.ScreenId.SelectMode && !UI.IsTransitioning())
                    {
                        if(modeSelected)
                        {
                            if(modeSelectedIsPlay)
                            {
                                UI.GotoScreen(UI.ScreenId.PlayMode);
                            }
                            else
                            {
                                UI.GotoScreen(UI.ScreenId.BrushMode);
                            }
                        }
                    }
                    else if(UI.GetCurrentScreen() == UI.ScreenId.PlayMode && !UI.IsTransitioning())
                    {
                        nextState = State.Play;
                    }
                    else if(UI.GetCurrentScreen() == UI.ScreenId.BrushMode && !UI.IsTransitioning())
                    {
                        nextState = State.Brush;
                    }
                }
                else if(state == State.Play)
                {
                    PlayMode.Update();

                    UpdateTime();

                    TracePlayer.Update(elapsedTime, timeBoost);
                }

                UI.Update(elapsedTime);

                UpdateElapsedTime();

                // Draw

                DrawBackground(window);

                UI.Draw(window, takeScreenshot);

                window.Display();

                // Flags

                takeScreenshot = false;
                modeSelected = false;
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
            window = new RenderWindow(mode, Config.windowTitle + " v" + AppConfig.appVersion, style);
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

        public static void TakeScreenshot()
        {
            takeScreenshot = true;
        }

        static void InitTurtle()
        {
            turtle = new Turtle();
        }

        public static Turtle GetTurtle()
        {
            return turtle;
        }

        public static void OnPlayModeSelected()
        {
            modeSelected = true;
            modeSelectedIsPlay = true;

        }

        public static void OnBrushModeSelected()
        {
            modeSelected = true;
            modeSelectedIsPlay = false;

        }

        public static void Quit()
        {
            window.Close();
        }


    }
}
