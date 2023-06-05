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
            
            if (password.Length < 8)
            {
                return false;
            }

            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                return false;
            }

            if (!Regex.IsMatch(password, "[a-z]"))
            {
                return false;
            }

            if (!Regex.IsMatch(password, "[0-9]"))
            {
                return false;
            }

            if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                return false;
            }

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

        public bool ValidatePhoneNumber(string phoneNumber)
        {
            // Remove any whitespace characters from the phone number
            phoneNumber = phoneNumber.Replace(" ", "");

            // Validate the phone number format
            if (phoneNumber.StartsWith("+") && phoneNumber.Length <= 13)
            {
                // Check if the remaining characters are digits
                for (int i = 1; i < phoneNumber.Length; i++)
                {
                    if (!char.IsDigit(phoneNumber[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
        public bool ValidateAddress(string address)
        {
            bool hasLetter = false;
            bool hasNumber = false;

            foreach (char c in address)
            {
                if (char.IsLetter(c))
                {
                    hasLetter = true;
                }
                else if (char.IsDigit(c))
                {
                    hasNumber = true;
                }

                if (hasLetter && hasNumber)
                {
                    return true;
                }
            }

            return false;
        }
        public bool ValidateCountry(string country)
        {
            string[] validCountries = { "Srbija", "Hrvatska", "Crna Gora", "Bosna i Hercegovina" };

            foreach (string validCountry in validCountries)
            {
                if (string.Equals(country, validCountry, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        public bool ValidateRegion(string region)
        {
            string[] validRegions = { 
        // Srbija
        "Vojvodina", "Beograd", "Šumadija", "Zapadna Srbija", "Južna Srbija", "Istočna Srbija",
        // Hrvatska
        "Zagreb", "Sjeverna Hrvatska", "Središnja Hrvatska", "Istočna Hrvatska", "Zapadna Hrvatska", "Južna Hrvatska",
        // Crna Gora
        "Crnogorsko Primorje", "Crnogorsko Gorje", "Crnogorsko Polje", "Centralna Crna Gora", "Crnogorsko Primorje", "Južna Crna Gora",
        // Bosna i Hercegovina
        "Sarajevo", "Sjeveroistočna Bosna", "Posavina", "Srednja Bosna", "Hercegovina-Zapad", "Hercegovina-Jug", "Zapadnohercegovački kanton"
    };

            foreach (string validRegion in validRegions)
            {
                if (string.Equals(region, validRegion, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public bool ValidateCity(string city)
        {
            string[] validCities = { 
        // Srbija
        "Novi Sad", "Beograd", "Niš", "Kragujevac", "Subotica", "Čačak",
        // Hrvatska
        "Zagreb", "Split", "Rijeka", "Osijek", "Zadar", "Dubrovnik",
        // Crna Gora
        "Podgorica", "Nikšić", "Pljevlja", "Cetinje", "Bar", "Herceg Novi",
        // Bosna i Hercegovina
        "Trebinje","Sarajevo", "Banja Luka", "Prijedor", "Doboj", "Modrica", "Gacko"
    };

            foreach (string validCity in validCities)
            {
                if (string.Equals(city, validCity, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }



    }
}
