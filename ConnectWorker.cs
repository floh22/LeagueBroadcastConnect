// League Broadcast Connect - League Broadcast (Essence) addon for game information in files
//     Copyright (C) 2021 Lars Eble
//     This file, ConnectWorker.cs, is part of League Broadcast Connect
//
//     League Broadcast Connect is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//
//     League Broadcast (Essence) is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY, without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
//     along with League Broadcast Connect.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Dynamic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueBroadcastConnect
{
    class ConnectWorker : BackgroundService
    {
        private readonly ILogger<ConnectWorker> _logger;
        private readonly CommandLineOptions _options;

        private ClientWebSocket socket;

        private string dragonIconLocation;

        private StateData pastState;

        public ConnectWorker(ILogger<ConnectWorker> logger, CommandLineOptions options)
        {
            _logger = logger;
            _options = options;
            if (_options.Path == "localFolder")
                _options.Path = Directory.GetCurrentDirectory();

            dragonIconLocation = Path.Combine(_options.Path, "dragons");
            //ExecuteAsync(CancellationToken.None);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
            try
            {
                socket = new ClientWebSocket();
                new Task(async () =>
                {
                    _logger.LogInformation("LeagueBroadcastConnect started");
                    _logger.LogInformation($"Connecting to ws://{_options.Url}:{_options.Port}/api");

                    _logger.LogInformation(socket.State.ToString());

                    while (true)
                    {

                        if (socket.State == WebSocketState.Closed || socket.State == WebSocketState.None || socket.State == WebSocketState.CloseReceived || socket.State == WebSocketState.Aborted)
                        {
                            try
                            {
                                socket.Abort();
                                socket = new ClientWebSocket();
                                await socket.ConnectAsync(new Uri($"ws://{_options.Url}:{_options.Port}/api"), CancellationToken.None);
                            } catch
                            {
                                //Retry connection once every 10 seconds
                                await Task.Delay(10000);
                                continue;
                            }
                            
                            if (socket.State == WebSocketState.Open)
                            {
                                _logger.LogInformation("LeagueBroadcastConnect connected");

                                new Task(async () => {
                                    var buffer = new ArraySegment<byte>(new byte[2048]);
                                    while (true)
                                    {
                                        try
                                        {
                                            WebSocketReceiveResult result;
                                            using MemoryStream ms = new();
                                            do
                                            {

                                                result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                                                ms.Write(buffer.Array, buffer.Offset, result.Count);
                                            } while (!result.EndOfMessage);

                                            if (result.MessageType == WebSocketMessageType.Close)
                                                break;

                                            ms.Seek(0, SeekOrigin.Begin);
                                            using StreamReader reader = new(ms, Encoding.UTF8);
                                            ReceiveEvent(await reader.ReadToEndAsync());
                                            await Task.Delay(20);
                                        } catch
                                        {
                                            break;
                                        }
                                        
                                    };
                                }).Start();
                            }
                        }
                    }
                }).Start();

                var tcs = new TaskCompletionSource<bool>();
                stoppingToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
                await tcs.Task;

                socket.Dispose();

                _logger.LogInformation("LeagueBroadcastConnect stopped");
            }
            catch (Exception e)
            {
                _logger.LogCritical($"LBC ERROR - {e.Message}");
            }
        }

        private void ReceiveEvent(string content)
        {
            HeartbeatEvent received = JsonConvert.DeserializeObject<HeartbeatEvent>(content);  
            if (received.eventType == "GameHeartbeat")
            {
                StateData state = received.stateData;
                

                int gameMin = (int) Math.Floor((double)received.stateData.gameTime / 60);
                int gameSec = (int) Math.Floor((double)received.stateData.gameTime % 60);

                dynamic converted = new ExpandoObject();
                converted.gameTime = $"{gameMin:D2}:{gameSec:D2}";

                if (received.stateData.scoreboard.GameTime != 0)
                {
                    converted.blueGold = $"{(int)Math.Floor((float)received.stateData.scoreboard.BlueTeam.Gold / 1000):D2}.{(int)(Math.Floor((float)received.stateData.scoreboard.BlueTeam.Gold % 1000) / 100)}k";
                    converted.blueKills = received.stateData.scoreboard.BlueTeam.Kills;
                    converted.blueTowers = received.stateData.scoreboard.BlueTeam.Towers;
                    
                    string[] blueDrakesOut = new string[4];
                    for (int i = 0; i < 4; i++)
                    {
                        if(i >= received.stateData.scoreboard.BlueTeam.Dragons.Count)
                        {
                            blueDrakesOut[i] = "None";
                            continue;
                        }
                        blueDrakesOut[i] = received.stateData.scoreboard.BlueTeam.Dragons[i];
                    }
                    converted.blueDrake0 = blueDrakesOut[0];
                    converted.blueDrake1 = blueDrakesOut[1];
                    converted.blueDrake2 = blueDrakesOut[2];
                    converted.blueDrake3 = blueDrakesOut[3];
                    

                    converted.redGold = $"{(int)Math.Floor((float)received.stateData.scoreboard.RedTeam.Gold / 1000):D2}.{(int)(Math.Floor((float)received.stateData.scoreboard.RedTeam.Gold % 1000) / 100)}k";
                    converted.redKills = received.stateData.scoreboard.RedTeam.Kills;
                    converted.redTowers = received.stateData.scoreboard.RedTeam.Towers;

                    string[] redDrakesOut = new string[4];
                    for (int i = 0; i < 4; i++)
                    {
                        if (i >= received.stateData.scoreboard.RedTeam.Dragons.Count)
                        {
                            redDrakesOut[i] = "None";
                            continue;
                        }
                        redDrakesOut[i] = received.stateData.scoreboard.RedTeam.Dragons[i];
                    }
                    converted.redDrake0 = redDrakesOut[0];
                    converted.redDrake1 = redDrakesOut[1];
                    converted.redDrake2 = redDrakesOut[2];
                    converted.redDrake3 = redDrakesOut[3];

                    converted.drakeTimer = $"{(int)Math.Floor(received.stateData.dragon.SpawnTimer / 60)}:{(int)Math.Floor(received.stateData.dragon.SpawnTimer % 60):D2}";
                    converted.baronTimer = $"{(int)Math.Floor(received.stateData.baron.SpawnTimer / 60)}:{(int)Math.Floor(received.stateData.baron.SpawnTimer % 60):D2}";
                }
               

                if(_options.Multifile)
                {
                    //Multi File
                    WritePropertyToFile("gameTime", converted.gameTime);

                    WritePropertyToFile("blueGold", converted.blueGold);
                    WritePropertyToFile("blueKills", converted.blueKills);
                    WritePropertyToFile("blueTowers", converted.blueTowers);

                    if (pastState == null || state.scoreboard.BlueTeam.Dragons.Count != pastState.scoreboard.BlueTeam.Dragons.Count)
                    {
                        File.Copy(Path.Combine(dragonIconLocation, converted.blueDrake0 + ".png"), Path.Combine(_options.Path, "blueDrake0.png"), true);
                        File.Copy(Path.Combine(dragonIconLocation, converted.blueDrake1 + ".png"), Path.Combine(_options.Path, "blueDrake1.png"), true);
                        File.Copy(Path.Combine(dragonIconLocation, converted.blueDrake2 + ".png"), Path.Combine(_options.Path, "blueDrake2.png"), true);
                        File.Copy(Path.Combine(dragonIconLocation, converted.blueDrake3 + ".png"), Path.Combine(_options.Path, "blueDrake3.png"), true);
                    }

                    WritePropertyToFile("redGold", converted.redGold);
                    WritePropertyToFile("redKills", converted.redKills);
                    WritePropertyToFile("redTowers", converted.redTowers);

                    if (pastState == null || state.scoreboard.RedTeam.Dragons.Count != pastState.scoreboard.RedTeam.Dragons.Count)
                    {
                        File.Copy(Path.Combine(dragonIconLocation, converted.redDrake0 + ".png"), Path.Combine(_options.Path, "redDrake0.png"), true);
                        File.Copy(Path.Combine(dragonIconLocation, converted.redDrake1 + ".png"), Path.Combine(_options.Path, "redDrake1.png"), true);
                        File.Copy(Path.Combine(dragonIconLocation, converted.redDrake2 + ".png"), Path.Combine(_options.Path, "redDrake2.png"), true);
                        File.Copy(Path.Combine(dragonIconLocation, converted.redDrake3 + ".png"), Path.Combine(_options.Path, "redDrake3.png"), true);
                    }

                    WritePropertyToFile("drakeTimer", converted.drakeTimer);
                    WritePropertyToFile("baronTimer", converted.baronTimer);
                } else
                {
                    //Single File
                    string toWrite = JsonConvert.SerializeObject(converted, Formatting.Indented);
                    File.WriteAllTextAsync(Path.Combine(_options.Path, _options.FileName), $"[{toWrite}]");
                }

                pastState = state;
            }
        }

        private void WritePropertyToFile(string propertyName, object property)
        {
            File.WriteAllTextAsync(Path.Combine(_options.Path, propertyName + ".txt"), property.ToString());
        }
    }
}
