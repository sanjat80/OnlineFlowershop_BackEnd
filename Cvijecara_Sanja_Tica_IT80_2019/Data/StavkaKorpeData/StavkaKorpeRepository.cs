using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.ProizvodModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.StavkaKorpeModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.StavkaKorpeData
{
    public class StavkaKorpeRepository : IStavkaKorpeRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;
        private IHttpContextAccessor httpContextAccessor;
        public StavkaKorpeRepository(CvijecaraContext context,IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            this.context = context;
            this.mapper = mapper;
            this.httpContextAccessor = httpContextAccessor;
        }
        public StavkaKorpeConfirmation CreateStavkaKorpe(StavkaKorpe stavkaKorpe)
        {
            var stavkaKorpeEntity = context.Add(stavkaKorpe);
            return mapper.Map<StavkaKorpeConfirmation>(stavkaKorpeEntity.Entity);
        }

        public void DeleteStavkaKorpe(int proizvodId,int korpaId)
        {
            var stavkaKorpe = GetStavkaKorpeById(proizvodId, korpaId);
            context.Remove(stavkaKorpe);
        }

        public List<StavkaKorpe> GetAllStavkaKorpe()
        {
            return context.StavkaKorpes.ToList();
        }

        public StavkaKorpe GetStavkaKorpeById(int proizvodId, int korpaId)
        {
            return context.StavkaKorpes.Find(proizvodId, korpaId);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateStavkaKorpe(StavkaKorpe stavkaKorpe)
        {
            //throw new NotImplementedException();
        }

        /*public void AddStavkaKorpeToKorpa(int proizvodId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            Korpa korpa = new Korpa();
            var kupac = context.Korisniks.Where(k => k.KorisnickoIme == username).FirstOrDefault();
            var krp = context.Korpas.FirstOrDefault(x => x.KorisnikId == kupac.KorisnikId);
            int korpaId = krp.KorpaId;
            var stavka = new StavkaKorpe
            {
                KorpaId = korpaId,
                ProizvodId = proizvodId,
                Kolicina = 1,
                PorudzbinaId = null
            };
            context.Add(stavka);
        }*/
        public void AddStavkaKorpeToUKorpa(int proizvodId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            var kupac = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == username);

            // Get the current user's basket
            var korpa = context.Korpas.Include(k => k.StavkaKorpes)
                                       .FirstOrDefault(k => k.KorisnikId == kupac.KorisnikId);

            // Check if the product already exists in the basket
            var existingStavka = korpa.StavkaKorpes.FirstOrDefault(s => s.ProizvodId == proizvodId);

            if (existingStavka != null)
            {
                existingStavka.Kolicina++;
            }
            else
            {
                var stavka = new StavkaKorpe
                {
                    KorpaId = korpa.KorpaId,
                    ProizvodId = proizvodId,
                    Kolicina = 1,
                    PorudzbinaId = null
                };
                context.Add(stavka);
            }

            context.SaveChanges();
        }

        /*public void RemoveItemFromCurrentKorpa(int proizvodId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            var kupac = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == username);

            // Get the current user's basket
            var korpa = context.Korpas.Include(k => k.StavkaKorpes)
                                       .FirstOrDefault(k => k.KorisnikId == kupac.KorisnikId);
            int korpaId = korpa.KorpaId;
            DeleteStavkaKorpe(proizvodId, korpaId);
        }*/
        public void RemoveItemFromCurrentKorpa(int proizvodId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            var kupac = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == username);

            // Get the current user's basket
            var korpa = context.Korpas.Include(k => k.StavkaKorpes)
                                       .FirstOrDefault(k => k.KorisnikId == kupac.KorisnikId);
            int korpaId = korpa.KorpaId;

            // Find the item to remove
            var stavkaKorpe = korpa.StavkaKorpes.FirstOrDefault(s => s.ProizvodId == proizvodId);

            if (stavkaKorpe != null)
            {
                // Remove the item from the basket
                korpa.StavkaKorpes.Remove(stavkaKorpe);
                context.SaveChanges();
            }
        }
        public StavkeKorpeByKorpaId ChangeKolicinaOrDelete(int proizvodId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            var kupac = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == username);

            // Get the current user's basket
            var korpa = context.Korpas.Include(k => k.StavkaKorpes)
                                       .FirstOrDefault(k => k.KorisnikId == kupac.KorisnikId);

            // Check if the product already exists in the basket
            var existingStavka = korpa.StavkaKorpes.FirstOrDefault(s => s.ProizvodId == proizvodId);
            if(existingStavka.Kolicina == 1)
            {
                DeleteStavkaKorpe(proizvodId, existingStavka.KorpaId);
                context.SaveChanges();
                return mapper.Map<StavkeKorpeByKorpaId>(existingStavka);
            }
            else
            {
                existingStavka.Kolicina--;
                context.SaveChanges();
                return mapper.Map<StavkeKorpeByKorpaId>(existingStavka);
            }
        }
        public StavkeKorpeByKorpaId ChangeKolicina(int proizvodId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            var kupac = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == username);

            // Get the current user's basket
            var korpa = context.Korpas.Include(k => k.StavkaKorpes)
                                       .FirstOrDefault(k => k.KorisnikId == kupac.KorisnikId);
            // Check if the product already exists in the basket
            int korpaId = korpa.KorpaId;
            var stavka = context.StavkaKorpes.FirstOrDefault(s => s.KorpaId == korpaId && s.ProizvodId == proizvodId);
            var proizvod = context.Proizvods.FirstOrDefault(p => p.ProizvodId == proizvodId);
            if(stavka.Kolicina>1)
            {
                stavka.Kolicina--;
                var skorpe = new StavkeKorpeByKorpaId
                {
                    Naziv = proizvod.Naziv,
                    Kolicina = stavka.Kolicina,
                    Cijena = (double)(proizvod.Cijena * stavka.Kolicina),
                    ProizvodId = proizvodId
                };
                context.SaveChanges();
                return skorpe;
            }
            else
            {
                RemoveItemFromCurrentKorpa(proizvodId);
                context.SaveChanges();
                return mapper.Map<StavkeKorpeByKorpaId>(stavka);
            }
        }
        /*public StavkeKorpeByKorpaId AddStavkaKorpeToUKorpaForPlus(int proizvodId)
        {
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
                string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
                var kupac = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == username);

                // Get the current user's basket
                var korpa = context.Korpas.Include(k => k.StavkaKorpes)
                                           .FirstOrDefault(k => k.KorisnikId == kupac.KorisnikId);
                // Check if the product already exists in the basket
                int korpaId = korpa.KorpaId;
                var stavka = context.StavkaKorpes.FirstOrDefault(s => s.KorpaId == korpaId && s.ProizvodId == proizvodId);
                var proizvod = context.Proizvods.FirstOrDefault(p => p.ProizvodId == proizvodId);
                stavka.Kolicina++;
                var skorpe = new StavkeKorpeByKorpaId
                {
                     Naziv = proizvod.Naziv,
                     Kolicina = stavka.Kolicina,
                     Cijena = (double)(proizvod.Cijena * stavka.Kolicina),
                     ProizvodId = proizvodId
                };
                context.SaveChanges();
                return skorpe;
        }*/
        public StavkeKorpeByKorpaId AddStavkaKorpeToUKorpaForPlus(DodajProizvod proizvodId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            var kupac = context.Korisniks.FirstOrDefault(k => k.KorisnickoIme == username);

            // Get the current user's basket
            var korpa = context.Korpas.Include(k => k.StavkaKorpes)
                                       .FirstOrDefault(k => k.KorisnikId == kupac.KorisnikId);
            // Check if the product already exists in the basket
            int korpaId = korpa.KorpaId;
            int prId = proizvodId.ProizvodId;
            var stavka = context.StavkaKorpes.FirstOrDefault(s => s.KorpaId == korpaId && s.ProizvodId == prId);
            var proizvod = context.Proizvods.FirstOrDefault(p => p.ProizvodId == prId);
            stavka.Kolicina++;
            var skorpe = new StavkeKorpeByKorpaId
            {
                Naziv = proizvod.Naziv,
                Kolicina = stavka.Kolicina,
                Cijena = (double)(proizvod.Cijena * stavka.Kolicina),
                ProizvodId = prId
            };
            context.SaveChanges();
            return skorpe;
        }

        public void UpdatePorudzbinaOnStavke()
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", ""));
            string username = token.Claims.FirstOrDefault(x => x.Type == "unique_name")?.Value;
            var kupac = context.Korisniks.Where(k => k.KorisnickoIme == username).FirstOrDefault();
            var existingKorpa = context.Korpas.FirstOrDefault(k => k.KorisnikId == kupac.KorisnikId);
            int korpaId = existingKorpa.KorpaId;
            var lastPorudzbina = context.Porudzbinas.OrderByDescending(p => p.PorudzbinaId).FirstOrDefault();
            int porudzbinaId = lastPorudzbina.PorudzbinaId;
            var  stavke = context.StavkaKorpes.Where(sk => sk.KorpaId == korpaId);
            foreach(StavkaKorpe stavka in stavke)
            {
                stavka.PorudzbinaId = porudzbinaId;
            }
            context.SaveChanges();
            
        }

    }
}
