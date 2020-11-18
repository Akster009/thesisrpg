using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;

namespace SZDRPG.Graphics
{
    public class GCharacter
    {
        public List<GPart> Parts = new List<GPart>();
        public List<Animation> Animations = new List<Animation>();
        public AnimationState State = new AnimationState();
        
        public void Draw(RenderWindow window, Vector2f position)
        {
            State.Position = position;
            State.Repeatable = Animations[State.ID].Repeatable;
            List<Sprite> sprites = new List<Sprite>();
            List<float> distances = new List<float>();
            for (int i = 0; i < Parts.Count; i++)
            {
                sprites.Add(Parts[i].Draw(Animations[State.ID].Parts[i], State));
                distances.Add(Parts[i].RelativeDistance(State));
            }

            DrawParts(window, sprites, distances);
        }

        private void DrawParts(RenderWindow window, List<Sprite> sprites, List<float> distances)
        {
            while (sprites.Count > 0)
            {
                int minInd = 0;
                float minDist = distances[0];
                for (int i = 0; i < distances.Count; i++)
                {
                    if (distances[i] < minDist)
                    {
                        minDist = distances[i];
                        minInd = i;
                    }
                }
                window.Draw(sprites[minInd]);
                sprites.RemoveAt(minInd);
                distances.RemoveAt(minInd);
            }
        }
    }
}