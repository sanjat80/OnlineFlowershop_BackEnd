namespace Cvijecara_Sanja_Tica_IT80_2019.AuthHelpers
{
    public interface IAuthRepository
    {
        public AuthToken Authenticate(string korisnickoIme, string lozinka, string tipKorisnika);
    }
}
