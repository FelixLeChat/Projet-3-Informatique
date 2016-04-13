using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using Helper.Jwt;
using HttpHelper.Hash;
using HttpHelper.Time;
using Microsoft.Web.WebSockets;
using Models.Communication;
using Models.Token;
using Newtonsoft.Json;

namespace PrincessAPI.Websocket
{
    public class ChatWebsocket : WebSocketHandler
    {
        // list of all clients connected
        private static readonly WebSocketCollection Clients = new WebSocketCollection();
        private static readonly Dictionary<string, ChatWebsocket> ClientIds = new Dictionary<string, ChatWebsocket>();
        public readonly List<string> MyCanalList = new List<string>();

        // Player Token
        private UserToken _playerToken = new UserToken();

        public override void OnOpen()
        {
            Clients.Add(this);

            // add to general canal
            //MyCanalList.Add(Sha1Hash.GetSha1HashData("General"));
        }

        public override void OnMessage(string message)
        {
            try
            {
                var decoded = new WebsocketChatMessage();

                // Try to decode message
                try
                {
                    decoded = JsonConvert.DeserializeObject<WebsocketChatMessage>(message);
                }
                catch (Exception e)
                {
                    Send(JsonConvert.SerializeObject(new ErrorMessage() {Error = "Json Invalide - " + e.Message}));
                    return;
                }

                if (decoded.IsValid())
                {
                    // Update token
                    if (_playerToken == null)
                        _playerToken = new UserToken();

                    if (string.IsNullOrWhiteSpace(_playerToken.UserId))
                    {
                        try
                        {
                            _playerToken = JwtHelper.DecodeToken(decoded.UserToken);
                        }
                        catch (Exception)
                        {
                            Send(JsonConvert.SerializeObject(new ErrorMessage() {Error = "Token Usager Invalide"}));
                            return;
                        }
                    }

                    #region Friend chat canal
                    // add client to player list
                    if(!ClientIds.ContainsKey(_playerToken.UserId))
                        ClientIds.Add(_playerToken.UserId, this);

                    // Information to create a new canal with specified user
                    if (!string.IsNullOrWhiteSpace(decoded.TargetUserHashId) && !string.IsNullOrWhiteSpace(decoded.TargetUserName))
                    {
                        // User is connected
                        if (ClientIds.ContainsKey(decoded.TargetUserHashId))
                        {
                            var otherPlayer = ClientIds[decoded.TargetUserHashId];

                            // create new chat with both users
                            var hashId = Sha1Hash.DoubleSha1Hash(BigInteger.Parse(_playerToken.UserId),
                                BigInteger.Parse(decoded.TargetUserHashId));

                            if (!otherPlayer.MyCanalList.Contains(hashId))
                            {
                                otherPlayer.MyCanalList.Add(hashId);
                                otherPlayer.Send(JsonConvert.SerializeObject(new WebsocketChatMessage()
                                {
                                    CanalId = hashId,
                                    CanalName = _playerToken.Username
                                }));
                            }

                            if (!MyCanalList.Contains(hashId))
                            {// Add canal to canal list
                                MyCanalList.Add(hashId);
                                // send to players
                                Send(JsonConvert.SerializeObject(new WebsocketChatMessage()
                                {
                                    CanalId = hashId,
                                    CanalName = decoded.TargetUserName
                                }));
                            }
                            else
                            {
                                Send(
                                JsonConvert.SerializeObject(new ErrorMessage()
                                {
                                    Error = "Vous êtes déjà dans le chat avec cet usager."
                                }));
                            }
                        }
                        else
                        {
                            Send(
                                JsonConvert.SerializeObject(new ErrorMessage()
                                {
                                    Error = "Le joueur que vous essayez de contecter n'est pas en ligne."
                                }));
                        }
                        return;
                    }
                    #endregion

                    #region create public canal
                    // Information to create/join a new chat canal
                    var chatName = decoded.CanalName;
                    if (!string.IsNullOrWhiteSpace(chatName))
                    {
                        var hashId = Sha1Hash.GetSha1HashData(chatName);
                        if (!MyCanalList.Contains(hashId))
                        {
                            // Add canal to canal list
                            MyCanalList.Add(hashId);
                        }
                        Send(JsonConvert.SerializeObject(new WebsocketChatMessage()
                        {
                            CanalId = hashId,
                            CanalName = chatName
                        }));
                        return;
                    }
                    #endregion

                    #region send message in canal
                    // Information to send a message to a chat canal
                    var chatId = decoded.CanalId;
                    var messageContent = decoded.Message;
                    if (!string.IsNullOrWhiteSpace(chatId) && !string.IsNullOrWhiteSpace(messageContent))
                    {
                        if (messageContent.Length > 255)
                        {
                            Send(JsonConvert.SerializeObject(new ErrorMessage() { Error = "Chat trop long" }));
                            return;
                        }

                        messageContent = ReplaceBadWords(messageContent);

                        var clients = Clients.Where(x => ((ChatWebsocket) x).MyCanalList.Contains(chatId)).ToList();
                        if (clients != null && clients.Count > 0)
                        {
                            if (clients.Count == 1)
                            {
                                clients[0].Send(JsonConvert.SerializeObject(new WebsocketChatMessage()
                                {
                                    Message =
                                       _playerToken.Username + " @ " +
                                       TimeHelper.CurrentCanadaTimeString() +
                                       " : (Vous êtes le seul dans ce canal) " + messageContent,
                                    CanalId = chatId
                                }));
                                return;
                            }

                            // send message to all clients who has this canal in their canal list
                            foreach (var client in clients)
                            {
                                client.Send(JsonConvert.SerializeObject(new WebsocketChatMessage()
                                {
                                    Message =
                                        _playerToken.Username + " @ " +
                                        TimeHelper.CurrentCanadaTimeString() +
                                        " : " + messageContent,
                                    CanalId = chatId
                                }));
                            }
                        }
                        return;
                    }
                    #endregion

                    #region Unregister from canal
                    // unregister from canal
                    if (!string.IsNullOrWhiteSpace(chatId))
                    {
                        if (MyCanalList.Contains(chatId))
                            MyCanalList.Remove(chatId);
                        else
                            Send(
                                JsonConvert.SerializeObject(new ErrorMessage()
                                {
                                    Error = "Vous êtes déjà enregistrer à ce canal."
                                }));
                    }
                    #endregion
                    return;

                }

                Send(
                    JsonConvert.SerializeObject(new ErrorMessage()
                    {
                        Error = "Format de communication invalide. Allez voir le document de communication."
                    }));
            }
            catch (Exception e)
            {
                Send(JsonConvert.SerializeObject(new ErrorMessage()
                {
                    Error = e.Message
                }));
            }
        }

        public override void OnClose()
        {
            Clients.Remove(this);
            if(ClientIds.ContainsKey(_playerToken.UserId))
                ClientIds.Remove(_playerToken.UserId);
        }

        private static string ReplaceBadWords(string input)
        {
            const string censoredText = "[PRINCESS]";
            const string patternTemplate = @"\b({0})(s?)\b";
            const RegexOptions options = RegexOptions.IgnoreCase;

            var badWords = new[] 
            { "ahole", "anus", "ash0le", "ash0les", "asholes", "ass", "Ass Monkey",
                "Assface", "assh0le", "assh0lez", "asshole", "assholes", "assholz",
                "asswipe", "azzhole", "bassterds", "bastard", "bastards", "bastardz",
                "basterds", "basterdz", "Biatch", "bitch", "bitches", "Blow Job",
                "boffing", "butthole", "buttwipe", "c0ck", "c0cks", "c0k", "Carpet Muncher",
                "cawk", "cawks", "Clit", "cnts", "cntz", "cock", "cockhead", "cock-head",
                "cocks", "CockSucker", "cock-sucker", "crap", "cum", "cunt", "cunts",
                "cuntz", "dick", "dild0", "dild0s", "dildo", "dildos", "dilld0",
                "dilld0s", "dominatricks", "dominatrics", "dominatrix", "dyke",
                "enema", "f u c k", "f u c k e r", "fag", "fag1t", "faget", "fagg1t",
                "faggit", "faggot", "fagit", "fags", "fagz", "faig", "faigs", "fart",
                "flipping the bird", "fuck", "fucker", "fuckin", "fucking", "fucks",
                "Fudge Packer", "fuk", "Fukah", "Fuken", "fuker", "Fukin", "Fukk", "Fukkah",
                "Fukken", "Fukker", "Fukkin", "g00k", "gay", "gayboy", "gaygirl", "gays",
                "gayz", "God-damned", "h00r", "h0ar", "h0re", "hells", "hoar", "hoor", "hoore",
                "jackoff", "jap", "japs", "jerk-off", "jisim", "jiss", "jizm", "jizz", "knob",
                "knobs", "knobz", "kunt", "kunts", "kuntz", "Lesbian", "Lezzian", "Lipshits",
                "Lipshitz", "masochist", "masokist", "massterbait", "masstrbait", "masstrbate",
                "masterbaiter", "masterbate", "masterbates", "Motha Fucker", "Motha Fuker", "Motha Fukkah",
                "Motha Fukker", "Mother Fucker", "Mother Fukah", "Mother Fuker", "Mother Fukkah",
                "Mother Fukker", "mother-fucker", "Mutha Fucker", "Mutha Fukah", "Mutha Fuker",
                "Mutha Fukkah", "Mutha Fukker", "n1gr", "nastt", "nigger;", "nigur;", "niiger;",
                "niigr;", "orafis", "orgasim;", "orgasm", "orgasum", "oriface",
                "orifice", "orifiss", "packi", "packie", "packy", "paki", "pakie",
                "paky", "pecker", "peeenus", "peeenusss", "peenus", "peinus", "pen1s",
                "penas", "penis", "penis-breath", "penus", "penuus", "Phuc", "Phuck", "Phuk",
                "Phuker", "Phukker", "polac", "polack", "polak", "Poonani", "pr1c", "pr1ck",
                "pr1k", "pusse", "pussee", "pussy", "puuke", "puuker", "queer", "queers",
                "queerz", "qweers", "qweerz", "qweir", "recktum", "rectum", "retard",
                "sadist", "scank", "schlong", "screwing", "semen", "sex", "sexy", "Sh!t",
                "sh1t", "sh1ter", "sh1ts", "sh1tter", "sh1tz", "shit", "shits", "shitter",
                "Shitty", "Shity", "shitz", "Shyt", "Shyte", "Shytty", "Shyty", "skanck",
                "skank", "skankee", "skankey", "skanks", "Skanky", "slut", "sluts", "Slutty",
                "slutz", "son-of-a-bitch", "tit", "turd", "va1jina", "vag1na", "vagiina",
                "vagina", "vaj1na", "vajina", "vullva", "vulva", "w0p", "wh00r", "wh0re",
                "whore", "xrated", "xxx", "b!+ch", "bitch", "blowjob", "clit", "arschloch",
                "fuck", "shit", "ass", "asshole", "b!tch", "b17ch", "b1tch", "bastard", "bi+ch",
                "boiolas", "buceta", "c0ck", "cawk", "chink", "cipa", "clits", "cock", "cum",
                "cunt", "dildo", "dirsa", "ejakulate", "fatass", "fcuk", "fuk", "fux0r", "hoer",
                "hore", "jism", "kawk", "l3itch", "l3i+ch", "lesbian", "masturbate", "masterbat",
                "masterbat3", "motherfucker", "s.o.b.", "mofo", "nazi", "nigga", "nigger",
                "nutsack", "phuck", "pimpis", "pusse", "pussy", "scrotum", "sh!t", "shemale",
                "shi+", "sh!+", "slut", "smut", "teets", "tits", "boobs", "b00bs", "teez",
                "testical", "testicle", "titt", "w00se", "jackoff", "wank", "whoar", "whore",
                "damn", "dyke", "fuck", "shit", "@$$", "amcik", "andskota", "arse", "assrammer",
                "ayir", "bi7ch", "bitch", "bollock", "breasts", "butt-pirate", "cabron", "cazzo",
                "chraa", "chuj", "Cock", "cunt", "d4mn", "daygo", "dego", "dick", "dike", "dupa",
                "dziwka", "ejackulate", "Ekrem", "Ekto", "enculer", "faen", "fag", "fanculo", "fanny",
                "feces", "feg", "Felcher", "ficken", "fitt", "Flikker", "foreskin", "Fotze", "Fu(", "fuk",
                "futkretzn", "gay", "gook", "guiena", "h0r", "h4x0r", "hell", "helvete", "hoer",
                "honkey", "Huevon", "hui", "injun", "jizz", "kanker", "kike", "klootzak", "kraut",
                "knulle", "kuk", "kuksuger", "Kurac", "kurwa", "kusi", "kyrpa", "lesbo", "mamhoon",
                "masturbat", "merd", "mibun", "monkleigh", "mouliewop", "muie", "mulkku", "muschi",
                "nazis", "nepesaurio", "nigger", "orospu", "paska", "perse", "picka", "pierdol",
                "pillu", "pimmel", "piss", "pizda", "poontsee", "poop", "porn", "p0rn", "pr0n",
                "preteen", "pula", "pule", "puta", "puto", "qahbeh", "queef", "rautenberg",
                "schaffer", "scheiss", "schlampe", "schmuck", "screw", "sh!t", "sharmuta", "sharmute",
                "shipal", "shiz", "skribz", "skurwysyn", "sphencter", "spic", "spierdalaj", "splooge",
                "suka", "b00b", "testicle", "titt", "twat", "vittu", "wank", "wetback", "wichser", "wop",
                "yed", "zabourah" };

            var badWordMatchers = badWords.
                Select(x => new Regex(string.Format(patternTemplate, Regex.Escape(x)), options));

            return badWordMatchers.
                Aggregate(input, (current, matcher) => matcher.Replace(current, censoredText));
        }
    }
}