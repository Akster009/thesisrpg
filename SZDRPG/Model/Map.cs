using System;
using System.Collections.Generic;
using System.IO;
using SFML.Graphics;
using SFML.System;

namespace SZDRPG.Model
{
    public class Map
    {
        public Vector2f Size;
        public Texture Tile;
        public float Scale = 1;
        public List<PEnvironment> Environment = new List<PEnvironment>();

        public void Draw(RenderWindow window)
        {
            Sprite tile = new Sprite(Tile);
            tile.Scale = new Vector2f(Scale, Scale);
            for (float i = 0; i < Size.X; i += Scale * Tile.Size.X)
            {
                for (float j = 0; j < Size.Y; j += Scale * Tile.Size.Y)
                {
                    tile.Position = new Vector2f(i,j);
                    window.Draw(tile);
                }
            }
        }

        public  Vector2f? IntersectAt(Vector2f entity, Vector2f position)
        {
            while (!(position.X >= 0 && position.X <= Size.X && position.Y >= 0 && position.Y <= Size.Y))
            {
                if (position.X < 0)
                {
                    position.Y = (0 - entity.X) * (position.Y - entity.Y) / (position.X - entity.X) + entity.Y;
                    position.X = 0;
                }
                else if (position.X > Size.X)
                {
                    position.Y = (Size.X - entity.X) * (position.Y - entity.Y) / (position.X - entity.X) + entity.Y;
                    position.X = Size.X;
                }
                else if (position.Y < 0)
                {
                    position.X = (0 - entity.Y) * (position.X - entity.X) / (position.Y - entity.Y) + entity.X;
                    position.Y = 0;
                }
                else if (position.Y > Size.Y)
                {
                    position.X = (Size.Y - entity.Y) * (position.X - entity.X) / (position.Y - entity.Y) + entity.X;
                    position.Y = Size.Y;
                }
            }

            return position;
        }
    }
}