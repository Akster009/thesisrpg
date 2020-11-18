using System.Collections.Generic;
using SFML.System;

namespace SZDRPG.Graphics
{
    public class AnimationStep
    {
        public float Rotation;
        public Time Duration;

        public AnimationStep()
        {
            
        }

        public AnimationStep(float rotation, Time duration)
        {
            Rotation = rotation;
            Duration = duration;
        }
    }
}