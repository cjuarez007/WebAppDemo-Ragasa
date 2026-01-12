
using Backend.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Porcentajes_EstandarController : ControllerBase
    {

        private readonly Data.DBC_porcentajes_estandar _context;
        public Porcentajes_EstandarController(Data.DBC_porcentajes_estandar context)
        {
            _context = context;
        }

        // GET: api/<Porcentajes_Estandar>
        [HttpGet]
        public ActionResult<IEnumerable<PorcentajesEstandar>> Get()
        {
            var datos = _context.PorcentajesEstandars.ToList();
            return Ok(datos);
        }

        // GET api/<Porcentajes_Estandar>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Porcentajes_Estandar>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Porcentajes_Estandar>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Porcentajes_Estandar>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
