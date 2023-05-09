using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.AuthHelpers;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KorisnikData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.ValidationData;
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
        private readonly IValidationRepository validationRepository;

        public AuthController(IKorisnikRepository korisnikRepository,IAuthRepository authRepository,IMapper mapper,IValidationRepository validationRepository)
        {
            this._korisnikRepository = korisnikRepository;
            this._authRepository = authRepository;
            this.mapper = mapper;
            this.validationRepository = validationRepository;
        }
        [AllowAnonymous]
        [HttpPost("login")]
        //[Route("/login")]
        public ActionResult Authenticate([FromBody] Kredencijali korisnik)
        {
            Korisnik user = _korisnikRepository.GetKorisnikByKorisnickoIme(korisnik.KorisnickoIme);
            if (user == null)
            {
                return NotFound("Ne postoji korisnik sa datim korisnickim imenom.");

            }
            else if (!BCrypt.Net.BCrypt.Verify(korisnik.Lozinka,user.Lozinka))
            {
                return Unauthorized("Ne poklapaju se lozinka i username.");
            }
            string tip;
            if(user.TipId == 1)
            {
                tip = "admin";
            } else if(user.TipId == 2)
            {
                tip = "registrovani";
            }
            else
            {
                tip = "registrovani";
            }
            AuthToken token = _authRepository.Authenticate(user.KorisnickoIme, user.Lozinka, tip);
            if (token == null)
            {
                return Unauthorized("Nije generisan token");
            }
            return Ok(token);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult Register([FromBody] KorisnikRegistrationDto korisnik)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!validationRepository.IsValidEmail(korisnik.Email))
                    {
                        return BadRequest("Email nije unesen u ispravnom formatu (npr) : john.doe@example.com!");
                    }
                    if(!validationRepository.ValidatePassword(korisnik.Lozinka))
                    {
                        return BadRequest("Lozinka nije unesena u dobrom formatu: mora sadrzati bar 8 karaktera, bar jedno veliko i jedno malo slovo, bar jednu cifru i jedan specijalni karakter.");
                    }
                    string? lozinka = korisnik.Lozinka;
                    string lozinka2 = BCrypt.Net.BCrypt.HashPassword(lozinka);
                    korisnik.Lozinka = lozinka2;
                    Korisnik user = mapper.Map<Korisnik>(korisnik);
                    user.TipId = 2;
                    user.StatusKorisnika = "aktivan";
                    KorisnikConfirmation confirmation = _korisnikRepository.CreateKorisnik(user);
                    _korisnikRepository.SaveChanges();
                    return Ok(mapper.Map<KorisnikRegistrationDto>(user));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            /*catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Tip korisnika koji je naveden ne postoji!");
            }*/
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja korisnika.");
            }
        }
    }
}
