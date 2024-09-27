using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System;
using System.Collections;

namespace TurtlesBeach
{
    internal class Turtle
    {
        public int steps { get { return trace.Count; } }
        public bool draw;
        public float posX { get { return trace[trace.Count - 1].x; } }
        public float posY { get { return trace[trace.Count - 1].y; } }
        public float angle
        {
            get
            {
                return NormalizeAngle(_angle);
            }
        }


        public struct Step
        {
            public float x;
            public float y;
            public float angle;
            public Color color;
            public bool draw;
        }

        List<Step> trace;

        float _angle;
        Color color;

        Random random;

        public Turtle(int colorR, int colorG, int colorB)
        {
            trace = new List<Step>();
            Step p = new Step();
            p.angle = 90;
            _angle = 90;
            random = new Random(0);
            color = new Color((byte)colorR, (byte)colorG, (byte)colorB);
            p.color = color;
            p.draw = true;
            draw = true;
            trace.Add(p);
        }

        float NormalizeAngle(float a)
        {
            float t = a / 360.0f;
            float n = (t - (int)t) * 360.0f;
            if (n < 0) { n += 360.0f; }
            return n;
        }

        public void Turn(float a)
        {
            _angle -= a;
        }

        public void Color(int r, int g, int b)
        {
            color = new Color((byte)r, (byte)g, (byte)b);
        }

        public void RandTurn(float a1, float a2)
        {
            Turn(a1 + (a2 - a1) * random.NextSingle());
        }

        public void RandColor()
        {
            color = new Color((byte)(random.Next()%255), (byte)(random.Next() % 255), (byte)(random.Next() % 255));
        }

        public void Walk(float d)
        {
            Step p1 = trace[trace.Count - 1];
            Step p2 = new Step();
            p2.x = p1.x + MathF.Cos(angle * MathF.PI / 180) * d;
            p2.y = p1.y + MathF.Sin(angle * MathF.PI / 180) * d;
            p2.angle = NormalizeAngle(_angle);
            p2.color = color;
            p2.draw = draw;

            trace.Add(p2);
        }

        public void RandWalk(float d1, float d2)
        {
            Walk(d1 + (d2 - d1) * random.NextSingle());
        }

        public void Origin()
        {
            Step p = new Step();
            p.x = 0;
            p.y = 0;
            p.angle = 90;
            p.color = color;
            p.draw = false;
            trace.Add(p);

            _angle = 90;

        }

        public void Reset()
        {
            trace.Clear();

            Step p = new Step();
            p.draw = true;
            trace.Add(p);

            draw = true;

            _angle = 90;

            random = new Random(0);
        }

        public List<Step> GetTrace()
        {
            return trace;
        }

        public void PrintTrace()
        {
            Console.WriteLine("********************************");
            for(int i = 0; i < trace.Count; i ++)
            {
                Console.WriteLine(trace[i].x + ", " + trace[i].y);
            }
            Console.WriteLine("********************************");
        }


    }
}
