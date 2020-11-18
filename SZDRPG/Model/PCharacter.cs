using System;
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
        
        public float Reach = 30;
        public float Speed = 300;
        public int Vigor = 10;
        public int Level = 1;
        public int Experience = 0;
        public int Health=10;
        public int Strength=10;
        public int Agility=10;
        public SZDRPG.Pathing.Path Path;
        public bool Busy = false;
        public PCharacter(string name, Game game)
        {
            Game = game;
            try
            {
                string[] lines = System.IO.File.ReadAllLines("../../../Resources/Graphics/Character/" + name + "/setup.txt");
                LoadCharacterRig(name, lines);
            }
            catch (FileNotFoundException fileNotFoundException)
            {
                Console.WriteLine("No graphics setup file for " + name);
                Console.WriteLine("Initializing as default texture");
                GPart defPart = new GPart();
                defPart.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/default.png")));
                GCharacter defCharacter = new GCharacter();
                defCharacter.Parts.Add(defPart);
                AnimationStep defStep = new AnimationStep();
                defStep.Duration = Time.FromSeconds(1f);
                defStep.Rotation = 0;
                AnimationPart defAnimPart = new AnimationPart();
                defAnimPart.Steps.Add(defStep);
                Animation defAnim = new Animation();
                defAnim.Parts.Add(defAnimPart);
                defCharacter.Animations.Add(defAnim);
                Display = defCharacter;
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                Console.WriteLine("No graphics setup file for " + name);
                Console.WriteLine("Initializing as default texture");
                GPart defPart = new GPart();
                defPart.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/default.png")));
                GCharacter defCharacter = new GCharacter();
                defCharacter.Parts.Add(defPart);
                AnimationStep defStep = new AnimationStep();
                defStep.Duration = Time.FromSeconds(1f);
                defStep.Rotation = 0;
                AnimationPart defAnimPart = new AnimationPart();
                defAnimPart.Steps.Add(defStep);
                Animation defAnim = new Animation();
                defAnim.Parts.Add(defAnimPart);
                defCharacter.Animations.Add(defAnim);
                Display = defCharacter;
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
                part.Origin = new Vector2f(float.Parse(originStrings[0]), float.Parse(originStrings[1]));
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
                Path = new Pathing.Path(Position,(Vector2f) Direction, Game, this);
                MoveAction(Direction, elapsed);
            }
            else
            {
                IdleAction(elapsed);
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

        public override void TakeDamage(PCharacter pCharacter, int attack)
        {
            Health -= (int)(attack * (1-DamageReduction()));
            if (Health < 0)
                Health = 0;
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
                UpdateState(2, elapsed);
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

        private void Hit(PEntity target)
        {
            target.TakeDamage(this, Strength);
            Busy = true;
        }

        private bool InReach(PEntity target)
        {
            return (Position.X - target.Position.X) * (Position.X - target.Position.X) +
                (Position.Y - target.Position.Y) * (Position.Y - target.Position.Y) < Reach * Reach;
        }
    }
}