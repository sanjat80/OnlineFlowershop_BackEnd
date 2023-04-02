using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Models.PorudzbinaModel
{
    public class PorudzbinaConfirmationDto
    {
        public int PorudzbinaId { get; set; }

        public string RedniBroj { get; set; } = null!;

        public DateTime DatumKreiranja { get; set; }

    }
}
