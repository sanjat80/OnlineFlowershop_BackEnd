using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Models.PakovanjeModel
{
    public class PakovanjeConfirmationDto
    {
        public int PakovanjeId { get; set; }

        public string Vrsta { get; set; } = null!;

        public decimal Cijena { get; set; }

        public string Valuta { get; set; } = null!;

    }
}
