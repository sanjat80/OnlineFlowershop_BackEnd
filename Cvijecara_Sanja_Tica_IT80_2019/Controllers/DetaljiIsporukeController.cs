using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.DetaljiIsporukeData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.PorudzbinaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.ValidationData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.DetaljiIsporukeModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KategorijaModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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
        private readonly IValidationRepository validationRepository;
  
        public DetaljiIsporukeController(IDetaljiIsporukeRepository detaljiIsporukeRepository,LinkGenerator linkGenerator,IMapper mapper,IValidationRepository validationRepository)
        {
            this.detaljiIsporukeRepository = detaljiIsporukeRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
            this.validationRepository = validationRepository;
        }

        [HttpGet]
        [HttpHead]
        [Authorize(Roles ="admin")]
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
        [Authorize(Roles ="admin,registrovani")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<DetaljiIsporukeDto> GetDetaljiIsporukeById(int id)
        {
            var detalji = detaljiIsporukeRepository.GetDetaljiIsporukeById(id);
            if (detalji == null)
            {
                return NotFound("Detalji isporuke sa proslijedjenim id-em nisu pronadjeni.");
            }
            detalji.IsporukaId = id;
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
                if(ModelState.IsValid)
                {
                    if(!validationRepository.ValidateDatumIsporuke((DateTime)detaljiIsporuke.DatumIsporuke))
                    {
                        return BadRequest("Datum isporuke ne moze biti danasnji datum, potrebno je navesti datum makar za jedan dan udaljen od danasnjeg!");
                    }
                    if (!validationRepository.ValidateAddress(detaljiIsporuke.Adresa))
                    {
                        return BadRequest("Adresa bi trebalo da sadrži naziv ulice i broj.");
                    }
                    if (!validationRepository.ValidatePhoneNumber(detaljiIsporuke.BrojTelefona))
                    {
                        return BadRequest("Broj telefona mora biti broj(+ znak je dozvoljen) i sadržati maksimalno 13 cifara!");
                    }
                    if (!validationRepository.ValidateCountry(detaljiIsporuke.Drzava))
                    {
                        return BadRequest("Drzave u kojima je moguca isporuka su Srbija, BiH, Crna Gora i Hrvatska!");
                    }
                    if (!validationRepository.ValidateRegion(detaljiIsporuke.Region))
                    {
                        return BadRequest("Regioni koji su podrzani su: \"Vojvodina\", \"Beograd\", \"Šumadija\", \"Zapadna Srbija\", \"Južna Srbija\", \"Istočna Srbija\",\r\n        // Hrvatska\r\n        \"Zagreb\", \"Sjeverna Hrvatska\", \"Središnja Hrvatska\", \"Istočna Hrvatska\", \"Zapadna Hrvatska\", \"Južna Hrvatska\",\r\n        // Crna Gora\r\n        \"Crnogorsko Primorje\", \"Crnogorsko Gorje\", \"Crnogorsko Polje\", \"Centralna Crna Gora\", \"Crnogorsko Primorje\", \"Južna Crna Gora\",\r\n        // Bosna i Hercegovina\r\n        \"Sarajevo\", \"Sjeveroistočna Bosna\", \"Posavina\", \"Srednja Bosna\", \"Hercegovina-Zapad\", \"Hercegovina-Jug\", \"Zapadnohercegovački kanton\"");
                    }
                    if (!validationRepository.ValidateCity(detaljiIsporuke.Grad))
                    {
                        return BadRequest("Gradovi koji su podrzani su:  // Srbija\r\n        \"Novi Sad\", \"Beograd\", \"Niš\", \"Kragujevac\", \"Subotica\", \"Čačak\",\r\n        // Hrvatska\r\n        \"Zagreb\", \"Split\", \"Rijeka\", \"Osijek\", \"Zadar\", \"Dubrovnik\",\r\n        // Crna Gora\r\n        \"Podgorica\", \"Nikšić\", \"Pljevlja\", \"Cetinje\", \"Bar\", \"Herceg Novi\",\r\n        // Bosna i Hercegovina\r\n        \"Trebinje\",\"Sarajevo\", \"Banja Luka\", \"Prijedor\", \"Doboj\", \"Modrica\", \"Gacko\"");
                    }

                    DetaljiIsporuke detalji = mapper.Map<DetaljiIsporuke>(detaljiIsporuke);
                    var porudzbina = detalji.PorudzbinaId;
                    List<int> porudzbine = detaljiIsporukeRepository.GetAllPorudzbinaId();
                    if (porudzbine.Contains(porudzbina))
                    {
                        DetaljiIsporukeConfirmation confirmation = detaljiIsporukeRepository.CreateDetaljiIsporuke(detalji);
                        detaljiIsporukeRepository.SaveChanges();
                        //string? location = linkGenerator.GetPathByAction("GetDetaljiIsporukeById", "DetaljiIsporukeController", new { detaljiId = confirmation.IsporukaId });
                        return Ok(mapper.Map<DetaljiIsporukeDto>(detalji));
                    }
                    else
                    {
                        return BadRequest("Porudzbina koju zelite da proslijedite kao strani kljuc nije pronadjena u bazi!");
                    }
                }
                else
                {
                    return BadRequest(ModelState);
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
        public IActionResult DeleteDetaljiIsporuke(int id)
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
                    return BadRequest("Porudzbina koju zelite da proslijedite kao strani kljuc nije pronadjena u bazi!");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }

        [HttpPost("detaljiPorudzbine")]
        public ActionResult<DetaljiIsporukeDto> CreateDetaljiIsporukeForPorudzbina([FromBody] DetaljiIsporukePorudzbinaDto detaljiIsporuke)
        {
            if (ModelState.IsValid)
            {
                
                if (!validationRepository.ValidateAddress(detaljiIsporuke.Adresa))
                {
                    return BadRequest("Adresa bi trebalo da sadrži naziv ulice i broj.");
                }
                if (!validationRepository.ValidatePhoneNumber(detaljiIsporuke.BrojTelefona))
                {
                    return BadRequest("Broj telefona mora biti broj(+ znak je dozvoljen) i sadržati maksimalno 13 cifara!");
                }
                if (!validationRepository.ValidateCountry(detaljiIsporuke.Drzava))
                {
                    return BadRequest("Drzave u kojima je moguca isporuka su Srbija, BiH, Crna Gora i Hrvatska!");
                }
                if (!validationRepository.ValidateRegion(detaljiIsporuke.Region))
                {
                    return BadRequest("Regioni koji su podrzani su: \"Vojvodina\", \"Beograd\", \"Šumadija\", \"Zapadna Srbija\", \"Južna Srbija\", \"Istočna Srbija\",\r\n        // Hrvatska\r\n        \"Zagreb\", \"Sjeverna Hrvatska\", \"Središnja Hrvatska\", \"Istočna Hrvatska\", \"Zapadna Hrvatska\", \"Južna Hrvatska\",\r\n        // Crna Gora\r\n        \"Crnogorsko Primorje\", \"Crnogorsko Gorje\", \"Crnogorsko Polje\", \"Centralna Crna Gora\", \"Crnogorsko Primorje\", \"Južna Crna Gora\",\r\n        // Bosna i Hercegovina\r\n        \"Sarajevo\", \"Sjeveroistočna Bosna\", \"Posavina\", \"Srednja Bosna\", \"Hercegovina-Zapad\", \"Hercegovina-Jug\", \"Zapadnohercegovački kanton\"");
                }
                if (!validationRepository.ValidateCity(detaljiIsporuke.Grad))
                {
                    return BadRequest("Gradovi koji su podrzani su:  // Srbija\r\n        \"Novi Sad\", \"Beograd\", \"Niš\", \"Kragujevac\", \"Subotica\", \"Čačak\",\r\n        // Hrvatska\r\n        \"Zagreb\", \"Split\", \"Rijeka\", \"Osijek\", \"Zadar\", \"Dubrovnik\",\r\n        // Crna Gora\r\n        \"Podgorica\", \"Nikšić\", \"Pljevlja\", \"Cetinje\", \"Bar\", \"Herceg Novi\",\r\n        // Bosna i Hercegovina\r\n        \"Trebinje\",\"Sarajevo\", \"Banja Luka\", \"Prijedor\", \"Doboj\", \"Modrica\", \"Gacko\"");
                }
                var detalji = detaljiIsporukeRepository.CreateDetaljiForKorpa(detaljiIsporuke);
                return Ok(detalji);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
