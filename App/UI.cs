using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System.Text;
using System.Numerics;
using static TurtleSandbox.TracePlayer;


namespace TurtleSandbox
{
    internal partial class UI
    {
        // Constants

        const float playTextX = 80;
        const float playTextY = 669;
        const float playTextScale = 1.0f;

        const float splashX = 530;
        const float splashY = 255;
        const float splashCloseOffsetX = 264;
        const float splashCloseOffsetY = 8;

        const int infoMessagesCount = 50;
        const float infoMessageDuration = 2.0f;
        const float infoMessageDistance = 150;
        const float infoMessageOffset = 35;
        const float infoMessageScale = 0.8f;

        const float statusBarX = 280;
        const float statusBarY = 670;
        const float statusAngleX = 325;
        const float statusAngleY = 669;
        const float statusPosXX = 428;
        const float statusPosXY = 669;
        const float statusPosYX = 526;
        const float statusPosYY = 669;

        const float statusTextScale = 1.0f;

        const float gridCoordinatesTextScale = 0.5f;
        const float gridCoordinatesTextOffsetX = 4.0f;
        const float gridCoordinatesTextOffsetY = 4.0f;
        const float gridSeparation = 50.0f;
        const float gridCoordinatesCursorOffsetX = 23;
        const float gridCoordinatesCursorOffsetY = -10;

        const float buttonBar1Scale = 1.0f;
        const float buttonBar1X = 25;
        const float buttonBar1Y = 666;
        const float buttonBarSeparation1 = 6;

        const float buttonBar2Scale = 1.0f;
        const float buttonBar2X = 650;
        const float buttonBar2Y = 666;
        const float buttonBarSeparation2 = 2;

        // Enums

        public enum InfoMessagePosition
        {
            Turtle,
            Toolbar
        };


        // Turtle

        static Sprite turtleSprite;
        static Texture turtleTexture;

        // Font

        static Font font;

        // Play

        static Text playText;

        // Cursor

        static Sprite cursorSprite;
        static Texture cursorTexture;

        // Splash

        static Sprite splashSprite;
        static Texture splashTexture;

        static Sprite splashCloseButtonSprite;
        static Texture splashCloseButtonTexture;

        static bool showSplash;

        // Grid

        static Text gridText;
        static Sprite gridLineSprite;
        static Texture gridLineTexture;

        // Messages

        static Text[] infoMessages;
        static bool[] infoMessagesFree;
        static float[] infoMessagesLifetime;
        static Vector2f[] infoMessagesPosition;
        static Dictionary<Turtle.OrderId, string> orderIdToString;

        // Button bar 1

        static Sprite buttonPreviousPlaySprite;
        static Sprite buttonNextPlaySprite;

        static Texture buttonPreviousPlayTexture;
        static Texture buttonNextPlayTexture;

        static Text textCurrentPlay;

        // Status bar

        static Text statusAngleText;
        static Text statusPosXText;
        static Text statusPosYText;
        static int statusPosX;
        static int statusPosY;
        static int statusAngle;

        static Sprite statusBarSprite;
        static Texture statusBarTexture;

        // Button bar 2

        static Sprite buttonPlaySprite;
        static Sprite buttonPauseSprite;
        static Sprite buttonForwardSprite;
        static Sprite buttonBackwardsSprite;
        static Sprite buttonRestartSprite;
        static Sprite buttonFastForwardSprite;
        static Sprite buttonFastBackwardsSprite;
        static Sprite buttonScreenshotSprite;
        static Sprite buttonMusicSprite;
        static Sprite buttonTurtleSprite;
        static Sprite buttonGridSprite;
        static Sprite buttonSplashSprite;

        static Texture buttonPlayTexture;
        static Texture buttonPauseTexture;
        static Texture buttonForwardTexture;
        static Texture buttonBackwardsTexture;
        static Texture buttonRestartTexture;
        static Texture buttonFastForwardTexture;
        static Texture buttonFastBackwardsTexture;
        static Texture buttonScreenshotTexture;
        static Texture buttonMusicOnTexture;
        static Texture buttonMusicOffTexture;
        static Texture buttonTurtleOnTexture;
        static Texture buttonTurtleOffTexture;
        static Texture buttonGridOnTexture;
        static Texture buttonGridOffTexture;
        static Texture buttonSplashOnTexture;
        static Texture buttonSplashOffTexture;

        // Flags

        static bool stopOnFastForwardOrBackwardsRelease;
        static bool playOnFastForwardOrBackwardsRelease;

        public static void Init(RenderWindow window)
        {
            // Register callbacks

            window.KeyPressed += OnKeyPressed;
            window.MouseButtonPressed += OnMouseButtonPressed;
            window.Closed += OnWindowClosed;

            // Init texts

            font = new Font("Assets/Font.ttf");

            playText = new Text();
            playText.Position = new Vector2f(playTextX, playTextY);
            playText.FillColor = new Color((byte)Config.toolbarR, (byte)Config.toolbarG, (byte)Config.toolbarB);
            playText.Scale = new Vector2f(playTextScale, playTextScale);
            playText.Font = font;
            playText.DisplayedString = "";

            statusPosX = 0;
            statusPosY = 0;
            statusAngle = 0;

            StringBuilder textBuilder = App.GetTextBuilder();

            statusAngleText = new Text();
            statusAngleText.Position = new Vector2f(statusAngleX, statusAngleY);
            statusAngleText.FillColor = new Color((byte)Config.toolbarR, (byte)Config.toolbarG, (byte)Config.toolbarB);
            statusAngleText.Scale = new Vector2f(statusTextScale, statusTextScale);
            statusAngleText.Font = font;
            textBuilder.Clear();
            statusAngleText.DisplayedString = textBuilder.ToString();

            statusPosXText = new Text();
            statusPosXText.Position = new Vector2f(statusPosXX, statusPosXY);
            statusPosXText.FillColor = new Color((byte)Config.toolbarR, (byte)Config.toolbarG, (byte)Config.toolbarB);
            statusPosXText.Scale = new Vector2f(statusTextScale, statusTextScale);
            statusPosXText.Font = font;
            textBuilder.Clear();
            statusPosXText.DisplayedString = textBuilder.ToString();

            statusPosYText = new Text();
            statusPosYText.Position = new Vector2f(statusPosYX, statusPosYY);
            statusPosYText.FillColor = new Color((byte)Config.toolbarR, (byte)Config.toolbarG, (byte)Config.toolbarB);
            statusPosYText.Scale = new Vector2f(statusTextScale, statusTextScale);
            statusPosYText.Font = font;
            textBuilder.Clear();
            statusPosYText.DisplayedString = textBuilder.ToString();

            gridText = new Text();
            gridText.Font = font;
            gridText.FillColor = new Color((byte)Config.gridR, (byte)Config.gridG, (byte)Config.gridB, (byte)Config.gridOpacity);

            turtleSprite = new Sprite();
            turtleTexture = new Texture("Assets/Turtle.png");
            turtleSprite.Texture = turtleTexture;
            turtleSprite.Origin = new Vector2f(50, 50);
            turtleSprite.Scale = new Vector2f(0.5f, 0.5f);

            orderIdToString = new Dictionary<Turtle.OrderId, string>();
            orderIdToString[Turtle.OrderId.origin] = "origin";
            orderIdToString[Turtle.OrderId.walk] = "walk";
            orderIdToString[Turtle.OrderId.turn] = "turn";
            orderIdToString[Turtle.OrderId.randTurn] = "randTurn";
            orderIdToString[Turtle.OrderId.randWalk] = "randWalk";
            orderIdToString[Turtle.OrderId.teleport] = "teleport";
            orderIdToString[Turtle.OrderId.remember] = "remember";
            orderIdToString[Turtle.OrderId.recall] = "recall";

            // Init cursor

            cursorTexture = new Texture("Assets/Cursor.png");
            cursorSprite = new Sprite();
            cursorSprite.Texture = cursorTexture;
            cursorSprite.Color = new Color((byte)Config.toolbarR, (byte)Config.toolbarG, (byte)Config.toolbarB);

            // Init splash

            splashTexture = new Texture("Assets/Splash/Splash.png");
            splashSprite = new Sprite();
            splashSprite.Texture = splashTexture;
            splashCloseButtonTexture = new Texture("Assets/Splash/SplashClose.png");
            splashCloseButtonSprite = new Sprite();
            splashCloseButtonSprite.Texture = splashCloseButtonTexture;

            // Init grid

            gridLineTexture = new Texture("Assets/Line.png");
            gridLineSprite = new Sprite();
            gridLineSprite.Texture = gridLineTexture;
            gridLineSprite.Origin = new Vector2f(gridLineTexture.Size.X / 2, gridLineTexture.Size.Y);
            gridLineSprite.Color = new Color((byte)Config.gridR, (byte)Config.gridG, (byte)Config.gridB, (byte)Config.gridOpacity);

            // Init button bar 1

            buttonNextPlayTexture = new Texture("Assets/Buttons/Right.png");
            buttonPreviousPlayTexture = new Texture("Assets/Buttons/Left.png");

            buttonNextPlaySprite = new Sprite();
            buttonNextPlaySprite.Texture = buttonNextPlayTexture;
            buttonPreviousPlaySprite = new Sprite();
            buttonPreviousPlaySprite.Texture = buttonPreviousPlayTexture;

            // Init status bar

            statusBarTexture = new Texture("Assets/StatusBar.png");

            statusBarSprite = new Sprite();
            statusBarSprite.Texture = statusBarTexture;

            // Init button bar 2


            buttonPlayTexture = new Texture("Assets/Buttons/Play.png");
            buttonPauseTexture = new Texture("Assets/Buttons/Pause.png");
            buttonForwardTexture = new Texture("Assets/Buttons/Forward.png");
            buttonBackwardsTexture = new Texture("Assets/Buttons/Backward.png");
            buttonRestartTexture = new Texture("Assets/Buttons/Restart.png");
            buttonFastForwardTexture = new Texture("Assets/Buttons/FastForward.png");
            buttonFastBackwardsTexture = new Texture("Assets/Buttons/FastBackward.png");
            buttonScreenshotTexture = new Texture("Assets/Buttons/Screenshot.png");
            buttonMusicOnTexture = new Texture("Assets/Buttons/MusicOn.png");
            buttonMusicOffTexture = new Texture("Assets/Buttons/MusicOff.png");
            buttonTurtleOnTexture = new Texture("Assets/Buttons/TurtleOn.png");
            buttonTurtleOffTexture = new Texture("Assets/Buttons/TurtleOff.png");
            buttonGridOnTexture = new Texture("Assets/Buttons/GridOn.png");
            buttonGridOffTexture = new Texture("Assets/Buttons/GridOff.png");
            buttonSplashOnTexture = new Texture("Assets/Buttons/SplashOn.png");
            buttonSplashOffTexture = new Texture("Assets/Buttons/SplashOff.png");

            buttonPlaySprite = new Sprite();
            buttonPlaySprite.Texture = buttonPlayTexture;
            buttonPauseSprite = new Sprite();
            buttonPauseSprite.Texture = buttonPauseTexture;
            buttonForwardSprite = new Sprite();
            buttonForwardSprite.Texture = buttonForwardTexture;
            buttonBackwardsSprite = new Sprite();
            buttonBackwardsSprite.Texture = buttonBackwardsTexture;
            buttonRestartSprite = new Sprite();
            buttonRestartSprite.Texture = buttonRestartTexture;
            buttonFastForwardSprite = new Sprite();
            buttonFastForwardSprite.Texture = buttonFastForwardTexture;
            buttonFastBackwardsSprite = new Sprite();
            buttonFastBackwardsSprite.Texture = buttonFastBackwardsTexture;
            buttonScreenshotSprite = new Sprite();
            buttonScreenshotSprite.Texture = buttonScreenshotTexture;
            buttonMusicSprite = new Sprite();
            buttonMusicSprite.Texture = buttonMusicOffTexture;
            buttonTurtleSprite = new Sprite();
            buttonTurtleSprite.Texture = buttonTurtleOffTexture;
            buttonGridSprite = new Sprite();
            buttonGridSprite.Texture = buttonGridOffTexture;
            buttonSplashSprite = new Sprite();
            buttonSplashSprite.Texture = buttonSplashOffTexture;

            ////////////////////////// Set elements position and size ////////////////////////////////

            // Splash

            splashSprite.Position = new Vector2f(splashX, splashY);
            splashCloseButtonSprite.Position = new Vector2f(splashX + splashCloseOffsetX, splashY + splashCloseOffsetY);

            float buttonWidth = buttonPlayTexture.Size.X;

            // Button bar 1

            buttonPreviousPlaySprite.Position   = new Vector2f(buttonBar1X + 0 * buttonWidth + 0 * buttonBarSeparation1, buttonBar1Y);
            buttonNextPlaySprite.Position       = new Vector2f(buttonBar1X + 3 * buttonWidth + 3 * buttonBarSeparation1, buttonBar1Y);

            Vector2f bar1Scale = new Vector2f(buttonBar1Scale, buttonBar1Scale);
            
            buttonPreviousPlaySprite.Scale = bar1Scale;
            buttonNextPlaySprite.Scale = bar1Scale;

            // Status bar

            statusBarSprite.Position = new Vector2f(statusBarX, statusBarY);

            // Button bar 2

            buttonRestartSprite.Position        = new Vector2f(buttonBar2X + 0 * buttonWidth + 0 * buttonBarSeparation2, buttonBar2Y);
            buttonFastBackwardsSprite.Position  = new Vector2f(buttonBar2X + 1 * buttonWidth + 1 * buttonBarSeparation2, buttonBar2Y);
            buttonBackwardsSprite.Position      = new Vector2f(buttonBar2X + 2 * buttonWidth + 2 * buttonBarSeparation2, buttonBar2Y);
            buttonPlaySprite.Position           = new Vector2f(buttonBar2X + 3 * buttonWidth + 3 * buttonBarSeparation2, buttonBar2Y);
            buttonPauseSprite.Position          = new Vector2f(buttonBar2X + 3 * buttonWidth + 3 * buttonBarSeparation2, buttonBar2Y);
            buttonForwardSprite.Position        = new Vector2f(buttonBar2X + 4 * buttonWidth + 4 * buttonBarSeparation2, buttonBar2Y);
            buttonFastForwardSprite.Position    = new Vector2f(buttonBar2X + 5 * buttonWidth + 5 * buttonBarSeparation2, buttonBar2Y);
            buttonTurtleSprite.Position         = new Vector2f(buttonBar2X + 7 * buttonWidth + 7 * buttonBarSeparation2, buttonBar2Y);
            buttonGridSprite.Position           = new Vector2f(buttonBar2X + 8 * buttonWidth + 8 * buttonBarSeparation2, buttonBar2Y);
            buttonMusicSprite.Position          = new Vector2f(buttonBar2X + 9 * buttonWidth + 9 * buttonBarSeparation2, buttonBar2Y);
            buttonScreenshotSprite.Position     = new Vector2f(buttonBar2X + 10 * buttonWidth + 10 * buttonBarSeparation2, buttonBar2Y);
            buttonSplashSprite.Position         = new Vector2f(buttonBar2X + 11 * buttonWidth + 11 * buttonBarSeparation2, buttonBar2Y);

            Vector2f bar2Scale = new Vector2f(buttonBar2Scale, buttonBar2Scale);
            buttonRestartSprite.Scale = bar2Scale;
            buttonFastBackwardsSprite.Scale = bar2Scale;
            buttonBackwardsSprite.Scale = bar2Scale;
            buttonPlaySprite.Scale = bar2Scale;
            buttonPauseSprite.Scale = bar2Scale;
            buttonForwardSprite.Scale = bar2Scale;
            buttonFastForwardSprite.Scale = bar2Scale;
            buttonTurtleSprite.Scale = bar2Scale;
            buttonGridSprite.Scale = bar2Scale;
            buttonScreenshotSprite.Scale = bar2Scale;


            // Init info messages

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

        public static void SetStatus(int posX, int posY, int angle)
        {
            statusPosX = posX;
            statusPosY = posY;
            statusAngle = angle;
        }

        public static void AddInfoMessage(string message, InfoMessagePosition position)
        {
            Vector2f p;

            if(position == InfoMessagePosition.Toolbar)
            {
                p = new Vector2f(buttonTurtleSprite.Position.X, buttonBar2Y);
            }
            else // position = InfoMessagePosition.Turtle
            {
                RenderWindow w = App.GetWindow();
                p = UI.TurtlePositionToScreen(statusPosX, statusPosY, w);
            }

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
            infoMessages[messageIndex].DisplayedString = message;
            infoMessagesPosition[messageIndex] = p;
            infoMessagesLifetime[messageIndex] = 0;
            infoMessagesFree[messageIndex] = false;

        }

        public static void Update(float elapsedTime)
        {
            RenderWindow window = App.GetWindow();

            for (int i = 0; i < infoMessages.Length; i++)
            {
                if (!infoMessagesFree[i])
                {
                    infoMessagesLifetime[i] += elapsedTime;
                    if (infoMessagesLifetime[i] >= infoMessageDuration) { infoMessagesFree[i] = true; }
                }
            }

            // Check input

            bool fastForwardKey = Keyboard.IsKeyPressed(Keyboard.Key.RShift);
            bool fastBackwardsKey = Keyboard.IsKeyPressed(Keyboard.Key.LShift);
            bool fastForwardMouse = false;
            bool fastBackwardsMouse = false;

            if (Mouse.IsButtonPressed(Mouse.Button.Left))
            {
                Vector2i p = Mouse.GetPosition(window);
                fastForwardMouse = buttonFastForwardSprite.GetGlobalBounds().Contains(p.X, p.Y);
                fastBackwardsMouse = buttonFastBackwardsSprite.GetGlobalBounds().Contains(p.X, p.Y);
            }

            bool fastForward = (fastForwardKey || fastForwardMouse);
            bool fastBackwards = (fastBackwardsKey || fastBackwardsMouse);

            TracePlayer.PlayState playState = TracePlayer.GetPlayState();

            if (fastForward || fastBackwards)
            {
                if(playState == TracePlayer.PlayState.playing) { playOnFastForwardOrBackwardsRelease = true; }
                else if(playState == TracePlayer.PlayState.stopped) { stopOnFastForwardOrBackwardsRelease = true; }

                if(fastForward) { TracePlayer.FastForward(); }
                else { TracePlayer.FastBackwards();  }
            }
            else
            {
                if(stopOnFastForwardOrBackwardsRelease) { TracePlayer.Stop(); }
                else if(playOnFastForwardOrBackwardsRelease) { TracePlayer.Play(); }

                playOnFastForwardOrBackwardsRelease = false;
                stopOnFastForwardOrBackwardsRelease = false;
                
            }
        }

        public static void Draw(RenderWindow window)
        {
            if(!Config.showToolbar) { return; }

            // Draw texts

            StringBuilder textBuilder = App.GetTextBuilder();

            textBuilder.Clear();
            textBuilder.AppendFormat(Texts.Get(Texts.Id.play), App.GetPlayIndex() + 1);
            playText.DisplayedString = textBuilder.ToString();

            textBuilder.Clear();
            textBuilder.AppendFormat(Texts.Get(Texts.Id.statusAngle), statusAngle);
            statusAngleText.DisplayedString = textBuilder.ToString();

            textBuilder.Clear();
            textBuilder.AppendFormat(Texts.Get(Texts.Id.statusPosX), statusPosX);
            statusPosXText.DisplayedString = textBuilder.ToString();

            textBuilder.Clear();
            textBuilder.AppendFormat(Texts.Get(Texts.Id.statusPosY), statusPosY);
            statusPosYText.DisplayedString = textBuilder.ToString();

            window.Draw(playText);
            window.Draw(statusAngleText);
            window.Draw(statusPosXText);
            window.Draw(statusPosYText);

            // Draw bars

            // Button bar 1

            window.Draw(buttonPreviousPlaySprite);
            window.Draw(buttonNextPlaySprite);

            // Status bar

            window.Draw(statusBarSprite);

            // Button bar 2

            window.Draw(buttonRestartSprite);
            window.Draw(buttonFastBackwardsSprite);

            TracePlayer.PlayState playState = TracePlayer.GetPlayState();

            bool isPlayingState = (playState == PlayState.playing || playState == PlayState.fastBackwards || playState == PlayState.fastForward);

            window.Draw(buttonRestartSprite);
            window.Draw(buttonFastBackwardsSprite);
            window.Draw(buttonBackwardsSprite);
            window.Draw(isPlayingState ? buttonPlaySprite : buttonPauseSprite);
            window.Draw(buttonForwardSprite);
            window.Draw(buttonFastForwardSprite);

            buttonTurtleSprite.Texture = (App.GetTurtleVisible() ? buttonTurtleOnTexture: buttonTurtleOffTexture);
            window.Draw(buttonTurtleSprite);

            buttonGridSprite.Texture = (Config.showGrid ? buttonGridOnTexture : buttonGridOffTexture);
            window.Draw(buttonGridSprite);
            window.Draw(buttonScreenshotSprite);
            buttonSplashSprite.Texture = (showSplash ? buttonSplashOnTexture : buttonSplashOffTexture);
            window.Draw(buttonSplashSprite);

            buttonMusicSprite.Texture = (App.IsMusicPlaying() ? buttonMusicOnTexture : buttonMusicOffTexture);
            window.Draw(buttonMusicSprite);

            // Draw info messages

            for (int i = 0; i < infoMessages.Length; i++)
            {
                if (!infoMessagesFree[i])
                {
                    float factor = infoMessagesLifetime[i] / infoMessageDuration;
                    float opacityFactor = MathF.Pow(1 - factor, 3);

                    Vector2f position = infoMessagesPosition[i];
                    infoMessages[i].Position = position + new Vector2f(0, -infoMessageOffset - infoMessageDistance * factor);
                    infoMessages[i].FillColor = new Color((byte)Config.toolbarR, (byte)Config.toolbarG, (byte)Config.toolbarB, (byte)(255 * opacityFactor));
                    window.Draw(infoMessages[i]);

                }
            }

            // Draw splash

            if(showSplash)
            {
                DrawSplash(window);
            }

            // Draw cursor

            Vector2f wp = (Vector2f)Mouse.GetPosition(window);
            cursorSprite.Position = wp;
            window.Draw(cursorSprite);

            if(Config.showGrid)
            {
                textBuilder.Clear();
                textBuilder.AppendFormat(Texts.Get(Texts.Id.gridCursorCoordinates), wp.X - window.Size.X / 2, -(wp.Y - window.Size.Y / 2));
                gridText.DisplayedString = textBuilder.ToString();
                gridText.Position = wp + new Vector2f(gridCoordinatesCursorOffsetX, gridCoordinatesCursorOffsetY);
                gridText.FillColor = new Color((byte)Config.toolbarR, (byte)Config.toolbarG, (byte)Config.toolbarB); 
                window.Draw(gridText);
            }
        }

        public static void DrawGrid(RenderWindow window)
        {
            float width = window.Size.X;
            float height = window.Size.Y;

            // Draw lines

            Vector2f center = new Vector2f(width / 2, height / 2);
            int XLines = (int)(width / 2.0f / gridSeparation);
            int YLines = (int)(height / 2.0f / gridSeparation);
            gridLineSprite.Rotation = 0;
            for(int i = -XLines; i <= XLines; i++)
            {
                gridLineSprite.Scale = new Vector2f((i != 0 ? 2.0f : 6.0f) / gridLineTexture.Size.X, height / gridLineTexture.Size.Y);
                gridLineSprite.Position = new Vector2f(center.X + i * gridSeparation, height); ;
                window.Draw(gridLineSprite);
            }

            gridLineSprite.Rotation = 90;
            for (int i = -YLines; i <= YLines; i++)
            {
                gridLineSprite.Scale = new Vector2f((i != 0 ? 2.0f : 6.0f) / gridLineTexture.Size.X, width / gridLineTexture.Size.Y);
                gridLineSprite.Position = new Vector2f(0, center.Y + i * gridSeparation);
                window.Draw(gridLineSprite);
            }

            // Draw coordinatas

            gridText.Scale = new Vector2f(gridCoordinatesTextScale, gridCoordinatesTextScale);

            float gridTextBaseX = MathF.Max(width / 2 - XLines * gridSeparation, 0);
            float gridTextBaseY = MathF.Max(height / 2 - YLines * gridSeparation, 0);

            StringBuilder textBuilder = App.GetTextBuilder();

            for (int i = -XLines; i <= XLines; i++)
            {
                textBuilder.Clear();
                textBuilder.AppendFormat(Texts.Get(Texts.Id.gridCoordinates), (int)(-i * gridSeparation));
                gridText.DisplayedString = textBuilder.ToString();
                gridText.Position = new Vector2f(width / 2 - i * gridSeparation + gridCoordinatesTextOffsetX, gridTextBaseY + gridCoordinatesTextOffsetY);
                window.Draw(gridText);
            }

            for (int i = -YLines; i <= YLines; i++)
            {
                textBuilder.Clear();
                textBuilder.AppendFormat(Texts.Get(Texts.Id.gridCoordinates), (int)(i * gridSeparation));
                gridText.DisplayedString = textBuilder.ToString();
                gridText.Position = new Vector2f(gridCoordinatesTextOffsetX, height / 2 - i * gridSeparation + gridCoordinatesTextOffsetY);
                window.Draw(gridText);
            }

        }

        public static void DrawTurtle(RenderWindow window)
        {
            turtleSprite.Position = UI.TurtlePositionToScreen(statusPosX, statusPosY, window);
            turtleSprite.Rotation = UI.TurtleAngleToScreenRotation(statusAngle);
            window.Draw(turtleSprite);

        }

        public static void DrawSplash(RenderWindow window, float opacity = 1.0f, bool withCloseButton = true)
        {
            splashSprite.Color = new Color(255, 255, 255, (byte)(255 * opacity));
            window.Draw(splashSprite);
            if(withCloseButton) { window.Draw(splashCloseButtonSprite); }
        }

        public static string FormatOrderInfo(Turtle.Order order)
        {
            StringBuilder textBuilder = App.GetTextBuilder();

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

        public static Vector2f TurtlePositionToScreen(float x, float y, RenderWindow window)
        {
            return new Vector2f(window.Size.X / 2, window.Size.Y / 2) + new Vector2f(x, -y) * AppConfig.pixelsPerStep;
        }

        public static float TurtleAngleToScreenRotation(float a)
        {
            return -a - 90;
        }

        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;

            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }

            if (App.GetState() != App.State.play) { return; }

            if (e.Code == Keyboard.Key.Space)
            {
                TracePlayer.SetStep(0);
                TracePlayer.Play();
            }
            else if (e.Code == Keyboard.Key.Right)
            {
                TracePlayer.StepForward();
                TracePlayer.Stop();
            }
            else if (e.Code == Keyboard.Key.Left)
            {
                TracePlayer.StepBackward();
                TracePlayer.Stop();
            }
            else if (e.Code == Keyboard.Key.Enter)
            {
                PlayState playState = TracePlayer.GetPlayState();
                if (playState == PlayState.playing) { TracePlayer.Stop(); }
                else if (playState == PlayState.stopped) { TracePlayer.Play(); }
            }
            else if (e.Code == Keyboard.Key.M)
            {
                App.SwitchMusic();
            }
            else if (e.Code == Keyboard.Key.H)
            {
                App.SwitchTurtle();
            }
            else if (e.Code == Keyboard.Key.I || e.Code == Keyboard.Key.Tab)
            {
                Config.showToolbar = !Config.showToolbar;
            }
            else if (e.Code == Keyboard.Key.G)
            {
                App.SwitchGrid();
            }
            else if (e.Code == Keyboard.Key.Num1)
            {
                App.SetPlayIndex(0);
            }
            else if (e.Code == Keyboard.Key.Num2)
            {
                App.SetPlayIndex(1);
            }
            else if (e.Code == Keyboard.Key.Num3)
            {
                App.SetPlayIndex(2);
            }
            else if (e.Code == Keyboard.Key.Num4)
            {
                App.SetPlayIndex(3);
            }
            else if (e.Code == Keyboard.Key.Num5)
            {
                App.SetPlayIndex(4);
            }
            else if (e.Code == Keyboard.Key.Num6)
            {
                App.SetPlayIndex(5);
            }
            else if (e.Code == Keyboard.Key.Num7)
            {
                App.SetPlayIndex(6);
            }
            else if (e.Code == Keyboard.Key.Num8)
            {
                App.SetPlayIndex(7);
            }
            else if (e.Code == Keyboard.Key.Num9)
            {
                App.SetPlayIndex(8);
            }
            else if (e.Code == Keyboard.Key.C)
            {
                App.TakeScreenshot();
            }
            else if(e.Code == Keyboard.Key.A)
            {
                showSplash = !showSplash;
            }
        }

        static void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;

            App.State state = App.GetState();

            if(state != App.State.play) { return; }

            if (e.Button == Mouse.Button.Left)
            {
                TracePlayer.PlayState playState = TracePlayer.GetPlayState(); 

                if (buttonPlaySprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    if(playState == TracePlayer.PlayState.playing) { TracePlayer.Stop(); }
                    else if(playState == TracePlayer.PlayState.stopped) { TracePlayer.Play(); }
                }
                else if (buttonRestartSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    TracePlayer.SetStep(0);
                    TracePlayer.Play();

                }
                else if (buttonForwardSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    TracePlayer.StepForward();
                    TracePlayer.Stop();
                }
                else if (buttonBackwardsSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    TracePlayer.StepBackward();
                    TracePlayer.Stop();

                }
                else if (buttonScreenshotSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    App.TakeScreenshot();
                }
                else if (buttonTurtleSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    App.SwitchTurtle();
                }
                else if (buttonGridSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    App.SwitchGrid();
                }
                else if (buttonMusicSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    App.SwitchMusic();
                }
                else if(buttonSplashSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    showSplash = !showSplash;
                }
                else if(splashCloseButtonSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    showSplash = false;
                }
                else if (buttonNextPlaySprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    App.NextPlayIndex();
                }
                else if (buttonPreviousPlaySprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    App.PreviousPlayIndex();
                }

            }
        }

        static void OnWindowClosed(object sender, EventArgs e)
        {
            App.Quit();
        }

    }
}
