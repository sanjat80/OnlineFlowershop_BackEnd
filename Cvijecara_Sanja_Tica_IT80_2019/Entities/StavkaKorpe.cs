using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cvijecara_Sanja_Tica_IT80_2019.Entities;

public partial class StavkaKorpe
{
    [Key]
    public int ProizvodId { get; set; }

    public int KorpaId { get; set; }

    public int Kolicina { get; set; }

    public int? PorudzbinaId { get; set; }

    public virtual Korpa Korpa { get; set; } = null!;

    public virtual Porudzbina? Porudzbina { get; set; }

    public virtual Proizvod Proizvod { get; set; } = null!;
}
