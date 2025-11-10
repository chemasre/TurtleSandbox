using SFML.Window;
using SFML.Graphics;
using SFML.System;
using SFML.Audio;
using System;
using System.Collections;
using System.Net.NetworkInformation;
using System.Drawing;

namespace TurtleSandbox
{
    internal class Turtle
    {
        // Constants

        public const int traceInitialCapacity = 1000;
        public const int saveSlots = 10;

        // Properties

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

        // Enums

        public enum OrderId
        {
            origin,
            turn,
            walk,
            randTurn,
            randWalk,
            teleport,
            memorize,
            recall,
            lookAt,
            goTo
        }

        // Structs

        public struct SavedLocation
        {
            public float x;
            public float y;
            public float angle;
        }

        public struct SavedColor
        {
            public float r;
            public float g;
            public float b;
        }

        public struct Order
        {
            public OrderId id;
            public float param1;
            public float param2;
            public float param3;
            public int intParam1;
            public int intParam2;
            public int intParam3;
        }

        public struct Step
        {
            public Order order;
            public float x;
            public float y;
            public float angle;
            public float colorR;
            public float colorG;
            public float colorB;
            public float opacity;
            public bool draw;
        }

        // Override order

        bool overrideOrder;
        Order overrideOrderValue;

        // Save and load

        List<SavedLocation> savedLocations;
        List<SavedColor> savedColors;
        List<float> savedOpacities;

        // Trace

        List<Step> trace;

        // State

        float _angle;
        float colorR;
        float colorG;
        float colorB;
        float opacity;

        Random random;       

        public Turtle()
        {
            trace = new List<Step>(traceInitialCapacity);
            savedLocations = new List<SavedLocation>(saveSlots);
            savedColors = new List<SavedColor>(saveSlots);
            savedOpacities = new List<float>(saveSlots);

            Reset();
        }

        public void Turn(float a)
        {
            _angle -= a;

            Order order = new Order();
            order.id = OrderId.turn;
            order.param1 = a;

            Step p = new Step();
            p.order =(overrideOrder ? overrideOrderValue: order);
            p.x = trace[trace.Count - 1].x;
            p.y = trace[trace.Count - 1].y;
            p.angle = NormalizeAngle(_angle);
            p.colorR = colorR;
            p.colorG = colorG;
            p.colorB = colorB;
            p.opacity = opacity;
            p.draw = false;

            trace.Add(p);

            overrideOrder = false;
        }

        public void LookAt(float x, float y)
        {
            float nextAngle = MathF.Atan2(y - posY, x - posX) * 180 / MathF.PI;
            float angle = CalculateTurn(_angle, nextAngle);

            overrideOrder = true;
            overrideOrderValue = new Order();
            overrideOrderValue.id = OrderId.lookAt;
            overrideOrderValue.param1 = x;
            overrideOrderValue.param2 = y;

            Turn(angle);
        }

        public void GoTo(float x, float y)
        {
            LookAt(x, y);

            float ax = x - posX;
            float ay = y - posY;
            float distance = MathF.Sqrt(ax * ax + ay * ay);

            overrideOrder = true;
            overrideOrderValue = new Order();
            overrideOrderValue.id = OrderId.goTo;
            overrideOrderValue.param1 = x;
            overrideOrderValue.param2 = y;

            Walk(distance);
        }


        public void Color(float r, float g, float b)
        {
            colorR = r;
            colorG = g;
            colorB = b;
        }

        public void AddColor(float r, float g, float b)
        {
            colorR += r;
            colorG += g;
            colorB += b;

            colorR = Clamp(colorR, 0, 255);
            colorG = Clamp(colorR, 0, 255);
            colorB = Clamp(colorR, 0, 255);
        }


        public void Opacity(float o)
        {
            opacity = o;
        }

        public void AddOpacity(float o)
        {
            opacity += o;

            opacity = Clamp(opacity, 0, 255);
        }

        public void RandTurn(float a1, float a2)
        {
            overrideOrder = true;
            overrideOrderValue = new Order();
            overrideOrderValue.id = OrderId.randTurn;
            overrideOrderValue.param1 = a1;
            overrideOrderValue.param2 = a2;

            Turn(a1 + (a2 - a1) * random.NextSingle());
        }

        public void RandColor()
        {
            colorR = random.NextSingle() * 255;
            colorG = random.NextSingle() * 255;
            colorB = random.NextSingle() * 255;
        }

        public void RandAddColor(float c1, float c2)
        {
            colorR += c1 + random.NextSingle() * (c2 - c1);
            colorG += c1 + random.NextSingle() * (c2 - c1);
            colorB += c1 + random.NextSingle() * (c2 - c1);

            colorR = Clamp(colorR, 0, 255);
            colorG = Clamp(colorG, 0, 255);
            colorB = Clamp(colorB, 0, 255);
        }

        public void RandOpacity()
        {
            opacity = random.NextSingle() * 255;
        }

        public void RandAddOpacity(float o1, float o2)
        {
            opacity += o1 + random.NextSingle() * (o2 - o1);

            opacity = Clamp(opacity, 0, 255);
        }

        public void Walk(float d)
        {
            Order order = new Order();
            order.id = OrderId.walk;
            order.param1 = d;

            Step p1 = trace[trace.Count - 1];
            Step p2 = new Step();
            p2.order = (overrideOrder ? overrideOrderValue : order);
            p2.x = p1.x + MathF.Cos(angle * MathF.PI / 180) * d;
            p2.y = p1.y + MathF.Sin(angle * MathF.PI / 180) * d;
            p2.angle = NormalizeAngle(_angle);
            p2.colorR = colorR;
            p2.colorG = colorG;
            p2.colorB = colorB;
            p2.opacity = opacity;
            p2.draw = draw;

            trace.Add(p2);

            overrideOrder = false;
        }

        public void RandWalk(float d1, float d2)
        {
            overrideOrder = true;
            overrideOrderValue = new Order();
            overrideOrderValue.id = OrderId.randWalk;
            overrideOrderValue.param1 = d1;
            overrideOrderValue.param2 = d2;


            Walk(d1 + (d2 - d1) * random.NextSingle());
        }

        public void Origin()
        {
            overrideOrder = true;
            overrideOrderValue = new Order();
            overrideOrderValue.id = OrderId.origin;

            Teleport(0, 0, 90);
        }

        public void Memorize(int slot = 0)
        {
            SavedLocation l = new SavedLocation();            
            l.x = posX;
            l.y = posY;
            l.angle = angle;

            savedLocations[slot] = l;
        }

        public void MemorizeColor(int slot = 0)
        {
            SavedColor c = new SavedColor();
            c.r = colorR;
            c.g = colorG;
            c.b = colorB;

            savedColors[slot] = c;
        }

        public void MemorizeOpacity(int slot = 0)
        {
            savedOpacities[slot] = opacity;
        }

        public void Recall(int slot = 0)
        {
            SavedLocation l = savedLocations[slot];

            overrideOrder = true;
            overrideOrderValue = new Order();
            overrideOrderValue.id = OrderId.recall;
            Teleport(l.x, l.y, l.angle);
        }

        public void RecallColor(int slot = 0)
        {
            colorR = savedColors[slot].r;
            colorG = savedColors[slot].g;
            colorB = savedColors[slot].b;
        }

        public void RecallOpacity(int slot = 0)
        {
            opacity = savedOpacities[slot];
        }

        public void Teleport(float posX, float posY, float angle)
        {
            Order order = new Order();
            order.id = OrderId.teleport;
            order.param1 = posX;
            order.param2 = posX;
            order.param3 = angle;

            Step p = new Step();
            p.order = (overrideOrder ? overrideOrderValue : order);
            p.x = posX;
            p.y = posY;
            p.angle = angle;
            p.colorR = colorR;
            p.colorG = colorG;
            p.colorB = colorB;
            p.opacity = opacity;
            p.draw = false;
            trace.Add(p);

            _angle = NormalizeAngle(angle);

            overrideOrder = false;
        }

        public void Reset()
        {
            draw = true;

            _angle = 90;
            random = new Random(0);
            colorR = 255;
            colorG = 255;
            colorB = 255;
            opacity = 255;

            trace.Clear();

            Order order = new Order();
            order.id = OrderId.origin;

            Step p = new Step();
            p.order = order;
            p.draw = true;
            p.colorR = 255;
            p.colorG = 255;
            p.colorB = 255;
            p.opacity = 255;
            p.angle = 90;
            trace.Add(p);

            savedLocations.Clear();
            savedColors.Clear();
            savedOpacities.Clear();
            for (int i = 0; i < saveSlots; i++)
            {
                savedLocations.Add(new SavedLocation());
                savedColors.Add(new SavedColor());
                savedOpacities.Add(0);
            }

            overrideOrder = false;
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
                Console.WriteLine(trace[i].order.id + ":" + trace[i].x + ", " + trace[i].y);
            }
            Console.WriteLine("********************************");
        }

        public static float NormalizeAngle(float a)
        {
            float t = a / 360.0f;
            float n = (t - (int)t) * 360.0f;
            if (n < 0) { n += 360.0f; }
            return n;
        }

        public static float CalculateTurn(float a1, float a2)
        {
            float a1n = NormalizeAngle(a1);
            float a2n = NormalizeAngle(a2);
            float r;

            a2n = NormalizeAngle(a2n - a1n);
            if(a2n > 180) { r = 360.0f - a2n; }
            else { r = -a2n; }

            return r;

        }

        float Clamp(float v, float min, float max)
        {
            return MathF.Max(MathF.Min(max, v), 0);
        }

    }
}
