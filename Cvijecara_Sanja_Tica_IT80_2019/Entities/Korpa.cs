using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cvijecara_Sanja_Tica_IT80_2019.Entities;

public partial class Korpa
{
    [Key]
    public int KorpaId { get; set; }

    public int Kolicina { get; set; }

    public decimal UkupanIznos { get; set; }
    [MaxLength(5)]
    public string Valuta { get; set; } = null!;

    public int KorisnikId { get; set; }

    public virtual ICollection<StavkaKorpe> StavkaKorpes { get; } = new List<StavkaKorpe>();
}
