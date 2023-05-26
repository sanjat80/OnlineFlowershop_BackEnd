﻿using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel;

namespace Cvijecara_Sanja_Tica_IT80_2019.Data.KorisnikData
{
    public interface IKorisnikRepository
    {
        List<Korisnik> GetAllKorisnik();
        Korisnik GetKorisnikById(int id);
        KorisnikConfirmation CreateKorisnik(Korisnik korisnik);
        Korisnik GetKorisnikByKorisnickoIme(string korisnickoIme);
        void UpdateKorisnik(Korisnik korisnik);
        void DeleteKorisnik(int id);
        bool SaveChanges();
        public KorisnikDto GetCurrentUser();
        public KorisnikKorpaDto GetKorpaForCurrentUser();
        KorisnikUpdateRegistrationDto GetKorisnikByKorisnickoImeProfile(string korisnickoIme);
    }
}
