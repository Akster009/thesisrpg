using System;
using SFML.System;

namespace SZDRPG.Model
{
    public static class EntityActions
    {
        public static void Throw(PCharacter character)
        {
            PProjectile sword = new PProjectile("Sword", character.Game);
            sword.Owner = character;
            sword.Position = character.Position;
            sword.Velocity = new Vector2f(MathF.Cos((MathF.PI / 180) * (character.Display.State.facing + 90)) * 300,
                MathF.Sin((MathF.PI / 180) * (character.Display.State.facing + 90)) * 300);
            sword.Display.State.facing = character.Display.State.facing;
            character.Game.Pentities.Add(sword);
        }

        public static void WhirlWind(PCharacter character)
        {
            foreach (var gameCharacter in character.Game.Characters)
            {
                if (gameCharacter != character && character.InReach(gameCharacter,2))
                {
                    character.Hit(gameCharacter);
                }
            }
        }

        public static void Lunge(PCharacter character)
        {
            character.Position += new Vector2f(MathF.Cos((MathF.PI / 180) * (character.Display.State.facing + 90)) * character.Speed*3,
                MathF.Sin((MathF.PI / 180) * (character.Display.State.facing + 90)) * character.Speed*3);
            if (character.Position.X < 0)
                character.Position.X = 0;
            if (character.Position.X > character.Game.Map.Size.X)
                character.Position.X = character.Game.Map.Size.X;
            if (character.Position.Y < 0)
                character.Position.Y = 0;
            if (character.Position.Y > character.Game.Map.Size.Y)
                character.Position.Y = character.Game.Map.Size.Y;
            foreach (var gameCharacter in character.Game.Characters)
            {
                if (gameCharacter != character && character.InReach(gameCharacter))
                {
                    character.Hit(gameCharacter);
                }
            }
        }
    }
}