using Models.Database;

namespace HttpHelper.Hash
{
    public class ModelHash
    {
        public static string GetUserHash(UserModel user)
        {
            return Sha1Hash.GetSha1HashData(user.Username);
        }
    }
}
