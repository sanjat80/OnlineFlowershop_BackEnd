using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cvijecara_Sanja_Tica_IT80_2019.Entities;

public partial class Proizvod
{
    [Key]
    public int ProizvodId { get; set; }

    public string Naziv { get; set; } = null!;

    public decimal? Cijena { get; set; }

    [MaxLength(5)]
    public string Valuta { get; set; } = null!;

    public string? Velicina { get; set; }

    public decimal Zalihe { get; set; }
    [ForeignKey("Pakovanje")]
    public int? PakovanjeId { get; set; }
    [ForeignKey("Kategorija")]
    public int KategorijaId { get; set; }
    [ForeignKey("Vrstum")]
    public int VrstaId { get; set; }

    public virtual Kategorija Kategorija { get; set; } = null!;

    public virtual Pakovanje? Pakovanje { get; set; }

    public virtual ICollection<StavkaKorpe> StavkaKorpes { get; } = new List<StavkaKorpe>();

    public virtual Vrstum Vrsta { get; set; } = null!;
}
