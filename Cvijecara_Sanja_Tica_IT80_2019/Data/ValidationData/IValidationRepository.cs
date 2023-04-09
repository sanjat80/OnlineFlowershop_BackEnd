namespace Cvijecara_Sanja_Tica_IT80_2019.Data.ValidationData
{
    public interface IValidationRepository
    {
        public bool ValidateValuta(string valuta);
        public bool IsValidEmail(string email);
        public bool ValidatePassword(string password);
        public bool ValidateDatumIsporuke(DateTime date);
    }
}
