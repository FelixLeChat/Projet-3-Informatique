using System;
using System.Collections.Generic;
using System.Linq;
using Helper.Jwt;
using Microsoft.Web.WebSockets;
using Models.Token;
using PrincessAPI.Services;

namespace PrincessAPI.Websocket
{
    public class ConnexionWebsocket : WebSocketHandler
    {
        private static readonly WebSocketCollection Clients = new WebSocketCollection();
        private UserToken _token;

        public static List<string> ConnectedUsersHash { get; set; } = new List<string>();

        public override void OnOpen()
        {
            Clients.Add(this);
        }

        public override void OnMessage(string message)
        {
            if (_token == null && !string.IsNullOrWhiteSpace(message))
            {
                try
                {
                    _token = JwtHelper.DecodeToken(message);

                    if (!ConnectedUsersHash.Contains(_token.UserId))
                        ConnectedUsersHash.Add(_token.UserId);
                }
                catch (Exception)
                {
                    _token = null;
                }
            }
        }

        public override void OnClose()
        {
            // User disconnected, delete all his games
            if (_token != null)
            {
                // connection remove
                if (ConnectedUsersHash.Contains(_token.UserId))
                    ConnectedUsersHash.Remove(_token.UserId);

                var gameService = new GameService(_token);
                var games = gameService.GetAllGames().Where(x => x.HostHashId == _token.UserId).ToList();
                foreach (var game in games)
                {
                    gameService.DeleteGame(game.HashId);
                }

                var remove = GameWebsocket.Games.Where(game => game.HostHashId == _token.UserId).ToList();
                foreach (var game in remove)
                {
                    GameWebsocket.Games.Remove(game);
                }
            }
        }
    }
}