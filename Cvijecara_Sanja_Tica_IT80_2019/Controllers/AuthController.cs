using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.AuthHelpers;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KorisnikData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/Account")]
    [Produces("application/json","application/xml")]
    public class AuthController:ControllerBase
    {
        private readonly IKorisnikRepository _korisnikRepository;
        private readonly IAuthRepository _authRepository;
        private readonly IMapper mapper;

        public AuthController(IKorisnikRepository korisnikRepository,IAuthRepository authRepository,IMapper mapper)
        {
            this._korisnikRepository = korisnikRepository;
            this._authRepository = authRepository;
            this.mapper = mapper;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("/login")]
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
        [HttpPost]
        [AllowAnonymous]
        [Route("/register")]
        public ActionResult Register([FromBody] KorisnikRegistrationDto korisnik)
        {
            try
            {
                string? lozinka = korisnik.Lozinka;
                string lozinka2 = BCrypt.Net.BCrypt.HashPassword(lozinka);
                korisnik.Lozinka = lozinka2;
                Korisnik user = mapper.Map<Korisnik>(korisnik);
                user.TipId = 2;
                user.StatusKorisnika = "aktivan";
                KorisnikConfirmation confirmation = _korisnikRepository.CreateKorisnik(user);
                _korisnikRepository.SaveChanges();
                return Ok(korisnik);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Tip korisnika koji je naveden ne postoji!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja pakovanja");
            }
        }
    }
}
