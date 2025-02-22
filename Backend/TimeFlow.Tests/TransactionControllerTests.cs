using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TimeFlow.API.Infrastructure;
using TimeFlow.DAL.Contexts;
using TimeFlow.DAL.Dtos;
using TimeFlow.DAL.Models;
using TimeFlow.DL.Repositories;
using TimeFlow.DL.Services;

namespace TimeFlow.Tests
{
    public class TransactionControllerTests
    {
        private IBaseRepository<User> UserRepository { get; set; }
        private IBaseRepository<Transaction> TransactionRepository { get; set; }
        private IBaseRepository<Category> CategoryRepository { get; set; }
        private IBaseRepository<FriendRequest> FriendRequestsRepository { get; set; }
        private IMapper Mapper { get; set; }

        private ServiceProvider ServiceProvider { get; set; }

        private TransactionService Service { get; set; }

        public TransactionControllerTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<DataContext>(options =>
                options.UseSqlite("Filename=TransactionsDatabase.db"));

            services.AddTransient<IBaseRepository<User>, BaseRepository<User>>();
            services.AddTransient<IBaseRepository<Transaction>, BaseRepository<Transaction>>();
            services.AddTransient<IBaseRepository<Category>, BaseRepository<Category>>();
            services.AddTransient<IBaseRepository<FriendRequest>, BaseRepository<FriendRequest>>();

            ServiceProvider = services.BuildServiceProvider();

            var scope = ServiceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;

            UserRepository = scopedServices.GetRequiredService<IBaseRepository<User>>();
            TransactionRepository = scopedServices.GetRequiredService<IBaseRepository<Transaction>>();
            CategoryRepository = scopedServices.GetRequiredService<IBaseRepository<Category>>();
            FriendRequestsRepository = scopedServices.GetRequiredService<IBaseRepository<FriendRequest>>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            var autoMapper = config.CreateMapper();

            Service = new TransactionService(UserRepository, TransactionRepository, CategoryRepository, FriendRequestsRepository, autoMapper);

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

            FriendRequest friendRequest = new FriendRequest
            {
                SenderId = 1,
                ReceiverId = 2,
                IsAccepted = true,
                SentAt = DateTime.Now
            };
            context.Add(friendRequest);

            Transaction transaction = new Transaction
            {
                Amount = 1000,
                CategoryId = 3,
                UserId = 1,
                Date = DateTime.Now,
                Description = "Bought spider-man 2"
            };
            context.Add(transaction);

            context.Categories.AddRange(new List<Category>
                {
                    new Category { Name = "Food" },
                    new Category { Name = "Transport" },
                    new Category { Name = "Entertainment" },
                    new Category { Name = "Health" },
                    new Category { Name = "Education" },
                    new Category { Name = "Other" }
                });

            context.SaveChanges();
        }

        [Fact]
        public async Task CreateTransactionTest()
        {
            //Arrange
            string userEmail = "osminogka@gmail.com";
            DateTime time = DateTime.Now;
            CreateTransactionDto transaction = new CreateTransactionDto
            {
                Amount = 2000,
                Category = "Food",
                Date = time,
                Description = "Dodo"
            };

            //Act
            var result = await Service.CreateTransactionAsync(transaction, userEmail);
            var result2 = await Service.GetTransactionsForSelfAsync(userEmail, time.Month, time.Year);

            //Assert
            Assert.True(result.Success);
            Assert.Equal(2, result2.Enum.ToArray().Length);
        }

        [Fact]
        public async Task GetTransactionsForSelfTest()
        {
            //Arrange
            string userEmail = "osminogka@gmail.com";
            DateTime time = DateTime.Now;

            //Act
            var result = await Service.GetTransactionsForSelfAsync(userEmail, time.Month, time.Year);

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Enum);
        }

        [Fact]
        public async Task GetTransactionsForFriendTest()
        {
            //Arrange
            string userEmail = "redter@gmail.com";
            string friendUsername = "Osminogka";
            string hacker = "hacker@gmail.com";
            DateTime time = DateTime.Now;

            //Act
            var result = await Service.GetTransactionsForFriendAsync(userEmail, friendUsername, time.Month, time.Year);
            var result2 = await Service.GetTransactionsForFriendAsync(hacker, friendUsername, time.Month, time.Year);

            //Assert
            Assert.True(result.Success);
            Assert.Single(result.Enum);

            Assert.False(result2.Success);
            Assert.Equal("You are not friends", result2.Message);
        }
    }
}
