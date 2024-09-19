using BussinessLevel.Dtos;
using Domain.Entity;
using Domain.Filters;
using Microsoft.AspNetCore.Http;

namespace BussinessLevel.Interfaces
{
    public interface IUserDataService
    {
        Task<IEnumerable<UserData>> GetUserDataAsync(UserDataFilter filter);
        Task UploadCsvDataAsync(IFormFile file);
        Task<UserDataDto> UpdateUserDataAsync(UserDataDto updatedUserData);
        Task RemoveAsync(int id);
    }
}
