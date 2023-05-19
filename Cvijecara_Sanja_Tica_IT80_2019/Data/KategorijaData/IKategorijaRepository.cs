using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData
{
    public interface IKategorijaRepository
    {
        List<Kategorija> GetAllKategorija();
        Kategorija GetKategorijaById(int id);
        KategorijaConfirmation CreateKategorija(Kategorija kategorija);
        void UpdateKategorija(Kategorija kategorija);
        void DeleteKategorija(int id);
        bool SaveChanges();
        List<int> GetAllKategorijaId();
    }
}
