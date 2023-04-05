using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KorisnikData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KorpaData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorisnikModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KorpaModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/korpe")]
    [Produces("application/json","application/xml")]
    [Authorize(Roles ="admin")]
    public class KorpaController:ControllerBase
    {
        private readonly IKorpaRepository korpaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public KorpaController(IKorpaRepository korpaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.korpaRepository = korpaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<KorpaDto>> GetAllKorpa()
        {
            var korpe = korpaRepository.GetAllKorpa();
            if (korpe == null || korpe.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<KorpaDto>>(korpe));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles ="registrovani")]
        public ActionResult<KorpaDto> GetKorpaById(int id)
        {
            var korpa = korpaRepository.GetKorpaById(id);
            if (korpa == null)
            {
                return NotFound("Korpa sa proslijedjenim id-em nije pronadjena.");
            }
            return Ok(mapper.Map<KorpaDto>(korpa));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorpaConfirmationDto> CreateKorpa([FromBody] KorpaCreationDto korpa)
        {
            try
            {
                Korpa basket = mapper.Map<Korpa>(korpa);
                KorpaConfirmation confirmation = korpaRepository.CreateKorpa(basket);
                korpaRepository.SaveChanges();
                //string? location = linkGenerator.GetPathByAction("GetKorisnikById", "Korisnik", new { korisnikId = confirmation.KorisnikId });
                return Ok(basket);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Korisnik koji je naveden kao vlasnik korpe je vec vlasnik neke druge korpe!");
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
        public IActionResult DeleteKorpa(int id)
        {
            try
            {
                var korpaModel = korpaRepository.GetKorpaById(id);
                if (korpaModel == null)
                {
                    return NotFound("Korpa sa proslijedjenim id-em nije pronadjena.");
                }
                korpaRepository.DeleteKorpa(id);
                korpaRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja korisnicke korpe.");
            }
        }
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorpaDto> UpdateKorpa(KorpaUpdateDto korpa)
        {
            try
            {
            var staraKorpa = korpaRepository.GetKorpaById(korpa.KorpaId);
            if (staraKorpa == null)
            {
                return NotFound("Korpa sa proslijedjenim id-em nije pronadjena.");
            }
            Korpa basket = mapper.Map<Korpa>(korpa);
            mapper.Map(basket, staraKorpa);
            korpaRepository.SaveChanges();
            return Ok(mapper.Map<KorpaDto>(staraKorpa));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }
        [HttpGet("stavkeKorpe/{korpaId}")]
        public ActionResult<List<string>> GetStavkeKorpeByKorpaId(int korpaId)
        {
            try
            {
                var stavke = korpaRepository.GetStavkeKorpeByKorpaId(korpaId);
                if (stavke.Count == 0)
                {
                    return NotFound("U korpi se ne nalazi nijedan proizvod.");
                }
                return stavke.ToList();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom pregleda proizvoda korpe sa proslijedjenim id-em.");
            }
        }

    }
}
