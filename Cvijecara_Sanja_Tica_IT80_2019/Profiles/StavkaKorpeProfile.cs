using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.StavkaKorpeModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Profiles
{
    public class StavkaKorpeProfile:Profile
    {
        public StavkaKorpeProfile()
        {
            CreateMap<StavkaKorpe, StavkaKorpeDto>();
            CreateMap<StavkaKorpeDto, StavkaKorpe>();
            CreateMap<StavkaKorpeUpdateDto, Korpa>();
            CreateMap<StavkaKorpeCreationDto, StavkaKorpe>();
            CreateMap<StavkaKorpe, StavkaKorpe>();
            CreateMap<StavkaKorpeConfirmation, StavkaKorpe>();
            CreateMap<StavkaKorpe, StavkaKorpeConfirmation>();
            CreateMap<StavkaKorpe, StavkaKorpeConfirmationDto>();
            CreateMap<StavkaKorpeConfirmationDto, StavkaKorpe>();
            CreateMap<StavkaKorpeConfirmation, StavkaKorpeConfirmationDto>();
        }
    }
}
