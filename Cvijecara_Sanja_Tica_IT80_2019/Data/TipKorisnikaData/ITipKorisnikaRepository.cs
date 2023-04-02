using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.TipKorisnikaData
{
    public interface ITipKorisnikaRepository
    {
        List<TipKorisnika> GetAllTipKorisnika();
        TipKorisnika GetTipKorisnikaById(int id);
        TipKorisnikaConfirmation CreateTipKorisnika(TipKorisnika tipKorisnika);
        void UpdateTipKorisnika(TipKorisnika tipKorisnika);
        void DeleteTipKorisnika(int id);
        bool SaveChanges();
    }
}
