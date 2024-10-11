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

        // Texts

        enum TextId
        {
            screenshotSaved,
            musicOn,
            musicOff,
            screenshotFilename,
            play,
            statusAngle,
            statusPosX,
            statusPosY,
            turtleOn,
            turtleOff,
            gridOn,
            gridOff,
            gridCoordinates,
            gridCursorCoordinates

        };
        
        static Dictionary<TextId, string> texts;

        static Font font;
        static Text playText;
        static Text statusAngleText;
        static Text statusPosXText;
        static Text statusPosYText;
        static int statusPosX;
        static int statusPosY;
        static int statusAngle;
        static Text gridText;

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

        static bool stopOnFastForwardOrBackwardsRelease;
        static bool playOnFastForwardOrBackwardsRelease;

        static void AddInfoMessage(string message, Vector2f position)
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
            infoMessages[messageIndex].DisplayedString = message;
            infoMessagesPosition[messageIndex] = position;
            infoMessagesLifetime[messageIndex] = 0;
            infoMessagesFree[messageIndex] = false;

        }

        static void InitUI(RenderWindow window)
        {
            // Register callbacks

            window.KeyPressed += OnKeyPressed;
            window.MouseButtonPressed += OnMouseButtonPressed;
            window.Closed += OnWindowClosed;

            // Init texts

            texts = new Dictionary<TextId, string>();
            texts[TextId.screenshotSaved] = "Screenshot saved";
            texts[TextId.musicOn] = "Music on";
            texts[TextId.musicOff] = "Music off";
            texts[TextId.screenshotFilename] = "screenshot{0:000}.png";
            texts[TextId.play] = "Play {0,1}";
            texts[TextId.statusAngle] = "{0,3}";
            texts[TextId.statusPosX] = "{0,3}";
            texts[TextId.statusPosY] = "{0,3}";
            texts[TextId.gridOn] = "Grid on";
            texts[TextId.gridOff] = "Grid off";
            texts[TextId.turtleOn] = "Turtle visible";
            texts[TextId.turtleOff] = "Turtle hidden";
            texts[TextId.gridCoordinates] = "{0}";
            texts[TextId.gridCursorCoordinates] = "({0,3},{1,3})";

            // Init texts

            font = new Font("Assets/Font.ttf");

            playText = new Text();
            playText.Position = new Vector2f(playTextX, playTextY);
            playText.FillColor = new Color((byte)toolbarR, (byte)toolbarG, (byte)toolbarB);
            playText.Scale = new Vector2f(playTextScale, playTextScale);
            playText.Font = font;
            textBuilder.Clear();
            playText.DisplayedString = textBuilder.ToString();

            statusPosX = 0;
            statusPosY = 0;
            statusAngle = 0;

            statusAngleText = new Text();
            statusAngleText.Position = new Vector2f(statusAngleX, statusAngleY);
            statusAngleText.FillColor = new Color((byte)toolbarR, (byte)toolbarG, (byte)toolbarB);
            statusAngleText.Scale = new Vector2f(statusTextScale, statusTextScale);
            statusAngleText.Font = font;
            textBuilder.Clear();
            statusAngleText.DisplayedString = textBuilder.ToString();

            statusPosXText = new Text();
            statusPosXText.Position = new Vector2f(statusPosXX, statusPosXY);
            statusPosXText.FillColor = new Color((byte)toolbarR, (byte)toolbarG, (byte)toolbarB);
            statusPosXText.Scale = new Vector2f(statusTextScale, statusTextScale);
            statusPosXText.Font = font;
            textBuilder.Clear();
            statusPosXText.DisplayedString = textBuilder.ToString();

            statusPosYText = new Text();
            statusPosYText.Position = new Vector2f(statusPosYX, statusPosYY);
            statusPosYText.FillColor = new Color((byte)toolbarR, (byte)toolbarG, (byte)toolbarB);
            statusPosYText.Scale = new Vector2f(statusTextScale, statusTextScale);
            statusPosYText.Font = font;
            textBuilder.Clear();
            statusPosYText.DisplayedString = textBuilder.ToString();

            gridText = new Text();
            gridText.Font = font;
            gridText.FillColor = new Color((byte)gridR, (byte)gridG, (byte)gridB, (byte)gridOpacity);

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
            cursorSprite.Color = new Color((byte)toolbarR, (byte)toolbarG, (byte)toolbarB);

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
            gridLineSprite.Color = new Color((byte)gridR, (byte)gridG, (byte)gridB, (byte)gridOpacity);

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
            buttonMusicSprite.Texture = playMusic ? buttonMusicOnTexture : buttonMusicOffTexture;
            buttonTurtleSprite = new Sprite();
            buttonTurtleSprite.Texture = turtleVisible ? buttonTurtleOnTexture: buttonTurtleOffTexture;
            buttonGridSprite = new Sprite();
            buttonGridSprite.Texture = showGrid ? buttonGridOnTexture : buttonGridOffTexture;
            buttonSplashSprite = new Sprite();
            buttonSplashSprite.Texture = showSplash ? buttonSplashOnTexture : buttonSplashOffTexture;

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

        private static void Window_Closed(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        static void UpdateUI(RenderWindow window, float elapsedTime)
        {
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

            if (fastForward || fastBackwards)
            {
                if(playState == PlayState.playing) { playOnFastForwardOrBackwardsRelease = true; }
                else if(playState == PlayState.stopped) { stopOnFastForwardOrBackwardsRelease = true; }

                playState = fastForward ? PlayState.fastForward : PlayState.fastBackwards;
            }
            else
            {
                if(stopOnFastForwardOrBackwardsRelease) { playState = PlayState.stopped; }
                else if(playOnFastForwardOrBackwardsRelease) { playState = PlayState.playing; }

                playOnFastForwardOrBackwardsRelease = false;
                stopOnFastForwardOrBackwardsRelease = false;
                
            }
        }

        static void DrawUI(RenderWindow window)
        {
            if(!showToolbar) { return; }

            // Draw texts

            textBuilder.Clear();
            textBuilder.AppendFormat(texts[TextId.play], playIndex + 1);
            playText.DisplayedString = textBuilder.ToString();

            textBuilder.Clear();
            textBuilder.AppendFormat(texts[TextId.statusAngle], statusAngle);
            statusAngleText.DisplayedString = textBuilder.ToString();

            textBuilder.Clear();
            textBuilder.AppendFormat(texts[TextId.statusPosX], statusPosX);
            statusPosXText.DisplayedString = textBuilder.ToString();

            textBuilder.Clear();
            textBuilder.AppendFormat(texts[TextId.statusPosY], statusPosY);
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

            bool isPlayingState = (playState == PlayState.playing || playState == PlayState.fastBackwards || playState == PlayState.fastForward);

            window.Draw(buttonRestartSprite);
            window.Draw(buttonFastBackwardsSprite);
            window.Draw(buttonBackwardsSprite);
            window.Draw(isPlayingState ? buttonPlaySprite : buttonPauseSprite);
            window.Draw(buttonForwardSprite);
            window.Draw(buttonFastForwardSprite);

            buttonTurtleSprite.Texture = (turtleVisible ? buttonTurtleOnTexture: buttonTurtleOffTexture);
            window.Draw(buttonTurtleSprite);

            buttonGridSprite.Texture = (showGrid ? buttonGridOnTexture : buttonGridOffTexture);
            window.Draw(buttonGridSprite);
            window.Draw(buttonScreenshotSprite);
            buttonSplashSprite.Texture = (showSplash ? buttonSplashOnTexture : buttonSplashOffTexture);
            window.Draw(buttonSplashSprite);

            buttonMusicSprite.Texture = (IsMusicPlaying() ? buttonMusicOnTexture : buttonMusicOffTexture);
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
                    infoMessages[i].FillColor = new Color((byte)toolbarR, (byte)toolbarG, (byte)toolbarB, (byte)(255 * opacityFactor));
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

            if(showGrid)
            {
                textBuilder.Clear();
                textBuilder.AppendFormat(texts[TextId.gridCursorCoordinates], wp.X - window.Size.X / 2, -(wp.Y - window.Size.Y / 2));
                gridText.DisplayedString = textBuilder.ToString();
                gridText.Position = wp + new Vector2f(gridCoordinatesCursorOffsetX, gridCoordinatesCursorOffsetY);
                gridText.FillColor = new Color((byte)toolbarR, (byte)toolbarG, (byte)toolbarB); 
                window.Draw(gridText);
            }
        }

        static void DrawGrid(RenderWindow window)
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

            for (int i = -XLines; i <= XLines; i++)
            {
                textBuilder.Clear();
                textBuilder.AppendFormat(texts[TextId.gridCoordinates], (int)(-i * gridSeparation));
                gridText.DisplayedString = textBuilder.ToString();
                gridText.Position = new Vector2f(width / 2 - i * gridSeparation + gridCoordinatesTextOffsetX, gridTextBaseY + gridCoordinatesTextOffsetY);
                window.Draw(gridText);
            }

            for (int i = -YLines; i <= YLines; i++)
            {
                textBuilder.Clear();
                textBuilder.AppendFormat(texts[TextId.gridCoordinates], (int)(i * gridSeparation));
                gridText.DisplayedString = textBuilder.ToString();
                gridText.Position = new Vector2f(gridCoordinatesTextOffsetX, height / 2 - i * gridSeparation + gridCoordinatesTextOffsetY);
                window.Draw(gridText);
            }

        }

        static void DrawSplash(RenderWindow window, float opacity = 1.0f, bool withCloseButton = true)
        {
            splashSprite.Color = new Color(255, 255, 255, (byte)(255 * opacity));
            window.Draw(splashSprite);
            if(withCloseButton) { window.Draw(splashCloseButtonSprite); }
        }

        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;

            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }

            if (state != AppState.play) { return; }

            if (e.Code == Keyboard.Key.Space)
            {
                stepIndex = 0;
                playState = PlayState.playing;
            }
            else if (e.Code == Keyboard.Key.Right)
            {
                stepForward = true;
                playState = PlayState.stopped;
            }
            else if (e.Code == Keyboard.Key.Left)
            {
                stepBackward = true;
                playState = PlayState.stopped;
            }
            else if (e.Code == Keyboard.Key.Enter)
            {
                if (playState == PlayState.playing) { playState = PlayState.stopped; }
                else if (playState == PlayState.stopped) { playState = PlayState.playing; }
            }
            else if (e.Code == Keyboard.Key.M)
            {
                SwitchMusic();
            }
            else if (e.Code == Keyboard.Key.H)
            {
                SwitchTurtle();
            }
            else if (e.Code == Keyboard.Key.I || e.Code == Keyboard.Key.Tab)
            {
                showToolbar = !showToolbar;
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
            else if (e.Code == Keyboard.Key.C)
            {
                takeScreenshot = true;
            }
            else if(e.Code == Keyboard.Key.A)
            {
                showSplash = !showSplash;
            }
        }

        static void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;

            if(state != AppState.play) { return; }

            if (e.Button == Mouse.Button.Left)
            {
                if (buttonPlaySprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    if(playState == PlayState.playing) { playState = PlayState.stopped; }
                    else if(playState == PlayState.stopped) { playState = PlayState.playing; }
                }
                else if (buttonRestartSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    stepIndex = 0;
                    playState = PlayState.playing;

                }
                else if (buttonForwardSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    stepForward = true;
                    playState = PlayState.stopped;
                }
                else if (buttonBackwardsSprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    stepBackward = true;
                    playState = PlayState.stopped;
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
                    if(nextPlayIndex + 1 >= playsCount) { nextPlayIndex = 0; }
                    else { nextPlayIndex ++; }
                }
                else if (buttonPreviousPlaySprite.GetGlobalBounds().Contains(e.X, e.Y))
                {
                    if (nextPlayIndex - 1 < 0) { nextPlayIndex = playsCount - 1; }
                    else { nextPlayIndex--; }
                }

            }
        }

        static void OnWindowClosed(object sender, EventArgs e)
        {
            window.Close();
        }

    }
}
