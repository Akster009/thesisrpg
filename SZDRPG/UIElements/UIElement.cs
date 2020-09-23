using System;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;
using SFML.Window;

namespace SZDRPG.UIElements
{
    public abstract class UIElement
    {
        public Vector2f Position;
        public Vector2f Size;
        public Color Color;
        public bool Absolute = true;
        
        public UIElement(Vector2f position, Vector2f size, Color color)
        {
            Position = position;
            Size = size;
            Color = color;
        }
        public abstract void OnClick(object sender, MouseButtonEventArgs args);

        public abstract void Display(RenderWindow window);

        public virtual bool OnElement(Vec2 position)
        {
            return position.X >= Position.X && position.X <= Position.X + Size.X && position.Y >= Position.Y &&
                   position.Y <= Position.Y + Size.Y;
        }
    }
}