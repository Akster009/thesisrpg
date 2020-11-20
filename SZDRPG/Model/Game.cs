using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SFML.Graphics;
using SFML.System;

namespace SZDRPG.Model
{
    public class Game
    {
        public List<PEntity> Pentities = new List<PEntity>();
        public List<PCharacter> Characters = new List<PCharacter>();
        public List<PCharacter> Enemies = new List<PCharacter>();
        public List<PCharacter> NetworkPlayers = new List<PCharacter>();
        public List<PProjectile> OutOfBounds = new List<PProjectile>();
        public PEnvironment Door;
        public Map Map;
        public bool Open;
        public bool Start = true;
        public int StatusLength;

        public void AddCharacter(string name, Vector2f size, Vector2f position)
        {
            PCharacter character = new PCharacter(name, this);
            character.Position = position;
            character.Size = size;
            character.GenerateHitmesh();
            Characters.Add(character);
            Pentities.Add(character);
        }
        public void AddCharacter(PCharacter character)
        {
            Characters.Add(character);
            Pentities.Add(character);
        }
        public void AddEnvironment(string name, Vector2f size, Vector2f position)
        {
            PEntity entity = new PEnvironment(name, this);
            entity.Position = position;
            entity.Size = size;
            entity.GenerateHitmesh();
        }
        public void AddProjectile(PProjectile projectile)
        {
            Pentities.Add(projectile);
        }
        public void NextStep(Time elapsed)
        {
            foreach (var enemy in Enemies)
            {
                enemy.Target = ClosestPlayer(enemy.Position);
            }
            foreach (var entity in Pentities)
            {
                entity.NextAction(elapsed);
            }

            foreach (var projectile in OutOfBounds)
            {
                Pentities.Remove(projectile);
            }
            OutOfBounds.Clear();

            HandleCharacters();
            SetDoor();
            if (Open && Door.OnEntity(Characters[0].Position))
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

        public void EndGame()
        {
            Start = true;
            SaveCharacter();
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
                if(Characters.Contains(entity))
                {
                    if (((PCharacter) entity).OnEntity(position))
                        return entity;
                }
                else
                {
                    if (entity.OnEntity(position))
                        return entity;
                }
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
            Map.GenerateTiles(4);
            Map.num++;
            PDoor door = new PDoor("Door", this);
            door.Position = new Vector2f(Map.Size.X/2,-180);
            door.Size = new Vector2f(188,180);
            door.GenerateHitmesh();
            Pentities.Add(door);
            Door = door;
            Open = false;
        }

        public void GenerateRoom()
        {
            Random rand = new Random();
            GameRoom room = new GameRoom(this,new Vector2i(rand.Next(30,70),rand.Next(30,70)));
            room.Game = this;
            int enemyNum = rand.Next(1, room.Size.X*32/500*room.Size.Y*32/500);
            for (int i = 0; i < enemyNum; i++)
            {
                PCharacter enemy = new PCharacter("Enemy", this);
                enemy.Size = new Vector2f(70,100);
                enemy.Position = new Vector2f(rand.Next(0, room.Size.X * 32), rand.Next(0, room.Size.Y * 32-200));
                enemy.GenerateHitmesh();
                enemy.Agility = 1;
                enemy.Vigor = 5;
                enemy.Strength = 3;
                enemy.Intelligence = 3;
                enemy.Spirit = 3;
                enemy.Level = Characters[0].Level;
                while(enemy.SkillPoints > 0)
                {
                    switch (rand.Next(5))
                    {
                        case 0:
                            enemy.Agility++;
                            break;
                        case 1:
                            enemy.Strength++;
                            break;
                        case 2:
                            enemy.Intelligence++;
                            break;
                        case 3:
                            enemy.Spirit++;
                            break;
                        case 4:
                            enemy.Vigor++;
                            break;
                    }

                    enemy.SkillPoints--;
                }

                enemy.Health = enemy.Vigor;
                enemy.Mana = enemy.Intelligence;
                Characters.Add(enemy);
                Enemies.Add(enemy);
            }
            //room.GenerateWalls();
            LoadRoom(room);
        }

        public void SetDoor()
        {
            if (Enemies.Count < 1 && !Open)
            {
                Door.LoadGraphics("OpenDoor");
                Open = true;
            }
        }

        public void SaveCharacter()
        {
            string[] lines =
            {
                Characters[0].Level.ToString(),
                Characters[0].Experience.ToString(),
                Characters[0].SkillPoints.ToString(),
                Characters[0].Vigor.ToString(),
                Characters[0].Strength.ToString(),
                Characters[0].Agility.ToString(),
                Characters[0].Intelligence.ToString(),
                Characters[0].Spirit.ToString()
            };
            try
            {
                File.WriteAllLines("../../../Resources/Saves/save.txt",lines);
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine(e);
                Directory.CreateDirectory("../../../Resources/Saves");
            }

        }
        
        public PCharacter LoadCharacter(Vector2f position, Vector2f size)
        {
            PCharacter character = new PCharacter("Player", this);
            try
            {
                string[] lines = System.IO.File.ReadAllLines("../../../Resources/Saves/save.txt");
                character.Level = int.Parse(lines[0]);
                character.Experience = int.Parse(lines[1]);
                character.SkillPoints = int.Parse(lines[2]);
                character.Vigor = int.Parse(lines[3]);
                character.Health = character.Vigor;
                character.Strength = int.Parse(lines[4]);
                character.Agility = int.Parse(lines[5]);
                character.Intelligence = int.Parse(lines[6]);
                character.Mana = character.Intelligence;
                character.Spirit = int.Parse(lines[7]);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                Console.WriteLine("No save file found");
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                Console.WriteLine("No save directory found");
            }
            character.Position = position;
            character.Size = size;
            character.GenerateHitmesh();
            return character;
        }

        public byte[] Status()
        {
            List<string> status = new List<string>();
            Map.Status(status);
            status.Add(Pentities.Count.ToString());
            foreach (var entity in Pentities)
            {
                if(NetworkPlayers.Contains(entity))
                    status.Add("Network");
                status.Add(entity.ToString());
            }

            string send = "";
            foreach (var statu in status)
            {
                send += statu + "|";
            }
            Byte[] sendBytes = Encoding.ASCII.GetBytes(send);
            StatusLength = sendBytes.Length;
            return sendBytes;
        }
    }
}