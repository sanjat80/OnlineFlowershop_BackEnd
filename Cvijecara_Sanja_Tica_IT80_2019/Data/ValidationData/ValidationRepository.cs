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
    }
}
