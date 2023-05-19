using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.VrstaData
{
    public interface IVrstaRepository
    {
        List<Vrstum> GetAllVrsta();
        Vrstum GetVrstaById(int id);
        VrstaConfirmation CreateVrsta(Vrstum vrsta);
        void UpdateVrsta(Vrstum vrsta);
        void DeleteVrsta(int id);
        bool SaveChanges();
        List<int> GetAllVrstaId();
    }
}
