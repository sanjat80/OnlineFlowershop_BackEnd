using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.KorisnikData
{
    public class KorisnikRepository : IKorisnikRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;
        private IHttpContextAccessor httpContextAccessor;

        public static List<Korisnik> Korisnici { get; set; } = new List<Korisnik>();

        public KorisnikRepository(CvijecaraContext context, IMapper mapper,IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
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

        public KorisnikDto GetCurrentUser()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            var kupac = context.Korisniks.Where(k => k.KorisnickoIme == username).FirstOrDefault(); 
            return mapper.Map<KorisnikDto>(kupac);
        }
        public KorisnikKorpaDto GetKorpaForCurrentUser()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            var kupac = context.Korisniks.Where(k => k.KorisnickoIme == username).FirstOrDefault();
            var existingKorpa = context.Korpas.FirstOrDefault(k => k.KorisnikId == kupac.KorisnikId);
            var korisnik = new KorisnikKorpaDto
            {
                Token = token.ToString(),
                KorisnickoIme = kupac.KorisnickoIme,
                KorpaId = existingKorpa.KorpaId
            };
            return korisnik;
        }

        public KorisnikUpdateRegistrationDto GetKorisnikByKorisnickoImeProfile(string korisnickoIme)
        {
            var user =  context.Korisniks.FirstOrDefault(ki => ki.KorisnickoIme == korisnickoIme);
            return mapper.Map<KorisnikUpdateRegistrationDto>(user);
        }

        public void UpdateKorisnikByAdmin(KorisnikAdminUpdate korisnik)
        {
            //
        }
        public void UpdateKorisnikByKorisnik(KorisnikUpdateRegistrationDto korisnik)
        {
            //throw new NotImplementedException();
        }


    }
}
