﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TimeFlow.DAL.Models;

namespace TimeFlow.DL.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IBaseRepository<User> _userRepository;

        public AccountRepository(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, IBaseRepository<User> userRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        public IQueryable<AppUser> Get() => _userManager.Users;

        public async Task<AppUser?> GetByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                User baseUser = new User
                {
                    Username = user.UserName,
                    Email = user.Email
                };
                await _userRepository.AddAsync(baseUser);

                return result;
            }
            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(AppUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateAsync(AppUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public UserManager<AppUser> GetUserManager()
        {
            return _userManager;
        }

        public async Task<SignInResult> CheckPasswordSignInAsync(AppUser user, string password, bool lockoutOnFailure)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure);
        }
    }
}
