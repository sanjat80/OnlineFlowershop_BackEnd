using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.ProizvodModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.ProizvodData
{
    public interface IProizvodRepository
    {
        List<Proizvod> GetAllProizvod(string? orderBy, string? searchTerm, string? kategorija, string? vrsta);
        Proizvod GetProizvodById(int id);
        ProizvodConfirmation CreateProizvod(Proizvod proizvod);
        void UpdateProizvod(Proizvod proizvod);
        void DeleteProizvod(int id);
        bool SaveChanges();
        ProizvodFrontDto GetProizvodByIdOnFront(int id);
        (List<string> kategorije, List<string> vrste) GetFilters();
    }
}
