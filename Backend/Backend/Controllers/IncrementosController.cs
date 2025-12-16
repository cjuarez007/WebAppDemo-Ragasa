using Backend.Data;
using Backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IncrementosController : ControllerBase
    {
        private readonly DBC_incrementos _context;
        public IncrementosController(DBC_incrementos context)
        {
            _context = context;
        }

        // GET: api/<IncrementosController>
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Incremento>>> Get()
        {
            var Data = await _context.Incrementos.ToListAsync();
            return Ok(Data);
        }

        // GET: api/incrementos/rol
        [HttpGet("jefes")]
        public async Task<ActionResult<IEnumerable<Incremento>>> GetRol1()
        {
            var data = await _context.Incrementos
                                     .Where(i => i.RolId == 1)
                                     .ToListAsync();
            return Ok(data);
        }

        // GET: api/incrementos/rol2-3
        [HttpGet("empleados")]
        public async Task<ActionResult<IEnumerable<Incremento>>> GetRol2y3()
        {
            var data = await _context.Incrementos
                                     .Where(i => i.RolId == 2 || i.RolId == 3)
                                     .ToListAsync();
            return Ok(data);
        }

        [HttpPut("updateIncremento/{nn}")]
        public async Task<IActionResult> Put(int nn, Incremento incremento)
        {
            if (nn != incremento.Nn)
                return BadRequest();

            _context.Entry(incremento).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Incrementos.Any(e => e.Nn == nn))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }



        // GET api/<IncrementosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            
            return "value";
        }

        // POST api/<IncrementosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<IncrementosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<IncrementosController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
