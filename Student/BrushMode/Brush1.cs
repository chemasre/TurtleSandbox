using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleSandbox
{
    internal partial class BrushMode
    {
        static void Brush1()
        {
            for(int i = 0; i < 3; i ++)
            {
                turtle.draw = false;
                turtle.Walk(stroke.distance / 3);
                turtle.draw = true;
                turtle.Turn(90);
                turtle.Walk(stroke.brushSize);
                turtle.Walk(-stroke.brushSize * 2);
                turtle.Walk(stroke.brushSize);
                turtle.Turn(-90);
            }
        }
    }
}
