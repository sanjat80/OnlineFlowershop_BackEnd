using Cvijecara_Sanja_Tica_IT80_2019.Entities;

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
        public decimal GetRacunPorudzbineByPorudzbinaId(int porudzbinaId);
        public decimal GetPopustNaPorudzbinuByPorudzbinaId(int porudzbinaId);
        public List<int> GetAllPorudzbinaId();
    }
}
