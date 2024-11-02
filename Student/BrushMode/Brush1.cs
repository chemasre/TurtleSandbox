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
                turtle.Walk(stroke.distance / 3);
                turtle.Turn(90);
                turtle.Walk(10);
                turtle.Walk(-20);
                turtle.Walk(10);
                turtle.Turn(-90);
            }
        }
    }
}
