using SFML.Graphics;
using SFML.System;

namespace SZDRPG.Model
{
    public class PDoor : PEnvironment
    {
        public PDoor(string name, Game game) : base(name, game)
        {
        }

        public override void NextAction(Time elapsed)
        {
            base.NextAction(elapsed);
            
        }

        public override bool IsCollidable()
        {
            return false;
        }
    }
}