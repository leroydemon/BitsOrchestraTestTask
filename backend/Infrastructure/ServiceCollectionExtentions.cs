using BussinessLevel.Interfaces;
using BussinessLevel.Profilies;
using BussinessLevel.Services;
using Domain.Interfaces;
using Domain.Repository;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Validators;

namespace Infrastructure
{
    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection ServiceCollections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddAutoMapper(typeof(UserDataProfile).Assembly);
            services.AddAuthentication();
            services.AddAuthorization();
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(typeof(UserDtoValidator).Assembly);

            services.AddScoped<IUserDataService, UserDataService>();
            services.AddScoped<IUserDataRepository, UserDataRepository>();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
