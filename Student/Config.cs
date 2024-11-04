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

        // PLAY MODE: Qué "play" quieres reproducir al principio (1 al 9)

        public static int play = 1;

        // BRUSH MODE: Qué distancia de trazo quieres tener seleccionada al principio (1 a 4)

        public static int strokeLength = 2;

        // BRUSH MODE: Qué brocha quieres tener seleccionada al principio (1 a 9)

        public static int brush = 1;

        // BRUSH MODE: Qué tamaño de brocha quieres tener seleccionado al principio (1 a 5)

        public static int brushSize = 2;

        // BRUSH MODE: Qué color de brocha quieres tener seleccionado al principio (1 a 16)

        public static int brushColor = 13;

        // BRUSH MODE: Qué opacidad de brocha quieres tener seleccionada al principio (1 a 10)

        public static int brushOpacity = 1;


        // Si quieres que inicialmente se reproduzca la música o no

        public static bool playMusic = true;

        // Si quieres ver la rejilla inicialmente o no

        public static bool showGrid = true;

        // Si quieres ver la barra de herramientas inicialmente o no

        public static bool showToolbar = true;

        // Color inicial de la arena

        public static int sandColor = 4;

        // Ancho de la línea que deja la tortuga

        public static float lineWidth = 4.0f;

        // Color de las herramientas de ayuda que aparecen en pantalla

        public static int toolbar1R = 255;
        public static int toolbar1G = 255;
        public static int toolbar1B = 255;

        public static int toolbar2R = 80;
        public static int toolbar2G = 200;
        public static int toolbar2B = 80;

        // Opacidad de la rejilla

        public static int gridOpacity = 96;

        // Si quieres saltarte la pantalla inicial o no

        public static bool skipSplash = false;
    }
}
