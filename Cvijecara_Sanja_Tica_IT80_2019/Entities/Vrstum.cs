using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cvijecara_Sanja_Tica_IT80_2019.Entities;

public partial class Vrstum
{
    [Key]
    public int VrstaId { get; set; }

    public string Naziv { get; set; } = null!;

    public virtual ICollection<Proizvod> Proizvods { get; } = new List<Proizvod>();
}
