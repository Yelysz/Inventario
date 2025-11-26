using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwt;

        public AuthController(JwtService jwt)
        {
            _jwt = jwt;
        }

        public record LoginDto(string Username, string Password);

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            // ⚠️ VALIDACIÓN TEMPORAL (puedes cambiarlo luego)
            if (dto.Username != "admin" || dto.Password != "1234")
            {
                return Unauthorized(new { message = "Credenciales inválidas" });
            }

            var token = _jwt.GenerateToken(dto.Username);

            return Ok(new { token });
        }
    }
}
