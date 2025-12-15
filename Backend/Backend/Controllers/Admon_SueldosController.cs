using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Admon_SueldosController : ControllerBase
    {
        // DB CONTEXT INJECTION
         private readonly DBC_admon_sueldos _context;
        public Admon_SueldosController(DBC_admon_sueldos context)
        {
            _context = context;
        }

        // GET: api/<Admon_SueldosController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Admon_SueldosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Admon_SueldosController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<AdmonSueldo> items)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // TRUNCATE antes de insertar
            await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE admon_sueldos");

            _context.AdmonSueldos.AddRange(items);
            await _context.SaveChangesAsync();

            return Ok(new { inserted = items.Count });
        }



        // PUT api/<Admon_SueldosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Admon_SueldosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
