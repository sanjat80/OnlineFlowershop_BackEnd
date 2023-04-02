using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KorisnikData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KategorijaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/korisnici")]
    [Consumes("application/json","application/xml")]
    public class KorisnikController:ControllerBase
    {
        private readonly IKorisnikRepository korisnikRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public KorisnikController(IKorisnikRepository korisnikRepository,LinkGenerator linkGenerator, IMapper mapper)
        {
            this.korisnikRepository = korisnikRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet]
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
        public ActionResult<KorisnikConfirmationDto> CreateKorisnik([FromBody] KorisnikCreationDto korisnik)
        {
            try
            {
            Korisnik user = mapper.Map<Korisnik>(korisnik);
            KorisnikConfirmation confirmation = korisnikRepository.CreateKorisnik(user);
            korisnikRepository.SaveChanges();
            //string? location = linkGenerator.GetPathByAction("GetKorisnikById", "Korisnik", new { korisnikId = confirmation.KorisnikId });
            return Ok(user);
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
        public ActionResult<KorisnikDto> UpdateKorisnik(KorisnikUpdateDto korisnik)
        {
            //try
            //{
            var stariKorisnik = korisnikRepository.GetKorisnikById(korisnik.KorisnikId);
            if (stariKorisnik == null)
            {
                return NotFound("Korisnik sa proslijedjenim id-em nije pronadjen.");
            }
            Korisnik user = mapper.Map<Korisnik>(korisnik);
            mapper.Map(user, stariKorisnik);
            korisnikRepository.SaveChanges();
            return Ok(mapper.Map<KorisnikDto>(stariKorisnik));
            /*}
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }*/
        }
    }
}
