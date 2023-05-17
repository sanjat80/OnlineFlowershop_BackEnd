using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KorisnikData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.ValidationData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KategorijaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/korisnici")]
    [Produces("application/json","application/xml")]
    //[Authorize(Roles ="admin")]
    public class KorisnikController:ControllerBase
    {
        private readonly IKorisnikRepository korisnikRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IValidationRepository validationRepository;
        public KorisnikController(IKorisnikRepository korisnikRepository, LinkGenerator linkGenerator, IMapper mapper, IValidationRepository validationRepository)
        {
            this.korisnikRepository = korisnikRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.validationRepository = validationRepository;
        }

        [HttpGet]
        [HttpHead]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<KorisnikDto>> GetAllKorisnik()
        {
            var korisnici = korisnikRepository.GetAllKorisnik();
            if (korisnici == null || korisnici.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<KorisnikDto>>(korisnici));
        }
        [HttpGet("{id}")]
        [ActionName("GetKorisnik")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<KorisnikDto> GetKorisnikById(int id)
        {
            var korisnik = korisnikRepository.GetKorisnikById(id);
            if (korisnik == null)
            {
                return NotFound("Korisnik sa proslijedjenim id-em nije pronadjen.");
            }
            return Ok(mapper.Map<KorisnikDto>(korisnik));
        }

        [HttpPost]
        [Authorize(Roles ="admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorisnikConfirmationDto> CreateKorisnik([FromBody] KorisnikCreationDto korisnik)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if (!validationRepository.IsValidEmail(korisnik.Email))
                    {
                        return BadRequest("Email nije unesen u ispravnom formatu (npr) : john.doe@example.com!");
                    }
                    if(!validationRepository.ValidatePassword(korisnik.Lozinka))
                    {
                        return BadRequest("Lozinka nije unesena u dobrom formatu: mora sadrzati bar 8 karaktera, bar jedno veliko i jedno malo slovo, bar jednu cifru i jedan specijalni karakter.");
                    }
                    if (!validationRepository.IsEmailUnique(korisnik.Email))
                    {
                        return BadRequest("Korisnik sa datim email-om vec postoji!");
                    }
                    if (!validationRepository.IsUsernameUnique(korisnik.KorisnickoIme))
                    {
                        return BadRequest("Korisnik sa datim korisnickim imenom vec postoji!");
                    }
                    string? lozinka = korisnik.Lozinka;
                    string lozinka2 = BCrypt.Net.BCrypt.HashPassword(lozinka);
                    korisnik.Lozinka = lozinka2;
                    Korisnik user = mapper.Map<Korisnik>(korisnik);
                    KorisnikConfirmation confirmation = korisnikRepository.CreateKorisnik(user);
                    korisnikRepository.SaveChanges();
                    return Ok(mapper.Map<KorisnikDto>(user));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch(Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Tip korisnika koji je naveden ne postoji!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja korisnika.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="admin,registrovani")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteKorisnik(int id)
        {
            try
            {
                var korisnikModel = korisnikRepository.GetKorisnikById(id);
                if (korisnikModel == null)
                {
                    return NotFound("Korisnik sa proslijedjenim id-em nije pronadjen.");
                }
                korisnikRepository.DeleteKorisnik(id);
                korisnikRepository.SaveChanges();
                return NoContent();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Proslijedjeni tip korisnika ne postoji u bazi!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja korisnika.");
            }
        }
        [HttpPut]
        [Consumes("application/json")]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorisnikDto> UpdateKorisnik(KorisnikUpdateDto korisnik)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if(!validationRepository.IsValidEmail(korisnik.Email))
                    {
                        return BadRequest("Email nije unesen u ispravnom formatu (npr) : john.doe@example.com!");
                    }
                    if (!validationRepository.ValidatePassword(korisnik.Lozinka))
                    {
                        return BadRequest("Lozinka nije unesena u dobrom formatu: mora sadrzati bar 8 karaktera, bar jedno veliko i jedno malo slovo, bar jednu cifru i jedan specijalni karakter.");
                    }
                    if (!validationRepository.IsEmailUnique(korisnik.Email))
                    {
                        return BadRequest("Korisnik sa datim email-om vec postoji!");
                    }
                    if (!validationRepository.IsUsernameUnique(korisnik.KorisnickoIme))
                    {
                        return BadRequest("Korisnik sa datim korisnickim imenom vec postoji!");
                    }
                    var stariKorisnik = korisnikRepository.GetKorisnikById(korisnik.KorisnikId);
                    if (stariKorisnik == null)
                    {
                        return NotFound("Korisnik sa proslijedjenim id-em nije pronadjen.");
                    }
                    Korisnik user = mapper.Map<Korisnik>(korisnik);
                    mapper.Map(user, stariKorisnik);
                    korisnikRepository.SaveChanges();
                    return Ok(mapper.Map<KorisnikDto>(stariKorisnik));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Proslijedjeni tip korisnika ne postoji u bazi!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja korisnika.");
            }
        }
        [HttpPut("registrovani")]
        [Authorize(Roles ="registrovani")]
        //[Route("/registrovani")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorisnikDto> UpdateRegistrovaniKorisnik(KorisnikUpdateRegistrationDto korisnik)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!validationRepository.IsValidEmail(korisnik.Email))
                    {
                        return BadRequest("Email nije unesen u ispravnom formatu (npr) : john.doe@example.com!");
                    }
                    if (!validationRepository.ValidatePassword(korisnik.Lozinka))
                    {
                        return BadRequest("Lozinka nije unesena u dobrom formatu: mora sadrzati bar 8 karaktera, bar jedno veliko i jedno malo slovo, bar jednu cifru i jedan specijalni karakter.");
                    }
                    if(!validationRepository.IsEmailUnique(korisnik.Email))
                    {
                        return BadRequest("Korisnik sa datim email-om vec postoji!");
                    }
                    if(!validationRepository.IsUsernameUnique(korisnik.KorisnickoIme))
                    {
                        return BadRequest("Korisnik sa datim korisnickim imenom vec postoji!");
                    }
                    var stariKorisnik = korisnikRepository.GetKorisnikById(korisnik.KorisnikId);
                    if (stariKorisnik == null)
                    {
                        return NotFound("Korisnik sa proslijedjenim id-em nije pronadjen.");
                    }
                    string? lozinka = korisnik.Lozinka;
                    string lozinka2 = BCrypt.Net.BCrypt.HashPassword(lozinka);
                    korisnik.Lozinka = lozinka2;
                    Korisnik user = mapper.Map<Korisnik>(korisnik);
                    user.TipId = 2;
                    user.StatusKorisnika = "aktivan";
                    mapper.Map(user, stariKorisnik);
                    korisnikRepository.SaveChanges();
                    return Ok(mapper.Map<KorisnikUpdateRegistrationDto>(stariKorisnik));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Proslijedjeni tip korisnika ne postoji u bazi!");
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja korisnika.");
            }
        }
        [Authorize(Roles ="registrovani, admin")]
        [Produces("application/json")]
        [HttpGet("trenutniKorisnik")]
        public ActionResult<KorisnikDto> GetCurrentUser()
        {
            try
            {
                var user = korisnikRepository.GetCurrentUser();
                if (user == null)
                {
                    return NoContent();
                }
                return user;
            }catch(Exception e)
            {
                return BadRequest(e);
            }
        }
        [Authorize(Roles ="admin, registrovani")]
        [HttpGet("korpaKorisnik")]
        public ActionResult<KorisnikKorpaDto> GetKorisnikAndKorpaFromCurrentUser()
        {
            var korisnik = korisnikRepository.GetKorpaForCurrentUser();
            if(korisnik == null)
            {
                return NotFound();
            }
            return Ok(korisnik);
        }

    }
}
