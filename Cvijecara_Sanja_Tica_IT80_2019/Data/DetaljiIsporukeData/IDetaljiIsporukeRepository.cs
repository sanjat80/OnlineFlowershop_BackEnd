using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.DetaljiIsporukeModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.DetaljiIsporukeData
{
    public interface IDetaljiIsporukeRepository
    {
        List<DetaljiIsporuke> GetAllDetaljiIsporuke();
        DetaljiIsporuke GetDetaljiIsporukeById(int id);
        DetaljiIsporukeConfirmation CreateDetaljiIsporuke(DetaljiIsporuke detaljiIsporuke);
        void UpdateDetaljiIsporuke(DetaljiIsporuke detaljiIsporuke);
        void DeleteDetaljiIsporuke(int id);
        bool SaveChanges();
        List<int> GetAllPorudzbinaId();
        DetaljiIsporukeDto CreateDetaljiForKorpa(DetaljiIsporukePorudzbinaDto detaljiIsporuke);

    }
}
