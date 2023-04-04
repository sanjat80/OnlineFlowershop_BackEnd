using Cvijecara_Sanja_Tica_IT80_2019.Entities;

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
    }
}
