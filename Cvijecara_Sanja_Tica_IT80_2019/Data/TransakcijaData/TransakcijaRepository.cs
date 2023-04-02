using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.TransakcijaData
{
    public class TransakcijaRepository : ITransakcijaRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;

        public static List<Transakcija> Transakcije { get; set; } = new List<Transakcija>();
        
        public TransakcijaRepository(CvijecaraContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        
        public TransakcijaConfirmation CreateTransakcija(Transakcija transakcija)
        {
            var transakcijaEntitet = context.Add(transakcija);
            return mapper.Map<TransakcijaConfirmation>(transakcijaEntitet.Entity);
        }

        public void DeleteTransakcija(int id)
        {
            var transakcija = GetTransakcijaById(id);
            context.Remove(transakcija);
        }

        public List<Transakcija> GetAllTransakcija()
        {
            return context.Transakcijas.ToList();
        }

        public Transakcija GetTransakcijaById(int id)
        {
            return context.Transakcijas.FirstOrDefault(t => t.TransakcijaId == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateTransakcija(Transakcija transakcija)
        {
            //throw new NotImplementedException();
        }
    }
}
