using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PrincessAPI.Models.Site;
using PrincessAPI.Services;

namespace PrincessAPI.Controllers.Site
{
    public class ProfilController : Controller
    {
        public ActionResult Index(string id = null)
        {
            var utilisateur = User as PrincessPrincipal;
            var ps = new ProfileService(null);
            if (id.IsNullOrWhiteSpace())
            {
                if (utilisateur != null)
                {
                    ps = new ProfileService(utilisateur.Token);
                    return View(ps.GetMyProfile());
                }
                return RedirectToAction("Index", "Home");
            }
            
            var profile = ps.GetUserProfile(id, false);
            if(!profile.IsPrivate)
                return View(profile);

            //profile prive
            if(utilisateur == null)
                return RedirectToAction("Index", "Home"); //utilisateur non connecte
            var fs = new FriendService(utilisateur.Token);
            if (fs.GetAllFriends().Any(f => f.HashId == profile.UserHashId)) //Dans ses amis
                return View(profile);
            return RedirectToAction("Index", "Home");



        }

        public ActionResult Edit()
        {
            var user = User as PrincessPrincipal;
            if (user != null)
            {
                var ps = new ProfileService(user.Token);
                return View(new EditProfileModel(ps.GetMyProfile()));
            }

            return Index();
        }


        [HttpPost]
        public ActionResult Edit(EditProfileModel model)
        {
            if(!ModelState.IsValid)
                return View(model);
            try
            {
                var user = User as PrincessPrincipal;
                if (user == null)
                    return Index();
                var ps = new ProfileService(user.Token);
                var currentProfile = ps.GetMyProfile();
                var updatedProfile = model.updatedProfile(currentProfile);
                ps.UpdateMyProfile(updatedProfile);


                var curUser = User as PrincessPrincipal;
                var cookieData = curUser.Profile;
                cookieData.Picture = updatedProfile.Picture;

                HttpCookie cookie = FormsAuthentication.GetAuthCookie(curUser.Profile.UserHashId, false);
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);

                FormsAuthenticationTicket newticket = new FormsAuthenticationTicket(
                    ticket.Version,
                    ticket.Name,
                    ticket.IssueDate,
                    ticket.Expiration,
                    ticket.IsPersistent,
                    JsonConvert.SerializeObject(cookieData),
                    ticket.CookiePath);

                cookie.Value = FormsAuthentication.Encrypt(newticket);
                Response.Cookies.Set(cookie);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                model.ErrorMessage = "Une erreur inattendue est survenue :"+e.ToString();
                return View(model);
            }

        }
   
        public ActionResult Delete()
        {
            var user = User as PrincessPrincipal;
            if (user != null)
            {
                var cs = new ConnexionService();
                cs.DeleteUser(user.Token);
                return RedirectToAction("LogOff", "Account");
            }
            return Index();
        }

        public ActionResult EnleverAmi(string id, string username)
        {
            if (!id.IsNullOrWhiteSpace() && !username.IsNullOrWhiteSpace())
            {
                var user = User as PrincessPrincipal;
                if (user != null)
                {
                    var fs = new FriendService(user.Token);
                    fs.RemoveFriend(id, username);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult AjouterAmi(string id, string username)
        {
            if (!id.IsNullOrWhiteSpace() && !username.IsNullOrWhiteSpace())
            {
                var user = User as PrincessPrincipal;
                if (user != null)
                {
                    var fs = new FriendService(user.Token);
                    fs.AddFriend(id, username);
                }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}