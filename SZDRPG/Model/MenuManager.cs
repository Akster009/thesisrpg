using System;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SZDRPG.UIElements;

namespace SZDRPG.Model
{
    public class MenuManager
    {
        public int menu;
        public UIWindow Menu;
        public string IP;

        public MenuManager(RenderWindow window)
        {
            menu = 0;
            Menu = BuildMainMenu(window);
            window.MouseButtonPressed += MenuMouseDown;
            window.MouseButtonReleased += MenuMouseUp;
            window.Closed += CloseWindow;
        }
        private void MenuMouseDown(object sender, MouseButtonEventArgs e)
        {
            Menu.OnClick(sender, e);
        }
        private void MenuMouseUp(object sender, MouseButtonEventArgs e)
        {
            Menu.OnRelease(sender, e);
        }
        private void CloseWindow(object sender, EventArgs args)
        {
            ((RenderWindow) sender).Close();
        }
        private void OpenSingleMenu(object sender, EventArgs args)
        {
            Menu = BuildSingleMenu((RenderWindow) sender);
        }
        private void OpenMultiMenu(object sender, EventArgs args)
        {
            Menu = BuildMultiMenu((RenderWindow) sender);
        }
        private void StartSingleGame(object sender, EventArgs args)
        {
            menu = 1;
        }
        private void StartNewSingleGame(object sender, EventArgs args)
        {
            menu = 2;
        }
        private void StartMultiGame(object sender, EventArgs args)
        {
            menu = 3;
        }
        private void JoinGame(object sender, EventArgs args)
        {
            menu = 4;
            IP = ((UITextField) Menu.Elements[0]).Text.DisplayedString;
        }
        private void ToJoinMenu(object sender, EventArgs args)
        {
            Menu = BuildJoinMenu((RenderWindow) sender);
        }
        private void BackToMainMenu(object sender, EventArgs args)
        {
            Menu = BuildMainMenu((RenderWindow) sender);
        }
        
        private void BackToMultiMenu(object sender, EventArgs args)
        {
            Menu = BuildMultiMenu((RenderWindow) sender);
        }
        private UIWindow BuildMainMenu(RenderWindow window)
        {
            UIWindow ret = new UIWindow(new Vector2f(0, 0), new Vector2f(window.Size.X, window.Size.Y), "../../../Resources/Images/Menu/main.jpg");
            Vector2f buttonSize = new Vector2f(window.Size.X/5, window.Size.Y/10);
            Vector2f buttonPos = new Vector2f((window.Size.X-buttonSize.X)/2, (window.Size.Y-buttonSize.Y*4)/2);
            UIButton singlePlayer = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y), buttonSize, new Color(41,182,246), "SINGLE PLAYER",new Vector2f(30,30),"Singleplayer", OpenSingleMenu);
            UIButton multiplayer = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y+buttonSize.Y*4/3),buttonSize, new Color(41,182,246), "MULTI PLAYER", new Vector2f(30,30), "Multiplayer", OpenMultiMenu);
            UIButton quitButton = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y+buttonSize.Y*2*4/3),buttonSize, new Color(41,182,246), "QUIT",new Vector2f(30,30),"Cross",CloseWindow);
            ret.Elements.Add(singlePlayer);
            ret.Elements.Add(multiplayer);
            ret.Elements.Add(quitButton);
            return ret;
        }
        private UIWindow BuildSingleMenu(RenderWindow window)
        {
            UIWindow ret = new UIWindow(new Vector2f(0, 0), new Vector2f(window.Size.X, window.Size.Y), "../../../Resources/Images/Menu/main.jpg");
            Vector2f buttonSize = new Vector2f(window.Size.X/5, window.Size.Y/10);
            Vector2f buttonPos = new Vector2f((window.Size.X-buttonSize.X)/2, (window.Size.Y-buttonSize.Y*4)/2);
            UIButton continueGame = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y), buttonSize, new Color(41,182,246), "CONTINUE GAME",new Vector2f(30,30),"", StartSingleGame);
            UIButton newGame = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y+buttonSize.Y*4/3), buttonSize, new Color(41,182,246), "START NEW GAME",new Vector2f(30,30),"", StartNewSingleGame);
            UIButton backButton = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y+buttonSize.Y*2*4/3),buttonSize, new Color(41,182,246), "BACK",new Vector2f(30,30),"Cross",BackToMainMenu);
            ret.Elements.Add(continueGame);
            ret.Elements.Add(newGame);
            ret.Elements.Add(backButton);
            return ret;
        }
        
        private UIWindow BuildMultiMenu(RenderWindow window)
        {
            UIWindow ret = new UIWindow(new Vector2f(0, 0), new Vector2f(window.Size.X, window.Size.Y), "../../../Resources/Images/Menu/main.jpg");
            Vector2f buttonSize = new Vector2f(window.Size.X/5, window.Size.Y/10);
            Vector2f buttonPos = new Vector2f((window.Size.X-buttonSize.X)/2, (window.Size.Y-buttonSize.Y*4)/2);
            UIButton hostGame = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y), buttonSize, new Color(41,182,246), "HOST GAME",new Vector2f(30,30),"", StartMultiGame);
            UIButton joinGame = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y+buttonSize.Y*4/3), buttonSize, new Color(41,182,246), "JOIN GAME",new Vector2f(30,30),"", ToJoinMenu);
            UIButton backButton = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y+buttonSize.Y*2*4/3),buttonSize, new Color(41,182,246), "BACK",new Vector2f(30,30),"Cross",BackToMainMenu);
            ret.Elements.Add(hostGame);
            ret.Elements.Add(joinGame);
            ret.Elements.Add(backButton);
            return ret;
        }
        
        private UIWindow BuildJoinMenu(RenderWindow window)
        {
            UIWindow ret = new UIWindow(new Vector2f(0, 0), new Vector2f(window.Size.X, window.Size.Y), "../../../Resources/Images/Menu/main.jpg");
            Vector2f buttonSize = new Vector2f(window.Size.X/5, window.Size.Y/10);
            Vector2f buttonPos = new Vector2f((window.Size.X-buttonSize.X)/2, (window.Size.Y-buttonSize.Y*4)/2);
            UITextField IPText = new UITextField(new Vector2f(buttonPos.X,buttonPos.Y), buttonSize, new Color(41,182,246),new Vector2f(30,30),"");
            IPText.Editable = true;
            IPText.Container = window;
            UIButton join = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y+buttonSize.Y*4/3), buttonSize, new Color(41,182,246), "JOIN",new Vector2f(30,30),"", JoinGame);
            UIButton backButton = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y+buttonSize.Y*2*4/3),buttonSize, new Color(41,182,246), "BACK",new Vector2f(30,30),"Cross",BackToMultiMenu);
            ret.Elements.Add(IPText);
            ret.Elements.Add(join);
            ret.Elements.Add(backButton);
            return ret;
        }

        public void HandleMenu(RenderWindow window)
        {
            window.DispatchEvents();
            window.Clear();
            Menu.Display(window);
            window.Display();
        }

        public void RemoveListeners(RenderWindow window)
        {
            window.MouseButtonPressed -= MenuMouseDown;
            window.MouseButtonReleased -= MenuMouseUp;
        }

        public void AddListeners(RenderWindow window)
        {
            window.MouseButtonPressed += MenuMouseDown;
            window.MouseButtonReleased += MenuMouseUp;
        }
    }
}