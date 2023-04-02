using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.TransakcijaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.VrstaModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Profiles
{
    public class TransakcijaProfile:Profile
    {
        public TransakcijaProfile()
        {
            CreateMap<Transakcija, TransakcijaDto>();
            CreateMap<TransakcijaUpdateDto, Transakcija>();
            CreateMap<TransakcijaCreationDto, Transakcija>();
            CreateMap<Transakcija,Transakcija>();
            CreateMap<TransakcijaConfirmation, Transakcija>();
            CreateMap<Transakcija, TransakcijaConfirmation>();
            CreateMap<Transakcija, TransakcijaConfirmationDto>();
            CreateMap<TransakcijaConfirmationDto, Transakcija>();
            CreateMap<TransakcijaConfirmation, TransakcijaConfirmationDto>();
        }
    }
}
