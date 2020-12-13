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
        public Vector3f Origin = new Vector3f(0,0,0);
        public int LastState = -1;
        public Time LastStateTime;

        public int RotationState(float facing)
        {
            if (facing <= 45 || facing > 315)
                return 0;
            if (facing <= 135 && facing > 45)
                return 3;
            if (facing <= 225 && facing > 135)
                return 2;
            if (facing <= 315 && facing > 225)
                return 1;
            return 0;
        }

        public float RelativeDistance(AnimationState state)
        {
            return MathF.Sin((MathF.PI / 180) * state.facing) * RotationCenter.X / 2;
        }

        public Vector2f GetOriginCenter(int direction)
        {
            switch (direction)
            {
                case 0:
                    return new Vector2f(Origin.X,Origin.Z);
                case 1:
                    return new Vector2f(-Origin.Y, Origin.Z);
                case 2:
                    return new Vector2f(-Origin.X,Origin.Z);
                case 3:
                    return new Vector2f(Origin.Y,Origin.Z);
            }
            return new Vector2f();
        }
        public Sprite Draw(AnimationPart part, AnimationState state)
        {
            AnimationStep step = part.CurrentStep(state.elapsed);
            if (step == null)
            {
                if (state.Repeatable)
                {
                    state.elapsed = Time.Zero;
                    step = part.CurrentStep(state.elapsed);
                }
                else
                {
                    state.Finished = true;
                }
            }
            if (state.ID != LastState || LastStateTime != part.TotalStepTimeUntil(state.elapsed) && step != null)
            {
                LastRotation = NewRotation;
                LastState = state.ID;
                LastStateTime = part.TotalStepTimeUntil(state.elapsed);
                NewRotation = step.Rotation;
            }
            int direction = RotationState(state.facing);
            Sprite ret = new Sprite(BaseTexture[direction]);
            if (step == null)
            {
                state.ID = 0;
                state.elapsed = Time.Zero;
                ret.Position = new Vector2f(
                    (float) (state.Position.X + Math.Cos((Math.PI / 180) * state.facing) * RotationCenter.X) -
                    ret.GetLocalBounds().Width / 2,
                    (float) (state.Position.Y + Math.Sin((Math.PI / 180) * state.facing) * RotationCenter.X / 2 +
                             RotationCenter.Y));
                return ret;
            }

            Time timeInState = state.elapsed - part.TotalStepTimeUntil(state.elapsed);
            float currentRotation;
            currentRotation = LastRotation + (NewRotation - LastRotation) * timeInState.AsSeconds() / step.Duration.AsSeconds();
            float actualRotation =
                MathF.Atan(MathF.Tan(currentRotation * MathF.PI / 180) * MathF.Sin(state.facing * MathF.PI / 180)) *
                180 / MathF.PI;
            float desiredHeight = (float) (Math.Cos((Math.PI / 180) * currentRotation) / Math.Cos((Math.PI / 180) * actualRotation) *
                                           BaseTexture[direction].GetLocalBounds().Height);
            ret.Scale = new Vector2f(1,desiredHeight/BaseTexture[direction].GetLocalBounds().Height);
            ret.Rotation = actualRotation;
            ret.Origin = GetOriginCenter(direction);
            ret.Position = new Vector2f(
                (float) (state.Position.X + Math.Cos((Math.PI / 180) * state.facing) * RotationCenter.X)  - Origin.X - ret.GetLocalBounds().Width / 2,
                (float) (state.Position.Y + Math.Sin((Math.PI / 180) * state.facing) * RotationCenter.X / 2 + RotationCenter.Y + Origin.Y));
            return ret;
        }
    }
}