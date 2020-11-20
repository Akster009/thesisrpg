using System;
using SFML.Graphics;
using SFML.System;
using SZDRPG.Graphics;

namespace SZDRPG.Model
{
    public class PProjectile : PEntity
    {
        public PCharacter Owner = null;
        public Vector2f Velocity = new Vector2f();
        public PProjectile(string name, Game game)
        {
            Name = name;
            Game = game;
            LoadProjectile(name);
        }

        private void LoadProjectile(string name)
        {
            GPart part = new GPart();
            part.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Environment/" + name + "/" + "Front.png")));
            part.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Environment/" + name + "/" + "Right.png")));
            part.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Environment/" + name + "/" + "Back.png")));
            part.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Environment/" + name + "/" + "Left.png")));
            GCharacter character = new GCharacter();
            character.Parts.Add(part);
            AnimationStep step = new AnimationStep();
            step.Duration = Time.FromSeconds(1f);
            step.Rotation = 0;
            AnimationPart animPart = new AnimationPart();
            animPart.Steps.Add(step);
            Animation anim = new Animation();
            anim.Parts.Add(animPart);
            character.Animations.Add(anim);
            Display = character;
        }

        public override void Draw(RenderWindow window)
        {
            Display.Draw(window, Position);
        }

        public override string ToString()
        {
            return "PProjectile|" + Name + "|" + (int) Position.X + "|" + (int) Position.Y + "|" + Display.State.ID +
                   "|" + Display.State.facing + "|" + Display.State.elapsed.AsSeconds();
        }

        public override int TakeDamage(PCharacter pCharacter, int attack)
        {
            return 0;
        }

        public override bool IsCollidable()
        {
            return false;
        }

        public override bool IsHittable()
        {
            return false;
        }

        public override void NextAction(Time elapsed)
        {
            if(Owner != null)
            {
                if (Move(elapsed))
                {
                    Game.OutOfBounds.Add(this);
                }
            }

            if (Position.X < 0 || Position.X > Game.Map.Size.X || Position.Y < 0 || Position.Y > Game.Map.Size.Y)
                Game.OutOfBounds.Add(this);
        }

        public bool Move(Time elapsed)
        { 
            Vector2f deltaFirst = new Vector2f(0,0);
            Vector2f deltaSecond = new Vector2f(0,0);
            if (Velocity.X < 0)
            {
                deltaFirst.X = -1;
                deltaSecond.X = -1;
            }
            else if ((Velocity * elapsed.AsSeconds()).X > 0)
            {
                deltaFirst.X = 1;
                deltaSecond.X = 1;
            }
            if ((Velocity * elapsed.AsSeconds()).Y < 0)
            {
                deltaFirst.Y = -1;
            }
            else if ((Velocity * elapsed.AsSeconds()).Y > 0)
            {
                deltaFirst.Y = 1;
            }
            float longer = MathF.Abs((Velocity * elapsed.AsSeconds()).X);
            float shorter = MathF.Abs((Velocity * elapsed.AsSeconds()).Y);
            if (!(longer > shorter))
            {
                longer = shorter;
                shorter = MathF.Abs((Velocity * elapsed.AsSeconds()).X);
                if ((Velocity * elapsed.AsSeconds()).Y < 0)
                    deltaSecond.Y = -1;
                else if ((Velocity * elapsed.AsSeconds()).Y > 0)
                    deltaSecond.Y = 1;
                deltaSecond.X = 0;
            }

            float numerator = longer / 2;
            for (int i = 0; i <= longer; i++)
            {
                PEntity found = Game.EntityAt(Position);
                if(found != null && found != Owner && found.IsHittable() && found != this)
                {
                    Owner.Experience += found.TakeDamage(Owner, Owner.Agility);
                    return true;
                }
                numerator += shorter;
                if (!(numerator < longer))
                {
                    numerator -= longer;
                    Position.X += deltaFirst.X;
                    Position.Y += deltaFirst.Y;
                }
                else
                {
                    Position.X += deltaSecond.X;
                    Position.Y += deltaSecond.Y;
                }
            }

            return false;
        }
    }
}