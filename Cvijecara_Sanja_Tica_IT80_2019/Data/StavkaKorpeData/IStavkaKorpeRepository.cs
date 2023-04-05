using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.StavkaKorpeData
{
    public interface IStavkaKorpeRepository
    {
        List<StavkaKorpe> GetAllStavkaKorpe();
        StavkaKorpe GetStavkaKorpeById(int proizvodId,int korpaId);
        StavkaKorpeConfirmation CreateStavkaKorpe(StavkaKorpe stavkaKorpe);
        void UpdateStavkaKorpe(StavkaKorpe stavkaKorpe);
        void DeleteStavkaKorpe(int proizvodId,int korpaId);
        bool SaveChanges();
    }
}
