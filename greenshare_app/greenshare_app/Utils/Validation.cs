using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace greenshare_app.Utils
{
    public class Validation
    {
        private readonly static Regex emailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
         
            Match match = emailRegex.Match(email);

            return match.Success;
        }

        public static bool PasswordsAreEqual(string password, string repeatPassword)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(repeatPassword)) return false;
            else if (password == repeatPassword) return true;
            return false;
        }
        
        public static bool ValidateDni(string dni)
        {
            return false;
        }
    }
}
