using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.PakovanjeModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.TipKorisnikaModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Profiles.PakovanjeProfiles
{
    public class PakovanjeProfile:Profile
    {
        public PakovanjeProfile()
        {
            CreateMap<Pakovanje, PakovanjeDto>();
            CreateMap<PakovanjeUpdateDto, Pakovanje>();
            CreateMap<PakovanjeCreationDto, Pakovanje>();
            CreateMap<Pakovanje, Pakovanje>();
            CreateMap<PakovanjeConfirmation, Pakovanje>();
            CreateMap<Pakovanje, PakovanjeConfirmation>();
            CreateMap<Pakovanje, PakovanjeConfirmationDto>();
            CreateMap<PakovanjeConfirmationDto, Pakovanje>();
            CreateMap<PakovanjeConfirmation, PakovanjeConfirmationDto>();
        }
    }
}
