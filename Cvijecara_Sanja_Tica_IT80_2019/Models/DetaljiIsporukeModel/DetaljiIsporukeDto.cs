using System.ComponentModel.DataAnnotations;

namespace Cvijecara_Sanja_Tica_IT80_2019.Models.DetaljiIsporukeModel
{
    public class DetaljiIsporukeDto
    {
        [Key]
        public int IsporukaId { get; set; }
        public bool Isporuceno { get; set; }

        public DateTime? DatumIsporuke { get; set; }

        public string Tip { get; set; } = null!;

        public string Adresa { get; set; } = null!;

        public string BrojTelefona { get; set; } = null!;

        public string Grad { get; set; } = null!;

        public string? Region { get; set; }

        public string Drzava { get; set; } = null!;

        public int PorudzbinaId { get; set; }
    }
}
