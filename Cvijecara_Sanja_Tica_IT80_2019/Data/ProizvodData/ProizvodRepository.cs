using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Extensions;
using Cvijecara_Sanja_Tica_IT80_2019.Models.ProizvodModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.ProizvodData
{
    public class ProizvodRepository : IProizvodRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;
        public static List<Proizvod> Proizvodi { get; set; } = new List<Proizvod>();
        public ProizvodRepository(CvijecaraContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public ProizvodConfirmation CreateProizvod(Proizvod proizvod)
        {
            var proizvodEntity = context.Add(proizvod);
            return mapper.Map<ProizvodConfirmation>(proizvodEntity.Entity);
        }

        public void DeleteProizvod(int id)
        {
            var proizvod = GetProizvodById(id);
            context.Remove(proizvod);
        }

        public List<Proizvod> GetAllProizvod(string? orderBy, string? searchTerm, string? kategorija,string? vrsta)
        {
            CvijecaraContext context = new CvijecaraContext();
            ProizvodExtension.Initialize(context);
            var query = context.Proizvods
                .Sort(orderBy)
                .Search(searchTerm)
                .Filter(kategorija, vrsta)
                .AsQueryable();

            return query.ToList();
        }

        public Proizvod GetProizvodById(int id)
        {
            return context.Proizvods.FirstOrDefault(p => p.ProizvodId==id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdateProizvod(Proizvod proizvod)
        {
            //throw new NotImplementedException();
        }

        public ProizvodFrontDto GetProizvodByIdOnFront(int id)
        {
            var proizvod = context.Proizvods.FirstOrDefault(p => p.ProizvodId == id);

            if (proizvod != null)
            {
                var kategorija = context.Kategorijas.FirstOrDefault(k => k.KategorijaId == proizvod.KategorijaId);
                var pakovanje = context.Pakovanjes.FirstOrDefault(p => p.PakovanjeId == proizvod.PakovanjeId);
                var vrsta = context.Vrsta.FirstOrDefault(v => v.VrstaId == proizvod.VrstaId);

                ProizvodFrontDto proizvodi = new ProizvodFrontDto
                {
                    ProizvodId = id,
                    Naziv = proizvod.Naziv,
                    Cijena = proizvod.Cijena,
                    Valuta = proizvod.Valuta,
                    Velicina = proizvod.Velicina,
                    Zalihe = proizvod.Zalihe,
                    Pakovanje = pakovanje.Vrsta,
                    Kategorija = kategorija.Naziv,
                    Vrsta = vrsta.Naziv
                };
                return proizvodi;
            }

            return null;
        }

        public (List<string>kategorije, List<string> vrste) GetFilters()
        {
            CvijecaraContext context1 = new CvijecaraContext();
            CvijecaraContext context2 = new CvijecaraContext();
            var kategorijaIds = context.Proizvods.Select(p => p.KategorijaId).Distinct().ToList();
            var vrstaIds = context.Proizvods.Select(p => p.VrstaId).Distinct().ToList();

            var kategorije = context1.Kategorijas
                .Where(k => kategorijaIds.Contains(k.KategorijaId))
                .Select(k => k.Naziv)
                .ToList();

            var vrste = context2.Vrsta
                .Where(v => vrstaIds.Contains(v.VrstaId))
                .Select(v => v.Naziv)
                .ToList();

            return (kategorije, vrste);

        }
    }
}
