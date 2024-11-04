using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleSandbox
{
    internal class Texts
    {
        // Enums

        public enum Id
        {
            screenshotSaved,
            musicOn,
            musicOff,
            screenshotFilename,
            play,
            statusAngle,
            statusPosX,
            statusPosY,
            turtleOn,
            turtleOff,
            gridOn,
            gridOff,
            gridCoordinates,
            gridCursorCoordinates,
            watermark,
            brush,
            strokeLength,
            brushSize,
            brushColor,
            brushOpacity,
            undo,
            redo,
            clear,
            save,
            load,
            sandColor



        };

        static Dictionary<Id, string> texts;

        public static void Init()
        {
            // Init texts

            texts = new Dictionary<Id, string>();
            texts[Id.screenshotSaved] = "Screenshot saved";
            texts[Id.musicOn] = "Music on";
            texts[Id.musicOff] = "Music off";
            texts[Id.screenshotFilename] = "screenshot{0:000}.png";
            texts[Id.play] = "Play {0,1}";
            texts[Id.statusAngle] = "{0,3}";
            texts[Id.statusPosX] = "{0,3}";
            texts[Id.statusPosY] = "{0,3}";
            texts[Id.gridOn] = "Grid on";
            texts[Id.gridOff] = "Grid off";
            texts[Id.turtleOn] = "Turtle visible";
            texts[Id.turtleOff] = "Turtle hidden";
            texts[Id.gridCoordinates] = "{0}";
            texts[Id.gridCursorCoordinates] = "({0,3},{1,3})";
            texts[Id.watermark] = "Turtle Sandbox v{0} by Sinestesia Game Design";
            texts[Id.strokeLength] = "Stroke length {0,1}px";
            texts[Id.brush] = "Brush {0,1}";
            texts[Id.brushSize] = "Brush size {0,1}px";
            texts[Id.brushColor] = "Brush color {0,1}";
            texts[Id.brushOpacity] = "Brush opacity {0,1}%";
            texts[Id.undo] = "Undo";
            texts[Id.redo] = "Redo";
            texts[Id.clear] = "Cleared";
            texts[Id.save] = "Saved!";
            texts[Id.load] = "Loaded";
            texts[Id.sandColor] = "Sand color {0, 1}";

        }

        public static string Get(Id id)
        {
            return texts[id];
        }

    }
}
