using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Services;
using Cyber360.DTOs;

namespace backend.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly NeondbContext _context;
        private readonly IMailService _mailService;

        public AuthController(NeondbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuario
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

            return Ok(new
            {
                message = "Inicio de sesión exitoso",
                usuario.IdUsuario,
                usuario.Nombre,
                usuario.Email,
                usuario.FkRol
            });
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Email == request.Email);

            // Mensaje genérico por seguridad
            if (usuario == null)
                return Ok(new { message = "Si el correo está registrado, se enviará un código." });

            // Generar código de 6 dígitos
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
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Email == request.Email);

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
            var usuario = await _context.Usuario.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null)
                return Unauthorized(new { message = "Usuario no encontrado." });

            var ahora = DateTime.UtcNow;

            if (usuario.CodigoExpira == null || ahora > usuario.CodigoExpira)
                return Unauthorized(new { message = "El código ya expiró." });

            // Hashear la nueva contraseña
            var hash = BCrypt.Net.BCrypt.HashPassword(request.NuevaContrasena);
            usuario.Contrasena = hash;

            // Limpiar el código y expiración
            usuario.CodigoRecuperacion = null;
            usuario.CodigoExpira = null;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Contraseña restablecida exitosamente." });
        }

    }
}
