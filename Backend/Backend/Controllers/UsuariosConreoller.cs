using Backend.Data;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosConreoller : ControllerBase
    {
        private readonly DBC_usuarios _context;

        public UsuariosConreoller(DBC_usuarios context)
        {
            _context = context;
        }

        // GET: api/<UsuariosConreoller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsuariosConreoller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsuariosConreoller>
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest model)
        {
            if (model.UsuarioId == 0 || string.IsNullOrEmpty(model.Password))
            {
                return BadRequest("Datos incompletos");
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u =>
                    u.UsuarioId == model.UsuarioId &&
                    u.Password == model.Password);

            if (usuario == null)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = "Usuario o contraseña incorrectos"
                });
            }

            return Ok(new
            {
                success = true,
                usuario = new
                {
                    usuario.UsuarioId,
                    usuario.Nombres,
                    usuario.ApellidoPaterno,
                    usuario.ApellidoMaterno,
                    usuario.Puesto,
                    usuario.NominaId,
                    usuario.RolId
                }
            });
        }

        // PUT api/<UsuariosConreoller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsuariosConreoller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class LoginRequest
    {
        public int UsuarioId { get; set; } = 0;
        public string Password { get; set; } = string.Empty;
    }

}
