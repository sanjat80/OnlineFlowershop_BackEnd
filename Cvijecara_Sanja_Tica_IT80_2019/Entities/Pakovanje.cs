using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cvijecara_Sanja_Tica_IT80_2019.Entities;

public partial class Pakovanje
{
    [Key]
    public int PakovanjeId { get; set; }

    public string Vrsta { get; set; } = null!;

    public decimal Cijena { get; set; }

    [MaxLength(5)]
    public string Valuta { get; set; } = null!;

    public string? Ukrasi { get; set; }

    public string? Posveta { get; set; }

    public virtual ICollection<Proizvod> Proizvods { get; } = new List<Proizvod>();
}
