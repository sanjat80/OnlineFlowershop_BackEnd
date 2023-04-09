﻿using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Models.PakovanjeModel
{
    public class PakovanjeDto
    {
        public string Vrsta { get; set; } = null!;

        public decimal Cijena { get; set; }

        public string Valuta { get; set; } = null!;

        public string? Ukrasi { get; set; }

        public string? Posveta { get; set; }

        //public virtual ICollection<Proizvod> Proizvods { get; } = new List<Proizvod>();
    }
}
