namespace Cvijecara_Sanja_Tica_IT80_2019.Models.ProizvodModel
{
    public class ProizvodUpdateDto
    {
        public int ProizvodId { get; set; }

        public string Naziv { get; set; } = null!;

        public decimal? Cijena { get; set; }

        public string Valuta { get; set; } = null!;

        public string? Velicina { get; set; }

        public decimal Zalihe { get; set; }

        public int? PakovanjeId { get; set; }

        public int KategorijaId { get; set; }

        public int VrstaId { get; set; }

    }
}
