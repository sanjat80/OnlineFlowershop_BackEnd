namespace Cvijecara_Sanja_Tica_IT80_2019.Models.TransakcijaModel
{
    public class TransakcijaConfirmationDto
    {
        public int TransakcijaId { get; set; }

        public DateTime DatumRealizacije { get; set; }

        public string NacinPlacanja { get; set; } = null!;
    }
}
