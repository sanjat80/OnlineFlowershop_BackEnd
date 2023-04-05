using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cvijecara_Sanja_Tica_IT80_2019.Entities;

public partial class StavkaKorpe
{
    [Key, Column(Order = 0)]
    public int ProizvodId { get; set; }
    [Key,Column(Order = 1)]
    public int KorpaId { get; set; }

    public int Kolicina { get; set; }
    [ForeignKey("Porudzbina")]
    public int? PorudzbinaId { get; set; }

    public virtual Korpa Korpa { get; set; } = null!;
    public virtual Porudzbina? Porudzbina { get; set; }

    public virtual Proizvod Proizvod { get; set; } = null!;
}
