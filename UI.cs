using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System.Text;


namespace TurtleSandbox
{
    internal partial class Program
    {
        const float infoTextX = 20;
        const float infoTextY = 666;
        const float infoTextScale = 1.0f;

        const int infoMessagesCount = 50;
        const float infoMessageDuration = 2.0f;
        const float infoMessageDistance = 150;
        const float infoMessageOffset = 35;
        const float infoMessageScale = 0.8f;

        const float buttonBar1Scale = 1.0f;
        const float buttonBar1X = 776;
        const float buttonBar1Y = 666;
        const float buttonBarSeparation1 = 6;

        const float buttonBar2Scale = 1.0f;
        const float buttonBar2X = 10;
        const float buttonBar2Y = 666;
        const float buttonBarSeparation2 = 2;

        const bool showPlayButtons = false;

        enum TextId
        {
            screenshotSaved,
            musicOn,
            musicOff,
            screenshotFilename,
            infoString,
            turtleOn,
            turtleOff,
            gridOn,
            gridOff

        };

        static Dictionary<TextId, string> texts;

        static Font font;
        static Text infoText;
        static int infoPosX;
        static int infoPosY;
        static int infoAngle;

        static Text[] infoMessages;
        static bool[] infoMessagesFree;
        static float[] infoMessagesLifetime;
        static Vector2f[] infoMessagesPosition;
        static Dictionary<Turtle.OrderId, string> orderIdToString;

        static Texture buttonPlayTexture;
        static Texture buttonPauseTexture;
        static Texture buttonForwardTexture;
        static Texture buttonBackwardsTexture;
        static Texture buttonRestartTexture;
        static Texture buttonScreenshotTexture;
        static Texture buttonMusicOnTexture;
        static Texture buttonMusicOffTexture;
        static Texture buttonTurtleOnTexture;
        static Texture buttonTurtleOffTexture;
        static Texture buttonGridOnTexture;
        static Texture buttonGridOffTexture;
        static Texture[] buttonPlayTexturesOn;
        static Texture[] buttonPlayTexturesOff;

        static Sprite buttonPlaySprite;
        static Sprite buttonPauseSprite;
        static Sprite buttonForwardSprite;
        static Sprite buttonBackwardsSprite;
        static Sprite buttonRestartSprite;
        static Sprite buttonScreenshotSprite;
        static Sprite buttonMusicSprite;
        static Sprite buttonTurtleSprite;
        static Sprite buttonGridSprite;
        static Sprite[] buttonPlaySprites;



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

        static void InitUI()
        {
            // Init texts

            texts = new Dictionary<TextId, string>();
            texts[TextId.screenshotSaved] = "Screenshot saved";
            texts[TextId.musicOn] = "Music on";
            texts[TextId.musicOff] = "Music off";
            texts[TextId.screenshotFilename] = "screenshot{0:000}.png";
            texts[TextId.infoString] = "Step {0,4} X {1,3} Y {2,3} Angle {3,3}";
            texts[TextId.gridOn] = "Grid on";
            texts[TextId.gridOff] = "Grid off";
            texts[TextId.turtleOn] = "Turtle visible";
            texts[TextId.turtleOff] = "Turtle hidden";

            // Init info text

            infoText = new Text();
            infoText.Position = new Vector2f(infoTextX, infoTextY);
            infoText.FillColor = new Color((byte)infoR, (byte)infoG, (byte)infoB);
            infoText.Scale = new Vector2f(infoTextScale, infoTextScale);
            font = new Font("Assets/Font.ttf");
            infoText.Font = font;

            textBuilder.Clear();
            infoText.DisplayedString = textBuilder.ToString();
            infoPosX = 0;
            infoPosY = 0;
            infoAngle = 0;
            orderIdToString = new Dictionary<Turtle.OrderId, string>();
            orderIdToString[Turtle.OrderId.origin] = "origin";
            orderIdToString[Turtle.OrderId.walk] = "walk";
            orderIdToString[Turtle.OrderId.turn] = "turn";
            orderIdToString[Turtle.OrderId.randTurn] = "randTurn";
            orderIdToString[Turtle.OrderId.randWalk] = "randWalk";
            orderIdToString[Turtle.OrderId.teleport] = "teleport";

            // Init button bars

            buttonPlayTexture = new Texture("Assets/Buttons/Play.png");
            buttonPauseTexture = new Texture("Assets/Buttons/Pause.png");
            buttonForwardTexture = new Texture("Assets/Buttons/Forward.png");
            buttonBackwardsTexture = new Texture("Assets/Buttons/Backward.png");
            buttonRestartTexture = new Texture("Assets/Buttons/Restart.png");
            buttonScreenshotTexture = new Texture("Assets/Buttons/Screenshot.png");
            buttonMusicOnTexture = new Texture("Assets/Buttons/MusicOn.png");
            buttonMusicOffTexture = new Texture("Assets/Buttons/MusicOff.png");
            buttonTurtleOnTexture = new Texture("Assets/Buttons/TurtleOn.png");
            buttonTurtleOffTexture = new Texture("Assets/Buttons/TurtleOff.png");
            buttonGridOnTexture = new Texture("Assets/Buttons/GridOn.png");
            buttonGridOffTexture = new Texture("Assets/Buttons/GridOff.png");

            buttonPlayTexturesOn = new Texture[playsCount];
            buttonPlayTexturesOff = new Texture[playsCount];

            for (int i = 0; i < playsCount; i++)
            {
                textBuilder.Clear();
                textBuilder.AppendFormat("Assets/Buttons/{0}On.png", i + 1);
                buttonPlayTexturesOn[i] = new Texture(textBuilder.ToString());
                textBuilder.Clear();
                textBuilder.AppendFormat("Assets/Buttons/{0}Off.png", i + 1);
                buttonPlayTexturesOff[i] = new Texture(textBuilder.ToString());

            }

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
            buttonScreenshotSprite = new Sprite();
            buttonScreenshotSprite.Texture = buttonScreenshotTexture;
            buttonMusicSprite = new Sprite();
            buttonMusicSprite.Texture = playMusic ? buttonMusicOnTexture : buttonMusicOffTexture;
            buttonTurtleSprite = new Sprite();
            buttonTurtleSprite.Texture = turtleVisible ? buttonTurtleOnTexture: buttonTurtleOffTexture;
            buttonGridSprite = new Sprite();
            buttonGridSprite.Texture = showGrid ? buttonGridOnTexture : buttonGridOffTexture;

            buttonPlaySprites = new Sprite[playsCount];

            for (int i = 0; i < playsCount; i++)
            {
                buttonPlaySprites[i] = new Sprite();
                buttonPlaySprites[i].Texture = i == (play + 1) ? buttonPlayTexturesOn[i] : buttonPlayTexturesOff[i];
            }

            float buttonWidth = buttonPlayTexture.Size.X;
            buttonRestartSprite.Position = new Vector2f(buttonBar1X + 0 * buttonWidth + 0 * buttonBarSeparation1, buttonBar1Y);
            buttonBackwardsSprite.Position = new Vector2f(buttonBar1X + 1 * buttonWidth + 1 * buttonBarSeparation1, buttonBar1Y);
            buttonPlaySprite.Position = new Vector2f(buttonBar1X + 2 * buttonWidth + 2 * buttonBarSeparation1, buttonBar1Y);
            buttonPauseSprite.Position = new Vector2f(buttonBar1X + 2 * buttonWidth + 2 * buttonBarSeparation1, buttonBar1Y);
            buttonForwardSprite.Position = new Vector2f(buttonBar1X + 3 * buttonWidth + 3 * buttonBarSeparation1, buttonBar1Y);
            buttonTurtleSprite.Position = new Vector2f(buttonBar1X + 5 * buttonWidth + 5 * buttonBarSeparation1, buttonBar1Y);
            buttonGridSprite.Position = new Vector2f(buttonBar1X + 6 * buttonWidth + 6 * buttonBarSeparation1, buttonBar1Y);
            buttonMusicSprite.Position = new Vector2f(buttonBar1X + 7 * buttonWidth + 7 * buttonBarSeparation1, buttonBar1Y);
            buttonScreenshotSprite.Position = new Vector2f(buttonBar1X + 8 * buttonWidth + 8 * buttonBarSeparation1, buttonBar1Y);

            Vector2f bar1Scale = new Vector2f(buttonBar1Scale, buttonBar1Scale);
            buttonRestartSprite.Scale = bar1Scale;
            buttonBackwardsSprite.Scale = bar1Scale;
            buttonPlaySprite.Scale = bar1Scale;
            buttonPauseSprite.Scale = bar1Scale;
            buttonForwardSprite.Scale = bar1Scale;
            buttonTurtleSprite.Scale = bar1Scale;
            buttonGridSprite.Scale = bar1Scale;
            buttonScreenshotSprite.Scale = bar1Scale;

            Vector2f bar2Scale = new Vector2f(buttonBar2Scale, buttonBar2Scale);
            for (int i = 0; i < playsCount; i++)
            {
                buttonPlaySprites[i].Position = new Vector2f(buttonBar2X + i * buttonWidth * bar2Scale.X + i * buttonBarSeparation2, buttonBar2Y);
                buttonPlaySprites[i].Scale = bar2Scale;
            }

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

        static void UpdateUI(float elapsedTime)
        {
            for (int i = 0; i < infoMessages.Length; i++)
            {
                if (!infoMessagesFree[i])
                {
                    infoMessagesLifetime[i] += elapsedTime;
                    if (infoMessagesLifetime[i] >= infoMessageDuration) { infoMessagesFree[i] = true; }
                }
            }
        }

        static void DrawUI(RenderWindow window)
        {
            if(!showInfo) { return; }

            // Draw info text

            textBuilder.Clear();
            textBuilder.AppendFormat(texts[TextId.infoString], stepIndex, infoPosX, infoPosY, infoAngle);

            infoText.DisplayedString = textBuilder.ToString();
            window.Draw(infoText);

            // Draw button bars

            window.Draw(buttonBackwardsSprite);
            window.Draw(playing ? buttonPlaySprite : buttonPauseSprite);
            window.Draw(buttonForwardSprite);
            window.Draw(buttonRestartSprite);

            buttonTurtleSprite.Texture = (turtleVisible ? buttonTurtleOnTexture: buttonTurtleOffTexture);
            window.Draw(buttonTurtleSprite);

            buttonGridSprite.Texture = (showGrid ? buttonGridOnTexture : buttonGridOffTexture);
            window.Draw(buttonGridSprite);
            window.Draw(buttonScreenshotSprite);

            buttonMusicSprite.Texture = (IsMusicPlaying() ? buttonMusicOnTexture : buttonMusicOffTexture);
            window.Draw(buttonMusicSprite);

            if(showPlayButtons)
            {
                for (int i = 0; i < playsCount; i++)
                {
                    if (i == playIndex) { buttonPlaySprites[i].Texture = buttonPlayTexturesOn[i]; }
                    else { buttonPlaySprites[i].Texture = buttonPlayTexturesOff[i]; }
                    window.Draw(buttonPlaySprites[i]);
                }
            }


            // Draw info messages

            for (int i = 0; i < infoMessages.Length; i++)
            {
                if (!infoMessagesFree[i])
                {
                    float factor = infoMessagesLifetime[i] / infoMessageDuration;
                    float opacityFactor = MathF.Pow(1 - factor, 3);

                    Vector2f position = infoMessagesPosition[i];
                    infoMessages[i].Position = position + new Vector2f(0, -infoMessageOffset - infoMessageDistance * factor);
                    infoMessages[i].FillColor = new Color((byte)infoR, (byte)infoG, (byte)infoB, (byte)(255 * opacityFactor));
                    window.Draw(infoMessages[i]);

                }
            }
        }

    }
}
