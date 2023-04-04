using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.KorpaData
{
    public class KorpaRepository : IKorpaRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;
        public static List<Korpa> Korpe { get; set; } = new List<Korpa>();
        public KorpaRepository(CvijecaraContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public KorpaConfirmation CreateKorpa(Korpa korpa)
        {
            var korpaEntitet = context.Add(korpa);
            return mapper.Map<KorpaConfirmation>(korpaEntitet.Entity);
        }

        public void DeleteKorpa(int id)
        {
            var korpa = GetKorpaById(id);
            context.Remove(korpa);
        }

        public List<Korpa> GetAllKorpa()
        {
            return context.Korpas.ToList();
        }

        public Korpa GetKorpaById(int id)
        {
            return context.Korpas.FirstOrDefault(k => k.KorpaId == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges()>0;
        }

        public void UpdateKorpa(Korpa korpa)
        {
           // throw new NotImplementedException();
        }
    }
}
