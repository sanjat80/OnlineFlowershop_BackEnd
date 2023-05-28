using System.ComponentModel.DataAnnotations;

namespace Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel
{
    public class KorisnikAdminUpdate
    {
        [Key]
        public int KorisnikId { get; set; }
        public string StatusKorisnika { get; set; }
        public int TipId { get; set; }
    }
}
