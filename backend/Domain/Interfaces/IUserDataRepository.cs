using Domain.Entity;

namespace Domain.Interfaces
{
    public interface IUserDataRepository
    {
        Task<IEnumerable<UserData>> ListAsync(ISpecification<UserData> spec);
        Task SaveChangesAsync();
        Task<UserData?> GetByIdAsync(int id);
        Task RemoveAsync(UserData user);
        Task<UserData> UpdateAsync(UserData entity);
        Task AddRangeAsync(IEnumerable<UserData> entities);
    }
}
