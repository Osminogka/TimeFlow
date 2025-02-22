using TimeFlow.DAL.Models;
using TimeFlow.DAL.SideModels;

namespace TimeFlow.DL.Services
{
    public interface IUserService
    {
        Task<ResponseList<string>> GetUsersAsync(string userEmail, int page);
        Task<ResponseMessage> ChangeAccountVisibilityAsync(string userEmail);
    }
}
