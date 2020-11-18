using System;
using System.Collections.Generic;
using System.Linq;
using SFML.Graphics;
using SFML.System;

namespace SZDRPG.Model
{
    public class Game
    {
        public List<PEntity> Pentities = new List<PEntity>();
        public List<PCharacter> Characters = new List<PCharacter>();
        public List<PCharacter> Enemies = new List<PCharacter>();
        public Map Map;
        public bool Open;
        public bool Start = true;

        public void AddCharacter(string name, Vector2f size, Vector2f position)
        {
            PCharacter character = new PCharacter(name, this);
            character.Position = position;
            character.Size = size;
            character.GenerateHitmesh();
            //character.hitMesh.Size = size;
            //character.hitMesh.GenerateHitMesh(character.Position);
            Characters.Add(character);
        }

        public void AddEnvironment(string name, Vector2f size, Vector2f position)
        {
            PEntity entity = new PEnvironment(name, this);
            entity.Position = position;
            entity.Size = size;
            entity.GenerateHitmesh();
            //entity.hitMesh.Size = size;
            //entity.hitMesh.GenerateHitMesh(entity.Position);
        }
        
        public void NextStep(Time elapsed)
        {
            foreach (var enemy in Enemies)
            {
                enemy.Target = ClosestPlayer(enemy.Position);
            }
            foreach (var entity in Characters)
            {
                entity.NextAction(elapsed);
            }

            HandleCharacters();
            SetDoor();
            if (Open && Pentities.Last().OnEntity(Characters[0].Position))
            {
                GenerateRoom();
            }
        }

        private PEntity ClosestPlayer(Vector2f enemyPosition)
        {
            PCharacter player = null;
            foreach (var character in Characters)
            {
                if (!Enemies.Contains(character))
                {
                    if (player == null)
                    {
                        player = character;
                    }
                    else if ((enemyPosition.X - character.Position.X) * (enemyPosition.X - character.Position.X) +
                             (enemyPosition.Y - character.Position.Y) * (enemyPosition.Y - character.Position.Y) >
                             (enemyPosition.X - player.Position.X) * (enemyPosition.X - player.Position.X) +
                             (enemyPosition.Y - player.Position.Y) * (enemyPosition.Y - player.Position.Y))
                        player = character;
                }
            }

            return player;
        }

        private void HandleCharacters()
        {
            if (Characters[0].Health == 0)
                EndGame();
            else
            {
                List<PCharacter> toBeRemoved = new List<PCharacter>();
                foreach (var character in Characters)
                {
                    if (character.Health == 0)
                    {
                        toBeRemoved.Add(character);
                    }
                }

                foreach (var character in toBeRemoved)
                {
                    if (Enemies.Contains(character))
                        Enemies.Remove(character);
                    Pentities.Remove(character);
                    Characters.Remove(character);
                }
            }
        }

        private void EndGame()
        {
            Start = true;
        }

        public void Draw(RenderWindow window)
        {
            Map.Draw(window);
            List<PEntity> draw = new List<PEntity>(Pentities); 
            while (draw.Count > 0)
            {
                PEntity minY = draw[0];
                foreach (var entity in draw)
                {
                    if (entity.Position.Y < minY.Position.Y)
                    {
                        minY = entity;
                    }
                }
                minY.Draw(window);
                draw.Remove(minY);
            }
        }

        public PEntity EntityAt(Vector2f position)
        {
            foreach (var entity in Pentities)
            {
                if (entity.OnEntity(position))
                    return entity;
            }

            return null;
        }

        public void LoadRoom(GameRoom gameroom)
        {
            Pentities.Clear();
            Pentities.AddRange(Characters);
            Characters[0].Position = new Vector2f(gameroom.Size.X*16, gameroom.Size.Y*32);
            Pentities.AddRange(gameroom.Environment);
            Map.Size = new Vector2f(gameroom.Size.X*32,gameroom.Size.Y*32);
            PDoor door = new PDoor("Door", this);
            door.Position = new Vector2f(Map.Size.X/2,-90);
            door.Size = new Vector2f(94,90);
            //door.hitMesh.Size = door.Size;
            //door.hitMesh.GenerateHitMesh(door.Position);
            door.GenerateHitmesh();
            Pentities.Add(door);
            Open = false;
        }

        public void GenerateRoom()
        {
            Random rand = new Random();
            GameRoom room = new GameRoom(this,new Vector2i(rand.Next(30,100),rand.Next(30,100)));
            int enemyNum = rand.Next(3, 10);
            for (int i = 0; i < enemyNum; i++)
            {
                PCharacter enemy = new PCharacter("Enemy", this);
                enemy.Size = new Vector2f(50,50);
                enemy.Position = new Vector2f(rand.Next(0, room.Size.X * 32), rand.Next(0, room.Size.Y * 32));
                enemy.GenerateHitmesh();
                Characters.Add(enemy);
                Enemies.Add(enemy);
            }
            LoadRoom(room);
        }

        public void SetDoor()
        {
            if (Enemies.Count < 1 && !Open)
            {
                ((PEnvironment) Pentities.Last()).LoadGraphics("OpenDoor");
                Open = true;
            }
        }
    }
}