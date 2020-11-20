using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SFML.System;
using SZDRPG.Model;
using SZDRPG.UIElements;

namespace SZDRPG.Network
{
    public class UDPClient
    {
        public string Host;
        public GameManager GameManager;
        public bool KeepRunning = true;
        public UdpClient UdpReceiver = new UdpClient(20201);
        public UdpClient UdpSender = new UdpClient(20200);

        public void Run()
        {
            Thread sender = new Thread(Send);
            sender.Start();
            Receive();
        }
        public void Receive()
        {
            
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 20201);
            while (KeepRunning)
            {
                Byte[] receiveBytes = UdpReceiver.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                HandleMessage(returnData, RemoteIpEndPoint);
            }
        }

        public void Send()
        {
            UdpSender.Connect(Host, 20200);
            byte[] join = Encoding.ASCII.GetBytes("JOIN|" + GameManager.Game.Characters[0]);
            UdpSender.Send(join, join.Length);
            while (KeepRunning)
            {
                UdpSender.Send(GameManager.NetworkIntent.GetMessage(), GameManager.NetworkIntent.Length);
                Thread.Sleep(10);
            }
        }
        private void HandleMessage(string returnData, IPEndPoint RemoteIpEndPoint)
        {
            string[] lines = returnData.Split("|");
            Game game = GameManager.Game;
            lock(game)
            {
                if (lines.Length > 0)
                {
                    int maplines = int.Parse(lines[3]) * 3 + 4;
                    if (int.Parse(lines[0]) != game.Map.num)
                    {
                        game.Map.num = int.Parse(lines[0]);
                        game.Map.Size = new Vector2f(int.Parse(lines[1]), int.Parse(lines[2]));
                        game.Map.Tiles.Clear();
                        game.Map.MapLayout.Clear();
                        for (int i = 0; i < int.Parse(lines[3]); i++)
                        {
                            game.Map.Tiles.Add(new Map.MapTileCenter(int.Parse(lines[4 + i * 3]),
                                new Vector2f(int.Parse(lines[5 + i * 3]), int.Parse(lines[6 + i * 3]))));
                        }

                        game.Map.LoadTiles();
                    }

                    game.Pentities.Clear();
                    game.Pentities.Add(game.Characters[0]);
                    int entityLineNums = 0;
                    for (int i = 0; i < int.Parse(lines[maplines]); i++)
                    {
                        if (lines[entityLineNums + maplines + 1].Equals("NetworkPCharacter"))
                        {
                            game.Characters[0].Position = new Vector2f(int.Parse(lines[entityLineNums + maplines + 3]),
                                int.Parse(lines[entityLineNums + maplines + 4]));
                            game.Characters[0].Display.State.ID = int.Parse(lines[entityLineNums + maplines + 5]);
                            game.Characters[0].Display.State.facing = float.Parse(lines[entityLineNums + maplines + 6]);
                            game.Characters[0].Experience = int.Parse(lines[entityLineNums + maplines + 8]);
                            game.Characters[0].Experience = int.Parse(lines[entityLineNums + maplines + 16]);
                            game.Characters[0].Experience = int.Parse(lines[entityLineNums + maplines + 17]);
                            entityLineNums += 17;
                        }
                        else if (lines[entityLineNums + maplines + 1].Equals("PCharacter"))
                        {
                            PCharacter character = new PCharacter(lines[entityLineNums + maplines + 2], game);
                            character.Position = new Vector2f(int.Parse(lines[entityLineNums + maplines + 3]),
                                int.Parse(lines[entityLineNums + maplines + 4]));
                            character.Display.State.ID = int.Parse(lines[entityLineNums + maplines + 5]);
                            character.Display.State.facing = float.Parse(lines[entityLineNums + maplines + 6]);
                            /*character.Display.State.elapsed =
                                Time.FromSeconds(float.Parse(lines[entityLineNums + maplines + 7]));*/
                            game.Pentities.Add(character);
                            entityLineNums += 17;
                        }
                        else if (lines[entityLineNums + maplines + 1].Equals("PProjectile"))
                        {
                            PProjectile projectile = new PProjectile(lines[entityLineNums + maplines + 2], game);
                            projectile.Position = new Vector2f(int.Parse(lines[entityLineNums + maplines + 3]),
                                int.Parse(lines[entityLineNums + maplines + 4]));
                            projectile.Display.State.ID = int.Parse(lines[entityLineNums + maplines + 5]);
                            projectile.Display.State.facing = float.Parse(lines[entityLineNums + maplines + 6]);
                            /*projectile.Display.State.elapsed =
                                Time.FromSeconds(float.Parse(lines[entityLineNums + maplines + 7]));*/
                            entityLineNums += 7;
                        }
                        else if (lines[entityLineNums + maplines + 1].Equals("PEnvironment"))
                        {
                            PEnvironment environment = new PEnvironment(lines[entityLineNums + maplines + 2], game);
                            environment.Position = new Vector2f(int.Parse(lines[entityLineNums + maplines + 3]),
                                int.Parse(lines[entityLineNums + maplines + 4]));
                            environment.Display.State.ID = int.Parse(lines[entityLineNums + maplines + 5]);
                            environment.Display.State.facing = float.Parse(lines[entityLineNums + maplines + 6]);
                            entityLineNums += 7;
                        }
                    }
                }
            }
        }
    }
}