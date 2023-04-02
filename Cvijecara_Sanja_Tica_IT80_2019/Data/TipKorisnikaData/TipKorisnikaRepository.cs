using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.TipKorisnikaData
{
    public class TipKorisnikaRepository:ITipKorisnikaRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;

        public static List<TipKorisnika> TipoviKorisnika { get; set; } = new List<TipKorisnika>();

        public TipKorisnikaRepository(CvijecaraContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public TipKorisnikaConfirmation CreateTipKorisnika(TipKorisnika tipKorisnika)
        {
            var tipEntity = context.Add(tipKorisnika);
            return mapper.Map<TipKorisnikaConfirmation>(tipEntity.Entity);
        }

        public void DeleteTipKorisnika(int id)
        {
            var tip = GetTipKorisnikaById(id);
            context.Remove(tip);
        }

        public List<TipKorisnika> GetAllTipKorisnika()
        {
            return context.TipKorisnikas.ToList();
        }

        public TipKorisnika GetTipKorisnikaById(int id)
        {
            return context.TipKorisnikas.FirstOrDefault(t => t.TipId == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges()>0;
        }

        public void UpdateTipKorisnika(TipKorisnika tipKorisnika)
        {
            //throw new NotImplementedException();
        }
    }
}
