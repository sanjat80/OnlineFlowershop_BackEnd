﻿using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.TipKorisnikaData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.TipKorisnikaModel;
using Microsoft.AspNetCore.Mvc;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/tipoviKorisnika")]
    [Produces("application/json","application/xml")]
    public class TipKorisnikaController : ControllerBase
    {
        private readonly ITipKorisnikaRepository tipRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public TipKorisnikaController(ITipKorisnikaRepository tipRepository,LinkGenerator linkGenerator,IMapper mapper)
        {
            this.tipRepository = tipRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<List<TipKorisnikaDto>> GetAllTipKorisnika()
        {
            var tipovi = tipRepository.GetAllTipKorisnika();
            if(tipovi == null || tipovi.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<TipKorisnikaDto>>(tipovi));
        }

        [HttpGet("{id}")]
        public ActionResult<TipKorisnikaDto> GetTipKorisnikaById(int id)
        {
            var tip = tipRepository.GetTipKorisnikaById(id);
            if(tip == null)
            {
                return NotFound("Tip korisnika sa proslijedjenim id-em nije pronadjen.");
            }
            return Ok(mapper.Map<TipKorisnikaDto>(tip));
        }
        [HttpPost]
        [Consumes("application/json")]
        public ActionResult<TipKorisnikaConfirmationDto> CreateTipKorisnika([FromBody]TipKorisnikaCreationDto tipKOrisnika)
        {
            try
            {
                TipKorisnika tip = mapper.Map<TipKorisnika>(tipKOrisnika);
                TipKorisnikaConfirmation confirmation = tipRepository.CreateTipKorisnika(tip);
                tipRepository.SaveChanges();
                //string? location = linkGenerator.GetPathByAction("GetTipKorisnikaById", "TipKorisnika", new { tipId = confirmation.TipId });
                return Ok(tip);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja tipa korisnika.");
            } 
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTipKorisnika(int id)
        {
            try
            {
                var tipModel = tipRepository.GetTipKorisnikaById(id);
                if(tipModel == null)
                {
                    return NotFound("Tip korisnika sa proslijedjenim id-em nije pronadjen.");
                }
                tipRepository.DeleteTipKorisnika(id);
                tipRepository.SaveChanges();
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja tipa korisnika");
            }
        }

        [HttpPut]
        [Consumes("application/json")]
        public ActionResult<TipKorisnikaDto> UpdateTipKorisnika(TipKorisnikaUpdateDto tipKorisnika)
        {
            try
            {
                var stariTip = tipRepository.GetTipKorisnikaById(tipKorisnika.TipId);
                if(stariTip == null)
                {
                    return NotFound("Tip korisnika sa proslijedjenim id-em nije pronadjen.");
                }
                TipKorisnika tip = mapper.Map<TipKorisnika>(tipKorisnika);
                mapper.Map(tip, stariTip);
                tipRepository.SaveChanges();
                return Ok(mapper.Map<TipKorisnikaDto>(stariTip));
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja tipa korisnika.");
            }
        }
    }
}
