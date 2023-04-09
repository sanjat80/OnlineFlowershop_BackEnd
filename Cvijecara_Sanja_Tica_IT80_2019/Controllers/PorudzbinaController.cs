using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.PorudzbinaData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KategorijaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.PorudzbinaModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/porudzbine")]
    [Produces("application/json","application/xml")]
    public class PorudzbinaController:ControllerBase
    {
        private readonly IPorudzbinaRepository porudzbinaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public PorudzbinaController(IPorudzbinaRepository porudzbinaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.porudzbinaRepository = porudzbinaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles ="admin")]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<PorudzbinaDto>> GetAllPorudzbina()
        {
            var porudzbine = porudzbinaRepository.GetAllPorudzbina();
            if (porudzbine == null || porudzbine.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<PorudzbinaDto>>(porudzbine));
        }
        [HttpGet("{id}")]
        [Authorize(Roles ="admin,registrovani")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<PorudzbinaDto> GetPorudzbinaById(int id)
        {
            var porudzbina = porudzbinaRepository.GetPorudzbinaById(id);
            if (porudzbina == null)
            {
                return NotFound("Porudzbina sa proslijedjenim id-em nije pronadjena.");
            }
            return Ok(mapper.Map<PorudzbinaDto>(porudzbina));
        }

        [Authorize(Roles = "admin,registrovani")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PorudzbinaConfirmationDto> CreatePorudzbina([FromBody] PorudzbinaCreationDto porudzbina)
        {
            try
            {
                Porudzbina por = mapper.Map<Porudzbina>(porudzbina);
                if(por.Racun != 0)
                {
                    return BadRequest("Iznos porudzbine se automatski unosi i azurira!");
                }
                if(por.Popust != 0)
                {
                    return BadRequest("Popust na porudzbinu se obracunava automatski, na osnovu iznosa porudzbine.");
                }
                PorudzbinaConfirmation confirmation = porudzbinaRepository.CreatePorudzbina(por);
                porudzbinaRepository.SaveChanges();
                //string? location = linkGenerator.GetPathByAction("GetTipKorisnikaById", "TipKorisnika", new { tipId = confirmation.TipId });
                return Ok(mapper.Map<PorudzbinaDto>(por));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja pakovanja");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="admin,registrovani")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeletePorudzbina(int id)
        {
            try
            {
                var porudzbinaModel = porudzbinaRepository.GetPorudzbinaById(id);
                if (porudzbinaModel == null)
                {
                    return NotFound("Porudzbina sa proslijedjenim id-em nije pronadjena.");
                }
                porudzbinaRepository.DeletePorudzbina(id);
                porudzbinaRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja porudzbine");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PorudzbinaDto> UpdatePorudzbina(PorudzbinaUpdateDto porudzbina)
        {
            try
            {
            var staraPorudzbina = porudzbinaRepository.GetPorudzbinaById(porudzbina.PorudzbinaId);
            if (staraPorudzbina == null)
            {
                return NotFound("Porudzbina sa proslijedjenim id-em nije pronadjena.");
            }
            Porudzbina por = mapper.Map<Porudzbina>(porudzbina);
            mapper.Map(por, staraPorudzbina);
            porudzbinaRepository.SaveChanges();
            return Ok(mapper.Map<PorudzbinaDto>(staraPorudzbina));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }

        [HttpGet("racun/{porudzbinaId}")]
        [Authorize(Roles ="admin,registrovani")]
        public ActionResult<decimal> GetRacunByPorudzbinaId(int porudzbinaId)
        {
            try
            {
                var racun = porudzbinaRepository.GetRacunPorudzbineByPorudzbinaId(porudzbinaId);
                if (racun == 0)
                {
                    return NotFound("Za porudzbinu jos uvijek nije evidentiran racun.");
                }
                return Ok(racun);
            }
            catch(System.InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Jos nije evidentiran racun za datu porudzbinu.");
            }
        }
        [HttpGet("popust/{porudzbinaId}")]
        [Authorize(Roles ="admin, registrovani")]
        public ActionResult<decimal> GetPopustNaPorudzbinuByPorudzbinaId(int porudzbinaId)
        {
            try
            {
                var popust = porudzbinaRepository.GetPopustNaPorudzbinuByPorudzbinaId(porudzbinaId);
                if (popust == 0)
                {
                    return NotFound("Za porudzbinu jos uvijek nije evidentiran popust.");
                }
                return Ok(popust);
            }
            catch (System.InvalidOperationException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Jos nije evidentiran popust za datu porudzbinu.");
            }
        }

    }
}
