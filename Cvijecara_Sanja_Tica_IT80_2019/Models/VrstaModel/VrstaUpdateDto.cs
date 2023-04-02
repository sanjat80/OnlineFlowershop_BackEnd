using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Models.VrstaModel
{
    public class VrstaUpdateDto
    {
        public int VrstaId { get; set; }

        public string Naziv { get; set; } = null!;

        public virtual ICollection<Proizvod> Proizvods { get; } = new List<Proizvod>();
    }
}
