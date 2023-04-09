using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KategorijaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Profiles
{
    public class KorisnikProfile:Profile
    {
        public KorisnikProfile()
        {
            CreateMap<Korisnik, KorisnikDto>();
            CreateMap<KorisnikDto, Korisnik>();
            CreateMap<KorisnikUpdateDto, Korisnik>();
            CreateMap<KorisnikCreationDto, Korisnik>();
            CreateMap<KorisnikRegistrationDto, Korisnik>();
            CreateMap<Korisnik, KorisnikRegistrationDto>();
            CreateMap<Korisnik, Korisnik>();
            CreateMap<KorisnikConfirmation, Korisnik>();
            CreateMap<Korisnik, KorisnikConfirmation>();
            CreateMap<Korisnik, KorisnikConfirmationDto>();
            CreateMap<KorisnikConfirmationDto, Korisnik>();
            CreateMap<KorisnikConfirmation, KorisnikConfirmationDto>();
            CreateMap<Korisnik, TipKorisnika>().ForMember(dest => dest.Tip, opt => opt.MapFrom(src => src.Tip.Tip));
            CreateMap<KorisnikUpdateRegistrationDto, Korisnik>();
            CreateMap<Korisnik, KorisnikUpdateRegistrationDto>();
        }
    }
}
