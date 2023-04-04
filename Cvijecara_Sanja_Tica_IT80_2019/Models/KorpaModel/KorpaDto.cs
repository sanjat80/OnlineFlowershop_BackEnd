using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Models.KorpaModel
{
    public class KorpaDto
    {

        public int Kolicina { get; set; }

        public decimal UkupanIznos { get; set; }

        public string Valuta { get; set; } = null!;

        public int KorisnikId { get; set; }

        //public virtual ICollection<StavkaKorpe> StavkaKorpes { get; } = new List<StavkaKorpe>();
    }
}
