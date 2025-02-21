using TimeFlow.DAL.Models;

namespace TimeFlow.DL.Services
{
    public interface IAccountService
    {
        Task<ResponseMessage> LoginAsync(LoginRequestModel loginModel);
        Task<ResponseMessage> RegisterAsync(RegisterRequestModel model);
    }
}
