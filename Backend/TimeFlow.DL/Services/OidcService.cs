using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TimeFlow.DAL.Models;
using TimeFlow.DL.Repositories;

namespace TimeFlow.DL.Services
{
    public class OidcService : IOidcService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public OidcService(
            IAccountRepository accountRepository,
            IBaseRepository<User> userRepository,
            IHttpClientFactory httpClientFactory,
            IConfiguration config)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<ResponseMessage> RegisterUserAsync(string accessToken)
        {
            var response = new ResponseMessage();
            var httpClient = _httpClientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7133/connect/userinfo");
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);

            var res = await httpClient.SendAsync(request);

            if (!res.IsSuccessStatusCode)
                return response;

            var json = await res.Content.ReadAsStringAsync();

            var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(json);

            var email = claims.ContainsKey("email") ? claims["email"]?.ToString() : null;

            var user = await _accountRepository.GetByEmailAsync(email);

            if(user == null)
            {
                string username = GenerateRandomUsername();
                user = new AppUser
                {
                    UserName = username,
                    Email = email,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                };

                var result = await _accountRepository.CreateWithOnlyUserAsync(user);
                if (!result.Succeeded)
                    return response;

                user = await _accountRepository.GetByEmailAsync(email);
            }

            response.Success = true;
            response.Message = await TokenGenerator(user);
            return response;
        }

        private async Task<string> TokenGenerator(AppUser user)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Jwt:key"]);
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = credentials,
                Subject = await GenerateClaims(user),
            };

            var token = handler.CreateToken(tokenDescriptor);
            return handler.WriteToken(token);
        }

        private async Task<ClaimsIdentity> GenerateClaims(AppUser user)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            claims.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            return claims;
        }

        private string GenerateRandomUsername()
        {
            var adjectives = new[] { "Cool", "Swift", "Mighty", "Silent", "Happy", "Lucky", "Clever" };
            var animals = new[] { "Fox", "Tiger", "Panda", "Eagle", "Wolf", "Hawk", "Bear" };
            var random = new Random();
            var adjective = adjectives[random.Next(adjectives.Length)];
            var animal = animals[random.Next(animals.Length)];
            var number = random.Next(1000, 999999);
            return $"{adjective}{animal}{number}";
        }
    }
}
