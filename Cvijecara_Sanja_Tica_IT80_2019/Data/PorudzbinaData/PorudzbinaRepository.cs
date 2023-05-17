using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.PorudzbinaModel;
using Microsoft.EntityFrameworkCore;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.PorudzbinaData
{
    public class PorudzbinaRepository : IPorudzbinaRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;
        public static List<Porudzbina> Porudzbine { get; set; } = new List<Porudzbina>();
        public PorudzbinaRepository(CvijecaraContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public PorudzbinaConfirmation CreatePorudzbina(Porudzbina porudzbina)
        {
            var porudzbinaEntity = context.Add(porudzbina);
            return mapper.Map<PorudzbinaConfirmation>(porudzbinaEntity.Entity);
        }

        public void DeletePorudzbina(int id)
        {
            var porudzbina = GetPorudzbinaById(id);
            context.Remove(porudzbina);
        }

        public List<Porudzbina> GetAllPorudzbina()
        {
            return context.Porudzbinas.ToList();
        }

        public Porudzbina GetPorudzbinaById(int id)
        {
            return context.Porudzbinas.FirstOrDefault(p => p.PorudzbinaId == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdatePorudzbina(Porudzbina porudzbina)
        {
            //throw new NotImplementedException();
        }

        public decimal GetRacunPorudzbineByPorudzbinaId(int porudzbinaId)
        {
            var porudzbina = GetPorudzbinaById(porudzbinaId);

            return (decimal)porudzbina.Racun;
        }

        public decimal GetPopustNaPorudzbinuByPorudzbinaId(int porudzbinaId)
        {
            var porudzbina = GetPorudzbinaById(porudzbinaId);
            return (decimal)porudzbina.Popust;
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

        public PorudzbinaDto CreatePorudzbinaForUser()
        {
            var porudzbina = new Porudzbina
            {
                RedniBroj = GenerateRandomString(5),
                DatumKreiranja = DateTime.Now,
                StatusPorudzbine = "U obradi",
                Racun = 0,
                Popust = 0
            };
            context.Add(porudzbina);
            context.SaveChanges();
            return mapper.Map<PorudzbinaDto>(porudzbina);
        }


        public string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}


