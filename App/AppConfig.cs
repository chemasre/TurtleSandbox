﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TurtleSandbox
{
    internal class AppConfig
    {
        public const string appVersion = "1.04";

        public const int windowWidth = 1280;
        public const int windowHeight = 720;
        public const float splashDuration = 3.0f;
        public const float splashFadeDuration = 0.5f;

        public const float pixelsPerStep = 1;

        public const float screenTransitionTime = 1.0f;

        public static float stepWait = 0.2f;

        public static int sandColorsCount = 16;

        // Brush mode

        public const int playsCount = 9;

        public const float timeBoostFast = 5.0f;
        public const float timeBoostSlow = 0.3f;

        // Brush mode

        public const int brushesCount = 9;

        public const float strokeMinLength = 100;

        public const int brushSizesCount = 5;
        public const int brushColorsCount = 16;
        public const int brushOpacitiesCount = 10;

        public const int turtleSpeeds = 3;
    }
}
