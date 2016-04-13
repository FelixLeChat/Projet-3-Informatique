using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Models;

namespace FrontEndAccess
{
    public static class FacebookAccess
    {
        public static void PostOnWall(string facebookToken, string message, string description = "", string link = "")
        {
            if (!string.IsNullOrWhiteSpace(facebookToken) && !string.IsNullOrWhiteSpace(message))
            {
                var url = "https://graph.facebook.com/me/feed?" + "access_token=" + facebookToken;

                var parameters = "name="+"Princess Love Balls";

                // DICKBUT : http://wp.jays-place.com/wp-content/uploads/2015/02/dickbut.png
                if (!string.IsNullOrWhiteSpace(link))
                {
                    parameters += "&picture=" + link;
                    parameters += "&link=" + "http://ec2-52-90-46-132.compute-1.amazonaws.com";
                }

                if (!string.IsNullOrWhiteSpace(description))
                    parameters += "&description=" + HttpUtility.UrlEncode(description);

                parameters += "&message=" + HttpUtility.UrlEncode(message);

                var webRequest = WebRequest.Create(url);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.Method = "POST";
                var bytes = Encoding.ASCII.GetBytes(parameters);
                webRequest.ContentLength = bytes.Length;

                var os = webRequest.GetRequestStream();
                os.Write(bytes, 0, bytes.Length);
                os.Close();
                // Send the request to Facebook, and query the result to get the confirmation code
                try
                {
                    var webResponse = webRequest.GetResponse();
                    StreamReader sr = null;
                    try
                    {
                        sr = new StreamReader(webResponse.GetResponseStream());
                        var postId = sr.ReadToEnd();
                    }
                    finally
                    {
                        sr?.Close();
                    }
                }
                catch (WebException ex)
                {
                    // To help with debugging, we grab the exception stream to get full error details
                    StreamReader errorStream = null;
                    try
                    {
                        errorStream = new StreamReader(stream: ex.Response.GetResponseStream());
                        var errorMessage = errorStream.ReadToEnd();
                    }
                    finally
                    {
                        errorStream?.Close();
                    }
                }
            }
        }

        public static void PostOnWall(string facebookToken, EnumsModel.Achievement achievement)
        {
            var message = "Félicitation, tu as gagner un succès en : " + EnumsModel.GetAchievementDescription(achievement);
            var achievementLink = EnumsModel.GetAchievementPath(achievement);
            PostOnWall(facebookToken, message, link:achievementLink);
        }
    }
}