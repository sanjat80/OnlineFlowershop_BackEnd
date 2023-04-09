using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KorisnikData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KorpaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.ValidationData;
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
    public class KorpaController:ControllerBase
    {
        private readonly IKorpaRepository korpaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;
        private readonly IValidationRepository validationRepository;

        public KorpaController(IKorpaRepository korpaRepository, LinkGenerator linkGenerator, IMapper mapper,IValidationRepository validationRepository)
        {
            this.korpaRepository = korpaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.validationRepository = validationRepository;
        }

        [HttpGet]
        [HttpHead]
        [Authorize(Roles ="admin")]
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
        [Authorize(Roles ="registrovani,admin")]
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
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorpaConfirmationDto> CreateKorpa([FromBody] KorpaCreationDto korpa)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if(!validationRepository.ValidateValuta(korpa.Valuta))
                    {
                        return BadRequest("Valuta mora biti neka od 3 dozvoljene: RSD,EUR,BAM i to duzine 3 karaktera.");
                    }
                    Korpa basket = mapper.Map<Korpa>(korpa);
                    var korisnik = basket.KorisnikId;
                    List<int> korisnici = korpaRepository.GetAllKorisnikId();
                    if (korisnici.Contains(korisnik))
                    {
                        KorpaConfirmation confirmation = korpaRepository.CreateKorpa(basket);
                        korpaRepository.SaveChanges();
                        return Ok(mapper.Map<KorpaDto>(basket));
                    }
                    else
                    {
                        return BadRequest("Korisnik ciji id zelite da navedete kao vlasnika korpe nije pronadjen!");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
                
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
        [Authorize(Roles ="admin")]
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
        [Authorize(Roles ="admin")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<KorpaDto> UpdateKorpa(KorpaUpdateDto korpa)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    if(!validationRepository.ValidateValuta(korpa.Valuta))
                    {
                        return BadRequest("Valuta mora biti neka od 3 dozvoljene: RSD,EUR,BAM i to duzine 3 karaktera.");
                    }
                    var staraKorpa = korpaRepository.GetKorpaById(korpa.KorpaId);
                    if (staraKorpa == null)
                    {
                        return NotFound("Korpa sa proslijedjenim id-em nije pronadjena.");
                    }
                    Korpa basket = mapper.Map<Korpa>(korpa);
                    var korisnik = basket.KorisnikId;
                    var korisnici = korpaRepository.GetAllKorisnikId();
                    if (korisnici.Contains(korisnik))
                    {
                        mapper.Map(basket, staraKorpa);
                        korpaRepository.SaveChanges();
                        return Ok(mapper.Map<KorpaDto>(staraKorpa));
                    }
                    else
                    {
                        return BadRequest("Korisnik kog zelite da navedete kao vlasnika korpe nije pronadjen!");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }
        [HttpGet("stavkeKorpe/{korpaId}")]
        [Authorize(Roles ="admin,registrovani")]
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
        [HttpGet("iznosKolicina/{korpaId}")]
        [Authorize(Roles = "admin,registrovani")]
        public ActionResult<IznosKolicinaKorpeDto> GetIznosAndKolicinaKorpeByKorpaId(int korpaId)
        {
            try
            {
                var iznosKolicina = korpaRepository.GetIznosAndKolicinaByKorpaId(korpaId);
                if(iznosKolicina == null)
                {
                    return NotFound("Nije pronadjena korpa sa datim id-em.");
                }
                return iznosKolicina;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom vracanja kolicine i iznosa date korpe.");
            }
            
        }
    }
}
