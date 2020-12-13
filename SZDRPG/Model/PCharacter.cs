using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SFML.Graphics;
using SFML.System;
using SZDRPG.Graphics;
using SZDRPG.Pathing;

namespace SZDRPG.Model
{
    public class PCharacter : PEntity
    {
        
        public float Reach = 50;
        public float Speed = 200;
        public int Vigor = 10;
        public int Strength=10;
        public int Agility=10;
        public int Intelligence = 10;
        public int Spirit = 10;
        private int level = 1;

        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                if (value < level)
                    level = value;
                while(level < value)
                {
                    SkillPoints += 5;
                    Vigor++;
                    Strength++;
                    Agility++;
                    Intelligence++;
                    Spirit++;
                    Health = Vigor;
                    Mana = Intelligence;
                    level++;
                }
            }
        }
        private int experience = 0;

        public int Experience
        {
            get
            {
                return experience;
            }
            set
            {
                while (value >= ExperienceToNextLevel())
                {
                    value -= ExperienceToNextLevel();
                    Level++;
                }

                experience = value;
            }
        }
        public int Health=10;
        public int Mana = 10;
        public SZDRPG.Pathing.Path Path;
        public bool Busy = false;
        public Time LastManaGain = Time.Zero;
        public int SkillPoints = 0;

        public class Ability
        {
            public delegate void AbilityDel(PCharacter Owner);
            public AbilityDel Cast;
            public Time Cooldown = Time.Zero;
            public Time LastUsed = Time.Zero;
            public int Mana = 0;
            public Ability(Time cooldown, int mana = 0, AbilityDel cast = null)
            {
                Cast = cast;
                Mana = mana;
                Cooldown = cooldown;
            }
            public float CooldownPercentage()
            {
                float cd = LastUsed.AsSeconds() / Cooldown.AsSeconds();
                if (cd > 1)
                    cd = 1;
                return cd;
            }
        }
        public List<Ability> Abilities = new List<Ability>();
        public PCharacter(string name, Game game)
        {
            Name = name;
            Game = game;
            Abilities.Add(new Ability(Time.FromSeconds(1)));
            try
            {
                string[] lines = System.IO.File.ReadAllLines("../../../Resources/Graphics/Character/" + name + "/setup.txt");
                LoadCharacterRig(name, lines);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                Console.WriteLine("No graphics setup file for " + name);
                Console.WriteLine("Initializing as default texture");
                LoadDefault();
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                Console.WriteLine("No graphics directory for " + name);
                Console.WriteLine("Initializing as default texture");
                LoadDefault();
            }
        }
        public int ExperienceToNextLevel()
        {
            return (int)((float) Level / (9 + Level) * 100);
        }

        public void IncreseSkill(int idx)
        {
            if(SkillPoints > 0)
            {
                switch (idx)
                {
                    case 0:
                        Vigor++;
                        break;
                    case 1:
                        Strength++;
                        break;
                    case 2:
                        Agility++;
                        break;
                    case 3:
                        Intelligence++;
                        break;
                    case 4:
                        Spirit++;
                        break;
                }

                SkillPoints--;
            }
        }
        private void LoadCharacterRig(string name, string[] lines)
        {
            Display = new GCharacter();
            int partnum = Int32.Parse(lines[0]);
            for (int i = 0; i < partnum; i++)
            {
                GPart part = new GPart();
                string partName = lines[i * 3 + 1];
                part.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/" + name + "/" + partName  + "/Front.png")));
                part.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/" + name + "/" + partName  + "/Right.png")));
                part.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/" + name + "/" + partName  + "/Back.png")));
                part.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/" + name + "/" + partName  + "/Left.png")));
                string[] rotCenterStrings = lines[i * 3 + 2].Split(" ");
                part.RotationCenter = new Vector2f(float.Parse(rotCenterStrings[0]), float.Parse(rotCenterStrings[1]));
                string[] originStrings = lines[i * 3 + 3].Split(" ");
                part.Origin = new Vector3f(float.Parse(originStrings[0]), float.Parse(originStrings[1]), float.Parse(originStrings[1]));
                Display.Parts.Add(part);
            }
            Display.State.ID = 0;
            Display.State.facing = 0;
            Display.State.elapsed = Time.Zero;
            int animnum = Int32.Parse(lines[partnum*3+1]);
            for (int i = 0; i < animnum; i++)
            {
                Animation animation = new Animation();
                string animName = lines[partnum * 3 + 2 + i];
                string[] amimLines = File.ReadAllLines("../../../Resources/Graphics/Character/" + name + "/Animations/" + animName + ".txt");
                int sum = 0;
                for (int j = 0; j < partnum; j++)
                {
                    AnimationPart part = new AnimationPart();
                    int animSteps = Int32.Parse(amimLines[sum++]);
                    for (int k = 0; k < animSteps; k++)
                    {
                        AnimationStep step = new AnimationStep();
                        string[] stepInfo = amimLines[sum++].Split(" ");
                        step.Duration = Time.FromSeconds(float.Parse(stepInfo[0]));
                        step.Rotation = float.Parse(stepInfo[1]);
                        part.Steps.Add(step);
                    }
                    animation.Parts.Add(part);
                }

                if (amimLines.Last() == "Y")
                    animation.Repeatable = true;
                else
                    animation.Repeatable = false;
                Display.Animations.Add(animation);
            }
        }
        public override void Draw(RenderWindow window)
        {
            Display.Draw(window, Position);
        }
        public override void NextAction(Time elapsed)
        {
            if (Health <= 0) return;
            foreach (var ability in Abilities)
            {
                ability.LastUsed += elapsed;
            }

            HandleManaGain(elapsed);
            if (Busy)
            {
                BusyAction(elapsed);
            }
            else if (Target != null)
            {
                AttackAction(elapsed);
            }
            else if (Direction != null)
            {
                Path = new Pathing.Path(Position, (Vector2f) Direction, Game, this);
                MoveAction(Direction, elapsed);
            }
            else
            {
                IdleAction(elapsed);
            }
        }

        private void HandleManaGain(Time elapsed)
        {
            if (Mana < Intelligence)
            {
                LastManaGain += elapsed;
                while (LastManaGain > Time.FromSeconds(10)/Spirit)
                {
                    Mana++;
                    LastManaGain -= Time.FromSeconds(10)/Spirit;
                }
            }
            else
            {
                LastManaGain = Time.Zero;
            }
        }

        public void DoSpecialAttack(int idx)
        {
            if(!Busy && Mana >= Abilities[idx].Mana && Abilities[idx].LastUsed >= Abilities[idx].Cooldown)
            {
                Abilities[idx].LastUsed = Time.Zero;
                Busy = true;
                UpdateState(2+idx, Time.Zero);
                Mana -= Abilities[idx].Mana;
                Abilities[idx].Cast(this);
            }
        }
        private void UpdateState(int idx, Time elapsed)
        {
            if (Display.State.ID != idx)
            {
                Display.State.ID = idx;
                Display.State.elapsed = Time.Zero;
            }
            else
            {
                Display.State.elapsed += elapsed;
            }
        }

        private void IdleAction(Time elapsed)
        {
            UpdateState(0,elapsed);
        }
        private void BusyAction(Time elapsed)
        {
            UpdateState(Display.State.ID,elapsed);
            if (Display.State.Finished)
            {
                Busy = false;
                Display.State.Finished = false;
                Target = null;
                Direction = null;
            }
        }

        public override int TakeDamage(PCharacter pCharacter, int attack)
        {
            Health -= (int)(attack * (1-DamageReduction()));
            if (Health < 0)
                Health = 0;
            if (Health == 0)
                return Level;
            return 0;
        }

        private float DamageReduction()
        {
            return (float)Agility / (50 + (float)Agility);
        }

        public override bool IsCollidable()
        {
            return false;
        }

        public override bool IsHittable()
        {
            return true;
        }

        private void AttackAction(Time elapsed)
        {
            if (InReach(Target) && Target.IsHittable())
            {
                UpdateState(2, Time.Zero);
                Abilities[0].LastUsed = Time.Zero;
                Hit(Target);
            }
            else
            {
                Path = new Pathing.Path(Position,Target.Position, Game, this);
                MoveAction(Target.Position, elapsed);
            }
        }

        private void MoveAction(Vector2f? targetPosition, Time elapsed)
        {
            UpdateState(1,elapsed);
            Display.State.UpdateFacing(Direction);
            if (Target != null)
            {
                if (Path.EndingPoint != Target.Position)
                {
                    Path.EndingPoint = Target.Position;
                    Path.FindPath();
                }
            }
            else if (targetPosition != null)
            {
                if (Path.EndingPoint != targetPosition)
                {
                    Path.EndingPoint = (Vector2f) targetPosition;
                    Path.FindPath();
                }
            }
            if (Path != null)
            {
                float speed = Speed * elapsed.AsSeconds();
                double distance = 0;
                while(distance < speed && Path.PathEdges.Count > 0)
                {
                    double edgeLength = Path.EdgeLength(Path.PathEdges.Count - 1); 
                    if (edgeLength < speed)
                    {
                        Vector2f edge = Path.PopLastEdge();
                        Move(edge);
                        distance += edgeLength;
                    }
                    else
                    {
                        Vector2f edge = Path.PopLastEdge();
                        double x, y;
                        if (edge.Y == 0)
                        {
                            x = speed;
                            y = 0;
                        }
                        else
                        {
                            x = Math.Abs(edge.X / edge.Y *
                                         Math.Sqrt((speed - distance) * (speed - distance) /
                                                   (edge.X / edge.Y * edge.X / edge.Y + 1)));
                            y = Math.Abs(Math.Sqrt((speed - distance) * (speed - distance) /
                                                   (edge.X / edge.Y * edge.X / edge.Y + 1)));
                        }
                        if (edge.X < 0)
                            x = -1 * x;
                        if (edge.Y < 0)
                            y = -1 * y;
                        Move(new Vector2f((float) x, (float) y));
                        Path.PathEdges.Add(new Vector2f((float)(edge.X-x),(float)(edge.Y-y)));
                        distance = speed;
                    }
                }
            }
            if(Path != null)
                Direction = Path.EndingPoint;

            if (Position == Direction)
            {
                Direction = null;
            }
        }

        private void Move(Vector2f? direction)
        {
            if(direction != null)
                Position += new Vector2f(direction.Value.X, direction.Value.Y);
        }

        public void Hit(PEntity target)
        {
            Experience += target.TakeDamage(this, Strength);
            Busy = true;
        }

        public bool InReach(PEntity target, float scale = 1)
        {
            return (Position.X - target.Position.X) * (Position.X - target.Position.X) +
                (Position.Y - target.Position.Y) * (Position.Y - target.Position.Y) < (Reach*scale) * (Reach*scale);
        }

        public override string ToString()
        {
            return "PCharacter|" + Name + "|" + (int) Position.X + "|" + (int) Position.Y + "|" + Display.State.ID +
                   "|" + Display.State.facing + "|" + Display.State.elapsed.AsSeconds() + "|" + Experience + "|" +
                   Level + "|" + SkillPoints + "|" + Vigor + "|" + Strength + "|" + Agility + "|" + Intelligence + "|" +
                   Spirit + "|" + Health + "|" + Mana;
        }

        public new bool OnEntity(Vector2f pos)
        {
            return pos.X >= Position.X - hitMesh.Size.X / 2 && pos.X <= Position.X + hitMesh.Size.X / 2 &&
                   pos.Y <= Position.Y && pos.Y >= Position.Y - hitMesh.Size.Y;
        }
    }
}