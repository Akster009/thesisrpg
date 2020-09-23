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

        public UIButton(Vector2f position, Vector2f size, Color color, string text, OnClickDel onClickFunction) : base(position, size, color)
        {
            OnClickFunction = onClickFunction;
            Text.DisplayedString = text;
            Text.Font = new Font("../../../Resources/Fonts/Roboto-Regular.ttf");
            Text.CharacterSize = 20;
            Text.Position = position;

        }

        public override void OnClick(object sender, MouseButtonEventArgs args)
        {
            OnClickFunction?.Invoke(sender, args);
        }

        public override void Display(RenderWindow window)
        {
            Shader blur = new Shader(null, null, "../../../Resources/Shader/blur.frag");
            RectangleShape Shadow = new RectangleShape(Size);
            Shadow.Position = new Vector2f(Position.X+3, Position.Y+3);
            Shadow.FillColor = new Color(0,0,0,125);
            RectangleShape background = new RectangleShape(Size);
            background.Position = Position;
            background.FillColor = Color;
            RectangleShape horizontal = new RectangleShape(new Vector2f(background.Size.X+2,background.Size.Y));
            horizontal.Position = new Vector2f(background.Position.X-1, background.Position.Y);
            horizontal.FillColor = Color;
            RectangleShape vertical = new RectangleShape(new Vector2f(background.Size.X,background.Size.Y+2));
            vertical.Position = new Vector2f(background.Position.X, background.Position.Y-1);
            vertical.FillColor = Color;
            
            window.Draw(Shadow, blur);
            window.Draw(horizontal);
            window.Draw(vertical);
            window.Draw(Text);
        }
    }
}