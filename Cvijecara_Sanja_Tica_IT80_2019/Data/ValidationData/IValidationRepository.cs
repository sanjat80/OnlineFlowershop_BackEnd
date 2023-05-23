namespace Cvijecara_Sanja_Tica_IT80_2019.Data.ValidationData
{
    public interface IValidationRepository
    {
        bool ValidateValuta(string valuta);
        bool IsValidEmail(string email);
        bool ValidatePassword(string password);
        bool ValidateDatumIsporuke(DateTime date);
        bool IsEmailUnique(string email);
        bool IsUsernameUnique(string username);
        bool ValidatePhoneNumber(string phoneNumber);
        bool ValidateAddress(string address);
    }
}
