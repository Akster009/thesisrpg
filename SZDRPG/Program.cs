using System;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SZDRPG.UIElements;

namespace SZDRPG
{
    class Program
    {
        public static UIWindow mainMenu;
        private static void MenuMouseHandler(object sender, MouseButtonEventArgs e)
        {
            mainMenu.OnClick(sender, e);
        }

        private static void CloseWindow(object sender, EventArgs args)
        {
            ((RenderWindow) sender).Close();
        }
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(960, 540), "Game Window");
            window.SetFramerateLimit(60);
            bool quit = false;
            int menu = 0;
            mainMenu = new UIWindow(new Vector2f(0, 0), new Vector2f(window.Size.X, window.Size.Y), Color.Red);
            Vector2f buttonSize = new Vector2f(window.Size.X/10, window.Size.Y/10);
            Vector2f buttonPos = new Vector2f((window.Size.X-buttonSize.X)/2, (window.Size.Y-buttonSize.Y)/2);
            UIButton quitButton = new UIButton(buttonPos,buttonSize, Color.Blue,"Quit",CloseWindow);
            mainMenu.Elements.Add(quitButton);
            window.MouseButtonPressed += MenuMouseHandler;
            window.Closed += CloseWindow;
            while (!quit && window.IsOpen)
            {
                switch (menu)
                {
                    case 0:
                        window.DispatchEvents();
                        window.Clear();
                        mainMenu.Display(window);
                        window.Display();
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
