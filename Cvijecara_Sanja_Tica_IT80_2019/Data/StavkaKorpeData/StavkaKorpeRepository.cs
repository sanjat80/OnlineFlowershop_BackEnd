using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.StavkaKorpeModel;
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

        public StavkaKorpeDto AddStavkaKorpeToKorpa(int proizvodId)
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
                Kolicina = 2,
                PorudzbinaId = null
            };
            context.Add(stavka);
            context.SaveChanges();
            return mapper.Map<StavkaKorpeDto>(stavka);
        }
    }
}
