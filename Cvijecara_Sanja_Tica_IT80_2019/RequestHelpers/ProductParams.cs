using System.Drawing;

namespace Cvijecara_Sanja_Tica_IT80_2019.RequestHelpers
{
    public class ProductParams :PaginationParams
    {

        public string OrderBy { get; set; }
        public string SearchTerm { get; set; }
        public string Kategorija { get; set; }
        public string Vrsta { get; set; }

    }
}
