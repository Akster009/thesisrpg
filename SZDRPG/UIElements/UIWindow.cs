using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using SFML.Window;

namespace SZDRPG.UIElements
{
    public class UIWindow : UIElement
    {
        public List<UIElement> Elements = new List<UIElement>();

        public RectangleShape Background = new RectangleShape();

        public UIWindow(Vector2f position, Vector2f size, Color color) : base(position, size, color)
        {
            Background.Position = Position;
            Background.Size = Size;
            Background.FillColor = Color;
        }

        public override void OnClick(object sender, MouseButtonEventArgs args)
        {
            foreach (var element in Elements)
            {
                Vector2f pos = new Vector2f(args.X,args.Y);
                if(element.OnElement(pos))
                    element.OnClick(sender, args);
            }
        }

        public override void Display(RenderWindow window)
        {
            window.Draw(Background);
            foreach (var element in Elements)
            {
                element.Display(window);
            }
        }
    }
}