using System;
using System.Collections.Generic;
using System.Linq;
using Helper.Jwt;
using Microsoft.Web.WebSockets;
using Models.Communication;
using Models.Database;
using Models.Token;
using Newtonsoft.Json.Linq;
using PrincessAPI.Services;

namespace PrincessAPI.Websocket
{
    public class GameWebsocket : WebSocketHandler
    {
        private static readonly WebSocketCollection Clients = new WebSocketCollection();
        private static readonly Dictionary<string, WebSocketHandler> clientUser = new Dictionary<string, WebSocketHandler>();

        public static object GameLock = new object();
        public static List<GameModel> Games = new List<GameModel>();
        public string hashId = null;
        private UserToken _token;

        public override void OnOpen()
        {
            Clients.Add(this);
        }

        public override void OnMessage(string message)
        {
            try
            {
                var jsonMessage = JObject.Parse(message);
                if (hashId == null)
                {
                    _token = JwtHelper.DecodeToken(jsonMessage["user_token"].ToString());
                    hashId = _token.UserId;
                    clientUser.Add(hashId, this);
                }

                var matchHashId = jsonMessage["match_id"].ToString();

                var curgame = Games.FirstOrDefault(g => g.HashId == matchHashId);
                if (curgame!= null)
                {
                    foreach (var playerId in curgame.ParticipantsHashId.Where(playerId => hashId != playerId))
                    {
                        clientUser[playerId].Send(message);
                    }

                    foreach (var specId in curgame.SpectatorsHashId)
                    {
                        if (clientUser.ContainsKey(specId))
                        {
                            clientUser[specId].Send(message);
                        }
                    }
                }
                    
            }
            catch
            {
               
            }
            
        }

        public override void OnClose()
        {
            if (hashId != null)
            {
                var curGame = Games.FirstOrDefault(g => g.ParticipantsHashId.Contains(hashId));
                if (curGame != null)
                {
                    if (curGame.HostHashId == hashId)
                    {
                        Games.Remove(curGame);
                    }
                    else
                    {
                        var gameService = new GameService(_token);
                        var disconnectMessage = new DisconnectMessage()
                        {
                            GameHashId = curGame.HashId
                        };
                        gameService.DisconnectOfGame(disconnectMessage);

                        dynamic leaveMessage = new JObject();
                        leaveMessage.user_id = hashId;
                        leaveMessage.match_id = curGame.HashId;
                        leaveMessage.event_type = 0;

                        foreach (var playerId in curGame.ParticipantsHashId)
                        {
                                clientUser[playerId].Send(leaveMessage.ToString());
                        }

                    }
                }
                if (hashId != null)
                    clientUser.Remove(hashId);
            }
            Clients.Remove(this);

        }
    }
}