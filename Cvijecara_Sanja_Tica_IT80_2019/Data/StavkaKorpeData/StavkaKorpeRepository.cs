using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.StavkaKorpeData
{
    public class StavkaKorpeRepository : IStavkaKorpeRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;

        public StavkaKorpeRepository(CvijecaraContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public StavkaKorpeConfirmation CreateStavkaKorpe(StavkaKorpe stavkaKorpe)
        {
            var stavkaKorpeEntity = context.Add(stavkaKorpe);
            return mapper.Map<StavkaKorpeConfirmation>(stavkaKorpeEntity.Entity);
        }

        public void DeleteStavkaKorpe(int id)
        {
            var stavkaKorpe = GetStavkaKorpeById(id);
            context.Remove(stavkaKorpe);
        }

        public List<StavkaKorpe> GetAllStavkaKorpe()
        {
            return context.StavkaKorpes.ToList();
        }

        public StavkaKorpe GetStavkaKorpeById(int id)
        {
            return context.StavkaKorpes.FirstOrDefault(sk => sk.KorpaId == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateStavkaKorpe(StavkaKorpe stavkaKorpe)
        {
            //throw new NotImplementedException();
        }
    }
}
