using Domain.Entity;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repository
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly AppDbContext _context;
        public UserDataRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserData>> ListAsync(ISpecification<UserData> spec)
        {
            IQueryable<UserData> query = _context.UserData.AsQueryable();

            if (spec.Criterias.Any())
            {
                foreach (var criteria in spec.Criterias)
                {
                    query = query.Where(criteria);
                }
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.GroupBy != null)
            {
                query = query.GroupBy(spec.GroupBy).SelectMany(x => x);
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return await query.ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<UserData> entities)
        {
            await _context.UserData.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveAsync(UserData user)
        {
            _context.UserData.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserData?> GetByIdAsync(int id)
        {
            return await _context.UserData.FindAsync(id);
        }

        public async Task<UserData> UpdateAsync(UserData entity)
        {
            _context.UserData.Update(entity);
            await _context.SaveChangesAsync();

            return entity;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
