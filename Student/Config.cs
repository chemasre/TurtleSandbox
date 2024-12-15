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

        // Dimensiones de la ventana. Como mínimo 1280x720

        public const int windowWidth = 1280;
        public const int windowHeight = 720;

        // Pantalla en la que quieres empezar
        // 0 -> Inicio normal
        // 1 -> Iniciar directamente en play mode
        // 2 -> Iniciar directamente en brush mode

        public static int startScreen = 0;

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

        // Opacidad de la rejilla

        public static int gridOpacity = 96;

    }
}
