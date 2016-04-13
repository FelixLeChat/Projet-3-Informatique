using Models;

namespace FrontEnd.Model
{
    public class Player
    {
        public string UserName { get; set; }
        public string HashId { get; set; }
        public EnumsModel.PrincessAvatar Picture { get; set; }
        public EnumsModel.PrincessTitle PrincessTitle { get; set; }
    }
}