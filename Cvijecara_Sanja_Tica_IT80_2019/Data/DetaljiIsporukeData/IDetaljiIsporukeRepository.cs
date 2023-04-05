using Cvijecara_Sanja_Tica_IT80_2019.Entities;

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
        public List<int> GetAllPorudzbinaId();
    }
}
