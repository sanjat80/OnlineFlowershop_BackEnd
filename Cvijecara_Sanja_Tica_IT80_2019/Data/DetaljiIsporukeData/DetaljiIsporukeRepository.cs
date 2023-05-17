using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.DetaljiIsporukeModel;

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
        public List<int> GetAllPorudzbinaId()
        {
            using (var context = new CvijecaraContext())
            {
                var naziviProizvoda = from p in context.Porudzbinas
                                      select p.PorudzbinaId;

                return naziviProizvoda.ToList();
            }
        }

        public DetaljiIsporukeDto CreateDetaljiForKorpa(DetaljiIsporukePorudzbinaDto detaljiIsporuke)
        {
            var lastPorudzbina = context.Porudzbinas.OrderByDescending(p => p.PorudzbinaId).FirstOrDefault();
            int porudzbinaId = lastPorudzbina.PorudzbinaId;
            var DetaljiIsporuke = new DetaljiIsporuke
            {
                Isporuceno = false,
                DatumIsporuke = DateTime.Now,
                Tip = "klasicna",
                Adresa = detaljiIsporuke.Adresa,
                BrojTelefona = detaljiIsporuke.BrojTelefona,
                Grad = detaljiIsporuke.Grad,
                Region = detaljiIsporuke.Region,
                Drzava = detaljiIsporuke.Drzava,
                PorudzbinaId = porudzbinaId
            };
            context.Add(DetaljiIsporuke);
            context.SaveChanges();
            return mapper.Map<DetaljiIsporukeDto>(DetaljiIsporuke);
        }

    }
}
