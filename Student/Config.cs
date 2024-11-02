using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleSandbox
{
    internal class Config
    {
        // Título de la ventana. Puedes poner tu nombre aquí

        public static string windowTitle = "Turtle Sandbox";

        // Qué "play" quieres reproducir al principio (1 al 9)

        public static int play = 1;

        // Qué brocha quieres tener seleccionada al principio (1 al 9)

        public static int brush = 1;

        // Si quieres que inicialmente se reproduzca la música o no

        public static bool playMusic = false;

        // Si quieres ver la rejilla inicialmente o no

        public static bool showGrid = true;

        // Si quieres ver la barra de herramientas inicialmente o no

        public static bool showToolbar = true;

        // Tiempo de espera en segundos entre un paso de la tortuga y el siguiente (puede ser 0)

        public static float stepWait = 0.2f;

        // Color inicial de la arena

        public static int sandR = 255;
        public static int sandG = 193;
        public static int sandB = 58;

        // Ancho de la línea que deja la tortuga

        public static float lineWidth = 4.0f;

        // Color de las herramientas de ayuda que aparecen en pantalla

        public static int toolbarR = 255;
        public static int toolbarG = 255;
        public static int toolbarB = 255;

        // Color de la rejilla

        public static int gridR = 255;
        public static int gridG = 255;
        public static int gridB = 255;

        // Opacidad de la rejilla

        public static int gridOpacity = 96;

        // Si quieres saltarte la pantalla inicial o no

        public static bool skipSplash = false;
    }
}
