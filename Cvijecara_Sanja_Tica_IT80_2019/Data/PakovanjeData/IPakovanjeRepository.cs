using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.PakovanjeData
{
    public interface IPakovanjeRepository
    {
        List<Pakovanje> GetAllPakovanje();
        Pakovanje GetPakovanjeById(int id);
        PakovanjeConfirmation CreatePakovanje(Pakovanje pakovanje);
        void UpdatePakovanje(Pakovanje pakovanje);
        void DeletePakovanje(int id);
        bool SaveChanges();
        List<int> GetAllPakovanjeId();
    }
}
