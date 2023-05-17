using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using System.ComponentModel.DataAnnotations;

namespace Cvijecara_Sanja_Tica_IT80_2019.Models.PorudzbinaModel
{
    public class PorudzbinaDto
    {
        [Key]
        public int PorudzbinaId { get; set; }
        public string RedniBroj { get; set; } = null!;

        public DateTime DatumKreiranja { get; set; }

        public string StatusPorudzbine { get; set; } = null!;

        public decimal? Racun { get; set; }

        public decimal? Popust { get; set; }

        //public virtual ICollection<StavkaKorpe> StavkaKorpes { get; } = new List<StavkaKorpe>();
    }
}
