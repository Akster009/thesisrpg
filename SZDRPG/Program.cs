using System;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SZDRPG
{
    class Program
    {
        private static void MenuMouseHandler(object sender, MouseButtonEventArgs e)
        {
            Vector2f position = ((RenderWindow)sender).MapPixelToCoords(new Vector2i(e.X, e.Y));
            if(position.X>(((RenderWindow)sender).DefaultView.Size.X-210)/2 && position.X < (((RenderWindow)sender).DefaultView.Size.X-210/2) + 210)
                if (position.Y > 299 && position.Y < 328)
                {
                }
                else if(position.Y > 329 && position.Y < 358)
                {
                }
                else if(position.Y > 359 && position.Y < 388)
                {
                }
        }
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(960, 540), "Game Window");
            window.SetFramerateLimit(60);
            bool quit = false;
            int menu = 0;
            window.MouseButtonPressed += MenuMouseHandler;
            while (!quit && window.IsOpen)
            {
                switch (menu)
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        break;
                    case 4:
                        break;
                    default:
                        quit = true;
                        break;
                }
            }
        }
    }
}
