using System.Collections.Generic;
using SFML.System;

namespace SZDRPG.Model
{
    public class GameRoom
    {
        public Game Game;
        public Vector2i Size;
        public List<PEnvironment> Environment = new List<PEnvironment>();
        public GameRoom(Game game, Vector2i size)
        {
            Game = game;
            Size = size;
        }

        public void GenerateWalls()
        {
            for (int i = 0; i < Size.X; i++)
            {
                for (int j = 0; j < Size.Y; j++)
                {
                    if (i == 0 || i == Size.X - 1)
                    {
                        PEnvironment wall = new PEnvironment("Wall", Game);
                        wall.Position = new Vector2f(i*32,j*32);
                        wall.Size = new Vector2f(32,32);
                        //wall.hitMesh.Size = wall.Size;
                        //wall.hitMesh.GenerateHitMesh(wall.Position);
                        wall.GenerateHitmesh();
                        Environment.Add(wall);
                    }

                    if (j == 0 || j == Size.Y - 1 && i != 0 && i != Size.X - 1)
                    {
                        PEnvironment wall = new PEnvironment("Wall", Game);
                        wall.Position = new Vector2f(i*32,j*32);
                        wall.Size = new Vector2f(32,32);
                        //wall.hitMesh.Size = wall.Size;
                        //wall.hitMesh.GenerateHitMesh(wall.Position);
                        wall.GenerateHitmesh();
                        Environment.Add(wall);
                    }
                }
            }
        }
        
    }
}