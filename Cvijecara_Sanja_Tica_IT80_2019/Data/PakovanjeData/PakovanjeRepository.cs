using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using System.Security.Principal;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.PakovanjeData
{
    public class PakovanjeRepository:IPakovanjeRepository
    {
        private readonly CvijecaraContext context;
        private readonly IMapper mapper;

        public static List<Pakovanje> Pakovanja { get; set; } = new List<Pakovanje>();

        public PakovanjeRepository(CvijecaraContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public PakovanjeConfirmation CreatePakovanje(Pakovanje pakovanje)
        {
            var pakovanjeEntity = context.Add(pakovanje);
            return mapper.Map<PakovanjeConfirmation>(pakovanjeEntity.Entity);
        }

        public void DeletePakovanje(int id)
        {
            var pakovanje = GetPakovanjeById(id);
            context.Remove(pakovanje);
        }

        public List<Pakovanje> GetAllPakovanje()
        {
            return context.Pakovanjes.ToList();
        }

        public Pakovanje GetPakovanjeById(int id)
        {
            return context.Pakovanjes.FirstOrDefault(p => p.PakovanjeId == id);
        }

        public bool SaveChanges()
        {
            return context.SaveChanges() > 0;
        }

        public void UpdatePakovanje(Pakovanje pakovanje)
        {
            //throw new NotImplementedException();
        }
        public List<int> GetAllPakovanjeId()
        {
            return context.Pakovanjes.Select(p => p.PakovanjeId).ToList();
        }
    }
}
