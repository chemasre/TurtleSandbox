﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtlesBeach
{
    internal partial class Program
    {
        // Título de la ventana. Puedes poner tu nombre aquí

        static string windowTitle = "Turtle Sandbox 1.02";

        // Qué "play" quieres reproducir al principio (1 al 9)

        static int play = 1;

        // Si quieres que inicialmente se reproduzca la música o no

        static bool playMusic = true;

        // Si quieres ver la rejilla inicialmente o no

        static bool showGrid = false;

        // Si quieres ver texto informativo inicialmente o no

        static bool showInfo = false;

        // Tiempo de espera en segundos entre un paso de la tortuga y el siguiente (puede ser 0)

        static float stepWait = 0.2f;

        // Color inicial de la arena

        static int sandR = 255;
        static int sandG = 193;
        static int sandB = 58;

        // Ancho de la línea que deja la tortuga

        static float lineWidth = 4.0f;

        // Color del texto informativo que aparece en pantalla

        static int infoR = 255;
        static int infoG = 255;
        static int infoB = 255;

        // Color de la rejilla

        static int gridR = 255;
        static int gridG = 255;
        static int gridB = 255;

        // Opacidad de la rejilla

        static int gridOpacity = 96;

    }
}
