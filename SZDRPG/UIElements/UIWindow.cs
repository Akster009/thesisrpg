using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using SFML.Window;
using SZDRPG.Graphics;

namespace SZDRPG.UIElements
{
    public class UIWindow : UIElement
    {
        public List<UIElement> Elements = new List<UIElement>();

        public RectangleShape Background = new RectangleShape();

        public Sprite BackgroundImage;

        public UIWindow(Vector2f position, Vector2f size, Color color) : base(position, size, color)
        {
            Background.Position = Position;
            Background.Size = Size;
            Background.FillColor = Color;
        }
        
        public UIWindow(Vector2f position, Vector2f size, string path) : base(position, size, Color.Black)
        {
            Background.Position = Position;
            Background.Size = Size;
            Background.FillColor = Color.Black;
            BackgroundImage = new Sprite(new Texture(path));
            BackgroundImage.Scale = new Vector2f(Size.X / BackgroundImage.GetGlobalBounds().Width,Size.Y / BackgroundImage.GetGlobalBounds().Height);
        }

        public override void OnClick(object sender, MouseButtonEventArgs args)
        {
            if(Visible)
            {
                foreach (var element in Elements)
                {
                    Vector2f pos = new Vector2f(args.X, args.Y);
                    if (element.OnElement(pos) && element.Visible)
                        element.OnClick(sender, args);
                }
            }
        }

        public override void OnRelease(object sender, MouseButtonEventArgs args)
        {
            foreach (var element in Elements)
            {
                element.OnRelease(sender, args);
            }
        }

        public override void Display(RenderWindow window)
        {
            if(Visible)
            {
                View old = new View(window.GetView());
                if(Absolute)
                    window.SetView(window.DefaultView);
                if (BackgroundImage != null)
                {
                    BackgroundImage.Position = Position;
                    window.Draw(BackgroundImage);
                }
                else
                    window.Draw(Background);
                foreach (var element in Elements)
                {
                    element.Display(window);
                }
                if(Absolute)
                    window.SetView(old);
            }
        }
    }
}