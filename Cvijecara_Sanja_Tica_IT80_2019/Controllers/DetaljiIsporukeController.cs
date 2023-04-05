using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.DetaljiIsporukeData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.PorudzbinaData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.DetaljiIsporukeModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KategorijaModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/detaljiIsporuke")]
    [Produces("application/json","application/xml")]
    public class DetaljiIsporukeController:ControllerBase
    {
        private readonly IDetaljiIsporukeRepository detaljiIsporukeRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
  
        public DetaljiIsporukeController(IDetaljiIsporukeRepository detaljiIsporukeRepository,LinkGenerator linkGenerator,IMapper mapper)
        {
            this.detaljiIsporukeRepository = detaljiIsporukeRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<DetaljiIsporukeDto>> GetAllDetaljiIsporuke()
        {
            var detalji = detaljiIsporukeRepository.GetAllDetaljiIsporuke();
            if (detalji == null || detalji.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<DetaljiIsporukeDto>>(detalji));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<DetaljiIsporukeDto> GetDetaljiIsporukeById(int id)
        {
            var detalji = detaljiIsporukeRepository.GetDetaljiIsporukeById(id);
            if (detalji == null)
            {
                return NotFound("Detalji isporuke sa proslijedjenim id-em nisu pronadjeni.");
            }
            return Ok(mapper.Map<DetaljiIsporukeDto>(detalji));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DetaljiIsporukeConfirmationDto> CreateDetaljiIsporuke([FromBody] DetaljiIsporukeCreationDto detaljiIsporuke)
        {
            try
            {
                DetaljiIsporuke detalji = mapper.Map<DetaljiIsporuke>(detaljiIsporuke);
                var porudzbina = detalji.PorudzbinaId;
                List<int> porudzbine = detaljiIsporukeRepository.GetAllPorudzbinaId();
                if (porudzbine.Contains(porudzbina))
               {
                    DetaljiIsporukeConfirmation confirmation = detaljiIsporukeRepository.CreateDetaljiIsporuke(detalji);
                    detaljiIsporukeRepository.SaveChanges();
                    //string? location = linkGenerator.GetPathByAction("GetDetaljiIsporukeById", "DetaljiIsporukeController", new { detaljiId = confirmation.IsporukaId });
                    return Ok(detalji);
                }
                else
                {
                    return NotFound("Porudzbina koju zelite da proslijedite kao strani kljuc nije pronadjena u bazi!");
                }
            }
            catch(Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Transakcija koja se odnosi na porudzbinu sa proslijedjenim id-em je vec kreirana, ili porudzbina ne postoji u bazi!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja pakovanja");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteKategorija(int id)
        {
            try
            {
                var detaljiModel = detaljiIsporukeRepository.GetDetaljiIsporukeById(id);
                if (detaljiModel == null)
                {
                    return NotFound("Detalji isporuke sa proslijedjenim id-em nisu pronadjeni.");
                }
                detaljiIsporukeRepository.DeleteDetaljiIsporuke(id);
                detaljiIsporukeRepository.SaveChanges();
                return NoContent();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Transakcija koja se odnosi na porudzbinu sa proslijedjenim id-em je vec kreirana, ili porudzbina ne postoji u bazi!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja detalja isporuke");
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DetaljiIsporukeDto> UpdateDetaljiIsporuke(DetaljiIsporukeUpdateDto detaljiIsporuke)
        {
            try
            {
                var stariDetalji = detaljiIsporukeRepository.GetDetaljiIsporukeById(detaljiIsporuke.IsporukaId);
                if (stariDetalji == null)
                {
                    return NotFound("Detalji isporuke sa proslijedjenim id-em nisu pronadjeni.");
                }
                DetaljiIsporuke detalji = mapper.Map<DetaljiIsporuke>(detaljiIsporuke);
                var porudzbina = detalji.PorudzbinaId;
                List<int> porudzbine = detaljiIsporukeRepository.GetAllPorudzbinaId();
                if(porudzbine.Contains(porudzbina))
                {
                    mapper.Map(detalji, stariDetalji);
                    detaljiIsporukeRepository.SaveChanges();
                    return Ok(mapper.Map<DetaljiIsporukeDto>(stariDetalji));
                }
                else
                {
                    return NotFound("Porudzbina koju zelite da proslijedite kao strani kljuc nije pronadjena u bazi!");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }
    }
}
