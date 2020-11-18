using System;
using System.IO;
using SFML.Graphics;
using SFML.System;
using SZDRPG.Graphics;

namespace SZDRPG.Model
{
    public class PEnvironment : PEntity
    {

        public PEnvironment(string name, Game game)
        {
            Game = game;
            try
            {
                LoadGraphics(name);
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

        public void LoadGraphics(string name)
        {
            GPart part = new GPart();
            part.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Environment/" + name + "/" + name + ".png")));
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

        public override void NextAction(Time elapsed)
        {
            
        }

        public override void TakeDamage(PCharacter pCharacter, int attack)
        {
            
        }

        public override bool IsCollidable()
        {
            return true;
        }

        public override bool IsHittable()
        {
            return false;
        }
    }
}