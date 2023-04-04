using Cvijecara_Sanja_Tica_IT80_2019.AuthHelpers;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KorisnikData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/login")]
    [Produces("application/json","application/xml")]
    public class AuthController:ControllerBase
    {
        private readonly IKorisnikRepository _korisnikRepository;
        private readonly IAuthRepository _authRepository;

        public AuthController(IKorisnikRepository korisnikRepository,IAuthRepository authRepository)
        {
            this._korisnikRepository = korisnikRepository;
            this._authRepository = authRepository;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Authenticate([FromBody] Kredencijali korisnik)
        {
            Korisnik user = _korisnikRepository.GetKorisnikByKorisnickoIme(korisnik.KorisnickoIme);
            if (user == null)
            {
                return NotFound("Ne postoji korisnik sa datim korisnickim imenom");

            }
            else if (!BCrypt.Net.BCrypt.Verify(korisnik.Lozinka,user.Lozinka))
            {
                return Unauthorized("Ne poklapaju se lozinka i username.");
            }
            string tip;
            if(user.TipId == 1)
            {
                tip = "admin";
            } else if (user.TipId == 2)
            {
                tip = "registrovani";
            }else
            {
                tip = "neregistrovani";
            }
            AuthToken token = _authRepository.Authenticate(user.KorisnickoIme, user.Lozinka, tip);
            if (token == null)
            {
                return Unauthorized("Nije generisan token");
            }
            return Ok(token);
        }
    }
}
