using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cvijecara_Sanja_Tica_IT80_2019.Entities;

public partial class TipKorisnika
{
    [Key]
    public int TipId { get; set; }

    public string Tip { get; set; } = null!;

    public virtual ICollection<Korisnik> Korisniks { get; } = new List<Korisnik>();
}
