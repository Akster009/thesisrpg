using System.Collections.Generic;
using SFML.Graphics;

namespace SZDRPG.Graphics
{
    public class GCharacter
    {
        public List<GPart> Parts = new List<GPart>();
        public List<Animation> Animations = new List<Animation>();
        public AnimationState State = new AnimationState();
        
        public void Draw(RenderWindow window)
        {
            for (int i = 0; i < Parts.Count; i++)
            {
                window.Draw(Parts[i].Draw(Animations[State.ID].Parts[i],State));
            }
        }
    }
}