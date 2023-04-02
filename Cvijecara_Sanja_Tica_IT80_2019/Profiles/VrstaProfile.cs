using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.PakovanjeModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.VrstaModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Profiles
{
    public class VrstaProfile:Profile
    {
        public VrstaProfile()
        {
            CreateMap<Vrstum, VrstaDto>();
            CreateMap<VrstaUpdateDto, Vrstum>();
            CreateMap<VrstaCreationDto, Vrstum>();
            CreateMap<Vrstum, Vrstum>();
            CreateMap<VrstaConfirmation, Vrstum>();
            CreateMap<Vrstum, VrstaConfirmation>();
            CreateMap<Vrstum, VrstaConfirmationDto>();
            CreateMap<VrstaConfirmationDto, Vrstum>();
            CreateMap<VrstaConfirmation, VrstaConfirmationDto>();
        }
    }
}
