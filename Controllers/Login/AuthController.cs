using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using backend.Models;
using backend.Services;
using Cyber360.DTOs;
using backend.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace backend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly NeondbContext _context;
        private readonly IMailService _mailService;
        private readonly IConfiguration _config;

        public AuthController(NeondbContext context, IMailService mailService, IConfiguration config)
        {
            _context = context;
            _mailService = mailService;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

                if (usuario == null)
                    return Unauthorized(new { message = "Correo o contraseña incorrectos." });

                var hash = usuario.Contrasena;
                bool contraseñaValida;

                if (hash.StartsWith("$2a$") || hash.StartsWith("$2b$") || hash.StartsWith("$2y$"))
                {
                    contraseñaValida = BCrypt.Net.BCrypt.Verify(request.Contrasena, hash);
                }
                else
                {
                    contraseñaValida = request.Contrasena == hash;
                }

                if (!contraseñaValida)
                    return Unauthorized(new { message = "Correo o contraseña incorrectos." });

                // 🔑 Validar que la clave JWT exista
                var jwtKey = _config["Jwt:Key"];
                if (string.IsNullOrEmpty(jwtKey))
                {
                    return StatusCode(500, new { message = "Error interno: JWT Key no configurada en el servidor." });
                }

                // 🔑 Generar el token JWT
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(jwtKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim("idUsuario", usuario.IdUsuario.ToString()),
                new Claim("nombre", usuario.Nombre),
                new Claim("email", usuario.Email),
                new Claim("rol", usuario.FkRol.ToString())
            }),
                    Expires = DateTime.UtcNow.AddHours(3),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return Ok(new
                {
                    message = "Inicio de sesión exitoso",
                    usuario.IdUsuario,
                    usuario.Nombre,
                    usuario.Email,
                    usuario.FkRol,
                    token = tokenHandler.WriteToken(token)
                });
            }
            catch (Exception ex)
            {
                // ⚠️ Devuelve un 500 con detalles básicos (sin exponer info sensible)
                return StatusCode(500, new
                {
                    message = "Error interno en el servidor.",
                    error = ex.Message
                });
            }
        }


        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
                return Ok(new { message = "Si el correo está registrado, se enviará un código." });

            var codigo = new Random().Next(100000, 999999).ToString();

            usuario.CodigoRecuperacion = codigo;
            usuario.CodigoExpira = DateTime.UtcNow.AddMinutes(10);

            await _context.SaveChangesAsync();
            await _mailService.SendPasswordResetCode(usuario.Email, usuario.Nombre, codigo);

            return Ok(new { message = "Si el correo está registrado, se enviará un código." });
        }

        [HttpPost("verify-code")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
                return Unauthorized(new { message = "Código inválido o expirado." });

            var ahora = DateTime.UtcNow;

            if (usuario.CodigoRecuperacion != request.Codigo || usuario.CodigoExpira == null || ahora > usuario.CodigoExpira)
            {
                return Unauthorized(new { message = "Código inválido o expirado." });
            }

            return Ok(new { message = "Código válido. Proceda a cambiar su contraseña." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
                return Unauthorized(new { message = "Usuario no encontrado." });

            var ahora = DateTime.UtcNow;

            if (usuario.CodigoExpira == null || ahora > usuario.CodigoExpira)
                return Unauthorized(new { message = "El código ya expiró." });

            var hash = BCrypt.Net.BCrypt.HashPassword(request.NuevaContrasena);
            usuario.Contrasena = hash;
            usuario.CodigoRecuperacion = null;
            usuario.CodigoExpira = null;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Contraseña restablecida exitosamente." });
        }
    }
}
