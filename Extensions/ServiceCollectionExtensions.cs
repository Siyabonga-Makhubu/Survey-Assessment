using Microsoft.EntityFrameworkCore;
using SurveyApi.Data;
using SurveyApi.Services;

namespace SurveyApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSurveyServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext
            services.AddDbContext<SurveyDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Add services
            services.AddScoped<ISurveyService, SurveyService>();

            return services;
        }
    }
}
