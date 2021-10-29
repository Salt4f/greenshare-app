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
            if (string.IsNullOrEmpty(dni) || dni.Length != 9) return false;
            string numbers = dni.Substring(0, 8);
            string leter = dni.Substring(8, 1);
            var numbersValid = int.TryParse(numbers, out int dniInteger);
            if (CalculateDniLeter(dniInteger) != leter)
            {
                //La letra del DNI es incorrecta
                return false;
            }
            if (!numbersValid)
            {
                //No se pudo convertir los números a formato númerico
                return false;
            }
            return true;
        }

        private static string CalculateDniLeter(int dniNumbers)
        {
            string[] control = { "T", "R", "W", "A", "G", "M", "Y", "F", "P", "D", "X", "B", "N", "J", "Z", "S", "Q", "V", "H", "L", "C", "K", "E" };
            var mod = dniNumbers % 23;
            return control[mod];
        }
    }
}
