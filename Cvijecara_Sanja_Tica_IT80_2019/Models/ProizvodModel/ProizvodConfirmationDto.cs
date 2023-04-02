namespace Cvijecara_Sanja_Tica_IT80_2019.Models.ProizvodModel
{
    public class ProizvodConfirmationDto
    {
        public int ProizvodId { get; set; }

        public string Naziv { get; set; } = null!;

        public decimal? Cijena { get; set; }

        public string Valuta { get; set; } = null!;

    }
}
