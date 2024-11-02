using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TurtleSandbox
{
    internal partial class BrushMode
    {
        static int brushIndex = 0;

        static Turtle turtle;

        struct Stroke
        {
            public float startPosX;
            public float startPosY;
            public float endPosX;
            public float endPosY;
            public float distance;
            public float startAngle;
            public float endAngle;
            public float segmentProgress;
        };

        static List<Stroke> strokeList;
        static bool recordingStroke;
        static Stroke stroke;
        static float segmentProgress;

        public static void Init()
        {
            brushIndex = Config.brush - 1;

            turtle = App.GetTurtle();

            turtle.Reset();

            List<Turtle.Step> trace = turtle.GetTrace();
            TracePlayer.SetTrace(trace);
            TracePlayer.SetStep(0);
            TracePlayer.Stop();

            strokeList = new List<Stroke>(1000);
            stroke = new Stroke();
        }

        public static int GetBrushIndex()
        {
            return brushIndex;
        }

        public static void SetBrushIndex(int index)
        {
            brushIndex = index;
        }

        public static void NextBrushIndex()
        {
            if (brushIndex + 1 >= AppConfig.playsCount) { brushIndex = 0; }
            else { brushIndex++; }

        }

        public static void PreviousBrushIndex()
        {
            if (brushIndex - 1 < 0) { brushIndex = AppConfig.brushesCount - 1; }
            else { brushIndex--; }
        }

        public static void Update()
        {

        }

        public static void BeginStroke(Vector2i screenPosition, RenderWindow window)
        {
            if(TracePlayer.GetPlayState() == TracePlayer.PlayState.stopped)
            {
                if(!recordingStroke)
                {
                    recordingStroke = true;
                    UI.ClearStrokePreview();
                    Vector2f tp = UI.ScreenPositionToTurtle(screenPosition.X, screenPosition.Y, window);
                    stroke.startPosX = tp.X;
                    stroke.startPosY = tp.Y;
                    UI.AddStrokePreview((Vector2f)screenPosition);
                    segmentProgress = 0;
                }
            }

        }

        public static void UpdateStroke(Vector2i screenPosition, RenderWindow window)
        {
            if (TracePlayer.GetPlayState() == TracePlayer.PlayState.stopped)
            {
                if (recordingStroke)
                {
                    Vector2f tp1 = new Vector2f(stroke.startPosX, stroke.startPosY);
                    Vector2f tp2 = UI.ScreenPositionToTurtle(screenPosition.X, screenPosition.Y, window);


                    Vector2f ap = tp2 - tp1;
                    float distance = MathF.Sqrt(ap.X * ap.X + ap.Y * ap.Y);
                    if (distance >= AppConfig.strokeMinLength)
                    {
                        UI.AddStrokePreview((Vector2f)screenPosition);
                        stroke.endPosX = tp2.X;
                        stroke.endPosY = tp2.Y;
                        stroke.distance = distance;
                        stroke.startAngle = MathF.Atan2(ap.Y, ap.X) * 180 / MathF.PI;
                        stroke.segmentProgress = segmentProgress;

                        strokeList.Add(stroke);

                        segmentProgress = 1;

                        stroke = new Stroke();
                        stroke.startPosX = tp2.X;
                        stroke.startPosY = tp2.Y;
                    }
                    else
                    {
                        UI.UpdateStrokePreview((Vector2f)screenPosition);
                    }

                }
            }
        }

        public static void EndStroke(Vector2i screenPosition, RenderWindow window)
        {
            if (TracePlayer.GetPlayState() == TracePlayer.PlayState.stopped)
            {
                Vector2f tp = UI.ScreenPositionToTurtle(screenPosition.X, screenPosition.Y, window);
                stroke.endPosX = tp.X;
                stroke.endPosY = tp.Y;
                stroke.distance = MathF.Sqrt(MathF.Pow(stroke.startPosX - tp.X, 2) + MathF.Pow(stroke.startPosY - tp.Y, 2));
                float aX = tp.X - stroke.startPosX;
                float aY = tp.Y - stroke.startPosY;            
                stroke.startAngle = MathF.Atan2(aY, aX) * 180 / MathF.PI;
                stroke.segmentProgress = segmentProgress;

                strokeList.Add(stroke);

                int i = strokeList.Count - 1;
                int segmentStart;

                while((int)(strokeList[i].segmentProgress)!= 0) { i --; }
                segmentStart = i;

                int segmentTotal = strokeList.Count - segmentStart;

                for(i = segmentStart; i < strokeList.Count; i++)
                {
                    stroke = strokeList[i];
                    stroke.segmentProgress = (i - segmentStart) / (float)segmentTotal;

                    if(i < strokeList.Count - 1) { stroke.endAngle = strokeList[i + 1].startAngle; }
                    else { stroke.endAngle = stroke.startAngle; }

                    strokeList[i] = stroke;
                }

                for(i = segmentStart; i < strokeList.Count; i++)
                {
                    stroke = strokeList[i];

                    turtle.Teleport(stroke.startPosX, stroke.startPosY, stroke.startAngle);
                    RunBrush();

                }

                recordingStroke = false;

                TracePlayer.SetEndStep();
            }

        }

        static void RunBrush()
        {
            if (brushIndex == 0) { Brush1(); }
            else if (brushIndex == 1) { Brush2(); }
            else if (brushIndex == 2) { Brush3(); }
            else if (brushIndex == 3) { Brush4(); }
            else if (brushIndex == 4) { Brush5(); }
            else if (brushIndex == 5) { Brush6(); }
            else if (brushIndex == 6) { Brush7(); }
            else if (brushIndex == 7) { Brush8(); }
            else // brushIndex == 8
            { Brush9(); }
        }

    }
}
