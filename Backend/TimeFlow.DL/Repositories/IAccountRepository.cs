using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TimeFlow.DAL.Models;

namespace TimeFlow.DL.Repositories
{
    public interface IAccountRepository
    {
        IQueryable<AppUser> Get();
        Task<AppUser?> GetByEmailAsync(string email);
        Task<IdentityResult> CreateAsync(AppUser user, string password);
        Task<IdentityResult> DeleteAsync(AppUser user);
        Task<IdentityResult> UpdateAsync(AppUser user);
        UserManager<AppUser> GetUserManager();
        Task<SignInResult> CheckPasswordSignInAsync(AppUser user, string password, bool lockoutOnFailure);
    }
}
