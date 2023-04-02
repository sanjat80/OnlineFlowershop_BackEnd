﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cvijecara_Sanja_Tica_IT80_2019.Entities;

public partial class Kategorija
{
    [Key]
    public int KategorijaId { get; set; }

    public string Naziv { get; set; } = null!;

    public string? Opis { get; set; }

    public virtual ICollection<Proizvod> Proizvods { get; } = new List<Proizvod>();
}
