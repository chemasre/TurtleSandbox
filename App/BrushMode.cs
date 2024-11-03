using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TurtleSandbox
{
    internal partial class BrushMode
    {
        static int turtleSpeed = 2;

        static int brushIndex = 0;
        static int brushColorIndex = 0;
        static int brushSizeIndex = 0;
        static int brushOpacityIndex = 0;

        static Turtle turtle;

        public static Color[] brushColors = new Color[] {  new Color(26, 28, 44),
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

        public static float[] brushSizes = new float[] {
                                                            3.0f,
                                                            6.0f,
                                                            9.0f,
                                                            12.0f,
                                                            15.0f,
                                                };

        public static float[] brushOpacities = new float[] {
                                                            1.0f * 255,
                                                            0.9f * 255,
                                                            0.8f * 255,
                                                            0.7f * 255,
                                                            0.6f * 255,
                                                            0.5f * 255,
                                                            0.4f * 255,
                                                            0.3f * 255,
                                                            0.2f * 255,
                                                            0.1f * 255,
                                                };

        public struct Stroke
        {
            public float startPosX { get; set; }
            public float startPosY { get; set; }
            public float endPosX { get; set; }
            public float endPosY { get; set; }
            public float distance { get; set; }
            public float startAngle { get; set; }
            public float endAngle { get; set; }
            public float startPercent { get; set; }
            public float endPercent { get; set; }

            public int brushIndex { get; set; }
            public float brushColorR { get; set; }
            public float brushColorG { get; set; }
            public float brushColorB { get; set; }
            public float brushSize { get; set; }
            public float brushOpacity { get; set; }
        };


        static List<Stroke> strokeList;
        static List<Stroke> strokeUndoList;
        static bool recordingStroke;
        static Stroke stroke;
        static float progress;

        public static void Init()
        {
            brushIndex = Config.brush - 1;
            brushSizeIndex = Config.brushSize - 1;
            brushColorIndex = Config.brushColor - 1;
            brushOpacityIndex = Config.brushOpacity - 1;


            turtle = App.GetTurtle();

            turtle.Reset();

            List<Turtle.Step> trace = turtle.GetTrace();
            TracePlayer.SetTrace(trace);
            TracePlayer.SetStep(0);
            TracePlayer.Stop();

            strokeList = new List<Stroke>(1000);
            strokeUndoList = new List<Stroke>(1000);
            stroke = new Stroke();
        }

        public static int GetTurtleSpeed()
        {
            return turtleSpeed;
        }

        public static void NextTurtleSpeed()
        {
            if (turtleSpeed + 1 >= AppConfig.playsCount) { turtleSpeed = 0; }
            else { turtleSpeed++; }

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
                    strokeUndoList.Clear();
                    UI.ClearStrokePreview();
                    Vector2f tp = UI.ScreenPositionToTurtle(screenPosition.X, screenPosition.Y, window);
                    stroke.startPosX = tp.X;
                    stroke.startPosY = tp.Y;
                    UI.AddStrokePreview((Vector2f)screenPosition);
                    UI.AddStrokePreview((Vector2f)screenPosition);
                    progress = 0;
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
                        stroke.startPercent = progress;
                        stroke.brushIndex = brushIndex;
                        stroke.brushColorR = brushColors[brushColorIndex].R;
                        stroke.brushColorG = brushColors[brushColorIndex].G;
                        stroke.brushColorB = brushColors[brushColorIndex].B;
                        stroke.brushSize = brushSizes[brushSizeIndex];
                        stroke.brushOpacity = brushOpacities[brushOpacityIndex];

                        strokeList.Add(stroke);

                        progress = 1;

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
            if(!recordingStroke) { return; }

            if (TracePlayer.GetPlayState() == TracePlayer.PlayState.stopped)
            {
                Vector2f tp = UI.ScreenPositionToTurtle(screenPosition.X, screenPosition.Y, window);
                stroke.endPosX = tp.X;
                stroke.endPosY = tp.Y;
                stroke.distance = MathF.Sqrt(MathF.Pow(stroke.startPosX - tp.X, 2) + MathF.Pow(stroke.startPosY - tp.Y, 2));
                float aX = tp.X - stroke.startPosX;
                float aY = tp.Y - stroke.startPosY;            
                stroke.startAngle = MathF.Atan2(aY, aX) * 180 / MathF.PI;
                stroke.startPercent = progress;
                stroke.brushIndex = brushIndex;
                stroke.brushColorR = brushColors[brushColorIndex].R;
                stroke.brushColorG = brushColors[brushColorIndex].G;
                stroke.brushColorB = brushColors[brushColorIndex].B;
                stroke.brushSize = brushSizes[brushSizeIndex];
                stroke.brushOpacity = brushOpacities[brushOpacityIndex];

                strokeList.Add(stroke);

                // End stroke sequence

                int i = strokeList.Count - 1;
                int sequenceStart;

                while((int)(strokeList[i].startPercent)!= 0) { i --; }
                sequenceStart = i;

                int sequenceTotal = strokeList.Count - sequenceStart;

                for(i = sequenceStart; i < strokeList.Count; i++)
                {
                    stroke = strokeList[i];
                    stroke.startPercent = (i - sequenceStart) / (float)sequenceTotal;
                    strokeList[i] = stroke;
                }

                for (i = sequenceStart; i < strokeList.Count; i++)
                {
                    stroke = strokeList[i];

                    if (i < strokeList.Count - 1)
                    {
                        Stroke next = strokeList[i + 1];
                        stroke.endAngle = next.startAngle;
                        stroke.endPercent = next.startPercent;
                    }
                    else
                    {
                        stroke.endAngle = stroke.startAngle;
                        stroke.endPercent = 1;
                    }

                    strokeList[i] = stroke;
                }

                for (i = sequenceStart; i < strokeList.Count; i++)
                {
                    stroke = strokeList[i];

                    turtle.Color(stroke.brushColorR, stroke.brushColorG, stroke.brushColorB);
                    turtle.Opacity(stroke.brushOpacity);
                    turtle.Teleport(stroke.startPosX, stroke.startPosY, stroke.startAngle);
                    RunBrush(stroke.brushIndex);

                }

                recordingStroke = false;

                if(turtleSpeed == 0) { TracePlayer.Play(); }
                else if(turtleSpeed == 1) { TracePlayer.FastForward(); }
                else { TracePlayer.SetEndStep(); }

                UI.ClearStrokePreview();
            }

        }

        static void RunBrush(int index)
        {
            if (index == 0) { Brush1(); }
            else if (index == 1) { Brush2(); }
            else if (index == 2) { Brush3(); }
            else if (index == 3) { Brush4(); }
            else if (index == 4) { Brush5(); }
            else if (index == 5) { Brush6(); }
            else if (index == 6) { Brush7(); }
            else if (index == 7) { Brush8(); }
            else // index == 8
            { Brush9(); }
        }

        static void PrintStrokeList(bool undo = false, bool brief = false)
        {
            List<Stroke> l = undo ? strokeUndoList : strokeList;

            Console.WriteLine("-------- " + (undo ? "undo " : "stroke ") + "list -----------");

            for(int i = 0; i < l.Count; i++)
            {
                Stroke s = l[i];

                if(brief) { Console.WriteLine(i + ": %" + s.startPercent); }
                else
                {
                    Console.WriteLine("--------[" + i + "]-----------");
                    Console.WriteLine("StartPos......." + s.startPosX + ", " + s.startPosY);
                    Console.WriteLine("EndPos........." + s.endPosX + ", " + s.endPosY);
                    Console.WriteLine("Distance......." + s.distance);
                    Console.WriteLine("StartAngle....." + s.startAngle);
                    Console.WriteLine("EndAngle......." + s.endAngle);
                    Console.WriteLine("StartFraction.." + s.startPercent);
                    Console.WriteLine("EndFraction...." + s.endPercent);
                }
            }
        }

        public static int GetBrushOpacityIndex()
        {
            return brushOpacityIndex;
        }

        public static void NextBrushOpacityIndex()
        {
            if (brushOpacityIndex + 1 >= AppConfig.brushOpacitiesCount) { brushOpacityIndex = 0; }
            else { brushOpacityIndex++; }
        }


        public static int GetBrushSizeIndex()
        {
            return brushSizeIndex;
        }

        public static void NextBrushSizeIndex()
        {
            if(brushSizeIndex + 1 >= AppConfig.brushSizesCount) { brushSizeIndex = 0; }
            else { brushSizeIndex ++; }
        }

        public static int GetBrushColorIndex()
        {
            return brushColorIndex;
        }

        public static void NextBrushColorIndex()
        {
            if (brushColorIndex + 1 >= AppConfig.brushColorsCount) { brushColorIndex = 0; }
            else { brushColorIndex++; }
        }

        public static void UndoStrokeSequence()
        {
            if(strokeList.Count == 0) { return; }

            int count = 0;
            while (strokeList[strokeList.Count - 1 - count].startPercent != 0) { count ++; }
            count ++;

            for(int i = 0; i < count; i++)
            {
                strokeUndoList.Insert(0, strokeList[strokeList.Count - 1]);
                strokeList.RemoveAt(strokeList.Count - 1);
            }

            RunStrokeList();
            TracePlayer.SetEndStep();
        }

        public static void RedoStrokeSequence()
        {
            if (strokeUndoList.Count == 0) { return; }

            bool found = false;
            int count = 1;
            while (!found)
            {
                if (count >= strokeUndoList.Count) { found = true; }
                else if (strokeUndoList[count].startPercent == 0) { found = true; }
                else { count++; }
            }

            for(int i = 0; i < count; i++)
            {
                strokeList.Add(strokeUndoList[0]);
                strokeUndoList.RemoveAt(0);
            }

            RunStrokeList();
            TracePlayer.SetEndStep();
        }

        public static void NewStrokeList()
        {
            strokeList.Clear();
            strokeUndoList.Clear();

            RunStrokeList();
            TracePlayer.SetEndStep();
        }

        public static void LoadStrokeList()
        {
            strokeUndoList.Clear();

            if(File.Exists("strokes.json"))
            {
                string json = File.ReadAllText("strokes.json", Encoding.UTF8);
                strokeList = JsonSerializer.Deserialize< List<Stroke> >(json);
                RunStrokeList();
                TracePlayer.SetEndStep();
            }
        }

        public static void SaveStrokeList()
        {
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.WriteIndented = true;
            string json = JsonSerializer.Serialize(strokeList, jsonOptions);
            File.WriteAllText("strokes.json", json, Encoding.UTF8);
        }

        static void RunStrokeList()
        {
            turtle.Reset();

            for (int i = 0; i < strokeList.Count; i++)
            {
                stroke = strokeList[i];

                turtle.Color(stroke.brushColorR, stroke.brushColorG, stroke.brushColorB);
                turtle.Opacity(stroke.brushOpacity);
                turtle.Teleport(stroke.startPosX, stroke.startPosY, stroke.startAngle);
                RunBrush(stroke.brushIndex);

            }

        }

    }
}
