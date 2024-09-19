using AutoMapper;
using BussinessLevel.Dtos;
using Domain.Entity;

namespace BussinessLevel.Profilies
{
    public class UserDataProfile : Profile
    {
        public UserDataProfile()
        {
            CreateMap<UserData, UserDataDto>()
                .ReverseMap();
            CreateMap<CsvUserData, UserData>()
                .ReverseMap();
        }
    }
}
