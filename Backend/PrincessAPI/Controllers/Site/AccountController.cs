using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Helper.Jwt;
using Microsoft.Ajax.Utilities;
using Models.Communication;
using Newtonsoft.Json;
using PrincessAPI.Models.Site;
using PrincessAPI.Services;

namespace PrincessAPI.Controllers.Site
{
    public class AccountController : Controller
    {
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterViewModel());
        }


        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ErrorMessage = "Erreur d'enregistrement";
                return View(model);
            }
            if (model.Username.IsNullOrWhiteSpace())
            {
                model.ErrorMessage = "Nom d'utilisateur est obligatoire";
                return View(model);
            }
            if (model.Password.IsNullOrWhiteSpace())
            {
                model.ErrorMessage = "Mot de passe est obligatoire";
                return View(model);
            }
            if (model.Password!= model.PasswordCopy)
            {
                model.ErrorMessage = "Les mots de passe nes sont pas identiques";
                return View(model);
            }

            try
            {
                var connexionService = new ConnexionService();
                var userTokenString = connexionService.Register(new RegisterMessage()
                {
                    Username = model.Username,
                    Password = model.Password
                });
                return ConnectUser(model.RememberMe, userTokenString);
            }
            catch (System.Web.Http.HttpResponseException e)
            {
                model.ErrorMessage = "Erreur d'enregistrement : " + e.Response.ReasonPhrase;
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult Connect(string userToken)
        {
            if (userToken.IsNullOrWhiteSpace())
            {
                return RedirectToAction("Login");
            }

            try
            {
                return ConnectUser(false, userToken);

            }
            catch (Exception)
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ErrorMessage = "Erreur d'enregistrement";
                return View(model);
            }
            if (model.Username.IsNullOrWhiteSpace())
            {
                model.ErrorMessage = "Nom d'utilisateur est obligatoire";
                return View(model);
            }
            if (model.Password.IsNullOrWhiteSpace())
            {
                model.ErrorMessage = "Mot de passe est obligatoire";
                return View(model);
            }

            try
            {
                var connexionService = new ConnexionService();
                var userTokenString = connexionService.Login(new LoginMessage()
                {
                    Username = model.Username,
                    Password = model.Password
                });
                return ConnectUser(model.RememberMe, userTokenString);
            }
            catch (System.Web.Http.HttpResponseException e)
            {
                model.ErrorMessage = "Erreur de connection : " + e.Response.ReasonPhrase;
                return View(model);
            }
        }

        private ActionResult ConnectUser(bool rememberMe, string userTokenString)
        {
            var token = JwtHelper.DecodeToken(userTokenString);

            var profileService = new ProfileService(token);

            var myProfile = profileService.GetMyProfile();
            string userData = JsonConvert.SerializeObject(new PrincessSerializable()
            {
                Id = myProfile.Id,
                Picture = myProfile.Picture,
                PrincessTitle = myProfile.PrincessTitle,
                Token = userTokenString,
                Username = myProfile.Username,
                UserHashId = myProfile.UserHashId
            });

            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                1,
                myProfile.UserHashId,
                DateTime.Now,
                DateTime.Now.AddMinutes(15),
                rememberMe, //pass here true, if you want to implement remember me functionality
                userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            Response.Cookies.Add(faCookie);
            return RedirectToAction("Index", "Profil");
        }
    }
}