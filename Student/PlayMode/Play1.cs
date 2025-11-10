
namespace TurtleSandbox
{
    internal partial class PlayMode
    {
        // Funciones básicas de la tortuga
        //
        //     turtle.Walk(distancia)   -> Le hace avanzar una cierta distancia
        //     turtle.Turn(angulo)      -> Le hace girar hacia la derecha si el ángulo es mayor que 0 o la izquierda si no
        //     turtle.Color(r,g,b)      -> Cambia el color con que dibuja la tortuga
        //     turtle.Opacity(a)        -> Cambia la opacidad con que dibuja la tortuga
        //     turtle.Origin()          -> Devuelve la tortuga al centro de la pantalla
        //
        //

        // Funciones que se pueden usar en casos justificados
        //
        //      turtle.Memorize()           -> Hace que la tortuga memorice la posición y ángulo en que está ahora
        //      turgle.Recall()           -> Devuelve a la tortuga a la posición y ángulo que había memorizado

        // Funciones que se consideran "hacer trampa"
        //
        //     turtle.Teleport(x,y,angulo) -> Teleporta a la tortuga a la posicion x, y con el angulo indicado.
        //     turtle.LookAt(x,y)          -> Hace que la tortuga gire hasta quedar mirando al punto x, y.
        //     turtle.GoTo(x,y)            -> Hace que la tortuga gire hasta quedar mirando al punto x, y y camine hasta él.

        // Funciones para introducir azar
        //
        //     turtle.RandColor()         -> Cambia el color de la tortuga por uno al azar
        //     turtle.RandWalk(min, max)  -> Le hace avanzar una distancia al azar entre min y max, ambos incluidos
        //     turtle.RandTurn(min, max)  -> Le hace girar un ángulo al azar entre min y max, ambos incluidos. Min y max pueden ser menores que 0.


        static void Play1()
        {
            turtle.Walk(100);
            turtle.Turn(90);
            turtle.Walk(100);
            turtle.Turn(90);
            turtle.Walk(100);
            turtle.Turn(90);
            turtle.Walk(100);
        }

    }
}
