using System;
using System.Collections.Generic;
using System.Threading;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using SZDRPG.Network;
using SZDRPG.UIElements;

namespace SZDRPG.Model
{
    public class GameManager
    {
        public Game Game;
        public bool Holding = false;
        public List<UIElement> Elements = new List<UIElement>();
        public bool exit;
        public NetworkIntent NetworkIntent = new NetworkIntent();

        private void GameMouseDown(object sender, MouseButtonEventArgs e)
        {
            OnClick(sender, e);
        }
        private void GameMouseUp(object sender, MouseButtonEventArgs e)
        {
            OnRelease(sender, e);
        }
        private void GameKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                exit = true;
                Game.EndGame();
            }
            else if (e.Code == Keyboard.Key.C)
            {
                Elements[0].Visible = !Elements[0].Visible;
            }
            else if (e.Code == Keyboard.Key.Q)
            {
                Game.Characters[0].DoSpecialAttack(2);
            }
            else if (e.Code == Keyboard.Key.E)
            {
                Game.Characters[0].DoSpecialAttack(3);
            }
        }
        private void NetworkMouseDown(object sender, MouseButtonEventArgs e)
        {
            NetworkOnClick(sender, e);
        }
        private void NetworkMouseUp(object sender, MouseButtonEventArgs e)
        {
            NetworkOnRelease(sender, e);
        }
        private void NetworkKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Escape)
            {
                exit = true;
                Game.EndGame();
            }
            else if (e.Code == Keyboard.Key.C)
            {
                Elements[0].Visible = !Elements[0].Visible;
            }
            else if (e.Code == Keyboard.Key.Q)
            {
                NetworkIntent.IntentNum = 2;
            }
            else if (e.Code == Keyboard.Key.E)
            {
                NetworkIntent.IntentNum = 3;
            }
        }
        
        private void VigorPlusButton(object sender, MouseButtonEventArgs e)
        {
            Game.Characters[0].IncreseSkill(0);
        }
        
        private void StrengthPlusButton(object sender, MouseButtonEventArgs e)
        {
            Game.Characters[0].IncreseSkill(1);
        }
        
        private void AgilityPlusButton(object sender, MouseButtonEventArgs e)
        {
            Game.Characters[0].IncreseSkill(2);
        }
        
        private void IntelligencePlusButton(object sender, MouseButtonEventArgs e)
        {
            Game.Characters[0].IncreseSkill(3);
        }
        
        private void SpiritPlusButton(object sender, MouseButtonEventArgs e)
        {
            Game.Characters[0].IncreseSkill(4);
        }

        public GameManager(RenderWindow window)
        {
            UIWindow CharSheet = new UIWindow(new Vector2f(window.Size.X / 3 * 2, 0),
                new Vector2f(window.Size.X / 3, window.Size.Y),
                "../../../Resources/Images/Game/Book.png");
            CharSheet.Visible = false;
            Vector2f buttonPos = new Vector2f(CharSheet.Size.X / 18 * 15+1, CharSheet.Size.Y / 13);
            Vector2f buttonSize = new Vector2f(CharSheet.Size.X / 18-2, CharSheet.Size.Y / 26-2);
            Vector2f textPos = new Vector2f(CharSheet.Size.X / 18 * 4+1, CharSheet.Size.Y / 13);
            UIButton vigor = new UIButton(
                new Vector2f(buttonPos.X, buttonPos.Y*4+1) + CharSheet.Position,
                buttonSize, new Color(229, 57, 53), "+",
                new Vector2f(3, 3), "", VigorPlusButton);
            UIButton strength = new UIButton(
                new Vector2f(buttonPos.X, buttonPos.Y * 5+1) + CharSheet.Position,
                buttonSize, new Color(229, 57, 53), "+",
                new Vector2f(3, 3), "", StrengthPlusButton);
            UIButton agility = new UIButton(
                new Vector2f(buttonPos.X, buttonPos.Y *6+1) + CharSheet.Position,
                buttonSize, new Color(229, 57, 53), "+",
                new Vector2f(3, 3), "", AgilityPlusButton);
            UIButton intelligence = new UIButton(
                new Vector2f(buttonPos.X, buttonPos.Y * 7+1) + CharSheet.Position,
                buttonSize, new Color(229, 57, 53), "+",
                new Vector2f(3, 3), "", IntelligencePlusButton);
            UIButton spirit = new UIButton(
                new Vector2f(buttonPos.X, buttonPos.Y * 8+1) + CharSheet.Position,
                buttonSize, new Color(229, 57, 53), "+",
                new Vector2f(3, 3), "", SpiritPlusButton);
            UITextField vigorText = new UITextField(new Vector2f(textPos.X, textPos.Y * 4+1) + CharSheet.Position,
                new Vector2f(buttonSize.X * 10 + 18, buttonSize.Y), new Color(215, 168, 100),
                new Vector2f(3, 3), "VIGOR");
            UITextField strengthText = new UITextField(new Vector2f(textPos.X, textPos.Y*5+1) + CharSheet.Position,
                new Vector2f(buttonSize.X * 10+ 18, buttonSize.Y), new Color(215, 168, 100),
                new Vector2f(3, 3), "STRENGTH");
            UITextField agilityText = new UITextField(new Vector2f(textPos.X, textPos.Y*6+1) + CharSheet.Position,
                new Vector2f(buttonSize.X * 10+ 18, buttonSize.Y), new Color(215, 168, 100),
                new Vector2f(3, 3), "AGILITY");
            UITextField intelligenceText = new UITextField(new Vector2f(textPos.X, textPos.Y*7+1) + CharSheet.Position,
                new Vector2f(buttonSize.X * 10+ 18, buttonSize.Y), new Color(215, 168, 100),
                new Vector2f(3, 3), "INTELLIGENCE");
            UITextField spiritText = new UITextField(new Vector2f(textPos.X, textPos.Y*8+1) + CharSheet.Position,
                new Vector2f(buttonSize.X * 10+ 18, buttonSize.Y), new Color(215, 168, 100),
                new Vector2f(3, 3), "SPIRIT");
            UITextField levelText = new UITextField(new Vector2f(textPos.X, textPos.Y*2+1) + CharSheet.Position,
                new Vector2f(buttonSize.X * 10+ 18, buttonSize.Y), new Color(215, 168, 100),
                new Vector2f(3, 3), "LEVEL");
            UITextField skillText = new UITextField(new Vector2f(textPos.X, textPos.Y*3+1) + CharSheet.Position,
                new Vector2f(buttonSize.X * 10+ 18, buttonSize.Y), new Color(215, 168, 100),
                new Vector2f(3, 3), "SKILL POINTS LEFT");
            UITextField levelNumText = new UITextField(new Vector2f(buttonPos.X, buttonPos.Y * 2+1) + CharSheet.Position,
                new Vector2f(buttonSize.X, buttonSize.Y), new Color(215, 168, 100),
                new Vector2f(3, 3), "");
            UITextField skillNumText = new UITextField(new Vector2f(buttonPos.X, buttonPos.Y * 3+1) + CharSheet.Position,
                new Vector2f(buttonSize.X, buttonSize.Y), new Color(215, 168, 100),
                new Vector2f(3, 3), "");
            CharSheet.Elements.Add(vigor);
            CharSheet.Elements.Add(strength);
            CharSheet.Elements.Add(agility);
            CharSheet.Elements.Add(intelligence);
            CharSheet.Elements.Add(spirit);
            CharSheet.Elements.Add(vigorText);
            CharSheet.Elements.Add(strengthText);
            CharSheet.Elements.Add(agilityText);
            CharSheet.Elements.Add(intelligenceText);
            CharSheet.Elements.Add(spiritText);
            CharSheet.Elements.Add(levelText);
            CharSheet.Elements.Add(skillText);
            CharSheet.Elements.Add(levelNumText);
            CharSheet.Elements.Add(skillNumText);
            UIBar Health = new UIBar(new Vector2f(10, 10), 
                new Vector2f(window.Size.X / 3, window.Size.Y / 20),
                new Color(183, 28, 28), "Health");
            UIBar Mana = new UIBar(new Vector2f(10, window.Size.Y / 20 + 20), 
                new Vector2f(window.Size.X / 3, window.Size.Y / 20),new Color(26, 35, 126), "Mana");
            UIBar XP = new UIBar(new Vector2f(10,window.Size.Y/20*2+30),
                new Vector2f(window.Size.X/3, window.Size.Y/20),  new Color(251,192,45), "Experience");
            float timerHeight = window.Size.Y / 10;
            float timerDistance = window.Size.Y / 20;
            UITimerIcon Attack1 =
                new UITimerIcon(
                    new Vector2f((window.Size.X - 3 * timerDistance - 4 * timerHeight) / 2,
                        window.Size.Y - timerDistance - timerHeight), new Vector2f(timerHeight, timerHeight),
                    new Color(84,110,122,125), "Attack");
            UITimerIcon Attack2 =
                new UITimerIcon(
                    new Vector2f((window.Size.X - 3 * timerDistance - 4 * timerHeight) / 2 + timerDistance + timerHeight,
                        window.Size.Y - timerDistance - timerHeight), new Vector2f(timerHeight, timerHeight),
                    new Color(84,110,122,125), "Throw");
            UITimerIcon Attack3 =
                new UITimerIcon(
                    new Vector2f((window.Size.X - 3 * timerDistance - 4 * timerHeight) / 2 + timerDistance * 2 + timerHeight * 2,
                        window.Size.Y - timerDistance - timerHeight), new Vector2f(timerHeight, timerHeight),
                    new Color(84,110,122,125), "Whirlwind");
            UITimerIcon Attack4 =
                new UITimerIcon(
                    new Vector2f((window.Size.X - 3 * timerDistance - 4 * timerHeight) / 2 + timerDistance * 3 + timerHeight * 3,
                        window.Size.Y - timerDistance - timerHeight), new Vector2f(timerHeight, timerHeight),
                    new Color(84,110,122,125), "Lunge");
            Elements.Add(CharSheet);
            Elements.Add(Health);
            Elements.Add(Mana);
            Elements.Add(XP);
            Elements.Add(Attack1);
            Elements.Add(Attack2);
            Elements.Add(Attack3);
            Elements.Add(Attack4);
        }

        public void Draw(RenderWindow window)
        {
            Game.Draw(window);
            foreach (var element in Elements)
            {
                element.Display(window);
            }
        }

        public void OnClick(object sender, MouseButtonEventArgs args)
        {
            Vector2f position = ((RenderWindow) sender).MapPixelToCoords(new Vector2i(args.X, args.Y));
            if(args.Button == Mouse.Button.Left)
            {
                bool UIClick = false;
                //Vector2f position = new Vector2f(args.X, args.Y);
                foreach (var element in Elements)
                {
                    Vector2f pos = new Vector2f(args.X, args.Y);
                    if (element.OnElement(pos))
                    {
                        UIClick = true;
                        element.OnClick(sender, args);
                    }
                }

                if (!UIClick)
                {
                    bool charAttack = false;
                    foreach (var character in Game.Characters)
                    {
                        if (character != Game.Pentities[0] && character.OnEntity(position))
                        {
                            charAttack = true;
                            Game.Characters[0].Target = character;
                            Game.Characters[0].Direction = null;
                        }
                    }
                    if(!charAttack)
                    {
                        bool attack = false;
                        foreach (var gameGentity in Game.Pentities)
                        {
                            if (gameGentity != Game.Pentities[0] && gameGentity.OnEntity(position))
                            {
                                attack = true;
                                Game.Characters[0].Target = gameGentity;
                                Game.Characters[0].Direction = null;
                            }
                        }

                        if (!attack)
                        {
                            Game.Characters[0].Direction = Game.Map.IntersectAt(Game.Characters[0].Position, position);
                            Game.Characters[0].Target = null;
                            Holding = true;
                        }
                    }
                }
            }
            else if (args.Button == Mouse.Button.Right)
            {
                Game.Characters[0].Display.State.UpdateFacing(position);
                Game.Characters[0].DoSpecialAttack(1);
            }
        }
        public void NetworkOnClick(object sender, MouseButtonEventArgs args)
        {
            Vector2f position = ((RenderWindow) sender).MapPixelToCoords(new Vector2i(args.X, args.Y));
            if(args.Button == Mouse.Button.Left)
            {
                bool UIClick = false;
                foreach (var element in Elements)
                {
                    Vector2f pos = new Vector2f(args.X, args.Y);
                    if (element.OnElement(pos))
                    {
                        UIClick = true;
                        element.OnClick(sender, args);
                    }
                }

                if (!UIClick)
                {
                    NetworkIntent.IntentNum = 0;
                    NetworkIntent.IntentPosition = position;
                    Holding = true;
                }
            }
            else if (args.Button == Mouse.Button.Right)
            {
                Game.Characters[0].Display.State.UpdateFacing(position);
                NetworkIntent.IntentNum = 1;
                NetworkIntent.IntentPosition = position;
            }
        }
        public void OnRelease(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Holding = false;
            Elements[0].OnRelease(sender,mouseButtonEventArgs);
        }
        public void NetworkOnRelease(object sender, MouseButtonEventArgs mouseButtonEventArgs)
        {
            Holding = false;
            Elements[0].OnRelease(sender,mouseButtonEventArgs);
        }
        public void HandleHolding(RenderWindow window)
        {
            if (Holding)
            {
                Game.Characters[0].Direction = Game.Map.IntersectAt(Game.Characters[0].Position,window.MapPixelToCoords(Mouse.GetPosition(window)));
                Game.Characters[0].Target = null;
                //Game.Characters[0].Display.State.UpdateFacing(window.MapPixelToCoords(Mouse.GetPosition(window)));
            }
        }
        public void NetworkHolding(RenderWindow window)
        {
            if (Holding)
            {
                NetworkIntent.IntentNum = 0;
                NetworkIntent.IntentPosition = (Vector2f) Game.Map.IntersectAt(Game.Characters[0].Position,window.MapPixelToCoords(Mouse.GetPosition(window)));
            }
        }
        
        public void UpdateUI()
        {
            if (Game.Characters[0].SkillPoints > 0)
            {
                for (int i = 0; i < 5 ; i++)
                {
                    ((UIWindow)Elements[0]).Elements[i].Color = new Color(118,255,3);
                }
            }
            else
            {
                for (int i = 0; i < 5 ; i++)
                {
                    ((UIWindow)Elements[0]).Elements[i].Color = new Color(229, 57, 53);
                }
            }

            ((UITextField)((UIWindow)Elements[0]).Elements[5]).UpdateText("VIGOR: " + Game.Characters[0].Vigor);
            ((UITextField)((UIWindow)Elements[0]).Elements[6]).UpdateText("STRENGTH: " + Game.Characters[0].Strength);
            ((UITextField)((UIWindow)Elements[0]).Elements[7]).UpdateText("AGILITY: " + Game.Characters[0].Agility);
            ((UITextField)((UIWindow)Elements[0]).Elements[8]).UpdateText("INTELLIGENCE: " + Game.Characters[0].Intelligence);
            ((UITextField)((UIWindow)Elements[0]).Elements[9]).UpdateText("SPIRIT: " + Game.Characters[0].Spirit);
            ((UITextField)((UIWindow)Elements[0]).Elements[12]).UpdateText(Game.Characters[0].Level.ToString());
            ((UITextField)((UIWindow)Elements[0]).Elements[13]).UpdateText(Game.Characters[0].SkillPoints.ToString());
            ((UIBar) Elements[1]).MaxValue = Game.Characters[0].Vigor;
            ((UIBar) Elements[1]).Value = Game.Characters[0].Health;
            ((UIBar) Elements[2]).MaxValue = Game.Characters[0].Intelligence;
            ((UIBar) Elements[2]).Value = Game.Characters[0].Mana;
            ((UIBar) Elements[3]).MaxValue = Game.Characters[0].ExperienceToNextLevel();
            ((UIBar) Elements[3]).Value = Game.Characters[0].Experience;
            ((UITimerIcon) Elements[4]).State = 1 - Game.Characters[0].Abilities[0].CooldownPercentage();
            ((UITimerIcon) Elements[5]).State = 1 - Game.Characters[0].Abilities[1].CooldownPercentage();
            ((UITimerIcon) Elements[6]).State = 1 - Game.Characters[0].Abilities[2].CooldownPercentage();
            ((UITimerIcon) Elements[7]).State = 1 - Game.Characters[0].Abilities[3].CooldownPercentage();
        }

        public void NextStep(View mainView, RenderWindow window, Time elapsed, bool single = true)
        {
            if(single)
                Game.NextStep(elapsed);
            UpdateUI();
            window.DispatchEvents();
            if(single)
                HandleHolding(window);
            else
                NetworkHolding(window);
            window.Clear();
            mainView.Center = new Vector2f(Game.Pentities[0].Position.X,
                Game.Pentities[0].Position.Y);
            window.SetView(mainView);
            Draw(window);
            window.Display();
        }

        public void Throw(PCharacter character)
        {
            PProjectile sword = new PProjectile("Sword", Game);
            sword.Owner = character;
            sword.Position = character.Position;
            sword.Velocity = new Vector2f(MathF.Cos((MathF.PI / 180) * (character.Display.State.facing + 90)) * 300,
                MathF.Sin((MathF.PI / 180) * (character.Display.State.facing + 90)) * 300);
            sword.Display.State.facing = character.Display.State.facing;
            Game.Pentities.Add(sword);
        }

        public void WhirlWind(PCharacter character)
        {
            foreach (var gameCharacter in Game.Characters)
            {
                if (gameCharacter != character && character.InReach(gameCharacter,2))
                {
                    character.Hit(gameCharacter);
                }
            }
        }

        public void Lunge(PCharacter character)
        {
            character.Position += new Vector2f(MathF.Cos((MathF.PI / 180) * (character.Display.State.facing + 90)) * character.Speed*2,
                MathF.Sin((MathF.PI / 180) * (character.Display.State.facing + 90)) * 100);
            if (character.Position.X < 0)
                character.Position.X = 0;
            if (character.Position.X > Game.Map.Size.X)
                character.Position.X = Game.Map.Size.X;
            if (character.Position.Y < 0)
                character.Position.Y = 0;
            if (character.Position.Y > Game.Map.Size.X)
                character.Position.Y = Game.Map.Size.X;
            foreach (var gameCharacter in Game.Characters)
            {
                if (gameCharacter != character && character.InReach(gameCharacter))
                {
                    character.Hit(gameCharacter);
                }
            }
        }
        public void InitGame(bool cont = false)
        {
            Game = new Game();
            Map map = new Map(4);
            map.Size = new Vector2f(1600,900);
            map.GenerateTiles(4);
            Game.Map = map;
            if (cont)
            {
                Game.AddCharacter(Game.LoadCharacter(new Vector2f(30, 30), new Vector2f(70, 100)));
                Game.Characters[0].Abilities.Add(new PCharacter.Ability(Time.FromSeconds(1), 1, Throw));
                Game.Characters[0].Abilities.Add(new PCharacter.Ability(Time.FromSeconds(3), 5, WhirlWind));
                Game.Characters[0].Abilities.Add(new PCharacter.Ability(Time.FromSeconds(5), 3, Lunge));
            }
            else
            {
                Game.AddCharacter("Player", new Vector2f(30, 30), new Vector2f(70, 100));
                Game.Characters[0].Abilities.Add(new PCharacter.Ability(Time.FromSeconds(1), 1, Throw));
                Game.Characters[0].Abilities.Add(new PCharacter.Ability(Time.FromSeconds(3), 5, WhirlWind));
                Game.Characters[0].Abilities.Add(new PCharacter.Ability(Time.FromSeconds(5), 3, Lunge));
            }
            GameRoom gameroom = new GameRoom(Game, new Vector2i(10,7));
            Game.LoadRoom(gameroom);
            Game.Start = false;
        }

        public void RunGame(RenderWindow window, bool continueGame = false)
        {
            window.MouseButtonPressed += GameMouseDown;
            window.MouseButtonReleased += GameMouseUp;
            window.KeyPressed += GameKeyDown;
            InitGame(continueGame);
            Clock timer = new Clock();
            View mainView = new SFML.Graphics.View
            {
                Size = new Vector2f(1600, 900),
                Center = new Vector2f(Game.Pentities[0].Position.X, Game.Pentities[0].Position.Y),
                Viewport = new FloatRect(0, 0, 1, 1)
            };
            while (!exit)
            {
                if (Game.Start)
                    InitGame(true);
                NextStep(mainView, window,timer.Restart());
            }
            window.MouseButtonPressed -= GameMouseDown;
            window.MouseButtonReleased -= GameMouseUp;
            window.KeyPressed -= GameKeyDown;
        }
        public void RunMultiGame(RenderWindow window, bool Host = true, string IP = "")
        {
            if(Host)
            {
                window.MouseButtonPressed += GameMouseDown;
                window.MouseButtonReleased += GameMouseUp;
                window.KeyPressed += GameKeyDown;
                InitGame(true);
                UDPServer server = new UDPServer();
                server.GameManager = this;
                Thread serverThread = new Thread(server.Run);
                serverThread.Start();
                Clock timer = new Clock();
                View mainView = new SFML.Graphics.View
                {
                    Size = new Vector2f(1600, 900),
                    Center = new Vector2f(Game.Pentities[0].Position.X, Game.Pentities[0].Position.Y),
                    Viewport = new FloatRect(0, 0, 1, 1)
                };
                while (!exit)
                {
                    if (Game.Start)
                        InitGame(true);
                    NextStep(mainView, window, timer.Restart());
                }
                server.KeepRunning = false;
                window.MouseButtonPressed -= GameMouseDown;
                window.MouseButtonReleased -= GameMouseUp;
                window.KeyPressed -= GameKeyDown;
            }
            else
            {
                window.MouseButtonPressed += NetworkMouseDown;
                window.MouseButtonReleased += NetworkMouseUp;
                window.KeyPressed += NetworkKeyDown;
                InitGame(true);
                UDPClient client = new UDPClient();
                client.Host = IP;
                client.GameManager = this;
                Thread clientThread = new Thread(client.Run);
                clientThread.Start();
                Clock timer = new Clock();
                View mainView = new SFML.Graphics.View
                {
                    Size = new Vector2f(1600, 900),
                    Center = new Vector2f(Game.Pentities[0].Position.X, Game.Pentities[0].Position.Y),
                    Viewport = new FloatRect(0, 0, 1, 1)
                };
                while (!exit)
                {
                    lock (Game)
                    {
                        NextStep(mainView,window,timer.Restart(), false);
                    }
                }
                client.KeepRunning = false;
            }
        }

        public byte[] Status()
        {
            return Game.Status();
        }
    }
}