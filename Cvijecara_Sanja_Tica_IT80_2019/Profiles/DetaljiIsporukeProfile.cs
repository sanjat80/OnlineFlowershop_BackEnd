using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.DetaljiIsporukeModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.TransakcijaModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Profiles
{
    public class DetaljiIsporukeProfile:Profile
    {
        public DetaljiIsporukeProfile()
        {
            CreateMap<DetaljiIsporuke, DetaljiIsporukeDto>();
            CreateMap<DetaljiIsporukeUpdateDto, DetaljiIsporuke>();
            CreateMap<DetaljiIsporukeCreationDto, DetaljiIsporuke>();
            CreateMap<DetaljiIsporuke, DetaljiIsporuke>();
            CreateMap<DetaljiIsporukeConfirmation, DetaljiIsporuke>();
            CreateMap<DetaljiIsporuke, DetaljiIsporukeConfirmation>();
            CreateMap<DetaljiIsporuke, DetaljiIsporukeConfirmationDto>();
            CreateMap<DetaljiIsporukeConfirmationDto, DetaljiIsporuke>();
            CreateMap<DetaljiIsporukeConfirmation, DetaljiIsporukeConfirmationDto>();
        }
    }
}
