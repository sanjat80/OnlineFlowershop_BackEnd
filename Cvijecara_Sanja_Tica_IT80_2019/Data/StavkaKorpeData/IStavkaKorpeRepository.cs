using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.ProizvodModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.StavkaKorpeModel;

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
        //public void AddStavkaKorpeToKorpa(int proizvodId);
        void AddStavkaKorpeToUKorpa(int proizvodId);
        void RemoveItemFromCurrentKorpa(int proizvodId);
        StavkeKorpeByKorpaId ChangeKolicinaOrDelete(int proizvodId);
        StavkeKorpeByKorpaId ChangeKolicina(int proizvodId);
        StavkeKorpeByKorpaId AddStavkaKorpeToUKorpaForPlus(DodajProizvod proizvodId);
        void UpdatePorudzbinaOnStavke();


    }
}
