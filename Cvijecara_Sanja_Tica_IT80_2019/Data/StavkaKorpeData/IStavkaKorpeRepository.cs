using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.StavkaKorpeData
{
    public interface IStavkaKorpeRepository
    {
        List<StavkaKorpe> GetAllStavkaKorpe();
        StavkaKorpe GetStavkaKorpeById(int id);
        StavkaKorpeConfirmation CreateStavkaKorpe(StavkaKorpe stavkaKorpe);
        void UpdateStavkaKorpe(StavkaKorpe stavkaKorpe);
        void DeleteStavkaKorpe(int id);
        bool SaveChanges();
    }
}
