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
            float width1 = stroke.startPercent * stroke.brushSize;
            float width2 = stroke.endPercent * stroke.brushSize;

            turtle.Opacity(255 - stroke.startPercent * 255);
            turtle.Turn(-90);
            turtle.Walk(width1);
            turtle.Turn(90);
            turtle.Walk(stroke.distance);
            turtle.Opacity(255 - stroke.endPercent * 255);
            turtle.Turn(90);
            turtle.Walk(width2 * 2);
            turtle.Turn(90);
            turtle.Walk(stroke.distance);
            turtle.Opacity(255 - stroke.startPercent * 255);
            turtle.Turn(90);
            turtle.Walk(width1);


        }
    }
}
