using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.PakovanjeModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.PorudzbinaModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Profiles
{
    public class PorudzbinaProfile:Profile
    {
        public PorudzbinaProfile()
        {
            CreateMap<Porudzbina, PorudzbinaDto>();
            CreateMap<PorudzbinaUpdateDto, Porudzbina>();
            CreateMap<PorudzbinaCreationDto, Porudzbina>();
            CreateMap<Porudzbina, PorudzbinaDto>();
            CreateMap<PorudzbinaDto, Porudzbina>();
            CreateMap<PorudzbinaConfirmation, Porudzbina>();
            CreateMap<Porudzbina, PorudzbinaConfirmation>();
            CreateMap<Porudzbina, PorudzbinaConfirmationDto>();
            CreateMap<PorudzbinaConfirmationDto, Porudzbina>();
            CreateMap<PorudzbinaConfirmation, PorudzbinaConfirmationDto>();
        }
    }
}
