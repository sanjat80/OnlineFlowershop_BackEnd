using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.PorudzbinaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.ProizvodData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.ValidationData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.PorudzbinaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.ProizvodModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/proizvodi")]
    [Produces("application/json","application/xml")]
    public class ProizvodController:ControllerBase
    {
        private readonly IProizvodRepository proizvodRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IValidationRepository validationRepository;

        public ProizvodController(IProizvodRepository proizvodRepository,LinkGenerator linkGenerator,IMapper mapper, IValidationRepository validationRepository)
        {
            this.proizvodRepository = proizvodRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.validationRepository = validationRepository;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<ProizvodDto>> GetAllProizvod()
        {
            var proizvodi = proizvodRepository.GetAllProizvod();
            if (proizvodi == null || proizvodi.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<ProizvodDto>>(proizvodi));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ProizvodDto> GetProizvodById(int id)
        {
            var proizvod = proizvodRepository.GetProizvodById(id);
            if (proizvod == null)
            {
                return NotFound("Proizvod sa proslijedjenim id-em nije pronadjen.");
            }
            proizvod.ProizvodId = id;
            return Ok(mapper.Map<ProizvodDto>(proizvod));
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ProizvodConfirmationDto> CreateProizvod([FromBody] ProizvodCreationDto proizvod)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if(!validationRepository.ValidateValuta(proizvod.Valuta))
                    {
                        return BadRequest("Valuta mora biti neka od 3 dozvoljene: RSD,EUR,BAM i to duzine 3 karaktera.");
                    }
                    Proizvod product = mapper.Map<Proizvod>(proizvod);
                    ProizvodConfirmation confirmation = proizvodRepository.CreateProizvod(product);
                    proizvodRepository.SaveChanges();
                    //string? location = linkGenerator.GetPathByAction("GetProizvodById", "Proizvod", new { proizvodId = confirmation.ProizvodId });
                    return Ok(mapper.Map<ProizvodDto>(product));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja pakovanja");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteProizvod(int id)
        {
            try
            {

                var proizvodModel = proizvodRepository.GetProizvodById(id);
                if (proizvodModel == null)
                {
                    return NotFound("Proizvod sa proslijedjenim id-em nije pronadjen.");
                }
                proizvodRepository.DeleteProizvod(id);
                proizvodRepository.SaveChanges();
                return NoContent();
            }
            catch(Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Nije moguce proslijediti neku od vrijednosti stranog kljuca, jer ne postoje u bazi!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja proizvoda.");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ProizvodDto> UpdateProizvod(ProizvodUpdateDto proizvod)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if(!validationRepository.ValidateValuta(proizvod.Valuta))
                    {
                        return BadRequest("Valuta mora biti neka od 3 dozvoljene: RSD,EUR,BAM i to duzine 3 karaktera.");
                    }
                    var stariProizvod = proizvodRepository.GetProizvodById(proizvod.ProizvodId);
                    if (stariProizvod == null)
                    {
                        return NotFound("Proizvod sa proslijedjenim id-em nije pronadjen.");
                    }
                    Proizvod product = mapper.Map<Proizvod>(proizvod);
                    mapper.Map(product, stariProizvod);
                    proizvodRepository.SaveChanges();
                    return Ok(mapper.Map<ProizvodDto>(stariProizvod));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status404NotFound, "Nije moguce proslijediti neku od vrijednosti stranog kljuca, jer ne postoje u bazi!");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }

    }
}
