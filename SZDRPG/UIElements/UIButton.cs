using System;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using SFML.Window;

namespace SZDRPG.UIElements
{
    public class UIButton : UIElement
    {
        public delegate void OnClickDel(object sender, MouseButtonEventArgs args);

        public Text Text = new Text();
        
        public OnClickDel OnClickFunction;

        public bool Selected = false;
        
        public Sprite Icon = null;
        
        public Vector2f Margin = new Vector2f();

        public UIButton(Vector2f position, Vector2f size, Color color, string text, Vector2f margin,string icon = "", OnClickDel onClickFunction=null) : base(position, size, color)
        {
            Margin = margin;
            if (icon != "")
            {
                LoadIcon(icon);
            }
            OnClickFunction = onClickFunction;
            Text.DisplayedString = text;
            Text.Font = new Font("../../../Resources/Fonts/Roboto-Bold.ttf");
            Text.CharacterSize = (uint) (Size.Y+Margin.Y)/3;
            if (Icon != null)
            {
                while (Text.GetLocalBounds().Width > Size.X-Margin.X*3 - Icon.GetGlobalBounds().Width)
                    Text.CharacterSize--;
                Vector2f padding = new Vector2f((Size.X-Text.GetGlobalBounds().Width+Icon.GetGlobalBounds().Width+Margin.X)/2, (Size.Y-Text.CharacterSize)/2);
                Text.Position = new Vector2f(Position.X+padding.X, Position.Y+padding.Y);
            }
            else
            {
                while (Text.GetLocalBounds().Width > Size.X - Margin.X)
                    Text.CharacterSize--;
                Vector2f padding = new Vector2f((Size.X-Text.GetLocalBounds().Width)/2, (Size.Y-Text.CharacterSize)/2);
                Text.Position = new Vector2f(Position.X+padding.X, Position.Y+padding.Y);
            }

        }

        public void LoadIcon(string name)
        {
            Icon = new Sprite(new Texture("../../../Resources/Images/Icons/" + name + ".png"));
            Icon.Position = Margin+Position;
            float scale = (Size.Y - Margin.Y*2) / Icon.GetLocalBounds().Height;
            Icon.Scale = new Vector2f(scale,scale);
        }

        public override void OnClick(object sender, MouseButtonEventArgs args)
        {
            Selected = true;
        }
        
        public override void OnRelease(object sender, MouseButtonEventArgs args)
        {
            Vector2f pos = new Vector2f(args.X,args.Y);
            if(Selected && OnElement(pos))
            {
                OnClickFunction?.Invoke(sender, args);
            }

            Selected = false;
        }

        public override void Display(RenderWindow window)
        {
            RectangleShape background = new RectangleShape(Size);
            background.Position = Position;
            background.FillColor = Color;
            if(Selected)
                background.FillColor = new Color((byte) (Color.R-10), (byte)(Color.G-10), (byte)(Color.B-10));
            RectangleShape shadow = new RectangleShape(new Vector2f(Size.X+3, Size.Y+3));
            shadow.Position = Position;
            shadow.FillColor = new Color(0,0,0,60);
            RectangleShape horizontal = new RectangleShape(new Vector2f(background.Size.X+2,background.Size.Y));
            horizontal.Position = new Vector2f(background.Position.X-1, background.Position.Y);
            if(Selected)
                horizontal.Position = new Vector2f(background.Position.X + 1, background.Position.Y + 2);
            horizontal.FillColor = background.FillColor;
            RectangleShape vertical = new RectangleShape(new Vector2f(background.Size.X,background.Size.Y+2));
            vertical.Position = new Vector2f(background.Position.X, background.Position.Y-1);
            if(Selected)
                vertical.Position = new Vector2f(background.Position.X+2, background.Position.Y+1);
            vertical.FillColor = background.FillColor;
            if (Selected)
            {
                Text.Position += new Vector2f(2, 2);
                if (Icon != null)
                    Icon.Position += new Vector2f(2, 2);
            }
            window.Draw(shadow);
            shadow.Size = Size;
            shadow.Position = new Vector2f(Position.X+2, Position.Y+2);
            window.Draw(shadow);
            window.Draw(horizontal);
            window.Draw(vertical);
            if(Icon != null)
                window.Draw(Icon);
            window.Draw(Text);
            if(Selected)
            {
                Text.Position -= new Vector2f(2, 2);
                if(Icon != null)
                    Icon.Position -= new Vector2f(2, 2);
            }
        }
    }
}