using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace SZDRPG.Graphics
{
    public class GPart
    {
        public List<Sprite> BaseTexture = new List<Sprite>();
        public float LastRotation = 0;
        public float NewRotation = 0;
        public Vector2f RotationCenter = new Vector2f(0,0);
        public Vector2f Origin = new Vector2f(0,0);
        public int LastState = -1;
        public Time LastStateTime;

        public int RotationState(float facing)
        {
            if (facing <= 45 || facing > 315)
                return 0;
            if (facing <= 135 && facing > 45)
                return 1;
            if (facing <= 225 && facing > 135)
                return 2;
            if (facing <= 315 && facing > 225)
                return 3;
            return 0;
        }
        public Sprite Draw(AnimationPart part, AnimationState state)
        {
            AnimationStep step = part.CurrentStep(state.elapsed);
            //Console.WriteLine(state.elapsed.AsSeconds() + "   " + part.CurrentStep(state.elapsed).Rotation);
            if (step == null)
            {
                state.elapsed = Time.Zero;
                step = part.CurrentStep(state.elapsed);
            }
            if (state.ID != LastState || LastStateTime != part.TotalStepTimeUntil(state.elapsed))
            {
                LastRotation = NewRotation;
                LastState = state.ID;
                LastStateTime = part.TotalStepTimeUntil(state.elapsed);
                NewRotation = step.Rotation;
            }
            int direction = RotationState(state.facing);
            Sprite ret = new Sprite(BaseTexture[direction]);
            Time timeInState = state.elapsed - part.TotalStepTimeUntil(state.elapsed);
            float currentRotation;
            currentRotation = LastRotation + (NewRotation - LastRotation) * timeInState.AsSeconds() / step.Duration.AsSeconds();
            float actualRotation = (state.facing - (int)(state.facing)/90*90)/ 90 * currentRotation;
            float desiredHeight = (float) (Math.Cos((Math.PI / 180) * currentRotation) / Math.Cos((Math.PI / 180) * actualRotation) *
                                           BaseTexture[direction].GetLocalBounds().Height);
            ret.Scale = new Vector2f(1,desiredHeight/BaseTexture[direction].GetLocalBounds().Height);
            ret.Rotation = actualRotation;
            ret.Position = new Vector2f((float) (state.Position.X+Math.Cos((Math.PI / 180) * state.facing)*RotationCenter.X),(float) (state.Position.Y+Math.Cos((Math.PI / 180) * state.facing)*RotationCenter.X+RotationCenter.Y));
            ret.Origin = Origin;
            //ret.Rotation = 90;
            return ret;
        }
    }
}