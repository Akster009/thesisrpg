using System;
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
        private static int menu;
        public static UIWindow Menu;
        public static GameWindow GameWindow = new GameWindow();
        public static bool exit = false;

        private static Game InitGame()
        {
            Game ret = new Game();
            Map map = new Map();
            map.Size = new Vector2f(1600,900);
            map.Tile = new Texture("../../../Resources/Graphics/Tile/Grass.png");
            ret.Map = map;
            ret.AddCharacter("Player", new Vector2f(30,30),new Vector2f(0,0) );
            GameRoom gameroom = new GameRoom(ret, new Vector2i(10,7));
            ret.LoadRoom(gameroom);
            ret.Start = false;
            return ret;
        }
        private static void MenuMouseDown(object sender, MouseButtonEventArgs e)
        {
            Menu.OnClick(sender, e);
        }
        private static void MenuMouseUp(object sender, MouseButtonEventArgs e)
        {
            Menu.OnRelease(sender, e);
        }
        
        private static void GameMouseDown(object sender, MouseButtonEventArgs e)
        {
            GameWindow.OnClick(sender, e);
        }
        private static void GameMouseUp(object sender, MouseButtonEventArgs e)
        {
            GameWindow.OnRelease(sender, e);
        }
        
        private static void GameKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                exit = true;
            }
        }
        private static void CloseWindow(object sender, EventArgs args)
        {
            ((RenderWindow) sender).Close();
        }
        private static void StartSinglePlayer(object sender, EventArgs args)
        {
            Menu = BuildSingleMenu((RenderWindow) sender);
        }
        
        private static void StartSingleGame(object sender, EventArgs args)
        {
            //GameWindow.Game = InitGame();
            menu = 1;
        }
        
        private static void BackSinglePlayer(object sender, EventArgs args)
        {
            Menu = BuildMainMenu((RenderWindow) sender);
        }

        private static UIWindow BuildMainMenu(RenderWindow window)
        {
            UIWindow ret = new UIWindow(new Vector2f(0, 0), new Vector2f(window.Size.X, window.Size.Y), "../../../Resources/Images/Menu/main.jpg");
            Vector2f buttonSize = new Vector2f(window.Size.X/10, window.Size.Y/10);
            Vector2f buttonPos = new Vector2f((window.Size.X-buttonSize.X)/2, (window.Size.Y-buttonSize.Y*4)/2);
            UIButton singlePlayer = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y), buttonSize, new Color(69,90,100), "SINGLE PLAYER", StartSinglePlayer);
            UIButton multiplayer = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y+buttonSize.Y*4/3),buttonSize, new Color(69,90,100), "MULTI PLAYER");
            UIButton quitButton = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y+buttonSize.Y*2*4/3),buttonSize, new Color(69,90,100), "QUIT",CloseWindow);
            ret.Elements.Add(singlePlayer);
            ret.Elements.Add(multiplayer);
            ret.Elements.Add(quitButton);
            return ret;
        }
        private static UIWindow BuildSingleMenu(RenderWindow window)
        {
            UIWindow ret = new UIWindow(new Vector2f(0, 0), new Vector2f(window.Size.X, window.Size.Y), new Color(207,216,220));
            Vector2f buttonSize = new Vector2f(window.Size.X/10, window.Size.Y/10);
            Vector2f buttonPos = new Vector2f((window.Size.X-buttonSize.X)/2, (window.Size.Y-buttonSize.Y*4)/2);
            UIButton singlePlayer = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y), buttonSize, new Color(69,90,100), "START GAME", StartSingleGame);
            UIButton backButton = new UIButton(new Vector2f(buttonPos.X,buttonPos.Y+buttonSize.Y*2*4/3),buttonSize, new Color(69,90,100), "BACK",BackSinglePlayer);
            ret.Elements.Add(singlePlayer);
            ret.Elements.Add(backButton);
            return ret;
        }
        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(1600, 900), "Game Window");
            window.SetFramerateLimit(60);
            bool quit = false;
            menu = 0;
            Menu = BuildMainMenu(window);
            window.MouseButtonPressed += MenuMouseDown;
            window.MouseButtonReleased += MenuMouseUp;
            window.Closed += CloseWindow;
            //GCharacter Test
            /*GPart leg = new GPart();
            leg.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/LegFront.png")));
            leg.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/LegRight.png")));
            leg.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/LegBack.png")));
            leg.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/LegLeft.png")));
            leg.RotationCenter = new Vector2f(-4,-32);
            leg.Origin = new Vector2f(4,0);
            GPart leg2 = new GPart();
            leg2.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/LegFront.png")));
            leg2.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/LegRight.png")));
            leg2.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/LegBack.png")));
            leg2.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/LegLeft.png")));
            leg2.RotationCenter = new Vector2f(4,-32);
            leg2.Origin = new Vector2f(4,0);
            GPart body = new GPart();
            body.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/Body.png")));
            body.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/Body.png")));
            body.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/Body.png")));
            body.BaseTexture.Add(new Sprite(new Texture("../../../Resources/Graphics/Character/Body.png")));
            body.RotationCenter = new Vector2f(0,-64);
            body.Origin = new Vector2f(8,0);
            GCharacter character = new GCharacter();
            character.Parts.Add(leg);
            character.Parts.Add(leg2);
            character.Parts.Add(body);
            --------------------------
            character.State.ID = 0;
            character.State.facing = 0;
            character.State.elapsed = Time.Zero;
            character.State.Position = new Vector2f(200,200);
            AnimationStep step1 = new AnimationStep();
            step1.Duration = Time.FromSeconds(0.5f);
            step1.Rotation = 45f;
            AnimationStep step2 = new AnimationStep();
            step2.Duration = Time.FromSeconds(0.5f);
            step2.Rotation = 0;
            AnimationStep step3 = new AnimationStep();
            step3.Duration = Time.FromSeconds(0.5f);
            step3.Rotation = -45f;
            AnimationStep step4 = new AnimationStep();
            step4.Duration = Time.FromSeconds(2);
            step4.Rotation = 0;
            AnimationPart animPart = new AnimationPart();
            animPart.Steps.Add(step1);
            animPart.Steps.Add(step2);
            animPart.Steps.Add(step3);
            animPart.Steps.Add(step2);
            AnimationPart animPart2 = new AnimationPart();
            animPart2.Steps.Add(step3);
            animPart2.Steps.Add(step2);
            animPart2.Steps.Add(step1);
            animPart2.Steps.Add(step2);
            AnimationPart animPart3 = new AnimationPart();
            animPart3.Steps.Add(step4);
            Animation anim = new Animation();
            anim.Parts.Add(animPart);
            anim.Parts.Add(animPart2);
            anim.Parts.Add(animPart3);
            character.Animations.Add(anim);
            character.State.facing = 45;
            character.State.elapsed = Time.Zero;*/
            //END Test
            Clock clock = new Clock();
            while (!quit && window.IsOpen)
            {
                /*if (clock.ElapsedTime.AsSeconds() > 2)
                    clock.Restart();
                character.State.elapsed = clock.ElapsedTime;
                character.State.facing += 1f;
                if (character.State.facing >= 360)
                    character.State.facing = 0;*/
                switch (menu)
                {
                    case 0:
                        window.DispatchEvents();
                        window.Clear();
                        Menu.Display(window);
                        //character.Draw(window);
                        window.Display();
                        break;
                    case 1:
                        PlaySingleGame(window);
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

        private static void PlaySingleGame(RenderWindow window)
        {
            window.MouseButtonPressed -= MenuMouseDown;
            window.MouseButtonPressed += GameMouseDown;
            window.MouseButtonReleased -= MenuMouseUp;
            window.MouseButtonReleased += GameMouseUp;
            window.KeyPressed += GameKeyDown;
            GameWindow.Game = InitGame();
            Clock timer = new Clock();
            View mainView = new SFML.Graphics.View
            {
                Size = new Vector2f(1600, 900),
                Center = new Vector2f(GameWindow.Game.Pentities[0].Position.X, GameWindow.Game.Pentities[0].Position.Y),
                Viewport = new FloatRect(0, 0, 1, 1)
            };
            while (!exit)
            {
                if (GameWindow.Game.Start)
                    GameWindow.Game = InitGame();
                GameWindow.Game.NextStep(timer.Restart());
                window.DispatchEvents();
                GameWindow.HandleHolding(window);
                window.Clear();
                mainView.Center = new Vector2f(GameWindow.Game.Pentities[0].Position.X,
                    GameWindow.Game.Pentities[0].Position.Y);
                window.SetView(mainView);
                GameWindow.Draw(window);
                window.Display();
            }

            exit = false;
            window.MouseButtonPressed += MenuMouseDown;
            window.MouseButtonPressed -= GameMouseDown;
            window.MouseButtonReleased += MenuMouseUp;
            window.MouseButtonReleased -= GameMouseUp;
            window.KeyPressed -= GameKeyDown;
            window.SetView(window.DefaultView);

            menu = 0;
        }
    }
}
