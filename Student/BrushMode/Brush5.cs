using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleSandbox
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

    //     turtle.Remember()         -> Hace que la tortuga memorice la posición y ángulo en que está ahora
    //     turgle.Recall()           -> Devuelve a la tortuga a la posición y ángulo que había memorizado

    // Datos disponibles en brush mode
    //
    //     stroke.posX         -> Posición inicial X del trazo
    //     stroke.posY         -> Posición inicial Y del trazo
    //     stroke.nextPosX     -> Posición final X del trazo
    //     stroke.nextPosY     -> Posición final Y del trazo
    //     stroke.distance     -> Distancia recorrida entre la posición inicial y final del trazo
    //     stroke.angle        -> Angulo inicial de la tortuga en el trazo
    //     stroke.nextAngle    -> Angulo final de la tortuga en el trazo
    //     stroke.turn         -> Cuánto debe girar la tortuga para ir del ángulo inicial al ángulo final
    //     stroke.percent      -> Qué porcentaje del trazo total representa el punto inicial del trazo
    //     stroke.nextPercent  -> Qué porcentaje del trazo total representa el punto final del trazo

    //     brush.size          -> Tamaño de la brocha en unidades de la tortuga
    //     brush.colorR        -> Componente R del color de la brocha (0..255)
    //     brush.colorG        -> Componente G del color de la brocha (0..255)
    //     brush.colorB        -> Componente B del color de la brocha (0..255)
    //     brush.opacity       -> Opacidad de la brocha (0..255)

    internal partial class BrushMode
    {
        static void Brush5()
        {
            turtle.Walk(stroke.distance);
        }
    }
}
