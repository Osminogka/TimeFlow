using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using TimeFlow.API.Controllers;
using TimeFlow.API.Infrastructure;
using TimeFlow.DAL.Models;
using TimeFlow.DL.Repositories;
using TimeFlow.DL.Services;

namespace Authentication.Tests
{
    public class AuthenticationControllerTests
    {
        private Mock<IAccountRepository> Repository { get; set; }
        private Mock<IBaseRepository<User>> UserRepository { get; set; }

        private AuthenticationController Controller { get; set; }

        public AuthenticationControllerTests()
        {
            var users = new List<AppUser>
            {
                new AppUser
                {
                    UserName = "Test",
                    Id = Guid.NewGuid().ToString(),
                    Email = "test@test.test"
                }

            };

            var list = new List<IdentityRole>()
            {
                new IdentityRole("Teacher"),
                new IdentityRole("Student")
            }.AsQueryable();

            var baseUsers = new List<User>
            {
                new User
                {
                    Username = "test",
                    Email = "test@gmail.com",
                    isPublic = false
                }
            };

            var claims = new List<Claim>()
            {
                new Claim("University", "DKU"),
                new Claim("Degree", "Noob")
            };

            var roles = new List<string>()
            {
                "Student"
            };

            Repository = new Mock<IAccountRepository>();
            UserRepository = new Mock<IBaseRepository<User>>();

            Repository.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) => users.SingleOrDefault(obj => obj.Email == email));

            Repository.Setup(x => x.CheckPasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.Run(() => Microsoft.AspNetCore.Identity.SignInResult.Success));

            Repository.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)
                .Callback<AppUser, string>((x, y) => users.Add(x));

            UserRepository.Setup(x => x.AddAsync(It.IsAny<User>()))
                .ReturnsAsync(1)
                .Callback<User>((x) => baseUsers.Add(x));

            var authService = new AccountService(Repository.Object, UserRepository.Object, GetTestConfiguration());
            var logger = new Mock<ILogger<AuthenticationController>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            var autoMapper = config.CreateMapper();

            Controller = new AuthenticationController(authService, Repository.Object, autoMapper, logger.Object);
        }

        [Fact]
        public async Task CanUserRegister()
        {
            //Arrange
            RegisterRequestModel model = new RegisterRequestModel
            {
                Email = "osminogka@test.test",
                Name = "Osminogka",
                Password = "Test123!"
            };

            //Act
            var result = await Controller.RegisterAsync(model);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseMessage>(okResult.Value);

            Assert.True(response.Success);
        }

        [Fact]
        public async Task CanUserLogin()
        {
            //Arrange
            LoginRequestModel model = new LoginRequestModel
            {
                Email = "test@test.test",
                Password = "Test123!"
            };

            //Act
            var result = await Controller.LoginAsync(model);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<ResponseMessage>(okResult.Value);

            Assert.Equal(true, response.Success);
        }

        private IConfiguration GetTestConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:Key", "EUt719k5GENP1pWWhrmyDldHPaKXyIa9yImWhPuqHBUlgZ10Fk"},
            // Add more settings as needed
        };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return configuration;
        }
    }
}