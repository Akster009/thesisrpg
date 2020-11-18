using SFML.System;

namespace SZDRPG.Graphics
{
    public class AnimationState
    {
        public int ID;
        public float facing;
        public Time elapsed;
        public Vector2f Position = new Vector2f(0,0);
    }
}