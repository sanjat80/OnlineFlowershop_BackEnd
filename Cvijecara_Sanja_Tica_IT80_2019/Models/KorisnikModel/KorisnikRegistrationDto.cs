namespace Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel
{
    public class KorisnikRegistrationDto
    {
        public string Ime { get; set; } = null!;

        public string Prezime { get; set; } = null!;

        public string? Adresa { get; set; }

        public string BrojTelefona { get; set; } = null!;

        public string? KorisnickoIme { get; set; }

        public string Email { get; set; } = null!;

        public string Lozinka { get; set; } = null!;
    }
}
