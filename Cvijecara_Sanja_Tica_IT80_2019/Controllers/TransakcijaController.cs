using AutoMapper;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.TransakcijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;
using Cvijecara_Sanja_Tica_IT80_2019.Models.KategorijaModel;
using Cvijecara_Sanja_Tica_IT80_2019.Models.TransakcijaModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Cvijecara_Sanja_Tica_IT80_2019.Controllers
{
    [ApiController]
    [Route("api/transakcije")]
    [Produces("application/json","application/xml")]
    [Authorize(Roles ="admin")]
    public class TransakcijaController:ControllerBase
    {
        private readonly ITransakcijaRepository transakcijaRepository;
        private readonly LinkGenerator linkGenerator;
        private readonly IMapper mapper;

        public TransakcijaController(ITransakcijaRepository transakcijaRepository,LinkGenerator linkGenerator, IMapper mapper)
        {
            this.transakcijaRepository = transakcijaRepository;
            this.linkGenerator = linkGenerator;
            this.mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult<List<TransakcijaDto>> GetAllTransakcija()
        {
            var transakcije = transakcijaRepository.GetAllTransakcija();
            if (transakcije == null || transakcije.Count == 0)
            {
                return NoContent();
            }
            return Ok(mapper.Map<List<TransakcijaDto>>(transakcije));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<TransakcijaDto> GetTransakcijaById(int id)
        {
            var transakcija = transakcijaRepository.GetTransakcijaById(id);
            if (transakcija == null)
            {
                return NotFound("Transakcija sa proslijedjenim id-em nije pronadjena.");
            }
            return Ok(mapper.Map<TransakcijaDto>(transakcija));
        }

        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TransakcijaConfirmationDto> CreateTransakcija([FromBody] TransakcijaCreationDto transakcija)
        {
            try
            { 
               Transakcija tr = mapper.Map<Transakcija>(transakcija);
                var porudzbina = tr.PorudzbinaId;
                var porudzbine = transakcijaRepository.GetAllPorudzbinaId();
                if(porudzbine.Contains(porudzbina))
                {
                    TransakcijaConfirmation confirmation = transakcijaRepository.CreateTransakcija(tr);
                    transakcijaRepository.SaveChanges();
                    //string? location = linkGenerator.GetPathByAction("GetTransakcijaById", "Transakcija", new { transakcijaId = confirmation.TransakcijaId });
                    return Ok(confirmation);
                }
                else
                {
                    return BadRequest("Porudzbina koju zelite da proslijedite kao strani kljuc nije pronadjena u bazi!");
                }
            }
            catch(Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Transakcija koja se odnosi na porudzbinu sa proslijedjenim id-em je vec kreirana, ili porudbzina ne postoji u bazi!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom kreiranja transakcije");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteTransakcija(int id)
        {
            try
            {
                var transakcijaModel = transakcijaRepository.GetTransakcijaById(id);
                if (transakcijaModel == null)
                {
                    return NotFound("Transakcija sa proslijedjenim id-em nije pronadjena.");
                }
                transakcijaRepository.DeleteTransakcija(id);
                transakcijaRepository.SaveChanges();
                return NoContent();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Transakcija koja se odnosi na porudzbinu sa proslijedjenim id-em je vec kreirana, ili porudbzina ne postoji u bazi!");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom brisanja transakcije");
            }
        }
        [HttpPut]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<TransakcijaDto> UpdateTransakcija(TransakcijaUpdateDto transakcija)
        {
            try
            {
                var staraTransakcija = transakcijaRepository.GetTransakcijaById(transakcija.TransakcijaId);
                if (staraTransakcija == null)
                {
                    return NotFound("Transakcija sa proslijedjenim id-em nije pronadjena.");
                }
                Transakcija tr = mapper.Map<Transakcija>(transakcija);
                var porudzbina = tr.PorudzbinaId;
                var porudzbine = transakcijaRepository.GetAllPorudzbinaId();
                if(porudzbine.Contains(porudzbina))
                {
                    mapper.Map(tr, staraTransakcija);
                    transakcijaRepository.SaveChanges();
                    return Ok(mapper.Map<TransakcijaDto>(staraTransakcija));
                }
                else
                {
                    return BadRequest("Porudzbina koju zelite da proslijedite kao strani kljuc nije pronadjena!");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Greska prilikom azuriranja kategorije.");
            }
        }

        /*private List<int> GetAllPorudzbinaId()
        {
            List<int> porudzbine;
            string connStr = @"Data Source=DESKTOP-RCH3286\SQLEXPRESS01;Initial Catalog=Cvijecara;Integrated Security=True;TrustServerCertificate=True";
            using(SqlConnection cn = new SqlConnection(connStr))
            {
                cn.Open();
                SqlCommand cmd = new SqlCommand("SELECT PORUDZBINA_ID FROM GIFTSHOP.TRANSAKCIJA");
                cmd.Connection = cn;
                var dataReader = cmd.ExecuteReader();
                porudzbine = GetList<int>(dataReader);
            }
            return porudzbine;
        }

        /*private List<T> GetList<T>(IDataReader reader)
        {
            List<T> list = new List<T>();
            while(reader.Read())
            {
                var type = typeof(T);
                T obj = (T)Activator.CreateInstance(type);
                foreach(var prop in type.GetProperties())
                {
                    var propType = prop.PropertyType;
                    prop.SetValue(obj, Convert.ChangeType(reader[prop.Name].ToString(), propType));
                }
                list.Add(obj);
            }
            return list;
        }*/
    }
}
