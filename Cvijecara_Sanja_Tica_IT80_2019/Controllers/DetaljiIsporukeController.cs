using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.DetaljiIsporukeData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.DetaljiIsporukeModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KategorijaModel;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/detaljiIsporuke")]
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
        public ActionResult<DetaljiIsporukeDto> GetDetaljiIsporukeById(int id)
        {
            var detalji = detaljiIsporukeRepository.GetDetaljiIsporukeById(id);
            if (detalji == null)
            {
                return NotFound("Detalji isporuke sa proslijedjenim id-em nisu pronadjeni.");
            }
            return Ok(mapper.Map<DetaljiIsporukeDto>(detalji));
        }

        [HttpPost]
        [Consumes("application/json")]
        public ActionResult<DetaljiIsporukeConfirmationDto> CreateDetaljiIsporuke([FromBody] DetaljiIsporukeCreationDto detaljiIsporuke)
        {
            try
            {
            DetaljiIsporuke detalji = mapper.Map<DetaljiIsporuke>(detaljiIsporuke);
            DetaljiIsporukeConfirmation confirmation = detaljiIsporukeRepository.CreateDetaljiIsporuke(detalji);
            detaljiIsporukeRepository.SaveChanges();
            string? location = linkGenerator.GetPathByAction("GetDetaljiIsporukeById", "DetaljiIsporuke", new { detaljiId = confirmation.IsporukaId });
            return Created(location,detalji);
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
        [HttpPut]
        [Consumes("application/json")]
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
            mapper.Map(detalji, stariDetalji);
            detaljiIsporukeRepository.SaveChanges();
            return Ok(mapper.Map<DetaljiIsporukeDto>(stariDetalji));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }
    }
}
