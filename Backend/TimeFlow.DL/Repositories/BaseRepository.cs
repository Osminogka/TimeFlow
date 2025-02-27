﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TimeFlow.DAL.Contexts;
using TimeFlow.DAL.Models;

namespace TimeFlow.DL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly DataContext _context;

        private readonly DbSet<T> _entities;

        public BaseRepository(DataContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        public DataContext GetContext()
        {
            return _context;
        }

        public async Task<int> AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("", "Input data is null");
            await _entities.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("", "Input data is null");

            var oldEntity = await _context.FindAsync<T>(entity.Id);
            if (oldEntity == null)
                throw new ArgumentNullException("", "Input data is null");
            _context.Entry(oldEntity).CurrentValues.SetValues(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException("", "Input data is null");

            _entities.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(long id)
        {
            return await _entities.SingleOrDefaultAsync(obj => obj.Id == id);
        }

        public IQueryable<T> Where(System.Linq.Expressions.Expression<Func<T, bool>> exp)
        {
            return _entities.Where(exp);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _entities.Any(predicate);
        }

        public async Task<T?> SingleOrDefaultAsync(System.Linq.Expressions.Expression<Func<T, bool>> exp)
        {
            return await _entities.SingleOrDefaultAsync(exp);
        }

        public async Task<int> DeleteRangeAsync(List<T> entities)
        {
            _context.RemoveRange(entities);
            return await _context.SaveChangesAsync();
        }
        
        public async Task<List<User>> GetNonFriendsAsync(long userId, int page, int pageSize)
        {
            return await _context.Users
                .Where(u => u.Id != userId &&
                            !_context.FriendRequests.Any(fr =>
                                ((fr.SenderId == userId && fr.ReceiverId == u.Id) ||
                                (fr.ReceiverId == userId && fr.SenderId == u.Id)) && fr.IsAccepted))
                .OrderBy(u => u.Username)
                .Skip(page * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
        
        public async Task<List<User>> GetNonFriendsAsyncByName(long userId, string friendName)
        {
            return await _context.Users
                .Where(u => u.Id != userId && u.Username == friendName &&
                            !_context.FriendRequests.Any(fr =>
                                ((fr.SenderId == userId && fr.ReceiverId == u.Id) ||
                                (fr.ReceiverId == userId && fr.SenderId == u.Id)) && fr.IsAccepted))
                .OrderBy(u => u.Username)
                .ToListAsync();
        }
    }
}
