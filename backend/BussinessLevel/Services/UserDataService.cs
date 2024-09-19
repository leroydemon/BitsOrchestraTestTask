using CsvHelper.Configuration;
using CsvHelper;
using Domain.Entity;
using Domain.Filters;
using Domain.Specification;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using Domain.Interfaces;
using BussinessLevel.Dtos;
using AutoMapper;
using BussinessLevel.Interfaces;

namespace BussinessLevel.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserDataRepository _userDataRepository;
        private readonly IMapper _mapper;

        public UserDataService(IUserDataRepository userDataRepository, IMapper mapper)
        {
            _userDataRepository = userDataRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserData>> GetUserDataAsync(UserDataFilter filter)
        {
            var specification = new UserDataSpecification(filter);
            return await _userDataRepository.ListAsync(specification);
        }

        public async Task UploadCsvDataAsync(IFormFile file)
        {
            using var stream = new StreamReader(file.OpenReadStream());
            using var csv = new CsvReader(stream, new CsvConfiguration(CultureInfo.InvariantCulture));

            try
            {
                var records = csv.GetRecords<CsvUserData>(); 
                var mapped = _mapper.Map<IEnumerable<UserData>>(records);
                await _userDataRepository.AddRangeAsync(mapped); 
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error processing CSV file: {ex.Message}", ex);
            }
        }

        public async Task<UserDataDto> UpdateUserDataAsync(UserDataDto updatedUserData)
        {
            var mapped = _mapper.Map<UserData>(updatedUserData);
            var updated = await _userDataRepository.UpdateAsync(mapped);

            return _mapper.Map<UserDataDto>(updated);
        }

        public async Task RemoveAsync(int id)
        {
            var user = await _userDataRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {id} does not exist.");
            }

            await _userDataRepository.RemoveAsync(user);
        }
    }
}
