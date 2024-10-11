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
        public const int traceInitialCapacity = 1000;

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

        public struct Location
        {
            public float x;
            public float y;
            public float angle;
        }

        Location savedLocation;

        public enum OrderId
        {
            origin,
            turn,
            walk,
            randTurn,
            randWalk,
            teleport,
            remember,
            recall
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

        bool overrideOrder;
        Order overrideOrderValue;

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

        List<Step> trace;

        float _angle;
        float colorR;
        float colorG;
        float colorB;
        float opacity;

        Random random;       

        public Turtle()
        {
            trace = new List<Step>(traceInitialCapacity);

            Reset();
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

        public void Color(float r, float g, float b)
        {
            colorR = r;
            colorG = g;
            colorB = b;
        }

        public void Opacity(int o)
        {
            opacity = o;
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

        public void Remember()
        {
            savedLocation.x = posX;
            savedLocation.y = posY;
            savedLocation.angle = angle;
        }

        public void Recall()
        {
            overrideOrder = true;
            overrideOrderValue = new Order();
            overrideOrderValue.id = OrderId.recall;
            Teleport(savedLocation.x, savedLocation.y, savedLocation.angle);
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
            p.angle = NormalizeAngle(angle);
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

            savedLocation = new Location();
            savedLocation.x = 0;
            savedLocation.y = 0;
            savedLocation.angle = 90;

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


    }
}
