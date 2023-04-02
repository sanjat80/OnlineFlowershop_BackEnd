using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.PorudzbinaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.ProizvodData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.PorudzbinaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.ProizvodModel;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/proizvodi")]
    [Consumes("application/json","application/xml")]
    public class ProizvodController:ControllerBase
    {
        private readonly IProizvodRepository proizvodRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public ProizvodController(IProizvodRepository proizvodRepository,LinkGenerator linkGenerator,IMapper mapper)
        {
            this.proizvodRepository = proizvodRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet]
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
        public ActionResult<ProizvodDto> GetProizvodById(int id)
        {
            var proizvod = proizvodRepository.GetProizvodById(id);
            if (proizvod == null)
            {
                return NotFound("Proizvod sa proslijedjenim id-em nije pronadjen.");
            }
            return Ok(mapper.Map<ProizvodDto>(proizvod));
        }

        [HttpPost]
        [Consumes("application/json")]
        public ActionResult<ProizvodConfirmationDto> CreateProizvod([FromBody] ProizvodCreationDto proizvod)
        {
            //try
            //{
            Proizvod product = mapper.Map<Proizvod>(proizvod);
            ProizvodConfirmation confirmation = proizvodRepository.CreateProizvod(product);
            proizvodRepository.SaveChanges();
            //string? location = linkGenerator.GetPathByAction("GetTipKorisnikaById", "TipKorisnika", new { tipId = confirmation.TipId });
            return Ok(product);
            /*}
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja pakovanja");
            }*/
        }

        [HttpDelete("{id}")]
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
        [HttpPut]
        [Consumes("application/json")]
        public ActionResult<ProizvodDto> UpdateProizvod(ProizvodUpdateDto proizvod)
        {
            try
            {
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
