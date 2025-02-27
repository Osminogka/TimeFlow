using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimeFlow.DAL.Dtos;
using TimeFlow.DAL.Models;
using TimeFlow.DAL.SideModels;
using TimeFlow.DL.Repositories;

namespace TimeFlow.DL.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Transaction> _transactionRepository;
        private readonly IBaseRepository<Category> _categoryRepository;
        private readonly IBaseRepository<FriendRequest> _friendRequestsRepository;
        private readonly IMapper _mapper;

        public TransactionService(IBaseRepository<User> userRepository, IBaseRepository<Transaction> transactionRepository, 
            IBaseRepository<Category> categoryRepository, IBaseRepository<FriendRequest> friendRequestsRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _categoryRepository = categoryRepository;
            _friendRequestsRepository = friendRequestsRepository;
            _mapper = mapper;
        }

        public async Task<ResponseMessage> CreateTransactionAsync(CreateTransactionDto createTransactionDto, string userEmail)
        {
            ResponseMessage response = new ResponseMessage();

            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if(user == null)
            {
                response.Message = "User not found";
                return response;
            }

            var category = await _categoryRepository.SingleOrDefaultAsync(obj => obj.Name == createTransactionDto.Category);
            if(category == null)
            {
                response.Message = "Such category doesn't exist";
                return response;
            }

            Transaction transaction = new Transaction
            {
                Amount = createTransactionDto.Amount,
                CategoryId = category.Id,
                Date = DateTime.Now,
                UserId = user.Id,
                Description = createTransactionDto.Description,
            };
            var result = await _transactionRepository.AddAsync(transaction);
            if (result != 1)
                return response;

            response.Success = true;
            response.Message = "Transaction successfully created";
            return response;
        }

        public async Task<ResponseList<ReadTransactionDto>> GetRecentTransactions(string userEmail, int page)
        {
            ResponseList<ReadTransactionDto> response = new ResponseList<ReadTransactionDto>();
            int pageSize = 20;

            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if(user == null)
            {
                response.Message = "Such user doesn't exist";
                return response;
            }

            var transactions = await _transactionRepository
                .Where(obj => obj.UserId == user.Id)
                .OrderByDescending(obj => obj.Date)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
            var categories = await _categoryRepository.GetAllAsync();

            foreach (Transaction transaction in transactions)
            {
                ReadTransactionDto readTransactionDto = new ReadTransactionDto
                {
                    Amount = transaction.Amount,
                    Category = categories.SingleOrDefault(obj => obj.Id == transaction.CategoryId)?.Name ?? "Unknown",
                    Date = transaction.Date,
                    Description = transaction.Description
                };
                response.Enum.Add(readTransactionDto);
            }

            response.Success = true;
            response.Message = "Got all transaction for this month";
            return response;
        }

        public async Task<ResponseList<ReadTransactionDto>> GetTransactionsForSelfAsync(string userEmail, int month, int year)
        {
            ResponseList<ReadTransactionDto> response = new ResponseList<ReadTransactionDto>();

            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if (user == null)
            {
                response.Message = "User not found";
                return response;
            }

            var transactions = await _transactionRepository.Where(obj => obj.UserId == user.Id && obj.Date.Month == month && obj.Date.Year == year).ToListAsync();
            var categories = await _categoryRepository.GetAllAsync();

            foreach(Transaction transaction in transactions)
            {
                ReadTransactionDto readTransactionDto = new ReadTransactionDto
                {
                    Amount = transaction.Amount,
                    Category = categories.SingleOrDefault(obj => obj.Id == transaction.CategoryId)?.Name ?? "Unknown",
                    Date = transaction.Date,
                    Description = transaction.Description
                };
                response.Enum.Add(readTransactionDto);
            }

            response.Success = true;
            response.Message = "Got all transaction for this month";
            return response;
        }

        public async Task<ResponseList<ReadFriendTransactionDto>> GetTransactionsForFriendAsync(string userEmail, string friendUsername, int month, int year)
        {
            ResponseList<ReadFriendTransactionDto> response = new ResponseList<ReadFriendTransactionDto>();

            var user = await _userRepository.SingleOrDefaultAsync(obj => obj.Email == userEmail);
            if (user == null)
            {
                response.Message = "User(you) not found";
                return response;
            }

            var friend = await _userRepository.SingleOrDefaultAsync(obj => obj.Username == friendUsername);
            if (friend == null)
            {
                response.Message = "User(friend) not found";
                return response;
            }

            var friendCheck = await _friendRequestsRepository.SingleOrDefaultAsync(obj => ((obj.ReceiverId == user.Id && obj.SenderId == friend.Id) 
                || (obj.ReceiverId == friend.Id && obj.SenderId == user.Id)) 
                && obj.IsAccepted == true);
            if(friendCheck == null)
            {
                response.Message = "You are not friends";
                return response;
            }

            var transactions = await _transactionRepository.Where(obj => obj.UserId == friend.Id && obj.Date.Month == month && obj.Date.Year == year).ToListAsync();
            var categories = await _categoryRepository.GetAllAsync();

            foreach (Transaction transaction in transactions)
            {
                ReadFriendTransactionDto readTransactionDto = new ReadFriendTransactionDto
                {
                    Amount = transaction.Amount,
                    Category = categories.SingleOrDefault(obj => obj.Id == transaction.CategoryId)!.Name,
                    Date = transaction.Date
                };
                response.Enum.Add(readTransactionDto);
            }

            response.Success = true;
            response.Message = "Got all transaction for this month";
            return response;
        }
    }
}
