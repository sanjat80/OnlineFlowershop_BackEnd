using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorpaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.StavkaKorpeModel;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.KorpaData
{
    public interface IKorpaRepository
    {
        List<Korpa> GetAllKorpa();
        Korpa GetKorpaById(int id);
        KorpaConfirmation CreateKorpa(Korpa korpa);
        void UpdateKorpa(Korpa korpa);
        void DeleteKorpa(int id);
        bool SaveChanges();
        List<string> GetStavkeKorpeByKorpaId(int korpaId);
        List<int> GetAllKorisnikId();
        IznosKolicinaKorpeDto GetIznosAndKolicinaByKorpaId(int korpaId);
        //public Korpa GetKorpaWithCookies();
        Korpa GetKorpaFromToken();
        KorpaConfirmation CreateKorpaForNewUser();
        KorpaDto CreateKorpaForNonLoggedUser();
        List<StavkeKorpeByKorpaId> GetStavkeKorpeByKorpa();
        KorpaDto GetKorpaFromCurrUser();
        int GetKorpaFromCurrentUser();
        Korpa GetKorpaFromLoggedUser();
        decimal CalculateKorpaAmount();
        decimal UpdateKorpaDetails(int korpaId);
    }
}
