using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.ProizvodData
{
    public interface IProizvodRepository
    {
        List<Proizvod> GetAllProizvod();
        Proizvod GetProizvodById(int id);
        ProizvodConfirmation CreateProizvod(Proizvod proizvod);
        void UpdateProizvod(Proizvod proizvod);
        void DeleteProizvod(int id);
        bool SaveChanges();
    }
}
