using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.VrstaData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KategorijaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.VrstaModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/vrste")]
    [Consumes("application/json","application/xml")]
    public class VrstaController:ControllerBase
    {
        private readonly IVrstaRepository vrstaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public VrstaController(IVrstaRepository vrstaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.vrstaRepository = vrstaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<VrstaDto>> GetAllVrsta()
        {
            var vrste = vrstaRepository.GetAllVrsta();
            if (vrste == null || vrste.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<VrstaDto>>(vrste));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<VrstaDto> GetVrstaById(int id)
        {
            var vrsta = vrstaRepository.GetVrstaById(id);
            if (vrsta == null)
            {
                return NotFound("Vrsta sa proslijedjenim id-em nije pronadjena.");
            }
            return Ok(mapper.Map<VrstaDto>(vrsta));
        }

        [HttpPost]
        [Consumes("application/json")]
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VrstaConfirmationDto> CreateVrsta([FromBody] VrstaCreationDto vrsta)
        {
            try
            {
            Vrstum vrs = mapper.Map<Vrstum>(vrsta);
            VrstaConfirmation confirmation = vrstaRepository.CreateVrsta(vrs);
            vrstaRepository.SaveChanges();
            //string? location = linkGenerator.GetPathByAction("GetTipKorisnikaById", "TipKorisnika", new { tipId = confirmation.TipId });
            return Ok(vrs);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja pakovanja");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteVrsta(int id)
        {
            try
            {
                var vrstaModel = vrstaRepository.GetVrstaById(id);
                if (vrstaModel == null)
                {
                    return NotFound("Vrsta sa proslijedjenim id-em nije pronadjena.");
                }
                vrstaRepository.DeleteVrsta(id);
                vrstaRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja vrste");
            }
        }
        [HttpPut]
        [Consumes("application/json")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VrstaDto> UpdateVrsta(VrstaUpdateDto vrsta)
        {
            try
            {
            var staraVrsta = vrstaRepository.GetVrstaById(vrsta.VrstaId);
            if (staraVrsta == null)
            {
                return NotFound("Vrsta sa proslijedjenim id-em nije pronadjena.");
            }
            Vrstum vrs = mapper.Map<Vrstum>(vrsta);
            mapper.Map(vrs, staraVrsta);
            vrstaRepository.SaveChanges();
            return Ok(mapper.Map<VrstaDto>(staraVrsta));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }
    }
}
