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
    [Authorize(Roles ="admin")]
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
                    string? lozinka = korisnik.Lozinka;
                    string lozinka2 = BCrypt.Net.BCrypt.HashPassword(lozinka);
                    korisnik.Lozinka = lozinka2;
                    Korisnik user = mapper.Map<Korisnik>(korisnik);
                    KorisnikConfirmation confirmation = korisnikRepository.CreateKorisnik(user);
                    korisnikRepository.SaveChanges();
                    //string? location = linkGenerator.GetPathByAction("GetKorisnikById", "Korisnik", new { korisnikId = confirmation.KorisnikId });
                    return Ok(confirmation);
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja pakovanja");
            }
        }

        [HttpDelete("{id}")]
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }
    }
}
