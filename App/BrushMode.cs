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
        static int strokeLengthIndex = 0;
        static int brushIndex = 0;
        static int brushColorIndex = 0;
        static int brushSizeIndex = 0;
        static int brushOpacityIndex = 0;

        static Turtle turtle;

        public static float[] strokeLengths = new float[] {
                                                            80.0f,
                                                            60.0f,
                                                            40.0f,
                                                            20.0f
                                                        };

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
            public float posX;
            public float posY;
            public float nextPosX;
            public float nextPosY;
            public float distance;
            public float angle;
            public float nextAngle;
            public float turn;
            public float progress;
            public float nextProgress;
        };

        public struct Brush
        {
            public float size;
            public float colorR;
            public float colorG;
            public float colorB;
            public float opacity;
        };

        public struct StrokeData
        {
            public float posX { get; set; }
            public float posY { get; set; }
            public float nextPosX { get; set; }
            public float nextPosY { get; set; }
            public float distance { get; set; }
            public float angle { get; set; }
            public float nextAngle { get; set; }
            public float turn { get; set; }
            public float progress { get; set; }
            public float nextProgress { get; set; }

            public int brushIndex { get; set; }
            public float brushSize { get; set; }
            public float brushColorR { get; set; }
            public float brushColorG { get; set; }
            public float brushColorB { get; set; }
            public float brushOpacity { get; set; }
        };


        static List<StrokeData> strokeList;
        static List<StrokeData> strokeUndoList;
        static bool recordingStroke;
        static StrokeData strokeData;
        static Stroke stroke;
        static Brush brush;
        static float progress;

        public static void Init()
        {
            strokeLengthIndex = Config.strokeLength - 1;
            brushIndex = Config.brush - 1;
            brushSizeIndex = Config.brushSize - 1;
            brushColorIndex = Config.brushColor - 1;
            brushOpacityIndex = Config.brushOpacity - 1;


            turtle = App.GetTurtle();

            turtle.Reset();

            List<Turtle.Step> trace = turtle.GetTrace();
            TracePlayer.SetTrace(trace);
            TracePlayer.Stop();

            strokeList = new List<StrokeData>(1000);
            strokeUndoList = new List<StrokeData>(1000);
            strokeData = new StrokeData();
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
                    strokeData.posX = tp.X;
                    strokeData.posY = tp.Y;
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
                    Vector2f tp1 = new Vector2f(strokeData.posX, strokeData.posY);
                    Vector2f tp2 = UI.ScreenPositionToTurtle(screenPosition.X, screenPosition.Y, window);


                    Vector2f ap = tp2 - tp1;
                    float distance = MathF.Sqrt(ap.X * ap.X + ap.Y * ap.Y);
                    if (distance >= strokeLengths[strokeLengthIndex])
                    {
                        UI.AddStrokePreview((Vector2f)screenPosition);
                        strokeData.nextPosX = tp2.X;
                        strokeData.nextPosY = tp2.Y;
                        strokeData.distance = distance;
                        strokeData.angle = Turtle.NormalizeAngle(MathF.Atan2(ap.Y, ap.X) * 180 / MathF.PI);
                        strokeData.progress = progress;
                        strokeData.brushIndex = brushIndex;
                        strokeData.brushColorR = brushColors[brushColorIndex].R;
                        strokeData.brushColorG = brushColors[brushColorIndex].G;
                        strokeData.brushColorB = brushColors[brushColorIndex].B;
                        strokeData.brushSize = brushSizes[brushSizeIndex];
                        strokeData.brushOpacity = brushOpacities[brushOpacityIndex];

                        strokeList.Add(strokeData);

                        progress = 1;

                        strokeData = new StrokeData();
                        strokeData.posX = tp2.X;
                        strokeData.posY = tp2.Y;
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
                strokeData.nextPosX = tp.X;
                strokeData.nextPosY = tp.Y;
                strokeData.distance = MathF.Sqrt(MathF.Pow(strokeData.posX - tp.X, 2) + MathF.Pow(strokeData.posY - tp.Y, 2));
                float aX = tp.X - strokeData.posX;
                float aY = tp.Y - strokeData.posY;            
                strokeData.angle = Turtle.NormalizeAngle(MathF.Atan2(aY, aX) * 180 / MathF.PI);
                strokeData.progress = progress;
                strokeData.brushIndex = brushIndex;
                strokeData.brushColorR = brushColors[brushColorIndex].R;
                strokeData.brushColorG = brushColors[brushColorIndex].G;
                strokeData.brushColorB = brushColors[brushColorIndex].B;
                strokeData.brushSize = brushSizes[brushSizeIndex];
                strokeData.brushOpacity = brushOpacities[brushOpacityIndex];

                strokeList.Add(strokeData);

                // End stroke sequence

                int i = strokeList.Count - 1;
                int sequenceStart;

                while((int)(strokeList[i].progress)!= 0) { i --; }
                sequenceStart = i;

                int sequenceTotal = strokeList.Count - sequenceStart;

                for(i = sequenceStart; i < strokeList.Count; i++)
                {
                    strokeData = strokeList[i];
                    strokeData.progress = (i - sequenceStart) / (float)sequenceTotal;
                    strokeList[i] = strokeData;
                }

                for (i = sequenceStart; i < strokeList.Count; i++)
                {
                    strokeData = strokeList[i];

                    if (i < strokeList.Count - 1)
                    {
                        StrokeData next = strokeList[i + 1];
                        strokeData.nextAngle = next.angle;
                        strokeData.turn = Turtle.CalculateTurn(strokeData.angle, strokeData.nextAngle);
                        strokeData.nextProgress = next.progress;
                    }
                    else
                    {
                        strokeData.nextAngle = strokeData.angle;
                        strokeData.turn = Turtle.CalculateTurn(strokeData.angle, strokeData.nextAngle);
                        strokeData.nextProgress = 1;
                    }

                    strokeList[i] = strokeData;
                }

                for (i = sequenceStart; i < strokeList.Count; i++)
                {
                    strokeData = strokeList[i];
                    RunBrush();

                }

                recordingStroke = false;

                TracePlayer.SetEndStep();

                UI.ClearStrokePreview();
            }

        }

        static void RunBrush()
        {
            stroke = new Stroke();
            stroke.posX = strokeData.posX;
            stroke.posY = strokeData.posY;
            stroke.nextPosX = strokeData.nextPosX;
            stroke.nextPosY = strokeData.nextPosY;
            stroke.distance = strokeData.distance;
            stroke.angle = strokeData.angle;
            stroke.nextAngle = strokeData.nextAngle;
            stroke.turn = strokeData.turn;
            stroke.progress = strokeData.progress;
            stroke.nextProgress = strokeData.nextProgress;

            brush = new Brush();
            brush.colorR = strokeData.brushColorR;
            brush.colorG = strokeData.brushColorG;
            brush.colorB = strokeData.brushColorB;
            brush.size = strokeData.brushSize;
            brush.opacity = strokeData.brushOpacity;

            turtle.Color(strokeData.brushColorR, strokeData.brushColorG, strokeData.brushColorB);
            turtle.Opacity(strokeData.brushOpacity);
            turtle.Teleport(strokeData.posX, strokeData.posY, strokeData.angle);

            if (strokeData.brushIndex == 0) { Brush1(); }
            else if (strokeData.brushIndex == 1) { Brush2(); }
            else if (strokeData.brushIndex == 2) { Brush3(); }
            else if (strokeData.brushIndex == 3) { Brush4(); }
            else if (strokeData.brushIndex == 4) { Brush5(); }
            else if (strokeData.brushIndex == 5) { Brush6(); }
            else if (strokeData.brushIndex == 6) { Brush7(); }
            else if (strokeData.brushIndex == 7) { Brush8(); }
            else // strokeData.brushIndex == 8
            { Brush9(); }
        }

        static void PrintStrokeList(bool undo = false, bool brief = false)
        {
            List<StrokeData> l = undo ? strokeUndoList : strokeList;

            Console.WriteLine("-------- " + (undo ? "undo " : "stroke ") + "list -----------");

            for(int i = 0; i < l.Count; i++)
            {
                StrokeData s = l[i];

                if(brief) { Console.WriteLine(i + ": %" + s.progress); }
                else
                {
                    Console.WriteLine("--------[" + i + "]-----------");
                    Console.WriteLine("StartPos......." + s.posX + ", " + s.posY);
                    Console.WriteLine("EndPos........." + s.nextPosX + ", " + s.nextPosY);
                    Console.WriteLine("Distance......." + s.distance);
                    Console.WriteLine("StartAngle....." + s.angle);
                    Console.WriteLine("EndAngle......." + s.nextAngle);
                    Console.WriteLine("Turn..........." + s.turn);
                    Console.WriteLine("StartFraction.." + s.progress);
                    Console.WriteLine("EndFraction...." + s.nextProgress);
                }
            }
        }

        public static int GetStrokeLengthIndex()
        {
            return strokeLengthIndex;
        }

        public static float GetStrokeLength()
        {
            return strokeLengths[strokeLengthIndex] * AppConfig.pixelsPerStep;
        }

        public static void NextStrokeLengthIndex(bool loop = true)
        {
            if (strokeLengthIndex + 1 >= AppConfig.strokeLengthsCount) { strokeLengthIndex = loop ? 0 : AppConfig.strokeLengthsCount - 1; }
            else { strokeLengthIndex++; }
        }

        public static void PreviousStrokeLengthIndex(bool loop = true)
        {
            if (strokeLengthIndex - 1 < 0) { strokeLengthIndex = loop ? AppConfig.strokeLengthsCount - 1 : 0; }
            else { strokeLengthIndex--; }
        }

        public static int GetBrushOpacityIndex()
        {
            return brushOpacityIndex;
        }

        public static float GetBrushOpacity()
        {
            return brushOpacities[brushOpacityIndex];
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

        public static float GetBrushSize()
        {
            return brushSizes[brushSizeIndex] * AppConfig.pixelsPerStep;
        }

        public static void NextBrushSizeIndex(bool loop = true)
        {
            if(brushSizeIndex + 1 >= AppConfig.brushSizesCount)
            { brushSizeIndex = loop ? 0 : AppConfig.brushSizesCount - 1; }
            else { brushSizeIndex ++; }
        }

        public static void PreviousBrushSizeIndex(bool loop = true)
        {
            if (brushSizeIndex - 1 <= 0 )
            { brushSizeIndex = loop ? AppConfig.brushSizesCount - 1 : 0; }
            else { brushSizeIndex--; }
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
            while (strokeList[strokeList.Count - 1 - count].progress != 0) { count ++; }
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
                else if (strokeUndoList[count].progress == 0) { found = true; }
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
                strokeList = JsonSerializer.Deserialize< List<StrokeData> >(json);
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

            TracePlayer.Reset();


            for (int i = 0; i < strokeList.Count; i++)
            {
                strokeData = strokeList[i];
                RunBrush();

            }

        }

    }
}
