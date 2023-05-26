using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Cvijecara_Sanja_Tica_IT80_2019.Extensions
{
    public static class ProizvodExtension
    {

        private static CvijecaraContext _context;

        public static void Initialize(CvijecaraContext context)
        {
            _context = context;
        }

        public static IQueryable<Proizvod> Sort(this IQueryable<Proizvod> query, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.Cijena);

            query = orderBy switch
            {
                "cijena" => query.OrderBy(p => p.Cijena),
                "cijenaDesc" => query.OrderByDescending(p => p.Cijena),
                _ => query.OrderBy(p => p.Naziv)
            };

            return query;
        }

        public static IQueryable<Proizvod> Search(this IQueryable<Proizvod> query, string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return query;
            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Naziv.ToLower().Contains(lowerCaseSearchTerm));
        }

        /*public static IQueryable<Proizvod> Filter(this IQueryable<Proizvod> query, string kategorija, string vrsta)
        {
            var filteredQuery = query;

            if (!string.IsNullOrEmpty(kategorija))
            {
                var kategorijaId = _context.Kategorijas.FirstOrDefault(k => k.Naziv.ToLower() == kategorija.ToLower())?.KategorijaId;
                if (kategorijaId != null)
                {
                    filteredQuery = filteredQuery.Where(p => p.KategorijaId == kategorijaId);
                }
            }

            if (!string.IsNullOrEmpty(vrsta))
            {
                var vrstaId = _context.Vrsta.FirstOrDefault(v => v.Naziv.ToLower() == vrsta.ToLower())?.VrstaId;
                if (vrstaId != null)
                {
                    filteredQuery = filteredQuery.Where(p => p.VrstaId == vrstaId);
                }
            }

            return filteredQuery;
        }*/
        public static IQueryable<Proizvod> Filter(this IQueryable<Proizvod> query, string kategorija, string vrsta)
        {
            var filteredQuery = query;

            if (!string.IsNullOrEmpty(kategorija))
            {
                using (var kategorijaContext = new CvijecaraContext())
                {
                    var kategorijaId = kategorijaContext.Kategorijas.FirstOrDefault(k => k.Naziv.ToLower() == kategorija.ToLower())?.KategorijaId;
                    if (kategorijaId != null)
                    {
                        filteredQuery = filteredQuery.Where(p => p.KategorijaId == kategorijaId);
                    }
                }
            }

            if (!string.IsNullOrEmpty(vrsta))
            {
                using (var vrstaContext = new CvijecaraContext())
                {
                    var vrstaId = vrstaContext.Vrsta.FirstOrDefault(v => v.Naziv.ToLower() == vrsta.ToLower())?.VrstaId;
                    if (vrstaId != null)
                    {
                        filteredQuery = filteredQuery.Where(p => p.VrstaId == vrstaId);
                    }
                }
            }

            return filteredQuery;
        }

    }
}
