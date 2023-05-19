using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData
{
    public class KategorijaRepository:IKategorijaRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;
        public static List<Kategorija> Kategorije { get; set; } = new List<Kategorija>();


        public KategorijaRepository(CvijecaraContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public KategorijaConfirmation CreateKategorija(Kategorija kategorija)
        {
            var kategorijaEntity = context.Add(kategorija);
            return mapper.Map<KategorijaConfirmation>(kategorijaEntity.Entity);
        }

        public void DeleteKategorija(int id)
        {
            var kategorija = GetKategorijaById(id);
            context.Remove(kategorija);
        }

        public List<Kategorija> GetAllKategorija()
        {
            return context.Kategorijas.ToList();
        }

        public Kategorija GetKategorijaById(int id)
        {
            return context.Kategorijas.FirstOrDefault(k => k.KategorijaId == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateKategorija(Kategorija kategorija)
        {
            //throw new NotImplementedException();
        }

        public List<int> GetAllKategorijaId()
        {
            return context.Kategorijas.Select(k => k.KategorijaId).ToList();
        }
    }
}
