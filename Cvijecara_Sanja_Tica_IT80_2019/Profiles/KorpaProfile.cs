using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorpaModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Profiles
{
    public class KorpaProfile : Profile
    {
        public KorpaProfile()
        {
            CreateMap<Korpa, KorpaDto>();
            CreateMap<KorpaDto, Korpa>();
            CreateMap<KorpaUpdateDto, Korpa>();
            CreateMap<KorpaCreationDto, Korpa>();
            CreateMap<Korpa, Korpa>();
            CreateMap<KorpaConfirmation, Korpa>();
            CreateMap<Korpa, KorpaConfirmation>();
            CreateMap<Korpa, KorpaConfirmationDto>();
            CreateMap<KorpaConfirmationDto, Korpa>();
            CreateMap<KorpaConfirmation, KorpaConfirmationDto>();
        }
    }
}
