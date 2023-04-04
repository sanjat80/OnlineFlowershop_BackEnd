namespace Cvijecara_Sanja_Tica_IT80_2019.Models.StavkaKorpeModel
{
    public class StavkaKorpeCreationDto
    {
        public int ProizvodId { get; set; }

        public int KorpaId { get; set; }

        public int Kolicina { get; set; }

        public int? PorudzbinaId { get; set; }
    }
}
