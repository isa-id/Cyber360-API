namespace backend.Services
{
    public interface IMailService
    {
        Task SendPasswordResetCode(string email, string nombre, string codigo);
    }
}
