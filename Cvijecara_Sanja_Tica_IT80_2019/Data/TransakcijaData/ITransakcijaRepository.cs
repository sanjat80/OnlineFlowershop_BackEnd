using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.TransakcijaData
{
    public interface ITransakcijaRepository
    {
        List<Transakcija> GetAllTransakcija();
        Transakcija GetTransakcijaById(int id);
        TransakcijaConfirmation CreateTransakcija(Transakcija transakcija);
        void UpdateTransakcija(Transakcija transakcija);
        void DeleteTransakcija(int id);
        bool SaveChanges();
    }
}
