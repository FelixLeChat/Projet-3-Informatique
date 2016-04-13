using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PrincessAPI.Models.Site
{
    public class LoginViewModel
    {
        [Display(Name = "Nom de princesse")]
        public string Username { get; set; }
        
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [Display(Name = "Se souvenir de moi?")]
        public bool RememberMe { get; set; }

        public String ErrorMessage { get; set; }
    }
}