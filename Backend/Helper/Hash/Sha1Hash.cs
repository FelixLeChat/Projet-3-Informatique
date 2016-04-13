using System.Security.Cryptography;
using System.Text;

namespace HttpHelper.Hash
{
    public class Sha1Hash
    {
        public static string GetSha1HashData(string data)
        {
            data = data.ToLower();
            //create new instance of md5
            var sha1 = SHA1.Create();

            //convert the input text to array of bytes
            var hashData = sha1.ComputeHash(Encoding.Default.GetBytes(data));

            //create new instance of StringBuilder to save hashed data
            var returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (var i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();
        }

        public static string DoubleSha1Hash(System.Numerics.BigInteger first, System.Numerics.BigInteger second)
        {
            return first > second ? GetSha1HashData(first.ToString()+second) : GetSha1HashData(second.ToString() + first);
        }
    }
}