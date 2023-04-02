using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.ProizvodData
{
    public class ProizvodRepository : IProizvodRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;
        public static List<Proizvod> Proizvodi { get; set; } = new List<Proizvod>();
        public ProizvodRepository(CvijecaraContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public ProizvodConfirmation CreateProizvod(Proizvod proizvod)
        {
            var proizvodEntity = context.Add(proizvod);
            return mapper.Map<ProizvodConfirmation>(proizvodEntity.Entity);
        }

        public void DeleteProizvod(int id)
        {
            var proizvod = GetProizvodById(id);
            context.Remove(proizvod);
        }

        public List<Proizvod> GetAllProizvod()
        {
            return context.Proizvods.ToList();
        }

        public Proizvod GetProizvodById(int id)
        {
            return context.Proizvods.FirstOrDefault(p => p.ProizvodId==id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateProizvod(Proizvod proizvod)
        {
            //throw new NotImplementedException();
        }
    }
}
