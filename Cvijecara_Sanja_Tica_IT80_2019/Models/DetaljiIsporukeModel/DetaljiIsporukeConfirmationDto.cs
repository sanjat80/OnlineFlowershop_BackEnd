namespace Cvijecara_Sanja_Tica_IT80_2019.Models.DetaljiIsporukeModel
{
    public class DetaljiIsporukeConfirmationDto
    {
        public int IsporukaId { get; set; }

        public bool Isporuceno { get; set; }

        public DateTime? DatumIsporuke { get; set; }

        public string Tip { get; set; } = null!;
    }
}
