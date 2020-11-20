using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SZDRPG.UIElements
{
    public class UITimerIcon : UIElement
    {
        public Sprite Icon;
        public float State;
        public UITimerIcon(Vector2f position, Vector2f size, Color color, string icon) : base(position, size, color)
        {
            LoadIcon(icon);
        }
        public void LoadIcon(string name)
        {
            Icon = new Sprite(new Texture("../../../Resources/Images/Icons/" + name + ".png"));
            Icon.Position = Position;
            float scale = Size.Y / Icon.GetLocalBounds().Height;
            Icon.Scale = new Vector2f(scale,scale);
        }

        public override void OnClick(object sender, MouseButtonEventArgs args)
        {
            
        }

        public override void OnRelease(object sender, MouseButtonEventArgs args)
        {
            
        }

        public override void Display(RenderWindow window)
        {
            View old = new View(window.GetView());
            if(Absolute)
                window.SetView(window.DefaultView);
            if(Icon != null)
            {
                Sprite shadow = new Sprite(Icon);
                shadow.Position += new Vector2f(2,2);
                shadow.Color = new Color(0,0,0);
                window.Draw(shadow);
                window.Draw(Icon);
            }
            VertexArray circle = new VertexArray(PrimitiveType.TriangleFan);
            circle.Append(new Vertex(Position+Size/2,Color));
            for (int i = 0; i < 20; i++)
            {

                circle.Append(new Vertex(new Vector2f(
                    -MathF.Sin(State * MathF.PI / 180 * 360 / 20 * i) * Size.X / 2 + Position.X + Size.X / 2,
                    -MathF.Cos(State * MathF.PI / 180 * 360 / 20 * i) * Size.Y / 2 + Position.Y + Size.Y / 2), Color));
            }
            window.Draw(circle);
            if(Absolute)
                window.SetView(old);
            
        }
    }
}