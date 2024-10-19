
namespace TurtleSandbox
{
    internal partial class App
    {
        // Funciones de la tortuga
        //
        //     turtle.Walk(distancia)   -> Le hace avanzar una cierta distancia
        //     turtle.Turn(angulo)      -> Le hace girar hacia la derecha si el ángulo es mayor que 0 o la izquierda si no
        //     turtle.Color(r,g,b)      -> Cambia el color con que dibuja la tortuga
        //     turtle.Opacity(a)        -> Cambia la opacidad con que dibuja la tortuga
        //     turtle.Origin()          -> Devuelve la tortuga al centro de la pantalla
        //
        //     turtle.Teleport(x,y,angulo) -> Teleporta a la tortuga a la posicion x, y con el angulo indicado (es un poco hacer trampa :) )
        //

        //     turtle.RandColor()         -> Cambia el color de la tortuga por uno al azar
        //     turtle.RandWalk(min, max)  -> Le hace avanzar una distancia al azar entre min y max, ambos incluidos
        //     turtle.RandTurn(min, max)  -> Le hace girar un ángulo al azar entre min y max, ambos incluidos. Min y max pueden ser menores que 0.

        //      turtle.Save()             -> Hace que la tortuga memorice la posición y ángulo en que está ahora
        //      turgle.Recall()           -> Devuelve a la tortuga a la posició y ángulo que había memorizado

        static void Play3()
        {
            turtle.draw = false;
            turtle.Turn(-90);
            turtle.Walk(600);
            turtle.Turn(-90);
            turtle.Walk(300);
            turtle.Turn(180);

            turtle.draw = true;
            for (int i = 0; i < 12; i++)
            {
                turtle.Walk(50);
                turtle.Turn(90);
                turtle.Walk(50);
                turtle.Turn(-90);
            }

            turtle.Turn(90);

            for (int i = 0; i < 12; i++)
            {
                turtle.Walk(50);
                turtle.Turn(90);
                turtle.Walk(50);
                turtle.Turn(-90);
            }

            turtle.Turn(180);
            turtle.Walk(1200);
        }

    }
}
