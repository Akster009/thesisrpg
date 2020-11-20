using System;
using System.Collections.Generic;
using System.Drawing;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using SZDRPG.Graphics;
using SZDRPG.Pathing;

namespace SZDRPG.Model
{
    public abstract class PEntity
    {
        public Vector2f Position;
        public Vector2f Size;
        public GCharacter Display;
        public abstract void Draw(RenderWindow window);
        public delegate void IntentDel(Vector2f direction, PEntity target);
        public PEntity Target;
        public Vector2f? Direction;
        public Game Game;
        public string Name;

        public abstract override string ToString();

        public class HitMesh
        {
            public class Node : WayPoint
            {
                public Vector2f RelativeLocation { get; set; }

            }

            public Vector2f Size { get; set; }
            public List<Node> TravelNodes { get; set; }
            public int Orientation { get; set; }

            public void GenerateHitMesh(Vector2f position)
            {
                TravelNodes = new List<Node>();
                for (int i = 0; i < 4; i++)
                {
                    TravelNodes.Add(new Node());
                }

                TravelNodes[0].Location = new Vector2f(position.X - 2-Size.X/2, position.Y - 2);
                TravelNodes[0].RelativeLocation = new Vector2f(-2 - Size.X / 2, -2);
                //Console.WriteLine(TravelNodes[0].Location.X + " " + TravelNodes[0].Location.Y);
                TravelNodes[1].Location = new Vector2f(position.X + Size.X / 2 + 2, position.Y - 2);
                TravelNodes[1].RelativeLocation = new Vector2f(Size.X / 2 + 2, -2);
                //Console.WriteLine(TravelNodes[1].Location.X + " " + TravelNodes[0].Location.Y);
                TravelNodes[2].Location = new Vector2f(position.X - 2 - Size.X / 2, position.Y + Size.Y + 2);
                TravelNodes[2].RelativeLocation = new Vector2f(-2 - Size.X / 2, Size.Y + 2);
                //Console.WriteLine(TravelNodes[2].Location.X + " " + TravelNodes[0].Location.Y);
                TravelNodes[3].Location = new Vector2f(position.X + Size.X / 2 + 2, position.Y + Size.Y + 2);
                TravelNodes[3].RelativeLocation = new Vector2f(Size.X / 2 + 2, Size.Y + 2);
                //Console.WriteLine(TravelNodes[3].Location.X + " " + TravelNodes[0].Location.Y);
            }
        }

        public HitMesh hitMesh = new HitMesh();

        public bool OnEntity(Vector2f pos)
        {
            return pos.X >= Position.X - hitMesh.Size.X / 2 && pos.X <= Position.X + hitMesh.Size.X / 2 &&
                   pos.Y >= Position.Y && pos.Y <= Position.Y + hitMesh.Size.Y;
        }

        public abstract int TakeDamage(PCharacter pCharacter, int attack);

        public abstract bool IsCollidable();
        public abstract bool IsHittable();

        public void GenerateHitmesh()
        {
            hitMesh.Size = Size;
            hitMesh.GenerateHitMesh(Position);
        }

        public abstract void NextAction(Time elapsed);

        public void LoadDefault()
        {
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
}