using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using SFML.System;
using SZDRPG.Model;

namespace SZDRPG.Network
{
    public class UDPServer
    {
        public bool KeepRunning = true;
        public string Message = "";
        public GameManager GameManager;
        public List<string> Connections = new List<string>();
        public UdpClient UdpReceiver = new UdpClient(20200);
        public UdpClient UdpSender = new UdpClient(20201);

        public void Run()
        {
            Thread sender = new Thread(Send);
            sender.Start();
            Receive();
        }

        public void Receive()
        {
            
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 20200);
            while (KeepRunning)
            {
                Byte[] receiveBytes = UdpReceiver.Receive(ref RemoteIpEndPoint);
                string returnData = Encoding.ASCII.GetString(receiveBytes);
                HandleMessage(returnData, RemoteIpEndPoint);
            }
        }

        public void Send()
        {
            while (KeepRunning)
            {
                lock(Connections)
                {
                    foreach (var connection in Connections)
                    {
                        UdpSender.Connect(connection, 20201);
                        UdpSender.Send(GameManager.Status(), GameManager.Game.StatusLength);
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void HandleMessage(string returnData, IPEndPoint RemoteIpEndPoint)
        {
            string[] lines = returnData.Split("|");
            if (lines.Length > 0)
            {
                string ip = RemoteIpEndPoint.Address.ToString();
                if (lines[0].Equals("JOIN") && lines[1].Equals("PCharacter"))
                {
                    if (!Connections.Contains(ip))
                    {
                        lock (Connections)
                        {
                            Connections.Add(ip);
                            PCharacter character = new PCharacter("Player",GameManager.Game);
                            character.Position = new Vector2f(int.Parse(lines[3]), int.Parse(lines[4]));
                            character.Experience = int.Parse(lines[8]);
                            character.Level = int.Parse(lines[9]);
                            character.SkillPoints = int.Parse(lines[10]);
                            character.Vigor = int.Parse(lines[11]);
                            character.Strength = int.Parse(lines[12]);
                            character.Agility = int.Parse(lines[13]);
                            character.Intelligence = int.Parse(lines[14]);
                            character.Spirit = int.Parse(lines[15]);
                            character.Health = character.Vigor;
                            character.Mana = character.Intelligence;
                            character.Abilities.Add(new PCharacter.Ability(Time.FromSeconds(1), 1, GameManager.Throw));
                            character.Abilities.Add(new PCharacter.Ability(Time.FromSeconds(3), 5, GameManager.WhirlWind));
                            character.Abilities.Add(new PCharacter.Ability(Time.FromSeconds(5), 3, GameManager.Lunge));
                            GameManager.Game.AddCharacter(character);
                            GameManager.Game.NetworkPlayers.Add(character);
                        }
                    }

                }
                else
                {
                    if (Connections.Contains(ip))
                    {
                        int num = 0;
                        for (int i = 0; i < Connections.Count; i++)
                        {
                            if (Connections[i].Equals(ip))
                                num = i;
                        }

                        PCharacter player = GameManager.Game.NetworkPlayers[num];
                        Vector2f position = new Vector2f(int.Parse(lines[1]),int.Parse(lines[2]));
                        switch (int.Parse(lines[0]))
                        {
                            case 0:
                                PEntity entity = GameManager.Game.EntityAt(position);
                                if (entity != null && entity != player)
                                {
                                    player.Target = entity;
                                    player.Direction = null;
                                }
                                else
                                {
                                    Console.WriteLine("Position: " + position.X + " " + position.Y);
                                    Console.WriteLine("Direction: " + GameManager.Game.Map.IntersectAt(player.Position, position).Value.X + " " + GameManager.Game.Map.IntersectAt(player.Position, position).Value.Y);
                                    player.Target = null;
                                    player.Direction = GameManager.Game.Map.IntersectAt(player.Position, position);
                                    if (player.Direction == player.Position)
                                        player.Direction = null;

                                }
                                break;
                            case 1:
                                player.Display.State.UpdateFacing(position);
                                player.DoSpecialAttack(1);
                                break;
                            case 2:
                                player.DoSpecialAttack(2);
                                break;
                            case 3:
                                player.DoSpecialAttack(3);
                                break;
                            default:
                                player.Target = null;
                                player.Direction = null;
                                break;
                        }
                    }
                }
            }
        }
    }
}