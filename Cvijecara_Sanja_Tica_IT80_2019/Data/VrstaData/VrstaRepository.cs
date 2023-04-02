using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.VrstaData
{
    public class VrstaRepository:IVrstaRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;
        public static List<Vrstum> Vrste { get; set; } = new List<Vrstum>();
        public VrstaRepository(CvijecaraContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public VrstaConfirmation CreateVrsta(Vrstum vrsta)
        {
            var vrstaEntity = context.Add(vrsta);
            return mapper.Map<VrstaConfirmation>(vrstaEntity.Entity);
        }

        public void DeleteVrsta(int id)
        {
            var vrsta = GetVrstaById(id);
            context.Remove(vrsta);
        }

        public List<Vrstum> GetAllVrsta()
        {
            return context.Vrsta.ToList();
        }

        public Vrstum GetVrstaById(int id)
        {
            return context.Vrsta.FirstOrDefault(v => v.VrstaId == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateVrsta(Vrstum vrsta)
        {
            //throw new NotImplementedException();
        }
    }
}
