using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.DetaljiIsporukeData
{
    public class DetaljiIsporukeRepository : IDetaljiIsporukeRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;
        public static List<DetaljiIsporuke> DetaljIsporuke { get; set; } = new List<DetaljiIsporuke>();

        public DetaljiIsporukeRepository(CvijecaraContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public DetaljiIsporukeConfirmation CreateDetaljiIsporuke(DetaljiIsporuke detaljiIsporuke)
        {
            var detaljiEntitet=context.Add(detaljiIsporuke);
            return mapper.Map<DetaljiIsporukeConfirmation>(detaljiEntitet.Entity);
        }

        public void DeleteDetaljiIsporuke(int id)
        {
            var detaljiIsp = GetDetaljiIsporukeById(id);
            context.Remove(detaljiIsp);
        }

        public List<DetaljiIsporuke> GetAllDetaljiIsporuke()
        {
            return context.DetaljiIsporukes.ToList();
        }

        public DetaljiIsporuke GetDetaljiIsporukeById(int id)
        {
            return context.DetaljiIsporukes.FirstOrDefault(di => di.IsporukaId == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateDetaljiIsporuke(DetaljiIsporuke detaljiIsporuke)
        {
            //throw new NotImplementedException();
        }
    }
}
