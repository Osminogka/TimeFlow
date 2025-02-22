using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimeFlow.DAL.Contexts;
using TimeFlow.DAL.Models;
using TimeFlow.DL.Repositories;
using TimeFlow.DL.Services;

namespace TimeFlow.Tests
{
    public class UserServiceTests
    {
        private IBaseRepository<User> UserRepository { get; set; }
        private IBaseRepository<FriendRequest> FriendRequestsRepository { get; set; }
        private IMapper Mapper { get; set; }

        private ServiceProvider ServiceProvider { get; set; }

        private UserService Service { get; set; }

        public UserServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlite("Filename=UsersDatabase.db"));

            services.AddTransient<IBaseRepository<User>, BaseRepository<User>>();
            services.AddTransient<IBaseRepository<Transaction>, BaseRepository<Transaction>>();
            services.AddTransient<IBaseRepository<Category>, BaseRepository<Category>>();
            services.AddTransient<IBaseRepository<FriendRequest>, BaseRepository<FriendRequest>>();

            ServiceProvider = services.BuildServiceProvider();

            var scope = ServiceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;

            UserRepository = scopedServices.GetRequiredService<IBaseRepository<User>>();
            FriendRequestsRepository = scopedServices.GetRequiredService<IBaseRepository<FriendRequest>>();


            Service = new UserService(UserRepository, FriendRequestsRepository);

            var context = ServiceProvider.GetRequiredService<DataContext>();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.Database.Migrate();

            var user1 = new User
            {
                Id = 1,
                Username = "Osminogka",
                Email = "osminogka@gmail.com"
            };
            context.Add(user1);

            var user2 = new User
            {
                Id = 2,
                Username = "Redter",
                Email = "redter@gmail.com"
            };
            context.Add(user2);

            var user3 = new User
            {
                Id = 3,
                Username = "Hacker",
                Email = "hacker@gmail.com"
            };
            context.Add(user3);

            context.SaveChanges();
        }

        [Fact]
        public async Task GetUsersTest()
        {
            //Arrange
            string userEmail = "osminogka@gmail.com";

            //Act
            var result = await Service.GetUsersAsync(userEmail, 0);

            //Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Enum.ToArray().Length);
        }

        [Fact]
        public async Task ChangeAccountVisibilityTest()
        {
            //Arrange
            string userEmail = "osminogka@gmail.com";

            //Act
            var result = await Service.ChangeAccountVisibilityAsync(userEmail);
            var user = await UserRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);

            //Assert
            Assert.True(result.Success);
            Assert.True(user.isPublic);
        }
    }
}
