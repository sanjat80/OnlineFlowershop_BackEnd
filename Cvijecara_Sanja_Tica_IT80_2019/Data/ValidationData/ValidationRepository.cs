using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.ValidationData
{
    public class ValidationRepository:IValidationRepository
    {
        public bool ValidateValuta(string valuta)
        {
            string[] valute = new string[] { "RSD", "EUR", "BAM" };
            if (string.IsNullOrEmpty(valuta))
            {
                return false;
            }
            if (!valute.Contains(valuta))
            {
                return false;
            }
            return true;
        }

        public bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";
            bool isValid = Regex.IsMatch(email, pattern);
            return isValid;
        }

        public bool ValidatePassword(string password)
        {
            // Password must be at least 8 characters long
            if (password.Length < 8)
            {
                return false;
            }

            // Password must contain at least one uppercase letter
            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                return false;
            }

            // Password must contain at least one lowercase letter
            if (!Regex.IsMatch(password, "[a-z]"))
            {
                return false;
            }

            // Password must contain at least one digit
            if (!Regex.IsMatch(password, "[0-9]"))
            {
                return false;
            }

            // Password must contain at least one special character
            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                return false;
            }

            // All password requirements met
            return true;
        }
        public bool ValidateDatumIsporuke(DateTime date)
        {
            
            DateTime currentDate = DateTime.Now.Date;
            DateTime minDate = currentDate.AddDays(1).Date;
            if (date.Date <= minDate)
            {
                return false;
            }

            return true;
        }

        public bool IsEmailUnique(string email)
        {
            using (var db = new CvijecaraContext())
            {
                var user = db.Korisniks.FirstOrDefault(u => u.Email == email);
                
                if(user != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public bool IsUsernameUnique(string username)
        {
            using (var db = new CvijecaraContext())
            {
                var user = db.Korisniks.FirstOrDefault(u => u.KorisnickoIme == username);

                if (user != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

    }
}
