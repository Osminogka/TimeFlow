using TimeFlow.DAL.Models;

namespace TimeFlow.DL.Services
{
    public interface IOidcService
    {
        Task<ResponseMessage> RegisterUserAsync(string code);
    }
}
