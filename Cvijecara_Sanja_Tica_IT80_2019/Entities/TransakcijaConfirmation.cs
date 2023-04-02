namespace Cvijecara_Sanja_Tica_IT80_2019.Entities
{
    public class TransakcijaConfirmation
    {
        public int TransakcijaId { get; set; }

        public DateTime DatumRealizacije { get; set; }

        public string NacinPlacanja { get; set; } = null!;

    }
}
