using System;
using SFML.System;

namespace SZDRPG.Graphics
{
    public class AnimationState
    {
        public int ID;
        public float facing;
        public Time elapsed;
        public Vector2f Position = new Vector2f(0,0);
        public bool Repeatable = true;
        public bool Finished = false;

        public void UpdateFacing(Vector2f? position)
        {
            Vector2f? vec = Position - position;
            if(vec != null)
            {
                float nfacing = MathF.Atan(vec.Value.Y / vec.Value.X) * 180 / MathF.PI;
                if (nfacing < 0)
                    nfacing = nfacing + 180;
                if (vec.Value.Y >= 0)
                    nfacing += 180;
                nfacing -= 90;
                if (nfacing < 0)
                    nfacing += 360;
                facing = nfacing;
            }
        }
    }
}