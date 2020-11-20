using System;
using System.Collections.Generic;
using System.IO;
using SFML.Graphics;
using SFML.System;

namespace SZDRPG.Model
{
    public class Map
    {
        public int num = 0;
        public Vector2f Size;
        public float Scale = 2;
        public List<PEnvironment> Environment = new List<PEnvironment>();

        public class MapTileCenter
        {
            public int Tile = 0;
            public Vector2f Position = new Vector2f();

            public MapTileCenter(int tile, Vector2f position)
            {
                Tile = tile;
                Position = position;
            }
        }
        public List<MapTileCenter> Tiles = new List<MapTileCenter>();
        public Dictionary<string,Texture> Textures = new Dictionary<string, Texture>();
        public List<List<string>> MapLayout = new List<List<string>>();

        public Map(int num)
        {
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    if (i == j)
                    {
                        Textures.Add(i.ToString(),new Texture("../../../Resources/Graphics/Tile/" + i + ".png"));
                    }
                    else
                    {
                        Textures.Add(i.ToString() + j.ToString(),new Texture("../../../Resources/Graphics/Tile/" + i + j + ".png"));
                    }
                }
            }
        }
        public void Draw(RenderWindow window)
        {
            Sprite sprite = new Sprite();
            for (int i = 0; i < MapLayout.Count; i++)
            {
                for (int j = 0; j < MapLayout[i].Count; j++)
                {
                    sprite.Texture = Textures[MapLayout[i][j]];
                    sprite.Position = new Vector2f(Textures["0"].Size.X * Scale * i,Textures["0"].Size.Y * Scale * j);
                    sprite.Scale = new Vector2f(Scale,Scale);
                    window.Draw(sprite);
                }
            }
        }

        private string GetTexture(Vector2f position)
        {
            MapTileCenter onPosition = GetClosestTile(position);
            MapTileCenter closestNeighbour = GetClosestNeigbour(position);
            if (onPosition.Tile != closestNeighbour.Tile)
            {
                return onPosition.Tile.ToString() + closestNeighbour.Tile.ToString();
            }

            return onPosition.Tile.ToString();
        }

        private MapTileCenter GetClosestTile(Vector2f position)
        {
            MapTileCenter closest = Tiles[0];
            foreach (var tile in Tiles)
            {
                if ((closest.Position - position).X * (closest.Position - position).X +
                    (closest.Position - position).Y * (tile.Position - position).Y <
                    (tile.Position - position).X * (tile.Position - position).X +
                    (tile.Position - position).Y * (tile.Position - position).Y)
                {
                    closest = tile;
                }
            }

            return closest;
        }

        private MapTileCenter GetClosestNeigbour(Vector2f position)
        {
            MapTileCenter closest = GetClosestTile(position + new Vector2f(Textures["1"].Size.X, 0));
            MapTileCenter next = GetClosestTile(position - new Vector2f(Textures["1"].Size.X, 0));
            if ((closest.Position - position).X * (closest.Position - position).X +
                (closest.Position - position).Y * (next.Position - position).Y <
                (next.Position - position).X * (next.Position - position).X +
                (next.Position - position).Y * (next.Position - position).Y)
            {
                closest = next;
            }

            next = GetClosestTile(position + new Vector2f(0, Textures["1"].Size.Y));
            if ((closest.Position - position).X * (closest.Position - position).X +
                (closest.Position - position).Y * (next.Position - position).Y <
                (next.Position - position).X * (next.Position - position).X +
                (next.Position - position).Y * (next.Position - position).Y)
            {
                closest = next;
            }
            next = GetClosestTile(position - new Vector2f(0, Textures["1"].Size.Y));
            if ((closest.Position - position).X * (closest.Position - position).X +
                (closest.Position - position).Y * (next.Position - position).Y <
                (next.Position - position).X * (next.Position - position).X +
                (next.Position - position).Y * (next.Position - position).Y)
            {
                closest = next;
            }

            return closest;

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

        public void LoadTiles()
        {
            for (float i = 0, n = 0; i < Size.X; i += Scale * Textures["0"].Size.X, n++)
            {
                MapLayout.Add(new List<string>());
                for (float j = 0; j < Size.Y; j += Scale * Textures["0"].Size.Y)
                {
                    MapLayout[(int)n].Add(GetTexture(new Vector2f(i,j)));
                }
            }
        }

        public void GenerateTiles(int num)
        {
            Tiles.Clear();
            MapLayout.Clear();
            Random rand = new Random();
            int tileNum = rand.Next(1, (int)(Size.X / 100 * Size.Y / 100));
            for (int i = 0; i < tileNum; i++)
            {
                Tiles.Add(new MapTileCenter(rand.Next(num),new Vector2f(rand.Next(0,(int)Size.X),rand.Next(0,(int)Size.Y))));
            }
            LoadTiles();
        }

        public void Status(List<string> status)
        {
            status.Add(num.ToString());
            status.Add(((int)Size.X).ToString());
            status.Add(((int)Size.Y).ToString());
            status.Add(Tiles.Count.ToString());
            foreach (var tile in Tiles)
            {
                status.Add(tile.Tile.ToString());
                status.Add(((int)tile.Position.X).ToString());
                status.Add(((int)tile.Position.Y).ToString());
            }
        }
    }
}