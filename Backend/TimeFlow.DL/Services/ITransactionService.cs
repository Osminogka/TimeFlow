using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeFlow.DAL.Dtos;
using TimeFlow.DAL.Models;
using TimeFlow.DAL.SideModels;

namespace TimeFlow.DL.Services
{
    public interface ITransactionService
    {
        Task<ResponseMessage> CreateTransactionAsync(CreateTransactionDto createTransactionDto, string userEmail);
        Task<ResponseList<ReadTransactionDto>> GetTransactionsForSelfAsync(string userEmail, int month, int year);
        Task<ResponseList<ReadFriendTransactionDto>> GetTransactionsForFriendAsync(string userEmail, string friendUsername, int month, int year);
    }
}
