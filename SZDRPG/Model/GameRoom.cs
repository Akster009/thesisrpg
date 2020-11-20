using System;
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
            Random rand = new Random();
            int wallNum = rand.Next(0, (int) (Size.X * Size.X * 0.0256));
            for (int i = 0; i < wallNum; i++)
            {
                PEnvironment wall = new PEnvironment("Wall", Game);
                wall.Size = new Vector2f(64,64);
                wall.GenerateHitmesh();
                wall.Position = new Vector2f(rand.Next(200,Size.X*32-200),rand.Next(200, Size.Y*32-200));
                if(!TooClose(wall.Position, 200))
                    Environment.Add(wall);
            }
        }

        private bool TooClose(Vector2f wallPosition, float distance)
        {
            foreach (var environment in Environment)
            {
                if ((environment.Position - wallPosition).X * (environment.Position - wallPosition).X +
                    (environment.Position - wallPosition).Y * (environment.Position - wallPosition).Y >
                    distance * distance)
                    return true;
            }
            return false;
        }
    }
}