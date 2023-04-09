using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorpaModel;
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
        public List<int> GetAllKorisnikId();
        public IznosKolicinaKorpeDto GetIznosAndKolicinaByKorpaId(int korpaId);
    }
}
