using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KategorijaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.PakovanjeModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Profiles
{
    public class KategorijaProfile:Profile
    {
        public KategorijaProfile()
        {
            CreateMap<Kategorija, KategorijaDto>();
            CreateMap<KategorijaDto, Kategorija>();
            CreateMap<KategorijaUpdateDto, Kategorija>();
            CreateMap<KategorijaCreationDto, Kategorija>();
            CreateMap<Kategorija, Kategorija>();
            CreateMap<KategorijaConfirmation, Kategorija>();
            CreateMap<Kategorija, KategorijaConfirmation>();
            CreateMap<Kategorija, KategorijaConfirmationDto>();
            CreateMap<KategorijaConfirmationDto, Kategorija>();
            CreateMap<KategorijaConfirmation, KategorijaConfirmationDto>();
        }
    }
}
