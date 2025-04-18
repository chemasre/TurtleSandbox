﻿using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System.Text;
using System.Numerics;


namespace TurtleSandbox
{
    internal partial class UI
    {
        // Constants

        const int selectorTextX = 80;
        const int selectorTextY = 669;
        const float playTextScale = 1.0f;

        const int splashX = 530;
        const int splashY = 255;
        const int splashCloseOffsetX = 264;
        const int splashCloseOffsetY = 8;

        const int infoMessagesCount = 50;
        const float infoMessageDuration = 2.0f;
        const int infoMessageDistance = 150;
        const int infoMessageOffset = 35;
        const float infoMessageScale = 0.8f;

        const int statusBarX = 280;
        const int statusBarY = 670;
        const int statusAngleX = 325;
        const int statusAngleY = 669;
        const int statusPosXX = 428;
        const int statusPosXY = 669;
        const int statusPosYX = 526;
        const int statusPosYY = 669;

        const float statusTextScale = 1.0f;

        const float gridCoordinatesTextScale = 0.5f;
        const int gridCoordinatesTextOffsetX = 4;
        const int gridCoordinatesTextOffsetY = 4;
        const int gridSeparation = 50;
        const int gridCoordinatesCursorOffsetX = 23;
        const int gridCoordinatesCursorOffsetY = -10;

        const float selectorBarScale = 1.0f;
        const int selectorBarX = 25;
        const int selectorBarY = 666;
        const int selectorBarSeparationPlayMode = 164;
        const int selectorBarSeparationBrushMode = 180;

        const float playbackBarScale = 1.0f;
        const int playbackBarX = 650;
        const int playbackBarY = 666;
        const int playbackBarSeparation = 2;

        const float utilsBarScale = 1.0f;
        const int utilsBarX = 962;
        const int utilsBarY = 666;
        const int utilsBarSeparation = 2;

        const int toolbarLight1R = 255;
        const int toolbarLight1G = 255;
        const int toolbarLight1B = 255;

        const int toolbarLight2R = 80;
        const int toolbarLight2G = 200;
        const int toolbarLight2B = 80;

        const int toolbarDark1R = 164;
        const int toolbarDark1G = 66;
        const int toolbarDark1B = 164;

        const int toolbarDark2R = 91;
        const int toolbarDark2G = 36;
        const int toolbarDark2B = 91;

        const float strokeBarScale = 1.0f;
        const int strokeBarX = 300;
        const int strokeBarY = 666;
        const int strokeBarSeparation = 2;

        const float undoBarScale = 1.0f;
        const int undoBarX = 548;
        const int undoBarY = 666;
        const int undoBarSeparation = 2;

        const float fileBarScale = 1.0f;
        const int fileBarX = 736;
        const int fileBarY = 666;
        const int fileBarSeparation = 2;

        // Enums

        public enum ScreenId
        {
            Splash,
            SelectMode,
            PlayMode,
            BrushMode
        };

        public enum InfoMessagePosition
        {
            Turtle,
            UtilsToolbar,
            StrokeToolbar,
            UndoToolbar,
            FileToolbar
        };

        public enum Anchor
        {
            bottomCenter,
            center,
            bottomLeft,
            bottomRight
        };

        public enum Shadow
        {
            none,
            small,
            big,
        };

        // Structs

        public struct Area
        {
            public Vector2f position;
            public Vector2f size;
            public Sprite content;
            public float colorR;
            public float colorG;
            public float colorB;

        }

        public struct InfoMessage
        {
            public Vector2f position;
            public Text text;
            public bool free;
            public float lifetime;
        }

        // Screen

        static ScreenId screenId;
        static ScreenId nextScreenId;
        static bool isTransitioning;
        static float transitionTimer;

        //////////////////////////////////////////////////////////////////////////
        /////////                  COMMON ELEMENTS                       /////////
        //////////////////////////////////////////////////////////////////////////

        // Turtle

        static Sprite turtleSprite;
        static Texture turtleTexture;

        static bool turtleVisible;

        // Watermark

        static Text watermarkText;

        // Font

        static Font font;

        // Cursor

        static Sprite cursorSprite;
        static Texture cursorTexture;

        // Background colors

        static Color[] sandColors = new Color[] {  new Color(26, 28, 44),
                                                           new Color(93, 39, 93),
                                                           new Color(177, 62, 83),
                                                           new Color(239, 125, 87),
                                                           new Color(255, 205, 117),
                                                           new Color(167, 240, 112),
                                                           new Color(56, 183, 100),
                                                           new Color(37, 113, 121),
                                                           new Color(41, 54, 111),
                                                           new Color(59, 93, 201),
                                                           new Color(65, 166, 246),
                                                           new Color(115, 239, 247),
                                                           new Color(244, 244, 244),
                                                           new Color(148, 176, 194),
                                                           new Color(86, 108, 134),
                                                           new Color(51, 60, 87)
                                                 };
        static int backgroundColorIndex;

        static bool[] backgroundNeedsLightMode = { true, true, true, true, true, false, true, true,
                                                   true, true, true, false, false, true, true, true };

        // Grid

        static Text gridText;
        static Sprite gridLineSprite;
        static Texture gridLineTexture;

        // Selector

        static Sprite selectorPreviousSprite;
        static Sprite selectorNextSprite;

        static Texture selectorPreviousTexture;
        static Texture selectorNextTexture;

        static Text selectorText;

        static bool showSelector;

        // Utils toolbar

        static Sprite buttonScreenshotSprite;
        static Sprite buttonMusicSprite;
        static Sprite buttonTurtleSprite;
        static Sprite buttonGridSprite;
        static Sprite buttonSandColorSprite;
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
        static Texture[] buttonSandColorTextures;
        static Texture buttonSplashOnTexture;
        static Texture buttonSplashOffTexture;

        static bool showUtilsToolbar;

        // Messages

        static InfoMessage[] infoMessages;
        static Dictionary<Turtle.OrderId, string> orderIdToString;

        // Areas

        static Sprite areaTopBorder;
        static Sprite areaTopBase;
        static Sprite areaBottomBorder;
        static Sprite areaBottomBase;
        static Sprite areaLeftBorder;
        static Sprite areaLeftBase;
        static Sprite areaRightBorder;
        static Sprite areaRightBase;
        static Sprite areaTopLeftBorder;
        static Sprite areaTopLeftBase;
        static Sprite areaTopRightBorder;
        static Sprite areaTopRightBase;
        static Sprite areaBottomLeftBorder;
        static Sprite areaBottomLeftBase;
        static Sprite areaBottomRightBorder;
        static Sprite areaBottomRightBase;
        static Sprite areaCenterBase;

        //////////////////////////////////////////////////////////////////////////
        /////////                  SPLASH                                /////////
        //////////////////////////////////////////////////////////////////////////

        static Sprite splashCloseButtonSprite;
        static Texture splashCloseButtonTexture;

        static float opacity;
        static Color toolbarLightColor1;
        static Color toolbarLightColor2;
        static Color toolbarDarkColor1;
        static Color toolbarDarkColor2;

        static Area splashArea;

        static bool showSplash;


        //////////////////////////////////////////////////////////////////////////
        /////////                  SELECT MODE                           /////////
        //////////////////////////////////////////////////////////////////////////

        // Select mode areas

        static Area selectPlayModeArea;
        static Area selectBrushModeArea;

        static bool showSelectModeAreas;

        //////////////////////////////////////////////////////////////////////////
        /////////                  BRUSH MODE                            /////////
        //////////////////////////////////////////////////////////////////////////

        // Stroke

        static Sprite strokePreviewSprite;
        static List<Vector2f> strokePreview;

        static bool recordingStroke;

        // Stroke toolbar

        static Sprite buttonUndoSprite;
        static Sprite buttonRedoSprite;
        static Sprite buttonLengthSprite;
        static Sprite buttonSizeSprite;
        static Sprite buttonColorSprite;
        static Sprite buttonOpacitySprite;

        static Texture buttonUndoTexture;
        static Texture buttonRedoTexture;
        static Texture[] buttonStrokeLengthTextures;
        static Texture[] buttonSizeTextures;
        static Texture[] buttonColorTextures;
        static Texture[] buttonOpacityTextures;

        static bool showStrokeToolbar;

        // File toolbar

        static Sprite buttonNewSprite;
        static Sprite buttonSaveSprite;
        static Sprite buttonLoadSprite;

        static Texture buttonNewTexture;
        static Texture buttonSaveTexture;
        static Texture buttonLoadTexture;

        static bool showFileToolbar;

        //////////////////////////////////////////////////////////////////////////
        /////////                  PLAY MODE                             /////////
        //////////////////////////////////////////////////////////////////////////


        // Status

        static Text statusAngleText;
        static Text statusPosXText;
        static Text statusPosYText;
        static int statusPosX;
        static int statusPosY;
        static int statusAngle;

        static Sprite statusBarSprite;
        static Texture statusBarTexture;

        static bool showStatus;

        // Playback toolbar

        static Sprite buttonPlaySprite;
        static Sprite buttonPauseSprite;
        static Sprite buttonForwardSprite;
        static Sprite buttonBackwardsSprite;
        static Sprite buttonRestartSprite;
        static Sprite buttonFastForwardSprite;
        static Sprite buttonFastBackwardsSprite;

        static bool showPlaybackToolbar;

        // Flags

        static bool stopOnFastForwardOrBackwardsRelease;
        static bool playOnFastForwardOrBackwardsRelease;

        public static void Init(RenderWindow window)
        {

            // Register callbacks

            window.KeyPressed += OnKeyPressed;
            window.MouseButtonPressed += OnMouseButtonPressed;
            window.MouseButtonReleased += OnMouseButtonReleased;
            window.MouseMoved += OnMouseMoved;
            window.MouseWheelScrolled += OnMouseWheelScrolled;
            window.Closed += OnWindowClosed;

            // Init screen

            screenId = ScreenId.Splash;
            nextScreenId = ScreenId.Splash;
            isTransitioning = false;

            // Init colors

            opacity = 1;

            toolbarLightColor1 = new Color((byte)toolbarLight1R, (byte)toolbarLight1G, (byte)toolbarLight1B, (byte)(255 * opacity));
            toolbarLightColor2 = new Color((byte)toolbarLight2R, (byte)toolbarLight2G, (byte)toolbarLight2B, (byte)(255 * opacity));

            toolbarDarkColor1 = new Color((byte)toolbarDark1R, (byte)toolbarDark1G, (byte)toolbarDark1B, (byte)(255 * opacity));
            toolbarDarkColor2 = new Color((byte)toolbarDark2R, (byte)toolbarDark2G, (byte)toolbarDark2B, (byte)(255 * opacity));

            // Init texts

            font = new Font("Assets/Font.ttf");

            selectorText = new Text();
            selectorText.Position = ReferenceToBottomLeft(new Vector2f(selectorTextX, selectorTextY));
            selectorText.FillColor = toolbarLightColor1;
            selectorText.Scale = new Vector2f(playTextScale, playTextScale);
            selectorText.Font = font;
            selectorText.DisplayedString = "";

            statusPosX = 0;
            statusPosY = 0;
            statusAngle = 0;

            StringBuilder textBuilder = App.GetTextBuilder();

            statusAngleText = new Text();
            statusAngleText.Position = ReferenceToBottomCenter(new Vector2f(statusAngleX, statusAngleY));
            statusAngleText.FillColor = toolbarLightColor1;
            statusAngleText.Scale = new Vector2f(statusTextScale, statusTextScale);
            statusAngleText.Font = font;
            textBuilder.Clear();
            statusAngleText.DisplayedString = textBuilder.ToString();

            statusPosXText = new Text();
            statusPosXText.Position = ReferenceToBottomCenter(new Vector2f(statusPosXX, statusPosXY));
            statusPosXText.FillColor = toolbarLightColor1;
            statusPosXText.Scale = new Vector2f(statusTextScale, statusTextScale);
            statusPosXText.Font = font;
            textBuilder.Clear();
            statusPosXText.DisplayedString = textBuilder.ToString();

            statusPosYText = new Text();
            statusPosYText.Position = ReferenceToBottomCenter(new Vector2f(statusPosYX, statusPosYY));
            statusPosYText.FillColor = toolbarLightColor1;
            statusPosYText.Scale = new Vector2f(statusTextScale, statusTextScale);
            statusPosYText.Font = font;
            textBuilder.Clear();
            statusPosYText.DisplayedString = textBuilder.ToString();

            backgroundColorIndex = Config.sandColor;

            gridText = new Text();
            gridText.Font = font;
            gridText.FillColor =  new Color(toolbarLightColor1.R, toolbarLightColor1.G, toolbarLightColor1.B, (byte)Config.gridOpacity);

            // Init turtle

            turtleSprite = new Sprite();
            turtleTexture = new Texture("Assets/Turtle.png");
            turtleSprite.Texture = turtleTexture;
            turtleSprite.Origin = new Vector2f(50, 50);
            turtleSprite.Scale = new Vector2f(0.5f, 0.5f);
            turtleSprite.Color = toolbarLightColor1;

            turtleVisible = true;

            // Init watermark

            watermarkText = new Text();
            watermarkText.Font = font;
            watermarkText.Style = Text.Styles.Bold;
            watermarkText.CharacterSize = 16;
            watermarkText.DisplayedString = String.Format(Texts.Get(Texts.Id.watermark), AppConfig.appVersion);
            watermarkText.FillColor = new Color((byte)(0.9234f * 255), (byte)(0.923f * 255), (byte)(0.923f * 255), (byte)(0.5f * 255));
            watermarkText.Position = (Vector2f)window.Size - new Vector2f(475, 30);

            orderIdToString = new Dictionary<Turtle.OrderId, string>();
            orderIdToString[Turtle.OrderId.origin] = "origin";
            orderIdToString[Turtle.OrderId.walk] = "walk";
            orderIdToString[Turtle.OrderId.turn] = "turn";
            orderIdToString[Turtle.OrderId.randTurn] = "randTurn";
            orderIdToString[Turtle.OrderId.randWalk] = "randWalk";
            orderIdToString[Turtle.OrderId.teleport] = "teleport";
            orderIdToString[Turtle.OrderId.memorize] = "memorize";
            orderIdToString[Turtle.OrderId.recall] = "recall";
            orderIdToString[Turtle.OrderId.lookAt] = "lookAt";
            orderIdToString[Turtle.OrderId.walkDistanceTo] = "walkDistanceTo";

            // Init cursor

            cursorTexture = new Texture("Assets/Cursor.png");
            cursorSprite = new Sprite();
            cursorSprite.Texture = cursorTexture;
            cursorSprite.Color = toolbarLightColor1;

            // Init stroke

            Texture strokePreviewTexture = new Texture("Assets/LineDashed.png");
            strokePreviewTexture.Smooth = true;
            strokePreviewTexture.Repeated = true;
            strokePreviewSprite = new Sprite();
            strokePreviewSprite.Texture = strokePreviewTexture;
            strokePreviewSprite.Origin = new Vector2f(strokePreviewTexture.Size.X / 2, 0);
            strokePreviewSprite.Scale = new Vector2f(1, 100);
            strokePreviewSprite.Color = toolbarLightColor1;
            IntRect rect = strokePreviewSprite.TextureRect;
            rect.Height = (int)BrushMode.GetStrokeLength();
            strokePreviewSprite.TextureRect = rect;

            strokePreview = new List<Vector2f>(1000);

            recordingStroke = false;

            // Init areas

            areaTopBorder = new Sprite() { Texture = new Texture("Assets/Areas/TopBorder.png"), Color = toolbarLightColor1 };
            areaTopBase = new Sprite() { Texture = new Texture("Assets/Areas/TopBase.png"), Color = toolbarLightColor1 };
            areaBottomBorder = new Sprite() { Texture = new Texture("Assets/Areas/BottomBorder.png"), Color = toolbarLightColor1 };
            areaBottomBase = new Sprite() { Texture = new Texture("Assets/Areas/BottomBase.png"), Color = toolbarLightColor1 };
            areaLeftBorder = new Sprite() { Texture = new Texture("Assets/Areas/LeftBorder.png"), Color = toolbarLightColor1 };
            areaLeftBase = new Sprite() { Texture = new Texture("Assets/Areas/LeftBase.png"), Color = toolbarLightColor1 };
            areaRightBorder = new Sprite() { Texture = new Texture("Assets/Areas/RightBorder.png"), Color = toolbarLightColor1 };
            areaRightBase = new Sprite() { Texture = new Texture("Assets/Areas/RightBase.png"), Color = toolbarLightColor1 };
            areaTopLeftBorder = new Sprite() { Texture = new Texture("Assets/Areas/TopLeftBorder.png"), Color = toolbarLightColor1 };
            areaTopLeftBase = new Sprite() { Texture = new Texture("Assets/Areas/TopLeftBase.png"), Color = toolbarLightColor1 };
            areaTopRightBorder = new Sprite() { Texture = new Texture("Assets/Areas/TopRightBorder.png"), Color = toolbarLightColor1 };
            areaTopRightBase = new Sprite() { Texture = new Texture("Assets/Areas/TopRightBase.png"), Color = toolbarLightColor1 };
            areaBottomLeftBorder = new Sprite() { Texture = new Texture("Assets/Areas/BottomLeftBorder.png"), Color = toolbarLightColor1 };
            areaBottomLeftBase = new Sprite() { Texture = new Texture("Assets/Areas/BottomLeftBase.png"), Color = toolbarLightColor1 };
            areaBottomRightBorder = new Sprite() { Texture = new Texture("Assets/Areas/BottomRightBorder.png"), Color = toolbarLightColor1 };
            areaBottomRightBase = new Sprite() { Texture = new Texture("Assets/Areas/BottomRightBase.png"), Color = toolbarLightColor1 };
            areaCenterBase = new Sprite() { Texture = new Texture("Assets/Areas/Base.png"), Color = toolbarLightColor1 };

            // Init splash

            splashCloseButtonTexture = new Texture("Assets/Splash/SplashClose.png");
            splashCloseButtonSprite = new Sprite();
            splashCloseButtonSprite.Texture = splashCloseButtonTexture;
            splashCloseButtonSprite.Color = toolbarLightColor1;

            splashArea = new Area();
            splashArea.position = ReferenceToCenter(new Vector2f(splashX, splashY));
            splashArea.size = new Vector2f(304, 130);
            splashArea.colorR = toolbarLightColor2.R;
            splashArea.colorG = toolbarLightColor2.G;
            splashArea.colorB = toolbarLightColor2.B;
            splashArea.content = new Sprite() { Texture = new Texture("Assets/Areas/SplashContent.png"), Color = toolbarLightColor1 };

            showSplash = false;

            // Init select mode areas

            selectPlayModeArea = new Area();
            selectPlayModeArea.position = ReferenceToCenter(new Vector2f(300, 270));
            selectPlayModeArea.size = new Vector2f(300, 130);
            selectPlayModeArea.colorR = toolbarLightColor2.R;
            selectPlayModeArea.colorG = toolbarLightColor2.G;
            selectPlayModeArea.colorB = toolbarLightColor2.B;
            selectPlayModeArea.content = new Sprite() { Texture = new Texture("Assets/Areas/SelectPlayModeContent.png"), Color = toolbarLightColor1 };

            selectBrushModeArea = new Area();
            selectBrushModeArea.position = ReferenceToCenter(new Vector2f(725, 270));
            selectBrushModeArea.size = new Vector2f(300, 130);
            selectBrushModeArea.colorR = toolbarLightColor2.R;
            selectBrushModeArea.colorG = toolbarLightColor2.G;
            selectBrushModeArea.colorB = toolbarLightColor2.B;
            selectBrushModeArea.content = new Sprite() { Texture = new Texture("Assets/Areas/SelectBrushModeContent.png"), Color = toolbarLightColor1 };

            // Init grid

            gridLineTexture = new Texture("Assets/Line.png");
            gridLineSprite = new Sprite();
            gridLineSprite.Texture = gridLineTexture;
            gridLineSprite.Origin = new Vector2f(gridLineTexture.Size.X / 2, gridLineTexture.Size.Y);
            gridLineSprite.Color = new Color(toolbarLightColor1.R, toolbarLightColor1.G, toolbarLightColor1.B, (byte)Config.gridOpacity);

            // Init selector bar

            selectorNextTexture = new Texture("Assets/Buttons/Right.png");
            selectorPreviousTexture = new Texture("Assets/Buttons/Left.png");

            selectorNextSprite = new Sprite();
            selectorNextSprite.Texture = selectorNextTexture;
            selectorNextSprite.Color = toolbarLightColor1;
            selectorPreviousSprite = new Sprite();
            selectorPreviousSprite.Texture = selectorPreviousTexture;
            selectorPreviousSprite.Color = toolbarLightColor1;

            // Init stroke bar

            buttonStrokeLengthTextures = new Texture[] { new Texture("Assets/Buttons/StrokeLength1.png"),
                                                    new Texture("Assets/Buttons/StrokeLength2.png"),
                                                    new Texture("Assets/Buttons/StrokeLength3.png"),
                                                    new Texture("Assets/Buttons/StrokeLength4.png") };

            buttonSizeTextures = new Texture[] {    new Texture("Assets/Buttons/Size1.png"),
                                                    new Texture("Assets/Buttons/Size2.png"),
                                                    new Texture("Assets/Buttons/Size3.png"),
                                                    new Texture("Assets/Buttons/Size4.png"),
                                                    new Texture("Assets/Buttons/Size5.png") };


            buttonColorTextures = new Texture[] {   new Texture("Assets/Buttons/Color01.png"),
                                                    new Texture("Assets/Buttons/Color02.png"),
                                                    new Texture("Assets/Buttons/Color03.png"),
                                                    new Texture("Assets/Buttons/Color04.png"),
                                                    new Texture("Assets/Buttons/Color05.png"),
                                                    new Texture("Assets/Buttons/Color06.png"),
                                                    new Texture("Assets/Buttons/Color07.png"),
                                                    new Texture("Assets/Buttons/Color08.png"),
                                                    new Texture("Assets/Buttons/Color09.png"),
                                                    new Texture("Assets/Buttons/Color10.png"),
                                                    new Texture("Assets/Buttons/Color11.png"),
                                                    new Texture("Assets/Buttons/Color12.png"),
                                                    new Texture("Assets/Buttons/Color13.png"),
                                                    new Texture("Assets/Buttons/Color14.png"),
                                                    new Texture("Assets/Buttons/Color15.png"),
                                                    new Texture("Assets/Buttons/Color16.png") };

            buttonOpacityTextures = new Texture[] { new Texture("Assets/Buttons/Opacity10.png"),
                                                    new Texture("Assets/Buttons/Opacity09.png"),
                                                    new Texture("Assets/Buttons/Opacity08.png"),
                                                    new Texture("Assets/Buttons/Opacity07.png"),
                                                    new Texture("Assets/Buttons/Opacity06.png"),
                                                    new Texture("Assets/Buttons/Opacity05.png"),
                                                    new Texture("Assets/Buttons/Opacity04.png"),
                                                    new Texture("Assets/Buttons/Opacity03.png"),
                                                    new Texture("Assets/Buttons/Opacity02.png"),
                                                    new Texture("Assets/Buttons/Opacity01.png") };

            buttonUndoTexture = new Texture("Assets/Buttons/Undo.png");
            buttonRedoTexture = new Texture("Assets/Buttons/Redo.png");

            buttonLengthSprite = new Sprite();
            buttonLengthSprite.Texture = buttonStrokeLengthTextures[BrushMode.GetStrokeLengthIndex()];
            buttonLengthSprite.Color = toolbarLightColor1;

            buttonSizeSprite = new Sprite();
            buttonSizeSprite.Texture = buttonSizeTextures[BrushMode.GetBrushSizeIndex()];
            buttonSizeSprite.Color = toolbarLightColor1;

            buttonColorSprite = new Sprite();
            buttonColorSprite.Texture = buttonColorTextures[BrushMode.GetBrushColorIndex()];
            buttonColorSprite.Color = new Color(255, 255, 255);

            buttonOpacitySprite = new Sprite();
            buttonOpacitySprite.Texture = buttonOpacityTextures[BrushMode.GetBrushOpacityIndex()];
            buttonOpacitySprite.Color = toolbarLightColor1;

            buttonUndoSprite = new Sprite();
            buttonUndoSprite.Texture = buttonUndoTexture;
            buttonUndoSprite.Color = toolbarLightColor1;

            buttonRedoSprite = new Sprite();
            buttonRedoSprite.Texture = buttonRedoTexture;
            buttonRedoSprite.Color = toolbarLightColor1;

            // Init file bar

            buttonNewTexture = new Texture("Assets/Buttons/New.png");
            buttonSaveTexture = new Texture("Assets/Buttons/Save.png");
            buttonLoadTexture = new Texture("Assets/Buttons/Load.png");

            buttonNewSprite = new Sprite();
            buttonNewSprite.Texture = buttonNewTexture;
            buttonNewSprite.Color = toolbarLightColor1;

            buttonSaveSprite = new Sprite();
            buttonSaveSprite.Texture = buttonSaveTexture;
            buttonSaveSprite.Color = toolbarLightColor1;

            buttonLoadSprite = new Sprite();
            buttonLoadSprite.Texture = buttonLoadTexture;
            buttonLoadSprite.Color = toolbarLightColor1;

            // Init status bar

            statusBarTexture = new Texture("Assets/StatusBar.png");

            statusBarSprite = new Sprite();
            statusBarSprite.Texture = statusBarTexture;
            statusBarSprite.Color = toolbarLightColor1;

            // Init playback toolbar

            buttonPlayTexture = new Texture("Assets/Buttons/Play.png");
            buttonPauseTexture = new Texture("Assets/Buttons/Pause.png");
            buttonForwardTexture = new Texture("Assets/Buttons/Forward.png");
            buttonBackwardsTexture = new Texture("Assets/Buttons/Backward.png");
            buttonRestartTexture = new Texture("Assets/Buttons/Restart.png");
            buttonFastForwardTexture = new Texture("Assets/Buttons/FastForward.png");
            buttonFastBackwardsTexture = new Texture("Assets/Buttons/FastBackward.png");

            // Init utils toolbar

            buttonScreenshotTexture = new Texture("Assets/Buttons/Screenshot.png");
            buttonMusicOnTexture = new Texture("Assets/Buttons/MusicOn.png");
            buttonMusicOffTexture = new Texture("Assets/Buttons/MusicOff.png");
            buttonTurtleOnTexture = new Texture("Assets/Buttons/TurtleOn.png");
            buttonTurtleOffTexture = new Texture("Assets/Buttons/TurtleOff.png");
            buttonGridOnTexture = new Texture("Assets/Buttons/GridOn.png");
            buttonGridOffTexture = new Texture("Assets/Buttons/GridOff.png");
            buttonSplashOnTexture = new Texture("Assets/Buttons/SplashOn.png");
            buttonSplashOffTexture = new Texture("Assets/Buttons/SplashOff.png");

            buttonSandColorTextures = new Texture[] { new Texture("Assets/Buttons/Color01.png"),
                                                    new Texture("Assets/Buttons/Color02.png"),
                                                    new Texture("Assets/Buttons/Color03.png"),
                                                    new Texture("Assets/Buttons/Color04.png"),
                                                    new Texture("Assets/Buttons/Color05.png"),
                                                    new Texture("Assets/Buttons/Color06.png"),
                                                    new Texture("Assets/Buttons/Color07.png"),
                                                    new Texture("Assets/Buttons/Color08.png"),
                                                    new Texture("Assets/Buttons/Color09.png"),
                                                    new Texture("Assets/Buttons/Color10.png"),
                                                    new Texture("Assets/Buttons/Color11.png"),
                                                    new Texture("Assets/Buttons/Color12.png"),
                                                    new Texture("Assets/Buttons/Color13.png"),
                                                    new Texture("Assets/Buttons/Color14.png"),
                                                    new Texture("Assets/Buttons/Color15.png"),
                                                    new Texture("Assets/Buttons/Color16.png") };

            buttonPlaySprite = new Sprite();
            buttonPlaySprite.Texture = buttonPlayTexture;
            buttonPlaySprite.Color = toolbarLightColor1;

            buttonPauseSprite = new Sprite();
            buttonPauseSprite.Texture = buttonPauseTexture;
            buttonPauseSprite.Color = toolbarLightColor1;
            buttonForwardSprite = new Sprite();
            buttonForwardSprite.Texture = buttonForwardTexture;
            buttonForwardSprite.Color = toolbarLightColor1;
            buttonBackwardsSprite = new Sprite();
            buttonBackwardsSprite.Texture = buttonBackwardsTexture;
            buttonBackwardsSprite.Color = toolbarLightColor1;
            buttonRestartSprite = new Sprite();
            buttonRestartSprite.Texture = buttonRestartTexture;
            buttonRestartSprite.Color = toolbarLightColor1;
            buttonFastForwardSprite = new Sprite();
            buttonFastForwardSprite.Texture = buttonFastForwardTexture;
            buttonFastForwardSprite.Color = toolbarLightColor1;
            buttonFastBackwardsSprite = new Sprite();
            buttonFastBackwardsSprite.Texture = buttonFastBackwardsTexture;
            buttonFastBackwardsSprite.Color = toolbarLightColor1;
            buttonScreenshotSprite = new Sprite();
            buttonScreenshotSprite.Texture = buttonScreenshotTexture;
            buttonScreenshotSprite.Color = toolbarLightColor1;
            buttonMusicSprite = new Sprite();
            buttonMusicSprite.Texture = buttonMusicOffTexture;
            buttonMusicSprite.Color = toolbarLightColor1;
            buttonTurtleSprite = new Sprite();
            buttonTurtleSprite.Texture = buttonTurtleOffTexture;
            buttonTurtleSprite.Color = toolbarLightColor1;
            buttonGridSprite = new Sprite();
            buttonGridSprite.Texture = buttonGridOffTexture;
            buttonGridSprite.Color = toolbarLightColor1;
            buttonSandColorSprite = new Sprite();
            buttonSandColorSprite.Texture = buttonSandColorTextures[backgroundColorIndex];
            buttonSandColorSprite.Color = new Color(255, 255, 255);
            buttonSplashSprite = new Sprite();
            buttonSplashSprite.Texture = buttonSplashOffTexture;
            buttonSplashSprite.Color = toolbarLightColor1;

            // Init info messages

            infoMessages = new InfoMessage[infoMessagesCount];

            for (int i = 0; i < infoMessagesCount; i++)
            {
                var text = new Text();
                text.Font = font;
                infoMessages[i].text = text;
                infoMessages[i].free = true;
                infoMessages[i].lifetime = 0;
            }


            LayoutElements();

        }

        private static void LayoutElements()
        {
            // Splash

            splashCloseButtonSprite.Position = ReferenceToCenter(new Vector2f(splashX + splashCloseOffsetX, splashY + splashCloseOffsetY));

            float buttonWidth = buttonPlayTexture.Size.X;

            // Selector bar

            Vector2f barScale = new Vector2f(selectorBarScale, selectorBarScale);

            selectorPreviousSprite.Scale = barScale;
            selectorNextSprite.Scale = barScale;


            // Stroke bar

            buttonSizeSprite.Position = new Vector2f(ReferenceToBottomCenterX(strokeBarX) + 0 * buttonWidth + 0 * strokeBarSeparation, ReferenceToBottomCenterY(strokeBarY));
            buttonColorSprite.Position = new Vector2f(ReferenceToBottomCenterX(strokeBarX) + 1 * buttonWidth + 1 * strokeBarSeparation, ReferenceToBottomCenterY(strokeBarY));
            buttonOpacitySprite.Position = new Vector2f(ReferenceToBottomCenterX(strokeBarX) + 2 * buttonWidth + 2 * strokeBarSeparation, ReferenceToBottomCenterY(strokeBarY));
            buttonLengthSprite.Position = new Vector2f(ReferenceToBottomCenterX(strokeBarX) + 3 * buttonWidth + 3 * strokeBarSeparation, ReferenceToBottomCenterY(strokeBarY));

            buttonUndoSprite.Position = new Vector2f(ReferenceToBottomCenterX(undoBarX) + 0 * buttonWidth + 0 * undoBarSeparation, ReferenceToBottomCenterY(strokeBarY));
            buttonRedoSprite.Position = new Vector2f(ReferenceToBottomCenterX(undoBarX) + 1 * buttonWidth + 1 * undoBarSeparation, ReferenceToBottomCenterY(strokeBarY));

            // File bar

            buttonNewSprite.Position = new Vector2f(ReferenceToBottomCenterX(fileBarX) + 0 * buttonWidth + 0 * fileBarSeparation, ReferenceToBottomCenterY(fileBarY));
            buttonSaveSprite.Position = new Vector2f(ReferenceToBottomCenterX(fileBarX) + 1 * buttonWidth + 1 * fileBarSeparation, ReferenceToBottomCenterY(fileBarY));
            buttonLoadSprite.Position = new Vector2f(ReferenceToBottomCenterX(fileBarX) + 2 * buttonWidth + 2 * fileBarSeparation, ReferenceToBottomCenterY(fileBarY));

            // Status bar

            statusBarSprite.Position = ReferenceToBottomCenter(new Vector2f(statusBarX, statusBarY));

            // Playback toolbar

            buttonRestartSprite.Position = new Vector2f(ReferenceToBottomCenterX(playbackBarX) + 0 * buttonWidth + 0 * playbackBarSeparation, ReferenceToBottomCenterY(playbackBarY));
            buttonFastBackwardsSprite.Position = new Vector2f(ReferenceToBottomCenterX(playbackBarX) + 1 * buttonWidth + 1 * playbackBarSeparation, ReferenceToBottomCenterY(playbackBarY));
            buttonBackwardsSprite.Position = new Vector2f(ReferenceToBottomCenterX(playbackBarX) + 2 * buttonWidth + 2 * playbackBarSeparation, ReferenceToBottomCenterY(playbackBarY));
            buttonPlaySprite.Position = new Vector2f(ReferenceToBottomCenterX(playbackBarX) + 3 * buttonWidth + 3 * playbackBarSeparation, ReferenceToBottomCenterY(playbackBarY));
            buttonPauseSprite.Position = new Vector2f(ReferenceToBottomCenterX(playbackBarX) + 3 * buttonWidth + 3 * playbackBarSeparation, ReferenceToBottomCenterY(playbackBarY));
            buttonForwardSprite.Position = new Vector2f(ReferenceToBottomCenterX(playbackBarX) + 4 * buttonWidth + 4 * playbackBarSeparation, ReferenceToBottomCenterY(playbackBarY));
            buttonFastForwardSprite.Position = new Vector2f(ReferenceToBottomCenterX(playbackBarX) + 5 * buttonWidth + 5 * playbackBarSeparation, ReferenceToBottomCenterY(playbackBarY));

            // Init utils toolbar

            buttonTurtleSprite.Position = new Vector2f(ReferenceToBottomRightX(utilsBarX) + 0 * buttonWidth + 0 * utilsBarSeparation, ReferenceToBottomRightY(utilsBarY));
            buttonGridSprite.Position = new Vector2f(ReferenceToBottomRightX(utilsBarX) + 1 * buttonWidth + 1 * utilsBarSeparation, ReferenceToBottomRightY(utilsBarY));
            buttonSandColorSprite.Position = new Vector2f(ReferenceToBottomRightX(utilsBarX) + 2 * buttonWidth + 2 * utilsBarSeparation, ReferenceToBottomRightY(utilsBarY));
            buttonMusicSprite.Position = new Vector2f(ReferenceToBottomRightX(utilsBarX) + 3 * buttonWidth + 3 * utilsBarSeparation, ReferenceToBottomRightY(utilsBarY));
            buttonScreenshotSprite.Position = new Vector2f(ReferenceToBottomRightX(utilsBarX) + 4 * buttonWidth + 4 * utilsBarSeparation, ReferenceToBottomRightY(utilsBarY));
            buttonSplashSprite.Position = new Vector2f(ReferenceToBottomRightX(utilsBarX) + 5 * buttonWidth + 5 * utilsBarSeparation, ReferenceToBottomRightY(utilsBarY));

            barScale = new Vector2f(playbackBarScale, playbackBarScale);
            buttonRestartSprite.Scale = barScale;
            buttonFastBackwardsSprite.Scale = barScale;
            buttonBackwardsSprite.Scale = barScale;
            buttonPlaySprite.Scale = barScale;
            buttonPauseSprite.Scale = barScale;
            buttonForwardSprite.Scale = barScale;
            buttonFastForwardSprite.Scale = barScale;
            buttonTurtleSprite.Scale = barScale;
            buttonGridSprite.Scale = barScale;
            buttonSandColorSprite.Scale = barScale;
            buttonScreenshotSprite.Scale = barScale;


            // Init info messages

            for (int i = 0; i < infoMessagesCount; i++)
            {
                infoMessages[i].text.Scale = new Vector2f(infoMessageScale, infoMessageScale);
                infoMessages[i].position = new Vector2f(0, 0);
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

            if (position == InfoMessagePosition.UtilsToolbar)
            {
                p = ReferenceToBottomRight(new Vector2f(utilsBarX, utilsBarY));
            }
            else if (position == InfoMessagePosition.StrokeToolbar)
            {
                p = ReferenceToBottomCenter(new Vector2f(strokeBarX, strokeBarY));
            }
            else if (position == InfoMessagePosition.UndoToolbar)
            {
                p = ReferenceToBottomCenter(new Vector2f(undoBarX, undoBarY));
            }
            else if (position == InfoMessagePosition.FileToolbar)
            {
                p = ReferenceToBottomCenter(new Vector2f(buttonNewSprite.Position.X, playbackBarY));
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
                if (infoMessages[j].free) { free = j; infoMessages[j].free = false; done = true; }
                else
                {
                    if (infoMessages[j].lifetime > oldestLifeTime)
                    {
                        oldestLifeTime = infoMessages[j].lifetime;
                        oldest = j;
                    }

                    j++;
                }
            }

            int messageIndex = free ?? oldest;
            infoMessages[messageIndex].text.DisplayedString = message;
            infoMessages[messageIndex].position = p;
            infoMessages[messageIndex].lifetime = 0;
            infoMessages[messageIndex].free = false;

        }

        public static void Update(float elapsedTime)
        {
            RenderWindow window = App.GetWindow();

            if(isTransitioning)
            {
                transitionTimer += elapsedTime;

                if(transitionTimer >= AppConfig.screenTransitionTime)
                {   
                    // Finished transition
                    screenId = nextScreenId;
                    opacity = 1;
                    isTransitioning = false;
                }
                else if(transitionTimer >= AppConfig.screenTransitionTime / 2)
                {
                    // Next screen fading in
                    screenId = nextScreenId;
                    opacity = (transitionTimer - AppConfig.screenTransitionTime / 2) / (AppConfig.screenTransitionTime / 2);
                }
                else
                {
                    // Current screen fading out
                    opacity = 1.0f - transitionTimer / (AppConfig.screenTransitionTime / 2);
                }
            }
            else
            {
                if(screenId != nextScreenId) { isTransitioning = true; transitionTimer = 0; }
            }

            // Update info messages

            for (int i = 0; i < infoMessages.Length; i++)
            {
                if (!infoMessages[i].free)
                {
                    infoMessages[i].lifetime += elapsedTime;
                    if (infoMessages[i].lifetime >= infoMessageDuration) { infoMessages[i].free = true; }
                }
            }


            if(screenId == ScreenId.PlayMode)
            {
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
        }

        public static void Draw(RenderWindow window, bool takeScreenshot = false)
        {
            TracePlayer.Draw(window, opacity);

            if (screenId == ScreenId.Splash)
            {
                DrawSplash(window, false);
            }
            else if(screenId == ScreenId.SelectMode)
            {
                DrawArea(selectPlayModeArea, window);
                DrawArea(selectBrushModeArea, window);
            }
            else if(screenId == ScreenId.PlayMode || screenId == ScreenId.BrushMode)
            {
                if (Config.showGrid && !takeScreenshot) { DrawGrid(window); }

                if (screenId == ScreenId.BrushMode) { DrawStrokePreview(window); }

                if (turtleVisible) { UI.DrawTurtle(window); }

                if (takeScreenshot)
                {
                    window.Draw(watermarkText);

                    TakeScreenshot(window);
                }

                if (!Config.showToolbar) { return; }

                // Draw bars

                // Selector bar

                int selectorSeparation = (screenId == ScreenId.PlayMode ? selectorBarSeparationPlayMode : selectorBarSeparationBrushMode);
                selectorPreviousSprite.Position = ReferenceToBottomLeft(new Vector2f(selectorBarX, selectorBarY));
                selectorNextSprite.Position = new Vector2f(ReferenceToBottomLeftX(selectorBarX + selectorSeparation), ReferenceToBottomLeftY(selectorBarY));

                DrawSprite(window, selectorPreviousSprite, Shadow.small);
                DrawSprite(window, selectorNextSprite, Shadow.small);

                StringBuilder textBuilder = App.GetTextBuilder();

                textBuilder.Clear();

                if (screenId == ScreenId.PlayMode) { textBuilder.AppendFormat(Texts.Get(Texts.Id.play), PlayMode.GetPlayIndex() + 1); }
                else { textBuilder.AppendFormat(Texts.Get(Texts.Id.brush), BrushMode.GetBrushIndex() + 1); }
                selectorText.DisplayedString = textBuilder.ToString();

                DrawText(window, selectorText, Shadow.small);

                if (screenId == ScreenId.PlayMode)
                {
                    // Status bar

                    DrawSprite(window, statusBarSprite, Shadow.small);

                    textBuilder.Clear();
                    textBuilder.AppendFormat(Texts.Get(Texts.Id.statusAngle), statusAngle);
                    statusAngleText.DisplayedString = textBuilder.ToString();

                    textBuilder.Clear();
                    textBuilder.AppendFormat(Texts.Get(Texts.Id.statusPosX), statusPosX);
                    statusPosXText.DisplayedString = textBuilder.ToString();

                    textBuilder.Clear();
                    textBuilder.AppendFormat(Texts.Get(Texts.Id.statusPosY), statusPosY);
                    statusPosYText.DisplayedString = textBuilder.ToString();

                    DrawText(window, statusAngleText, Shadow.small);
                    DrawText(window, statusPosXText, Shadow.small);
                    DrawText(window, statusPosYText, Shadow.small);

                    // Playback toolbar

                    DrawSprite(window, buttonRestartSprite, Shadow.small);
                    DrawSprite(window, buttonFastBackwardsSprite, Shadow.small);

                    TracePlayer.PlayState playState = TracePlayer.GetPlayState();

                    bool isPlayingState = (playState == TracePlayer.PlayState.playing ||
                                           playState == TracePlayer.PlayState.fastBackwards ||
                                           playState == TracePlayer.PlayState.fastForward);

                    DrawSprite(window, buttonRestartSprite, Shadow.small);
                    DrawSprite(window, buttonFastBackwardsSprite, Shadow.small);
                    DrawSprite(window, buttonBackwardsSprite, Shadow.small);
                    DrawSprite(window, isPlayingState ? buttonPlaySprite : buttonPauseSprite, Shadow.small);
                    DrawSprite(window, buttonForwardSprite, Shadow.small);
                    DrawSprite(window, buttonFastForwardSprite, Shadow.small);


                }
                else // screenId == ScreenId.BrushMode
                {
                    // Stroke bar

                    buttonSizeSprite.Texture = buttonSizeTextures[BrushMode.GetBrushSizeIndex()];
                    buttonColorSprite.Texture = buttonColorTextures[BrushMode.GetBrushColorIndex()];
                    buttonOpacitySprite.Texture = buttonOpacityTextures[BrushMode.GetBrushOpacityIndex()];
                    buttonLengthSprite.Texture = buttonStrokeLengthTextures[BrushMode.GetStrokeLengthIndex()];

                    DrawSprite(window, buttonSizeSprite, Shadow.small);
                    DrawSprite(window, buttonColorSprite, Shadow.small);
                    DrawSprite(window, buttonOpacitySprite, Shadow.small);
                    DrawSprite(window, buttonLengthSprite, Shadow.small);
                    DrawSprite(window, buttonUndoSprite, Shadow.small);
                    DrawSprite(window, buttonRedoSprite, Shadow.small);

                    // File bar

                    DrawSprite(window, buttonNewSprite, Shadow.small);
                    DrawSprite(window, buttonSaveSprite, Shadow.small);
                    DrawSprite(window, buttonLoadSprite, Shadow.small);

                }

                // Utils toolbar

                buttonTurtleSprite.Texture = (turtleVisible ? buttonTurtleOnTexture: buttonTurtleOffTexture);
                DrawSprite(window, buttonTurtleSprite, Shadow.small);

                buttonGridSprite.Texture = (Config.showGrid ? buttonGridOnTexture : buttonGridOffTexture);
                DrawSprite(window, buttonGridSprite, Shadow.small);
                buttonSandColorSprite.Texture = buttonSandColorTextures[backgroundColorIndex];
                DrawSprite(window, buttonSandColorSprite, Shadow.small);
                DrawSprite(window, buttonScreenshotSprite, Shadow.small);
                buttonSplashSprite.Texture = (showSplash ? buttonSplashOnTexture : buttonSplashOffTexture);
                DrawSprite(window, buttonSplashSprite, Shadow.small);

                buttonMusicSprite.Texture = (App.IsMusicPlaying() ? buttonMusicOnTexture : buttonMusicOffTexture);
                DrawSprite(window, buttonMusicSprite, Shadow.small);

                // Draw info messages

                for (int i = 0; i < infoMessages.Length; i++)
                {
                    if (!infoMessages[i].free)
                    {
                        float factor = infoMessages[i].lifetime / infoMessageDuration;
                        float opacityFactor = MathF.Pow(1 - factor, 3);

                        Vector2f position = infoMessages[i].position;
                        Color color = infoMessages[i].text.FillColor;
                        infoMessages[i].text.Position = position + new Vector2f(0, -infoMessageOffset - infoMessageDistance * factor);
                        infoMessages[i].text.FillColor = new Color(color.R, color.G, color.B, (byte)(255 * opacityFactor * opacity));

                        DrawText(window, infoMessages[i].text, Shadow.small);

                    }
                }

                // Draw splash

                if(showSplash) { DrawSplash(window); }

                // Draw coordinates near to cursor when grid is visible

                if (Config.showGrid)
                {
                    Vector2f wp1 = (Vector2f)Mouse.GetPosition(window);
                    textBuilder.Clear();
                    textBuilder.AppendFormat(Texts.Get(Texts.Id.gridCursorCoordinates), wp1.X - window.Size.X / 2, -(wp1.Y - window.Size.Y / 2));
                    gridText.DisplayedString = textBuilder.ToString();
                    gridText.Position = wp1 + new Vector2f(gridCoordinatesCursorOffsetX, gridCoordinatesCursorOffsetY);
                    DrawText(window, gridText, Shadow.small);
                }

            }

            // Draw cursor

            Vector2f wp2 = (Vector2f)Mouse.GetPosition(window);
            cursorSprite.Position = wp2;
            DrawSprite(window, cursorSprite, Shadow.small);

        }

        public static void GotoScreen(ScreenId id, bool inmediate = false)
        {
            if(inmediate) { screenId = id; nextScreenId = id; }
            else { nextScreenId = id; }
            
        }

        public static ScreenId GetCurrentScreen()
        {
            return screenId;
        }

        public static bool IsTransitioning()
        {
            return isTransitioning;
        }

        public static int GetBackgroundColorIndex()
        {
            return backgroundColorIndex;
        }

        public static Color GetBackgroundColor()
        {
            return sandColors[backgroundColorIndex];
        }

        public static void NextBackgroundColorIndex()
        {
            if (backgroundColorIndex + 1 >= AppConfig.sandColorsCount) { backgroundColorIndex = 0; }
            else { backgroundColorIndex++; }

            TracePlayer.SetBackgroundColor(sandColors[backgroundColorIndex]);

            SetToolbarTheme(backgroundNeedsLightMode[backgroundColorIndex]);


        }

        static void SetToolbarTheme(bool light)
        {
            Color color1 = light ? toolbarLightColor1 : toolbarDarkColor1;
            Color color2 = light ? toolbarLightColor2 : toolbarDarkColor2;


            strokePreviewSprite.Color = color1;
            selectorText.FillColor = color1;
            statusAngleText.FillColor = color1;
            statusPosXText.FillColor = color1;
            statusPosYText.FillColor = color1;
            gridText.FillColor = new Color(color1.R, color1.G, color1.B, (byte)Config.gridOpacity);
            //turtleSprite.Color = color1;
            cursorSprite.Color = color1;
            strokePreviewSprite.Color = color1;
            areaTopBorder.Color = color1;
            areaTopBase.Color = color1;
            areaBottomBorder.Color = color1;
            areaBottomBase.Color = color1;
            areaLeftBorder.Color = color1;
            areaLeftBase.Color = color1;
            areaRightBorder.Color = color1;
            areaRightBase.Color = color1;
            areaTopLeftBorder.Color = color1;
            areaTopLeftBase.Color = color1;
            areaTopRightBorder.Color = color1;
            areaTopRightBase.Color = color1;
            areaBottomLeftBorder.Color = color1;
            areaBottomLeftBase.Color = color1;
            areaBottomRightBorder.Color = color1;
            areaBottomRightBase.Color = color1;
            areaCenterBase.Color = color1;
            splashCloseButtonSprite.Color = color1;
            splashArea.content.Color = color1;
            selectPlayModeArea.content.Color = color1;
            selectBrushModeArea.content.Color = color1;
            gridLineSprite.Color = new Color(color1.R, color1.G, color1.B, (byte)Config.gridOpacity);
            selectorNextSprite.Color = color1;
            selectorPreviousSprite.Color = color1;
            buttonLengthSprite.Color = color1;
            buttonSizeSprite.Color = color1;
            buttonOpacitySprite.Color = color1;
            buttonUndoSprite.Color = color1;
            buttonRedoSprite.Color = color1;
            buttonNewSprite.Color = color1;
            buttonSaveSprite.Color = color1;
            buttonLoadSprite.Color = color1;
            statusBarSprite.Color = color1;
            buttonPlaySprite.Color = color1;
            buttonPauseSprite.Color = color1;
            buttonForwardSprite.Color = color1;
            buttonBackwardsSprite.Color = color1;
            buttonRestartSprite.Color = color1;
            buttonFastForwardSprite.Color = color1;
            buttonFastBackwardsSprite.Color = color1;
            buttonScreenshotSprite.Color = color1;
            buttonMusicSprite.Color = color1;
            buttonTurtleSprite.Color = color1;
            buttonGridSprite.Color = color1;
            buttonSplashSprite.Color = color1;
            splashCloseButtonSprite.Color = color1;
            selectorNextSprite.Color = color1;
            selectorPreviousSprite.Color = color1;
            statusBarSprite.Color = color1;
            buttonRestartSprite.Color           = color1;
            buttonFastBackwardsSprite.Color     = color1;
            buttonBackwardsSprite.Color         = color1;
            buttonPlaySprite.Color              = color1;
            buttonPauseSprite.Color             = color1;
            buttonForwardSprite.Color           = color1;
            buttonFastForwardSprite.Color       = color1;
            buttonTurtleSprite.Color = color1;
            buttonGridSprite.Color = color1;
            buttonMusicSprite.Color = color1;
            buttonScreenshotSprite.Color = color1;
            buttonSplashSprite.Color = color1;

            splashArea.colorR = color2.R;
            splashArea.colorG = color2.G;
            splashArea.colorB = color2.B;
            selectPlayModeArea.colorR = color2.R;
            selectPlayModeArea.colorG = color2.G;
            selectPlayModeArea.colorB = color2.B;
            selectBrushModeArea.colorR = color2.R;
            selectBrushModeArea.colorG = color2.G;
            selectBrushModeArea.colorB = color2.B;

            for(int i = 0; i < infoMessages.Length; i++)
            {
                infoMessages[i].text.FillColor = color1;
            }

        }



        public static void SwitchGrid()
        {
            Config.showGrid = !Config.showGrid;

            UI.AddInfoMessage(Texts.Get(Config.showGrid ? Texts.Id.gridOn : Texts.Id.gridOff), UI.InfoMessagePosition.UtilsToolbar);

        }

        public static void DrawGrid(RenderTarget target)
        {
            float width = target.Size.X;
            float height = target.Size.Y;

            // Draw lines

            Vector2f center = new Vector2f(width / 2, height / 2);
            int XLines = (int)(width / 2.0f / gridSeparation);
            int YLines = (int)(height / 2.0f / gridSeparation);
            gridLineSprite.Rotation = 0;
            for(int i = -XLines; i <= XLines; i++)
            {
                gridLineSprite.Scale = new Vector2f((i != 0 ? 2.0f : 6.0f) / gridLineTexture.Size.X, height / gridLineTexture.Size.Y);
                gridLineSprite.Position = new Vector2f(center.X + i * gridSeparation, height);
                DrawSprite(target, gridLineSprite);
            }

            gridLineSprite.Rotation = 90;
            for (int i = -YLines; i <= YLines; i++)
            {
                gridLineSprite.Scale = new Vector2f((i != 0 ? 2.0f : 6.0f) / gridLineTexture.Size.X, width / gridLineTexture.Size.Y);
                gridLineSprite.Position = new Vector2f(0, center.Y + i * gridSeparation);
                DrawSprite(target, gridLineSprite);
            }

            // Draw coordinates

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
                DrawText(target, gridText, Shadow.small);
            }

            for (int i = -YLines; i <= YLines; i++)
            {
                textBuilder.Clear();
                textBuilder.AppendFormat(Texts.Get(Texts.Id.gridCoordinates), (int)(i * gridSeparation));
                gridText.DisplayedString = textBuilder.ToString();
                gridText.Position = new Vector2f(gridCoordinatesTextOffsetX, height / 2 - i * gridSeparation + gridCoordinatesTextOffsetY);
                DrawText(target, gridText, Shadow.small);
            }

        }

        public static void SwitchSplash()
        {
            showSplash = !showSplash;
        }

        public static void SwitchTurtle()
        {
            turtleVisible = !turtleVisible;
            UI.AddInfoMessage(Texts.Get(turtleVisible ? Texts.Id.turtleOn : Texts.Id.turtleOff), UI.InfoMessagePosition.UtilsToolbar);
        }

        public static void DrawTurtle(RenderTarget target)
        {
            turtleSprite.Position = UI.TurtlePositionToScreen(statusPosX, statusPosY, target);
            turtleSprite.Rotation = UI.TurtleAngleToScreenRotation(statusAngle);
            DrawSprite(target, turtleSprite);

        }

        static FloatRect GetAreaRect(Area area)
        {
            var r = new FloatRect(area.position, area.size);

            return r;
        }

        public static void DrawArea(Area area, RenderTarget target)
        {
            float cornerWidth = areaTopLeftBase.Texture.Size.X;
            float cornerHeight = areaTopLeftBase.Texture.Size.Y;
            float horizontalWidth = (area.size.X - 2 * cornerWidth);
            float verticalHeight = (area.size.Y - 2 * cornerHeight);
            float horizontalScale = horizontalWidth / cornerWidth;
            float verticalScale = verticalHeight / cornerHeight;

            Color color = new Color((byte)area.colorR, (byte)area.colorG, (byte)area.colorB, (byte)(255 * opacity));

            Vector2f p = area.position;
            Vector2f s = new Vector2f(1, 1);
            areaTopLeftBase.Position = p;
            areaTopLeftBase.Color = color;
            areaTopLeftBorder.Position = p;

            p = area.position + new Vector2f(cornerWidth, 0);
            s = new Vector2f(horizontalScale, 1);
            areaTopBase.Position = p;
            areaTopBase.Scale = s;
            areaTopBase.Color = color;
            areaTopBorder.Position = p;
            areaTopBorder.Scale = s;

            p = area.position + new Vector2f(cornerWidth, 0) + new Vector2f(horizontalWidth, 0);
            areaTopRightBase.Position = p;
            areaTopRightBase.Color = color;
            areaTopRightBorder.Position = p;

            p = area.position + new Vector2f(0, cornerHeight);
            s = new Vector2f(1, verticalScale);
            areaLeftBase.Position = p;
            areaLeftBase.Scale = s;
            areaLeftBase.Color = color;
            areaLeftBorder.Position = p;
            areaLeftBorder.Scale = s;

            p = area.position + new Vector2f(cornerWidth, cornerHeight);
            s = new Vector2f(horizontalScale, verticalScale);
            areaCenterBase.Position = p;
            areaCenterBase.Scale = s;
            areaCenterBase.Color = color;
            area.content.Position = p;

            p = area.position + new Vector2f(cornerWidth, cornerHeight) + new Vector2f(horizontalWidth, 0);
            s = new Vector2f(1, verticalScale);
            areaRightBase.Position = p;
            areaRightBase.Scale = s;
            areaRightBase.Color = color;
            areaRightBorder.Position = p;
            areaRightBorder.Scale = s;

            p = area.position + new Vector2f(0, cornerHeight) + new Vector2f(0, verticalHeight);
            areaBottomLeftBase.Position = p;
            areaBottomLeftBase.Color = color;
            areaBottomLeftBorder.Position = p;

            p = area.position + new Vector2f(cornerWidth, cornerHeight) + new Vector2f(0, verticalHeight);
            s = new Vector2f(horizontalScale, 1);
            areaBottomBase.Position = p;
            areaBottomBase.Scale = s;
            areaBottomBase.Color = color;
            areaBottomBorder.Position = p;
            areaBottomBorder.Scale = s;

            p = area.position + new Vector2f(cornerWidth, cornerHeight) + new Vector2f(horizontalWidth, verticalHeight);
            areaBottomRightBase.Position = p;
            areaBottomRightBase.Color = color;
            areaBottomRightBorder.Position = p;

            DrawSprite(target, areaTopLeftBase, Shadow.big);
            DrawSprite(target, areaTopBase, Shadow.big);
            DrawSprite(target, areaTopRightBase, Shadow.big);
            DrawSprite(target, areaLeftBase, Shadow.big);
            DrawSprite(target, areaCenterBase, Shadow.big);
            DrawSprite(target, areaRightBase, Shadow.big);
            DrawSprite(target, areaBottomLeftBase, Shadow.big);
            DrawSprite(target, areaBottomBase, Shadow.big);
            DrawSprite(target, areaBottomRightBase, Shadow.big);

            DrawSprite(target, areaTopLeftBorder);
            DrawSprite(target, areaTopBorder);
            DrawSprite(target, areaTopRightBorder);
            DrawSprite(target, areaLeftBorder);
            DrawSprite(target, areaRightBorder);
            DrawSprite(target, areaBottomBorder);
            DrawSprite(target, areaBottomLeftBorder);
            DrawSprite(target, areaBottomRightBorder);

            DrawSprite(target, area.content);
        }

        public static void DrawSplash(RenderTarget target, bool withCloseButton = true)
        {
            DrawArea(splashArea, target);
            if(withCloseButton) { DrawSprite(target, splashCloseButtonSprite, Shadow.small); }
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

        public static Vector2f TurtlePositionToScreen(float x, float y, RenderTarget target)
        {
            return new Vector2f(target.Size.X / 2, target.Size.Y / 2) + new Vector2f(x, -y) * AppConfig.pixelsPerStep;
        }

        public static float TurtleAngleToScreenRotation(float a)
        {
            return -a - 90;
        }

        public static Vector2f ScreenPositionToTurtle(float x, float y, RenderTarget target)
        {
            Vector2f p = new Vector2f(x, y) - new Vector2f(target.Size.X / 2, target.Size.Y / 2);
            return new Vector2f(p.X, -p.Y) / AppConfig.pixelsPerStep;
        }

        static float ScreenRotationToTurtleAngle(float a)
        {
            return -(a + 90);
        }

        static int ReferenceToBottomCenterX(int x) { return ReferenceToAnchorX(x, Anchor.bottomCenter); }
        static int ReferenceToBottomCenterY(int y) { return ReferenceToAnchorY(y, Anchor.bottomCenter); }
        static int ReferenceToCenterX(int x) { return ReferenceToAnchorX(x, Anchor.center); }
        static int ReferenceToCenterY(int y) { return ReferenceToAnchorY(y, Anchor.center); }
        static int ReferenceToBottomLeftX(int x) { return ReferenceToAnchorX(x, Anchor.bottomLeft); }
        static int ReferenceToBottomLeftY(int y) { return ReferenceToAnchorY(y, Anchor.bottomLeft); }
        static int ReferenceToBottomRightX(int x) { return ReferenceToAnchorX(x, Anchor.bottomRight); }
        static int ReferenceToBottomRightY(int y) { return ReferenceToAnchorY(y, Anchor.bottomRight); }

        static Vector2f ReferenceToBottomCenter(Vector2f p) { return ReferenceToAnchor(p, Anchor.bottomCenter); }
        static Vector2f ReferenceToCenter(Vector2f p) { return ReferenceToAnchor(p, Anchor.center); }
        static Vector2f ReferenceToBottomLeft(Vector2f p) { return ReferenceToAnchor(p, Anchor.bottomLeft); }
        static Vector2f ReferenceToBottomRight(Vector2f p) { return ReferenceToAnchor(p, Anchor.bottomRight); }

        static int ReferenceToAnchorX(int x, Anchor anchor)
        {
            RenderWindow window = App.GetWindow();

            int transformed;

            if(anchor == Anchor.bottomCenter)
            {
                float offset = (float)(window.Size.X - AppConfig.referenceWindowWidth) / 2;
                transformed = (int)(offset + x);

            }
            else if(anchor == Anchor.center)
            {
                transformed = (int)(window.Size.X / 2.0f + (x - AppConfig.referenceWindowWidth / 2.0f));
            }
            else if (anchor == Anchor.bottomLeft)
            {
                transformed = x;
            }
            else // if(anchor == Anchor.bottomRight)
            {
                transformed = (int)(window.Size.X - (AppConfig.referenceWindowWidth - x));
            }

            return transformed;
        }

        static int ReferenceToAnchorY(int y, Anchor anchor)
        {
            RenderWindow window = App.GetWindow();

            int transformed;

            if(anchor == Anchor.bottomCenter || anchor == Anchor.bottomLeft || anchor == Anchor.bottomRight)
            {
                transformed = (int)(window.Size.Y - (AppConfig.referenceWindowHeight - y));
            }
            else // if(anchor == Anchor.center)
            {
                transformed = (int)(window.Size.Y / 2.0f + (y - AppConfig.referenceWindowHeight / 2.0f));
            }

            return transformed;
        }

        static Vector2f ReferenceToAnchor(Vector2f p, Anchor anchor)
        {
            return new Vector2f(ReferenceToAnchorX((int)p.X, anchor), ReferenceToAnchorY((int)p.Y, anchor));
        }


        static void DrawSprite(RenderTarget target, Sprite sprite, Shadow shadow = Shadow.none)
        {

            Color previousColor = sprite.Color;

            if (AppConfig.shadowsEnabled && shadow != Shadow.none)
            {
                Vector2f previousPosition = sprite.Position;
                float amount = (shadow == Shadow.big ? 30 : 20);
                int steps = (shadow == Shadow.big ? 8 : 4);

                for (int i = 0; i < steps; i++)
                {
                    sprite.Position += new Vector2f(1, 1);
                    sprite.Color = new Color(0, 0, 0, (byte)((steps - i - 1) / amount * previousColor.A * opacity));
                    target.Draw(sprite);
                }

                sprite.Position = previousPosition;
            }

            sprite.Color = new Color(previousColor.R, previousColor.G, previousColor.B, (byte)(previousColor.A * opacity));
            target.Draw(sprite);
            sprite.Color = previousColor;
        }

        static void DrawText(RenderTarget target, Text text, Shadow shadow = Shadow.none)
        {
            Color previousColor = text.FillColor;

            if (AppConfig.shadowsEnabled && shadow != Shadow.none)
            {
                Vector2f previousPosition = text.Position;
                float amount = (shadow == Shadow.big ? 30 : 20);
                int steps = (shadow == Shadow.big ? 8 : 4);

                for (int i = 0; i < steps; i++)
                {
                    text.Position += new Vector2f(1, 1);
                    text.FillColor = new Color(0, 0, 0, (byte)((steps - i - 1) / amount * previousColor.A * opacity));
                    target.Draw(text);
                }

                text.Position = previousPosition;
            }

            text.FillColor = new Color(previousColor.R, previousColor.G, previousColor.B, (byte)(previousColor.A * opacity));
            target.Draw(text);
            text.FillColor = previousColor;
        }

        static void OnKeyPressed(object sender, KeyEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;

            if (e.Code == Keyboard.Key.Escape)
            {
                window.Close();
            }

            if(isTransitioning) { return; }

            if(screenId == ScreenId.PlayMode || screenId == ScreenId.BrushMode)
            {
                if(screenId == ScreenId.PlayMode)
                {
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
                        TracePlayer.PlayState playState = TracePlayer.GetPlayState();
                        if (playState == TracePlayer.PlayState.playing) { TracePlayer.Stop(); }
                        else if (playState == TracePlayer.PlayState.stopped) { TracePlayer.Play(); }
                    }
                }
                else // screenId == ScreenId.PlayMode
                {
                    if (e.Control && e.Code == Keyboard.Key.Z)
                    {
                        Undo();
                    }
                    else if (e.Control && e.Code == Keyboard.Key.Y)
                    {
                        Redo();
                    }
                    else if (e.Control && e.Code == Keyboard.Key.N)
                    {
                        New();
                    }
                    else if (e.Control && e.Code == Keyboard.Key.S)
                    {
                        Save();
                    }
                    else if (e.Control && e.Code == Keyboard.Key.O)
                    {
                        Load();
                    }
                }


                if (e.Code == Keyboard.Key.M)
                {
                    App.SwitchMusic();
                }
                else if (e.Code == Keyboard.Key.H)
                {
                    UI.SwitchTurtle();
                }
                else if (e.Code == Keyboard.Key.I || e.Code == Keyboard.Key.Tab)
                {
                    Config.showToolbar = !Config.showToolbar;
                }
                else if (e.Code == Keyboard.Key.G)
                {
                    UI.SwitchGrid();
                }
                else if (e.Code == Keyboard.Key.Num1)
                {
                    if (screenId == ScreenId.PlayMode) { PlayMode.SetPlayIndex(0); }
                    else { BrushMode.SetBrushIndex(0); }
                }
                else if (e.Code == Keyboard.Key.Num2)
                {
                    if (screenId == ScreenId.PlayMode) { PlayMode.SetPlayIndex(1); }
                    else { BrushMode.SetBrushIndex(1); }
                }
                else if (e.Code == Keyboard.Key.Num3)
                {
                    if (screenId == ScreenId.PlayMode) { PlayMode.SetPlayIndex(2); }
                    else { BrushMode.SetBrushIndex(2); }
                }
                else if (e.Code == Keyboard.Key.Num4)
                {
                    if (screenId == ScreenId.PlayMode) { PlayMode.SetPlayIndex(3); }
                    else { BrushMode.SetBrushIndex(3); }
                }
                else if (e.Code == Keyboard.Key.Num5)
                {
                    if (screenId == ScreenId.PlayMode) { PlayMode.SetPlayIndex(4); }
                    else { BrushMode.SetBrushIndex(4); }
                }
                else if (e.Code == Keyboard.Key.Num6)
                {
                    if (screenId == ScreenId.PlayMode) { PlayMode.SetPlayIndex(5); }
                    else { BrushMode.SetBrushIndex(5); }
                }
                else if (e.Code == Keyboard.Key.Num7)
                {
                    if (screenId == ScreenId.PlayMode) { PlayMode.SetPlayIndex(6); }
                    else { BrushMode.SetBrushIndex(6); }
                }
                else if (e.Code == Keyboard.Key.Num8)
                {
                    if (screenId == ScreenId.PlayMode) { PlayMode.SetPlayIndex(7); }
                    else { BrushMode.SetBrushIndex(7); }
                }
                else if (e.Code == Keyboard.Key.Num9)
                {
                    if (screenId == ScreenId.PlayMode) { PlayMode.SetPlayIndex(8); }
                    else { BrushMode.SetBrushIndex(8); }
                }
                else if (e.Code == Keyboard.Key.C)
                {
                    App.TakeScreenshot();
                }
                else if (e.Code == Keyboard.Key.A)
                {
                    UI.SwitchSplash();
                }

            }

        }

        public static void ClearStrokePreview()
        {
            strokePreview.Clear();
        }

        public static void AddStrokePreview(Vector2f point)
        {
            strokePreview.Add(point);
        }

        public static void UpdateStrokePreview(Vector2f point)
        {
            strokePreview[strokePreview.Count - 1] = point;
        }


        public static void DrawStrokePreview(RenderTarget target)
        {
            IntRect rect = strokePreviewSprite.TextureRect;
            rect.Height = (int)BrushMode.GetStrokeLength();
            strokePreviewSprite.TextureRect = rect;

            for (int i = 0; i < strokePreview.Count - 1; i ++)
            {
                Vector2f p1 = strokePreview[i];
                Vector2f p2 = strokePreview[i + 1];
                float aX = p2.X - p1.X;
                float aY = p2.Y - p1.Y;
                float rotation = MathF.Atan2(aY, aX) * 180 / MathF.PI - 90;
                float length = MathF.Sqrt(aX * aX + aY * aY) * AppConfig.pixelsPerStep;
                strokePreviewSprite.Position = p1;
                strokePreviewSprite.Rotation = rotation;
                strokePreviewSprite.Scale = new Vector2f(Config.lineWidth / 50.0f, length / rect.Height);
                strokePreviewSprite.Color = toolbarLightColor1;
                DrawSprite(target, strokePreviewSprite);

            }
        }

        static void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;

            if (isTransitioning) { return; }

            bool processed = false;

            if (screenId == ScreenId.SelectMode)
            {
                Vector2f wp = (Vector2f)Mouse.GetPosition(window);

                if (GetAreaRect(selectPlayModeArea).Contains(wp)) { App.OnPlayModeSelected(); processed = true; }
                else if (GetAreaRect(selectBrushModeArea).Contains(wp)) { App.OnBrushModeSelected(); processed = true; }
            }
            else if(screenId == ScreenId.PlayMode || screenId == ScreenId.BrushMode)
            {
                if (e.Button == Mouse.Button.Left)
                {
                    if (buttonScreenshotSprite.GetGlobalBounds().Contains(e.X, e.Y))
                    {
                        App.TakeScreenshot();
                        processed = true;
                    }
                    else if (buttonTurtleSprite.GetGlobalBounds().Contains(e.X, e.Y))
                    {
                        UI.SwitchTurtle();
                        processed = true;
                    }
                    else if (buttonGridSprite.GetGlobalBounds().Contains(e.X, e.Y))
                    {
                        UI.SwitchGrid();
                        processed = true;
                    }
                    else if (buttonSandColorSprite.GetGlobalBounds().Contains(e.X, e.Y))
                    {
                        NextBackgroundColorIndex();

                        StringBuilder textBuilder = App.GetTextBuilder();
                        textBuilder.Clear();
                        textBuilder.AppendFormat(Texts.Get(Texts.Id.sandColor), backgroundColorIndex + 1);
                        UI.AddInfoMessage(textBuilder.ToString(), UI.InfoMessagePosition.UtilsToolbar);

                        processed = true;
                    }
                    else if (buttonMusicSprite.GetGlobalBounds().Contains(e.X, e.Y))
                    {
                        App.SwitchMusic();
                        processed = true;
                    }
                    else if(buttonSplashSprite.GetGlobalBounds().Contains(e.X, e.Y))
                    {
                        UI.SwitchSplash();
                        processed = true;
                    }
                    else if(showSplash && splashCloseButtonSprite.GetGlobalBounds().Contains(e.X, e.Y))
                    {
                        showSplash = false;
                        processed = true;
                    }
                    else if (selectorNextSprite.GetGlobalBounds().Contains(e.X, e.Y))
                    {
                        if(screenId == ScreenId.PlayMode) { PlayMode.NextPlayIndex(); }
                        else { BrushMode.NextBrushIndex(); }
                        processed = true;
                    }
                    else if (selectorPreviousSprite.GetGlobalBounds().Contains(e.X, e.Y))
                    {
                        if (screenId == ScreenId.PlayMode) { PlayMode.PreviousPlayIndex(); }
                        else { BrushMode.PreviousBrushIndex(); }
                        processed = true;
                    }
                }

                if (screenId == ScreenId.PlayMode)
                {
                    if (e.Button == Mouse.Button.Left)
                    {
                        // Playback toolbar

                        TracePlayer.PlayState playState = TracePlayer.GetPlayState();

                        if (buttonPlaySprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            if (playState == TracePlayer.PlayState.playing) { TracePlayer.Stop(); }
                            else if (playState == TracePlayer.PlayState.stopped) { TracePlayer.Play(); }
                            processed = true;
                        }
                        else if (buttonRestartSprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            TracePlayer.SetStep(0);
                            TracePlayer.Play();
                            processed = true;

                        }
                        else if (buttonForwardSprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            TracePlayer.StepForward();
                            TracePlayer.Stop();
                            processed = true;
                        }
                        else if (buttonBackwardsSprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            TracePlayer.StepBackward();
                            TracePlayer.Stop();
                            processed = true;

                        }
                    }
                }
                else // screenId == ScreenId.BrushMode
                {
                    if (screenId == ScreenId.BrushMode && !processed)
                    {
                        if (buttonSizeSprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            BrushSizeUp(true);

                            processed = true;
                        }
                        else if (buttonColorSprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            BrushMode.NextBrushColorIndex();

                            StringBuilder textBuilder = App.GetTextBuilder();
                            textBuilder.Clear();
                            textBuilder.AppendFormat(Texts.Get(Texts.Id.brushColor), BrushMode.GetBrushColorIndex() + 1);
                            UI.AddInfoMessage(textBuilder.ToString(), UI.InfoMessagePosition.StrokeToolbar);

                            processed = true;
                        }
                        else if (buttonOpacitySprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            BrushMode.NextBrushOpacityIndex();

                            StringBuilder textBuilder = App.GetTextBuilder();
                            textBuilder.Clear();
                            textBuilder.AppendFormat(Texts.Get(Texts.Id.brushOpacity), (int)(BrushMode.GetBrushOpacity() / 255 * 100));
                            UI.AddInfoMessage(textBuilder.ToString(), UI.InfoMessagePosition.StrokeToolbar);

                            processed = true;
                        }
                        else if (buttonLengthSprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            StrokeLengthUp(true);

                            processed = true;
                        }
                        else if (buttonUndoSprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            Undo();
                            processed = true;
                        }
                        else if (buttonRedoSprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            Redo();
                            processed = true;
                        }
                        else if (buttonNewSprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            New();
                            processed = true;
                        }
                        else if (buttonLoadSprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            Load();
                            processed = true;
                        }
                        else if (buttonSaveSprite.GetGlobalBounds().Contains(e.X, e.Y))
                        {
                            Save();
                            processed = true;
                        }
                        else if(e.Button == Mouse.Button.Left)
                        {
                            BrushMode.BeginStroke(Mouse.GetPosition(window), window);
                            recordingStroke = true;
                            processed = true;
                        }
                        else if (e.Button == Mouse.Button.Right && !recordingStroke)
                        {
                            BrushMode.NextBrushIndex();
                            processed = true;
                        }
                    }

                }
            }


        }

        static void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;

            if (isTransitioning) { return; }

            if (screenId == ScreenId.BrushMode)
            {
                BrushMode.UpdateStroke(Mouse.GetPosition(window), window);
            }
        }

        static void OnMouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
        {
            if(screenId == ScreenId.BrushMode)
            {
                bool control = Keyboard.IsKeyPressed(Keyboard.Key.LControl) ||
                               Keyboard.IsKeyPressed(Keyboard.Key.RControl);

                if(control)
                {
                    if(e.Delta > 0) { StrokeLengthUp(false); }
                    else { StrokeLengthDown(false); }
                }
                else
                {
                    if (e.Delta > 0) { BrushSizeUp(false); }
                    else { BrushSizeDown(false); }
                }
            }
        }

        static void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;

            if (isTransitioning) { return; }

            if(screenId == ScreenId.BrushMode)
            {
                if(e.Button == Mouse.Button.Left && recordingStroke)
                {
                    BrushMode.EndStroke(Mouse.GetPosition(window), window);
                    recordingStroke = false;
                }
            }

        }

        static void Undo()
        {
            BrushMode.UndoStrokeSequence();
            UI.AddInfoMessage(Texts.Get(Texts.Id.undo), UI.InfoMessagePosition.UndoToolbar);
        }

        static void Redo()
        {
            BrushMode.RedoStrokeSequence();
            UI.AddInfoMessage(Texts.Get(Texts.Id.redo), UI.InfoMessagePosition.UndoToolbar);
        }

        static void New()
        {
            BrushMode.NewStrokeList();
            UI.AddInfoMessage(Texts.Get(Texts.Id.clear), UI.InfoMessagePosition.FileToolbar);
        }

        static void Save()
        {
            BrushMode.SaveStrokeList();
            UI.AddInfoMessage(Texts.Get(Texts.Id.save), UI.InfoMessagePosition.FileToolbar);

        }

        static void Load()
        {
            BrushMode.LoadStrokeList();
            UI.AddInfoMessage(Texts.Get(Texts.Id.load), UI.InfoMessagePosition.FileToolbar);

        }

        static void StrokeLengthUp(bool loop = true)
        {
            BrushMode.NextStrokeLengthIndex(loop);

            StringBuilder textBuilder = App.GetTextBuilder();
            textBuilder.Clear();
            textBuilder.AppendFormat(Texts.Get(Texts.Id.strokeLength), BrushMode.GetStrokeLength());
            UI.AddInfoMessage(textBuilder.ToString(), UI.InfoMessagePosition.StrokeToolbar);

        }

        static void StrokeLengthDown(bool loop = true)
        {
            BrushMode.PreviousStrokeLengthIndex(loop);

            StringBuilder textBuilder = App.GetTextBuilder();
            textBuilder.Clear();
            textBuilder.AppendFormat(Texts.Get(Texts.Id.strokeLength), BrushMode.GetStrokeLength());
            UI.AddInfoMessage(textBuilder.ToString(), UI.InfoMessagePosition.StrokeToolbar);

        }

        static void BrushSizeUp(bool loop = true)
        {
            BrushMode.NextBrushSizeIndex(loop);

            StringBuilder textBuilder = App.GetTextBuilder();
            textBuilder.Clear();
            textBuilder.AppendFormat(Texts.Get(Texts.Id.brushSize), BrushMode.GetBrushSize() + 1);
            UI.AddInfoMessage(textBuilder.ToString(), UI.InfoMessagePosition.StrokeToolbar);

        }

        static void BrushSizeDown(bool loop = true)
        {
            BrushMode.PreviousBrushSizeIndex(loop);

            StringBuilder textBuilder = App.GetTextBuilder();
            textBuilder.Clear();
            textBuilder.AppendFormat(Texts.Get(Texts.Id.brushSize), BrushMode.GetBrushSize() + 1);
            UI.AddInfoMessage(textBuilder.ToString(), UI.InfoMessagePosition.StrokeToolbar);

        }

        static void TakeScreenshot(RenderWindow window)
        {
            Texture texture = new Texture(window.Size.X, window.Size.Y);
            texture.Update(window);
            Image image = texture.CopyToImage();
            StringBuilder textBuilder = App.GetTextBuilder();

            bool done = false;
            int index = 0;
            while (index < 1000 && !done)
            {
                textBuilder.Clear();
                textBuilder.AppendFormat(Texts.Get(Texts.Id.screenshotFilename), index);
                string fileName = textBuilder.ToString();

                if (!File.Exists(fileName))
                {
                    UI.AddInfoMessage(Texts.Get(Texts.Id.screenshotSaved), UI.InfoMessagePosition.UtilsToolbar);
                    image.SaveToFile(fileName);
                    done = true;
                }
                else { index++; }


            }

        }

        static void OnWindowClosed(object sender, EventArgs e)
        {
            if(screenId == ScreenId.BrushMode) { BrushMode.SaveStrokeList(true); }
            App.Quit();
        }


    }
}
