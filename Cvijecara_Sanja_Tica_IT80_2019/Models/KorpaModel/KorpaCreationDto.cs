using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using System.Drawing;

namespace Cvijecara_Sanja_Tica_IT80_2019.Models.KorpaModel
{
    public class KorpaCreationDto
    {   

        public int Kolicina { get; set; }

        public decimal UkupanIznos { get; set; }

        public string Valuta { get; set; } = null!;

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }

        public int KorisnikId { get; set; }

    }
}
