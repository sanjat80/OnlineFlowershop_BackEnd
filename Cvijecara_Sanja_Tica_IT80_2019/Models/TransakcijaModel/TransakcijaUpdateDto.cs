namespace Cvijecara_Sanja_Tica_IT80_2019.Models.TransakcijaModel
{
    public class TransakcijaUpdateDto
    {
        public int TransakcijaId { get; set; }

        public DateTime DatumRealizacije { get; set; }

        public string NacinPlacanja { get; set; } = null!;

        public bool Placeno { get; set; }

        public int PorudzbinaId { get; set; }
    }
}
