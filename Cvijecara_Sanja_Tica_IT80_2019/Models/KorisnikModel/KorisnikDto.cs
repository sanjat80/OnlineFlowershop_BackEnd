using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using System.ComponentModel.DataAnnotations;

namespace Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel
{
    public class KorisnikDto
    {
        [Key]
        public int KorisnikId { get; set; }
        public string Ime { get; set; } = null!;

        public string Prezime { get; set; } = null!;

        public string? Adresa { get; set; }

        public string BrojTelefona { get; set; } = null!;

        public string? StatusKorisnika { get; set; }

        public string? KorisnickoIme { get; set; }

        public string Email { get; set; } = null!;

        public string Lozinka { get; set; } = null!;

        public int TipId { get; set; }

        //public virtual TipKorisnika Tip { get; set; } = null!;
    }
}
