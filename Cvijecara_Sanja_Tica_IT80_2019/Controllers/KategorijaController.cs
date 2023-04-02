using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.PakovanjeData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KategorijaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.PakovanjeModel;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/kategorije")]
    [Consumes("application/json","application/xml")]
    public class KategorijaController:ControllerBase
    {
        private readonly IKategorijaRepository kategorijaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public KategorijaController(IKategorijaRepository kategorijaRepository, LinkGenerator linkGenerator, IMapper mapper)
        {
            this.kategorijaRepository = kategorijaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }
        [HttpGet]
        public ActionResult<List<KategorijaDto>> GetAllKategorija()
        {
            var kategorije = kategorijaRepository.GetAllKategorija();
            if (kategorije == null || kategorije.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<KategorijaDto>>(kategorije));
        }
        [HttpGet("{id}")]
        public ActionResult<KategorijaDto> GetKategorijaById(int id)
        {
            var kategorija = kategorijaRepository.GetKategorijaById(id);
            if (kategorija == null)
            {
                return NotFound("Kategorija sa proslijedjenim id-em nije pronadjena.");
            }
            return Ok(mapper.Map<KategorijaDto>(kategorija));
        }

        [HttpPost]
        [Consumes("application/json")]
        public ActionResult<KategorijaConfirmationDto> CreateKategorija([FromBody] KategorijaCreationDto kategorija)
        {
            //try
            //{
            Kategorija kat = mapper.Map<Kategorija>(kategorija);
            KategorijaConfirmation confirmation = kategorijaRepository.CreateKategorija(kat);
            kategorijaRepository.SaveChanges();
            //string? location = linkGenerator.GetPathByAction("GetTipKorisnikaById", "TipKorisnika", new { tipId = confirmation.TipId });
            return Ok(kat);
            /*}
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja pakovanja");
            }*/
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteKategorija(int id)
        {
            try
            {
                var kategorijaModel = kategorijaRepository.GetKategorijaById(id);
                if (kategorijaModel == null)
                {
                    return NotFound("Kategorija sa proslijedjenim id-em nije pronadjena.");
                }
                kategorijaRepository.DeleteKategorija(id);
                kategorijaRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja kategorije");
            }
        }
        [HttpPut]
        [Consumes("application/json")]
        public ActionResult<KategorijaDto> UpdateKategorija(KategorijaUpdateDto kategorija)
        {
            try
            {
                var staraKategorija = kategorijaRepository.GetKategorijaById(kategorija.KategorijaId);
                if (staraKategorija == null)
                {
                    return NotFound("Kategorija sa proslijedjenim id-em nije pronadjena.");
                }
                Kategorija kat = mapper.Map<Kategorija>(kategorija);
                mapper.Map(kat, staraKategorija);
                kategorijaRepository.SaveChanges();
                return Ok(mapper.Map<KategorijaDto>(staraKategorija));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }
    }
}
