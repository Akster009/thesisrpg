using System.Collections.Generic;
using SFML.System;

namespace SZDRPG.Graphics
{
    public class AnimationPart
    {
        public List<AnimationStep> Steps = new List<AnimationStep>();

        public AnimationStep CurrentStep(Time elapsed)
        {
            Time sum = Time.Zero;
            foreach (var step in Steps)
            {
                sum += step.Duration;
                if (sum > elapsed)
                    return step;
            }

            return null;
        }

        public Time TotalStepTimeUntil(Time elapsed)
        {
            Time sum = Time.Zero;
            Time tmp = Time.Zero;
            foreach (var step in Steps)
            {
                sum += step.Duration;
                if (sum > elapsed)
                    return tmp;
                tmp = sum;
            }

            return tmp;
        }
    }
}