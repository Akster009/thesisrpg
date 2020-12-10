using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using SFML.Window;

namespace SZDRPG.UIElements
{
    public class UITextField : UIElement
    {
        public bool Editable = false;
        public Vector2f Margin;
        public Text Text = new Text();
        
        private void TextKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                Container.KeyPressed -= TextKeyDown;
            }
            else if (e.Code == Keyboard.Key.Backspace && Text.DisplayedString.Length>0)
            {
                UpdateText(Text.DisplayedString.Substring(0, Text.DisplayedString.Length - 1));
            }
            else if(e.Code == Keyboard.Key.Num0 || e.Code == Keyboard.Key.Numpad0)
            {
                UpdateText(Text.DisplayedString + "0");
            }
            else if(e.Code == Keyboard.Key.Num1 || e.Code == Keyboard.Key.Numpad1)
            {
                UpdateText(Text.DisplayedString + "1");
            }
            else if(e.Code == Keyboard.Key.Num2 || e.Code == Keyboard.Key.Numpad2)
            {
                UpdateText(Text.DisplayedString + "2");
            }
            else if(e.Code == Keyboard.Key.Num3 || e.Code == Keyboard.Key.Numpad3)
            {
                UpdateText(Text.DisplayedString + "3");
            }
            else if(e.Code == Keyboard.Key.Num4 || e.Code == Keyboard.Key.Numpad4)
            {
                UpdateText(Text.DisplayedString + "4");
            }
            else if(e.Code == Keyboard.Key.Num5 || e.Code == Keyboard.Key.Numpad5)
            {
                UpdateText(Text.DisplayedString + "5");
            }
            else if(e.Code == Keyboard.Key.Num6 || e.Code == Keyboard.Key.Numpad6)
            {
                UpdateText(Text.DisplayedString + "6");
            }
            else if(e.Code == Keyboard.Key.Num7 || e.Code == Keyboard.Key.Numpad7)
            {
                UpdateText(Text.DisplayedString + "7");
            }
            else if(e.Code == Keyboard.Key.Num8 || e.Code == Keyboard.Key.Numpad8)
            {
                UpdateText(Text.DisplayedString + "8");
            }
            else if(e.Code == Keyboard.Key.Num9 || e.Code == Keyboard.Key.Numpad9)
            {
                UpdateText(Text.DisplayedString + "9");
            }
            else if (e.Code == Keyboard.Key.Period)
            {
                UpdateText(Text.DisplayedString + ".");
            }
            else
            {
                UpdateText(Text.DisplayedString + e.Code);
            }
        }
        public UITextField(Vector2f position, Vector2f size, Color color, Vector2f margin, string text) : base(position, size, color)
        {
            Text.FillColor = Color.White;
            Margin = margin;
            Text.Font = new Font("../../../Resources/Fonts/Roboto-Bold.ttf");
            UpdateText(text);   
        }

        public override void OnClick(object sender, MouseButtonEventArgs args)
        {
            if (Editable)
            {
                Container.KeyPressed -= TextKeyDown;
                Container.KeyPressed += TextKeyDown;
            }
        }

        public override void OnRelease(object sender, MouseButtonEventArgs args)
        {
            if (!OnElement(new Vec2(args.X, args.Y)) && Container != null)
                Container.KeyPressed -= TextKeyDown;
        }

        public override void Display(RenderWindow window)
        {
            RectangleShape background = new RectangleShape(Size);
            background.Position = Position;
            background.FillColor = Color;
            RectangleShape horizontal = new RectangleShape(new Vector2f(background.Size.X+2,background.Size.Y));
            horizontal.Position = new Vector2f(background.Position.X-1, background.Position.Y);
            horizontal.FillColor = background.FillColor;
            RectangleShape vertical = new RectangleShape(new Vector2f(background.Size.X,background.Size.Y+2));
            vertical.Position = new Vector2f(background.Position.X, background.Position.Y-1);
            vertical.FillColor = background.FillColor;
            window.Draw(horizontal);
            window.Draw(vertical);
            window.Draw(Text);
        }

        public void UpdateText(string text)
        {
            Text.DisplayedString = text;
            Text.CharacterSize = (uint) (Size.Y+Margin.Y)/3;
            while (Text.GetLocalBounds().Width > Size.X - Margin.X)
                Text.CharacterSize--;
            Vector2f padding = new Vector2f((Size.X-Text.GetLocalBounds().Width)/2, (Size.Y-Text.CharacterSize)/2);
            Text.Position = new Vector2f(Position.X+padding.X, Position.Y+padding.Y);
        }
    }
}