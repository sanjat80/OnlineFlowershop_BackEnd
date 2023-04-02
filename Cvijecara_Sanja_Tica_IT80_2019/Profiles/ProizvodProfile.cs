using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.ProizvodModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Profiles
{
    public class ProizvodProfile:Profile
    {
        public ProizvodProfile()
        {
            CreateMap<Proizvod, ProizvodDto>();
            CreateMap<ProizvodDto, Proizvod>();
            CreateMap<ProizvodUpdateDto, Proizvod>();
            CreateMap<ProizvodCreationDto, Proizvod>();
            CreateMap<Proizvod,Proizvod>();
            CreateMap<ProizvodConfirmation, Proizvod>();
            CreateMap<Proizvod, ProizvodConfirmation>();
            CreateMap<Proizvod, ProizvodConfirmationDto>();
            CreateMap<ProizvodConfirmationDto, Proizvod>();
            CreateMap<ProizvodConfirmation, ProizvodConfirmationDto>();
        }
    }
}
