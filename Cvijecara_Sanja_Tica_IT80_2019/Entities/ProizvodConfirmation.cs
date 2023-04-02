using System.ComponentModel.DataAnnotations.Schema;

namespace Cvijecara_Sanja_Tica_IT80_2019.Entities
{
    public class ProizvodConfirmation
    {
        public int ProizvodId { get; set; }

        public string Naziv { get; set; } = null!;

        public decimal? Cijena { get; set; }

        public string Valuta { get; set; } = null!;

    }
}
