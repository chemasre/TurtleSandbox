
namespace TurtleSandbox
{
    internal partial class PlayMode
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

        //      turtle.Remember()         -> Hace que la tortuga memorice la posición y ángulo en que está ahora
        //      turgle.Recall()           -> Devuelve a la tortuga a la posició y ángulo que había memorizado

        static void DrawPointyStar()
        {

            turtle.draw = false;
            turtle.Walk(10);
            turtle.draw = true;

            for (int i = 0; i < 6; i++)
            {
                turtle.Turn(-20);
                turtle.Walk(30);
                turtle.Turn(160);
                turtle.Walk(30);
                turtle.Turn(160);
            }

        }

        static void DrawEspiral()
        {
            float distance = 5;

            while (distance > 0)
            {
                turtle.Turn(10);
                turtle.Walk(distance);
                distance = distance - 0.1f;
            }
        }

        static void DrawRoundStar()
        {
            for (int i = 0; i < 10; i++)
            {
                turtle.Turn(36);
                turtle.Walk(2);
            }
        }

        static void DrawCatCoronaLeft()
        {
            float distance = 25;

            // Segment 1

            for (int i = 0; i < 6; i++)
            {
                turtle.Walk(distance);
                turtle.Turn(10);

                distance = distance - 1;
            }

            // Segment 2

            distance = 3;

            for (int i = 0; i < 9; i++)
            {
                turtle.Walk(distance);
                turtle.Turn(-18);
            }

            // Segment 3

            distance = 26;

            for (int i = 0; i < 6; i++)
            {
                turtle.Walk(distance);
                turtle.Turn(-14);

                distance = distance - 1.5f;
            }

            // Segment 4

            distance = 3;

            for (int i = 0; i < 10; i++)
            {
                turtle.Walk(distance);
                turtle.Turn(18);
            }

        }

        static void DrawCatCoronaRight()
        {
            float distance = 25;

            // Segment 1

            for (int i = 0; i < 6; i++)
            {
                turtle.Walk(distance);
                turtle.Turn(-10);

                distance = distance - 1;
            }

            // Segment 2

            distance = 3;

            for (int i = 0; i < 9; i++)
            {
                turtle.Walk(distance);
                turtle.Turn(18);
            }

            // Segment 3

            distance = 26;

            for (int i = 0; i < 6; i++)
            {
                turtle.Walk(distance);
                turtle.Turn(14);

                distance = distance - 1.5f;
            }

            // Segment 4

            distance = 3;

            for (int i = 0; i < 10; i++)
            {
                turtle.Walk(distance);
                turtle.Turn(-18);
            }

        }

        static void Play6()
        {

            // Face

            float distanceFace;

            distanceFace = 45;

            turtle.Teleport(0, 218, 0);

            turtle.Walk(-20);

            for (int i = 0; i < 20; i++)
            {
                turtle.Walk(distanceFace);
                turtle.Turn(18);

            }

            // Left eye

            int signEye;

            signEye = -1;

            turtle.Teleport(-40, 104, 90);

            turtle.Turn(signEye * 10);
            turtle.Walk(45);
            turtle.Turn(signEye * 55);
            turtle.Walk(40);
            turtle.Turn(signEye * 100);
            turtle.Walk(55);
            turtle.Turn(signEye * 95);
            turtle.Walk(60);

            // Right eye

            turtle.Teleport(40, 104, 90);

            signEye = 1;

            turtle.Turn(signEye * 10);
            turtle.Walk(45);
            turtle.Turn(signEye * 55);
            turtle.Walk(40);
            turtle.Turn(signEye * 100);
            turtle.Walk(55);
            turtle.Turn(signEye * 95);
            turtle.Walk(60);

            // Left nostril

            float signNose;

            signNose = -1;

            turtle.Teleport(-11, 66, 90);

            turtle.Turn(signNose * 10);
            turtle.Walk(25);
            turtle.Turn(signNose * 120);
            turtle.Walk(30);
            turtle.Turn(signNose * 125);
            turtle.Walk(30);

            // Right nostril

            turtle.Teleport(11, 66, 90);

            signNose = 1;

            turtle.Turn(signNose * 10);
            turtle.Walk(25);
            turtle.Turn(signNose * 120);
            turtle.Walk(30);
            turtle.Turn(signNose * 125);
            turtle.Walk(30);


            // Mouth

            float mouthWidth;
            float mouthDistance;

            mouthWidth = 20;
            mouthDistance = 20;
            

            turtle.Teleport(112, 34, 235);

            for(int i = 0; i < 12; i++)
            {
                turtle.Walk(mouthDistance);
                turtle.Turn(90);
                turtle.Walk(mouthWidth / 2);
                turtle.Turn(180);
                turtle.Walk(mouthWidth);
                turtle.Turn(180);
                turtle.Walk(mouthWidth / 2);
                turtle.Turn(-90);

                turtle.Turn(9);
            }


            // Neck

            turtle.Teleport(-20, -66, 270);
            turtle.Walk(80);
            turtle.Teleport(20, -66, 270);
            turtle.Walk(80);

            // Cat

            turtle.Teleport(20, -144, 30);

            float distanceCat;
            float signCat;

            distanceCat = 10;
            signCat = 1;

            for(int i = 0; i < 12; i ++)
            {
                turtle.Walk(distanceCat);
                turtle.Turn(signCat * 5);
                distanceCat = distanceCat - 0.75f;
            }

            for (int i = 0; i < 18; i++)
            {
                turtle.Walk(distanceCat);
                turtle.Turn(signCat * 9);
                distanceCat = distanceCat + 0.75f;
            }

            turtle.Teleport(-20, -144, 150);

            distanceCat = 10;
            signCat = -1;

            for (int i = 0; i < 12; i++)
            {
                turtle.Walk(distanceCat);
                turtle.Turn(signCat * 5);
                distanceCat = distanceCat - 0.75f;
            }

            for (int i = 0; i < 18; i++)
            {
                turtle.Walk(distanceCat);
                turtle.Turn(signCat * 9);
                distanceCat = distanceCat + 0.75f;
            }

            // Cat left eye

            turtle.Teleport(-20, -170, 0);

            for (int i = 0; i < 10; i++)
            {
                turtle.Walk(7);
                turtle.Turn(36);
            }

            // Cat right eye

            turtle.Teleport(20, -170, 0);

            for (int i = 0; i < 10; i++)
            {
                turtle.Walk(7);
                turtle.Turn(36);
            }

            // Cat up part

            turtle.Teleport(-20, -144, 20);

            for (int i = 0; i < 4; i++)
            {
                turtle.Walk(11);
                turtle.Turn(13);
            }


            // Cat corona

            turtle.Teleport(94, -146, 0);

            DrawCatCoronaRight();
            DrawCatCoronaRight();
            DrawCatCoronaRight();

            for (int i = 0; i < 10; i++)
            {
                turtle.Walk(3);
                turtle.Turn(18);
            }

            for (int i = 0; i < 18; i++)
            {
                turtle.Walk(10);
                turtle.Turn(2);
            }

            for (int i = 0; i < 6; i++)
            {
                turtle.Walk(15);
                turtle.Turn(-10);
            }

            turtle.Teleport(-94, -146, 180);

            DrawCatCoronaLeft();
            DrawCatCoronaLeft();
            DrawCatCoronaLeft();

            for (int i = 0; i < 10; i++)
            {
                turtle.Walk(3);
                turtle.Turn(-18);
            }

            for (int i = 0; i < 18; i++)
            {
                turtle.Walk(10);
                turtle.Turn(-2);
            }

            for (int i = 0; i < 6; i++)
            {
                turtle.Walk(15);
                turtle.Turn(10);
            }


            // Jacket

            // Right part 1

            turtle.Teleport(96, -286, 285);

            for (int i = 0; i < 10; i++)
            {
                turtle.Walk(8);
                turtle.Turn(2);
            }

            // Left part 1

            turtle.Teleport(-96, -286, 255);

            for (int i = 0; i < 10; i++)
            {
                turtle.Walk(8);
                turtle.Turn(-2);
            }

            // Right part 2

            turtle.Teleport(156, -330, 275);

            for (int i = 0; i < 10; i++)
            {
                turtle.Walk(8);
                turtle.Turn(2);
            }

            // Right part 2

            turtle.Teleport(-156, -330, 265);

            for (int i = 0; i < 10; i++)
            {
                turtle.Walk(8);
                turtle.Turn(-2);
            }

            // Fondo

            for (int i = 0; i < 80; i++)
            {
                turtle.draw = false;
                turtle.Origin();
                turtle.RandTurn(-90, 90);
                turtle.RandWalk(250, 600);
                turtle.draw = true;
                DrawRoundStar();


            }

            for (int i = 0; i < 15; i++)
            {
                turtle.draw = false;
                turtle.Origin();
                turtle.RandTurn(-90, 90);
                turtle.RandWalk(250, 600);
                turtle.draw = true;
                DrawPointyStar();
            }

            for (int i = 0; i < 8; i++)
            {
                turtle.draw = false;
                turtle.Origin();
                turtle.RandTurn(-90, 90);
                turtle.RandWalk(250, 600);
                turtle.draw = true;
                DrawEspiral();


            }

        }
    }
}
