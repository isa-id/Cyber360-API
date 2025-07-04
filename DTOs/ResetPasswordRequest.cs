namespace Cyber360.DTOs
{
    public class ResetPasswordRequest
    {
        public required string Email { get; set; }
        public required string NuevaContrasena { get; set; }
    }
}   
