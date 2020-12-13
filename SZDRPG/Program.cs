using System;
using System.Linq;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SZDRPG.Graphics;
using SZDRPG.Model;
using SZDRPG.UIElements;

namespace SZDRPG
{
    class Program
    {
        public static GameManager GameManager;

        public static MenuManager MenuManager;
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(1600, 900), "Game Window");
            window.SetFramerateLimit(60);
            bool quit = false;
            MenuManager = new MenuManager(window);
            while (!quit && window.IsOpen)
            {
                switch (MenuManager.menu)
                {
                    case 0:
                        MenuManager.HandleMenu(window);
                        break;
                    case 1:
                        MenuManager.RemoveListeners(window);
                        PlaySingleGame(window, true);
                        MenuManager.AddListeners(window);
                        break;
                    case 2:
                        MenuManager.RemoveListeners(window);
                        PlaySingleGame(window, false);
                        MenuManager.AddListeners(window);
                        break;
                    case 3:
                        MenuManager.RemoveListeners(window);
                        PlayMultiGame(window, true);
                        MenuManager.AddListeners(window);
                        break;
                    case 4:
                        MenuManager.RemoveListeners(window);
                        PlayMultiGame(window, false);
                        MenuManager.AddListeners(window);
                        break;
                    default:
                        quit = true;
                        break;
                }
            }
        }

        private static void PlaySingleGame(RenderWindow window, bool continueGame = false)
        {
            GameManager = new GameManager(window);
            GameManager.BackgroundImage = new Sprite(new Texture("../../../Resources/Images/Game/Background.png"));
            GameManager.RunGame(window, continueGame);
            window.SetView(window.DefaultView);
            MenuManager.menu = 0;
        }
        private static void PlayMultiGame(RenderWindow window, bool host)
        {
            GameManager = new GameManager(window);
            GameManager.BackgroundImage = new Sprite(new Texture("../../../Resources/Images/Game/Background.png"));
            try
            {
                GameManager.RunMultiGame(window, host, MenuManager.IP);
            }
            catch (Exception e)
            {
                MenuManager.Error = "A network error has occured";
                MenuManager.Menu = MenuManager.BuildMultiMenu(window);
            }
            window.SetView(window.DefaultView);
            MenuManager.menu = 0;
        }
    }
}
