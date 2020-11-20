using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SZDRPG.UIElements
{
    public class UIBar : UIElement
    {
        public float MaxValue;
        public float Value;
        public bool Vertical;
        public bool ShowText = true;
        public Sprite Icon = null;
        public Text Text = new Text();

        public UIBar(Vector2f position, Vector2f size, Color color, string icon = "") : base(position, size, color)
        {
            if (icon != "")
            {
                Icon = new Sprite(new Texture("../../../Resources/Images/Icons/" + icon + ".png"));
                Icon.Position = Position;
                float scale = Size.Y / Icon.GetLocalBounds().Height;
                Icon.Scale = new Vector2f(scale,scale);
            }
            Text.Font = new Font("../../../Resources/Fonts/Roboto-Bold.ttf");
            Text.CharacterSize = (uint) (Size.Y)/3;
            if (Icon != null)
            {
                while (Text.GetLocalBounds().Width > Size.X - Icon.GetGlobalBounds().Width)
                    Text.CharacterSize--;
                Vector2f padding = new Vector2f((Size.X-Text.GetGlobalBounds().Width+Icon.GetGlobalBounds().Width)/2, (Size.Y-Text.CharacterSize)/2);
                Text.Position = new Vector2f(Position.X+padding.X, Position.Y+padding.Y);
            }
            else
            {
                while (Text.GetLocalBounds().Width > Size.X)
                    Text.CharacterSize--;
                Vector2f padding = new Vector2f((Size.X-Text.GetLocalBounds().Width)/2, (Size.Y-Text.CharacterSize)/2);
                Text.Position = new Vector2f(Position.X+padding.X, Position.Y+padding.Y);
            }
        }

        public override void OnClick(object sender, MouseButtonEventArgs args)
        {
            
        }

        public override void OnRelease(object sender, MouseButtonEventArgs args)
        {
            
        }

        public override void Display(RenderWindow window)
        {
            if(Visible)
            {
                View old = new View(window.GetView());
                if(Absolute)
                    window.SetView(window.DefaultView);
                RectangleShape rect = new RectangleShape();
                if (Icon != null)
                {
                    if (Vertical)
                    {
                        rect.Size = new Vector2f(Size.X, (Size.Y - Icon.GetGlobalBounds().Height-10) * Value / MaxValue);
                        rect.Position = new Vector2f(Position.X,Position.Y + Icon.GetGlobalBounds().Height + 10);
                    }
                    else
                    {
                        rect.Size = new Vector2f((Size.X - Icon.GetGlobalBounds().Width-10) * Value / MaxValue, Size.Y);
                        rect.Position = new Vector2f(Position.X + Icon.GetGlobalBounds().Width + 10,Position.Y);
                    }
                    rect.FillColor = Color;
                }
                else
                {
                    if (Vertical)
                        rect.Size = new Vector2f(Size.X, Size.Y * Value / MaxValue);
                    else
                        rect.Size = new Vector2f(Size.X * Value / MaxValue, Size.Y);
                    rect.Position = Position;
                    rect.FillColor = Color;
                }
                if(Icon != null)
                    window.Draw(Icon);
                window.Draw(rect);
                if (ShowText)
                {
                    Text.DisplayedString = Value + "/" + MaxValue;
                    window.Draw(Text);
                }
                if(Absolute)
                    window.SetView(old);
            }
        }
    }
}