using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimeFlow.DAL.Contexts;
using TimeFlow.DAL.Models;
using TimeFlow.DL.Repositories;
using TimeFlow.DL.Services;

namespace TimeFlow.Tests
{
    public class FriendRequestServiceTests
    {
        private IBaseRepository<User> UserRepository { get; set; }
        private IBaseRepository<FriendRequest> FriendRequestsRepository { get; set; }

        private ServiceProvider ServiceProvider { get; set; }

        private FriendRequestService Service { get; set; }

        public FriendRequestServiceTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlite("Filename=FriendsRequestsDatabase.db"));

            services.AddTransient<IBaseRepository<User>, BaseRepository<User>>();
            services.AddTransient<IBaseRepository<Transaction>, BaseRepository<Transaction>>();
            services.AddTransient<IBaseRepository<Category>, BaseRepository<Category>>();
            services.AddTransient<IBaseRepository<FriendRequest>, BaseRepository<FriendRequest>>();

            ServiceProvider = services.BuildServiceProvider();

            var scope = ServiceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;

            UserRepository = scopedServices.GetRequiredService<IBaseRepository<User>>();
            FriendRequestsRepository = scopedServices.GetRequiredService<IBaseRepository<FriendRequest>>();

            Service = new FriendRequestService(UserRepository, FriendRequestsRepository);

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

            var user4 = new User
            {
                Id = 4,
                Username = "Noname",
                Email = "noname@gmail.com"
            };
            context.Add(user4);

            FriendRequest friendRequest = new FriendRequest
            {
                SenderId = 1,
                ReceiverId = 2,
                IsAccepted = true,
                SentAt = DateTime.Now
            };
            context.Add(friendRequest);

            FriendRequest friendRequest2 = new FriendRequest
            {
                SenderId = 3,
                ReceiverId = 1,
                IsAccepted = false,
                SentAt = DateTime.Now
            };
            context.Add(friendRequest2);

            context.SaveChanges();
        }

        [Fact]
        public async Task GetFriendRequestsTest()
        {
            //Arrange
            string userEmail = "osminogka@gmail.com";

            //Act
            var result = await Service.GetFriendRequestsAsync(userEmail);

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Enum);
            Assert.Equal("Hacker", result.Enum[0]);
        }

        [Fact]
        public async Task GetFriendsListTest()
        {
            //Arrange
            string userEmail = "osminogka@gmail.com";

            //Act
            var result = await Service.GetFriendsListAsync(userEmail);

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Enum);
            Assert.Equal("Redter", result.Enum[0]);
        }

        [Fact]
        public async Task SendFriendRequestTest()
        {
            //Arrange
            string userEmail = "osminogka@gmail.com";
            string receiverUsername = "Noname";

            //Act
            var result = await Service.SendFriendRequestAsync(userEmail, receiverUsername);
            var result2 = await Service.GetFriendRequestsAsync("noname@gmail.com");

            //Assert
            Assert.True(result.Success);
            Assert.True(result2.Success);
            Assert.Single(result2.Enum);

        }

        [Fact]
        public async Task AcceptFriendRequestTest()
        {
            //Arrange
            string userEmail = "osminogka@gmail.com";
            string senderUsername = "Hacker";

            //Act
            var result = await Service.AcceptFriendRequestAsync(userEmail, senderUsername);
            var result2 = await Service.GetFriendsListAsync(userEmail);

            //Assert
            Assert.True(result.Success);
            Assert.True(result2.Success);
            Assert.Equal(2, result2.Enum.ToArray().Length);
        }

        [Fact]
        public async Task RejectFriendRequestTest()
        {
            //Arrange
            string userEmail = "osminogka@gmail.com";
            string senderUsername = "Hacker";

            //Act
            var result = await Service.RejectFriendRequestAsync(userEmail, senderUsername);
            var result2 = await Service.GetFriendsListAsync(userEmail);

            //Assert
            Assert.True(result.Success);
            Assert.True(result2.Success);
            Assert.Single(result2.Enum);
        }
    }
}
