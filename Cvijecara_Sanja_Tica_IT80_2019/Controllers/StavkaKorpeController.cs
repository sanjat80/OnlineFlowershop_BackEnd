using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.StavkaKorpeData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.StavkaKorpeModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/stavkeKorpe")]
    [Produces("application/json","application/xml")]
    public class StavkaKorpeController : ControllerBase
    { 
        private readonly IStavkaKorpeRepository stavkaKorpeRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public StavkaKorpeController(IStavkaKorpeRepository stavkaKorpeRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.stavkaKorpeRepository = stavkaKorpeRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<StavkaKorpeDto>> GetAllStavkaKorpe()
        {
            var stavke = stavkaKorpeRepository.GetAllStavkaKorpe();
            if (stavke == null || stavke.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<StavkaKorpeDto>>(stavke));
        }
        [HttpGet("{proizvodId}/{korpaId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = "admin,registrovani")]
        public ActionResult<StavkaKorpeDto> GetStavkaKorpeById(int proizvodId,int korpaId)
        {
            var stavka = stavkaKorpeRepository.GetStavkaKorpeById(proizvodId,korpaId);
            if (stavka == null)
            {
                return NotFound("Stavka korpe sa proslijedjenim id-em nije pronadjena.");
            }
            return Ok(mapper.Map<StavkaKorpeDto>(stavka));
        }

        [HttpPost]
        [Consumes("application/json")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StavkaKorpeConfirmationDto> CreateStavkaKorpe([FromBody] StavkaKorpeCreationDto stavkaKorpe)
        {
            try
            {
                StavkaKorpe stavka = mapper.Map<StavkaKorpe>(stavkaKorpe);
                StavkaKorpeConfirmation confirmation = stavkaKorpeRepository.CreateStavkaKorpe(stavka);
                stavkaKorpeRepository.SaveChanges();
                //string? location = linkGenerator.GetPathByAction("GetKorisnikById", "Korisnik", new { korisnikId = confirmation.KorisnikId });
                return Ok(mapper.Map<StavkaKorpeDto>(stavka));
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Korisnik koji je naveden kao vlasnik korpe je vec vlasnik neke druge korpe!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja stavke korpe");
            }
        }

        [HttpDelete("{proizvodId}/{korpaId}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteStavkaKorpe(int proizvodId,int korpaId)
        {
            try
            {
                var stavkaKorpeModel = stavkaKorpeRepository.GetStavkaKorpeById(proizvodId, korpaId);
                if (stavkaKorpeModel == null)
                {
                    return NotFound("Stavka korpe sa proslijedjenim id-em nije pronadjena.");
                }
                stavkaKorpeRepository.DeleteStavkaKorpe(proizvodId,korpaId);
                stavkaKorpeRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja stavke korpe.");
            }
        }
        [HttpPut]
        [Consumes("application/json")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StavkaKorpeDto> UpdateStavkaKorpe(StavkaKorpeUpdateDto stavkaKorpe)
        {
            try
            {
                var staraStavka = stavkaKorpeRepository.GetStavkaKorpeById(stavkaKorpe.ProizvodId,stavkaKorpe.KorpaId);
                if (staraStavka == null)
                {
                    return NotFound("Stavka sa proslijedjenim id-em nije pronadjena.");
                }
                StavkaKorpe stavka = mapper.Map<StavkaKorpe>(stavkaKorpe);
                mapper.Map(stavka, staraStavka);
                stavkaKorpeRepository.SaveChanges();
                return Ok(mapper.Map<StavkaKorpeDto>(staraStavka));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }
    }
}
