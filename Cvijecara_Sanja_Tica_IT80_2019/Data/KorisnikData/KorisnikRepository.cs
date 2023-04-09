using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.KorisnikData
{
    public class KorisnikRepository : IKorisnikRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;

        public static List<Korisnik> Korisnici { get; set; } = new List<Korisnik>();

        public KorisnikRepository(CvijecaraContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public KorisnikConfirmation CreateKorisnik(Korisnik korisnik)
        {
            var korisnikEntity = context.Add(korisnik);
            return mapper.Map<KorisnikConfirmation>(korisnikEntity.Entity);
        }

        public void DeleteKorisnik(int id)
        {
            var korisnik = GetKorisnikById(id);
            context.Remove(korisnik);
        }

        public List<Korisnik> GetAllKorisnik()
        {
            return context.Korisniks.Include(t=> t.Tip).ToList();
        }

        public Korisnik GetKorisnikById(int id)
        {
            return context.Korisniks.FirstOrDefault(k => k.KorisnikId == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateKorisnik(Korisnik korisnik)
        {
            //throw new NotImplementedException();
        }

        public Korisnik GetKorisnikByKorisnickoIme(string korisnickoIme)
        {
            return context.Korisniks.FirstOrDefault(ki => ki.KorisnickoIme == korisnickoIme);
        }

        /*public string GetTipKorisnikaByTipId()
        {
            var tip = 
        }*/
    }
}
