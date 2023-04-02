using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cvijecara_Sanja_Tica_IT80_2019.Entities;

public partial class Porudzbina
{
    [Key]
    public int PorudzbinaId { get; set; }

    public string RedniBroj { get; set; } = null!;

    public DateTime DatumKreiranja { get; set; }

    public string StatusPorudzbine { get; set; } = null!;

    public decimal? Racun { get; set; }

    public decimal? Popust { get; set; }

    public virtual ICollection<StavkaKorpe> StavkaKorpes { get; } = new List<StavkaKorpe>();
}
