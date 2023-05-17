using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.PorudzbinaModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.PorudzbinaData
{
    public interface IPorudzbinaRepository
    {
        List<Porudzbina> GetAllPorudzbina();
        Porudzbina GetPorudzbinaById(int id);
        PorudzbinaConfirmation CreatePorudzbina(Porudzbina porudzbina);
        void UpdatePorudzbina(Porudzbina porudzbina);
        void DeletePorudzbina(int id);
        bool SaveChanges();
        decimal GetRacunPorudzbineByPorudzbinaId(int porudzbinaId);
        decimal GetPopustNaPorudzbinuByPorudzbinaId(int porudzbinaId);
        List<int> GetAllPorudzbinaId();
        PorudzbinaDto CreatePorudzbinaForUser();
    }
}
