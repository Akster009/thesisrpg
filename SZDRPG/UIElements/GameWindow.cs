using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SZDRPG.Model;

namespace SZDRPG.UIElements
{
    public class GameWindow
    {
        public Game Game;
        public bool Holding = false;
        public List<UIElement> Elements = new List<UIElement>();

        public void Draw(RenderWindow window)
        {
            Game.Draw(window);
        }

        public void OnClick(object sender, MouseButtonEventArgs args)
        {
            bool UIClick = false;
            Vector2f position = ((RenderWindow)sender).MapPixelToCoords(new Vector2i(args.X, args.Y));
            //Vector2f position = new Vector2f(args.X, args.Y);
            foreach (var element in Elements)
            {
                Vector2f pos = new Vector2f(args.X,args.Y);
                if (element.OnElement(pos))
                {
                    UIClick = true;
                    element.OnClick(sender, args);
                }
            }

            if (!UIClick)
            {
                bool attack = false;
                foreach (var gameGentity in Game.Pentities)
                {
                    if (gameGentity != Game.Pentities[0] && gameGentity.OnEntity(position))
                    {
                        attack = true;
                        Game.Characters[0].Target = gameGentity;
                        Game.Characters[0].Direction = null;
                        //Game.Characters[0].Display.State.UpdateFacing(position);
                        Console.WriteLine(Game.Pentities[0].Display.State.facing);
                    }
                }

                if (!attack)
                {
                    Game.Characters[0].Direction = Game.Map.IntersectAt(Game.Characters[0].Position, position);
                    Game.Characters[0].Target = null;
                    //Game.Characters[0].Display.State.UpdateFacing(position);
                    Holding = true;
                }
            }
        }

        public void OnRelease(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Holding = false;
        }

        public void HandleHolding(RenderWindow window)
        {
            if (Holding)
            {
                Game.Characters[0].Direction = Game.Map.IntersectAt(Game.Characters[0].Position,window.MapPixelToCoords(Mouse.GetPosition(window)));
                Game.Characters[0].Target = null;
                //Game.Characters[0].Display.State.UpdateFacing(window.MapPixelToCoords(Mouse.GetPosition(window)));
            }
        }
    }
}