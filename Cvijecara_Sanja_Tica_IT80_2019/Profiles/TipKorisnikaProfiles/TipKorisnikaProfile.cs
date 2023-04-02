using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.TipKorisnikaModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Profiles.TipKorisnikaProfiles
{
    public class TipKorisnikaProfile:Profile
    {
        public TipKorisnikaProfile()
        {
            CreateMap<TipKorisnika, TipKorisnikaDto>();
            CreateMap<TipKorisnikaUpdateDto, TipKorisnika>();
            CreateMap<TipKorisnikaCreationDto, TipKorisnika>();
            CreateMap<TipKorisnika, TipKorisnika>();
            CreateMap<TipKorisnikaConfirmation, TipKorisnika>();
            CreateMap<TipKorisnika, TipKorisnikaConfirmation>();
            CreateMap<TipKorisnika, TipKorisnikaConfirmationDto>();
            CreateMap<TipKorisnikaConfirmation, TipKorisnikaConfirmationDto>();
        }
    }
}
