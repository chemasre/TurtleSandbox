using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleSandbox
{
    internal partial class BrushMode
    {
        static void Brush2()
        {
            float width = stroke.segmentProgress * 20;

            turtle.Turn(-90);
            turtle.Walk(width);
            turtle.Turn(90);
            turtle.Walk(stroke.distance);
            turtle.Turn(90);
            turtle.Walk(width * 2);
            turtle.Turn(90);
            turtle.Walk(stroke.distance);
            turtle.Walk(width);
            turtle.Opacity(255 - stroke.segmentProgress * 255);


        }
    }
}
