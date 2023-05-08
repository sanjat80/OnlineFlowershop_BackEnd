using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorpaModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Cvijecara_Sanja_Tica_IT80_2019.Models.StavkaKorpeModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.KorpaData
{
    public class KorpaRepository : IKorpaRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public static List<Korpa> Korpe { get; set; } = new List<Korpa>();
        public KorpaRepository(CvijecaraContext context, IMapper mapper, IHttpContextAccessor _httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this._httpContextAccessor = _httpContextAccessor;
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
        public List<string> GetStavkeKorpeByKorpaId(int korpaId)
        {
            using (var context = new CvijecaraContext())
            {
                var naziviProizvoda = from p in context.Proizvods
                                      join sk in context.StavkaKorpes on p.ProizvodId equals sk.ProizvodId
                                      where sk.KorpaId == korpaId
                                      select p.Naziv;

                return naziviProizvoda.ToList();
            }
        }

        public List<int> GetAllKorisnikId()
        {
            using (var context = new CvijecaraContext())
            {
                var korisnici = from k in context.Korisniks
                                select k.KorisnikId;

                return korisnici.ToList();
            }
        }

        public IznosKolicinaKorpeDto GetIznosAndKolicinaByKorpaId(int korpaId)
        {
            var korpa = context.Korpas.FirstOrDefault(k => k.KorpaId == korpaId);
            var iznosKolicinaDto = new IznosKolicinaKorpeDto
            {
                UkupanIznos = korpa.UkupanIznos,
                Kolicina = korpa.Kolicina
            };
            return iznosKolicinaDto;
        }

        /*public Korpa GetKorpaWithCookies()
        {
            var korisnikIdValue = _httpContextAccessor.HttpContext.Request.Cookies["KorisnikId"];
            int korisnikId = 0;
            if (int.TryParse(korisnikIdValue, out korisnikId))
            {
                using (var cntxt = new CvijecaraContext())
                {
                     Korpa basket = cntxt.Korpas.FirstOrDefault(x => x.KorisnikId == korisnikId);
                     return basket;
                }
            }
            else
            {
                return null;
            }
        }*/

        /*public Korpa GetKorpaFromToken()
        {
            var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            var korisnikIdClaim = identity.FindFirst("KorisnikId");

            if (korisnikIdClaim != null && int.TryParse(korisnikIdClaim.Value, out int korisnikId))
            {
                using (var context = new CvijecaraContext())
                {
                    var korpa = context.Korpas.FirstOrDefault(x => x.KorisnikId.ToString() == korisnikIdClaim.Value);
                    return korpa;
                }
            }
            else
            {
                return null;
            }
        }*/
        /*public Korpa GetKorpaFromToken()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var authorizationHeader = httpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var tokenString = authorizationHeader.Substring("Bearer ".Length).Trim();
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(tokenString);
                var korisnikIdClaim = token.Claims.FirstOrDefault(x => x.Type == "KorisnikId");
                if (korisnikIdClaim != null && int.TryParse(korisnikIdClaim.Value, out int korisnikId))
                {
                    using (var context = new CvijecaraContext())
                    {
                        var korpa = context.Korpas.FirstOrDefault(x => x.KorisnikId == korisnikId);
                        return korpa;
                    }
                }
            }
            return null;
        }*/
        public Korpa GetKorpaFromToken()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            Korpa korpa = new Korpa();
            var kupac = context.Korisniks.Where(k => k.KorisnickoIme == username).FirstOrDefault();
            var krp = context.Korpas.FirstOrDefault(x => x.KorisnikId == kupac.KorisnikId);
            return krp;
        }

        public KorpaConfirmation CreateKorpaForNewUser()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(_httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            var kupac = context.Korisniks.Where(k => k.KorisnickoIme == username).FirstOrDefault();

            var existingKorpa = context.Korpas.FirstOrDefault(k => k.KorisnikId == kupac.KorisnikId);
            if (existingKorpa != null)
            {
                return mapper.Map<KorpaConfirmation>(existingKorpa);
            }

            var korpa = new Korpa
            {
                //KorpaId = GenerateNewId(),
                Kolicina = 0,
                UkupanIznos = 0,
                Valuta = "RSD",
                KorisnikId = kupac.KorisnikId
            };
            context.Korpas.Add(korpa);
            context.SaveChanges();
            return mapper.Map<KorpaConfirmation>(korpa);
        }

        /*private int GenerateNewId()
        {
            var lastRecordId = context.Korpas.OrderByDescending(x => x.KorpaId).Select(x => x.KorpaId).FirstOrDefault();
            return lastRecordId + 1;
        }*/

        public KorpaDto CreateKorpaForNonLoggedUser()
        {
            var korpa = new Korpa
            {
                //KorpaId = GenerateNewId(),
                Kolicina = 0,
                UkupanIznos = 0,
                Valuta = "RSD",
                KorisnikId = 15
            };
            context.Korpas.Add(korpa);
            context.SaveChanges();
            return mapper.Map<KorpaDto>(korpa);
        }
        private int GenerateNewKorisnikId()
        {
            Random random = new Random();
            int minValue = 1;
            int maxValue = 100;
            int randomValue = random.Next(minValue, maxValue);
            return randomValue;
        }
        public List<StavkeKorpeByKorpaId> GetStavkeKorpeByKorpa(int korpaId)
        {
            using (var context = new CvijecaraContext())
            {
                var proizvodi = from p in context.Proizvods
                                join sk in context.StavkaKorpes on p.ProizvodId equals sk.ProizvodId
                                where sk.KorpaId == korpaId
                                select new { p.Naziv, sk.Kolicina };

                var stavkeKorpe = proizvodi.Select(p => new StavkeKorpeByKorpaId
                {
                    Naziv = p.Naziv,
                    Kolicina = p.Kolicina
                }).ToList();
                return stavkeKorpe;
            }
        }
        public KorpaDto GetKorpaFromCurrUser() 
        {
            var korpa = CreateKorpaForNonLoggedUser();
            return korpa;
        }
    }
}
