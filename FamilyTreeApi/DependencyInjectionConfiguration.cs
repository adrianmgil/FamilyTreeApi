using FamilyTreeApi.DataAccess;
using FamilyTreeApi.Services;

namespace FamilyTreeApi.DependencyInjection
{
    public static class DependencyInjectionConfiguration
    {
        public static void RegisterDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<IFamilyDataAccess, FamilyDataAccess>();
            services.AddScoped<IFamilyServices, FamilyServices>();
        }
    }
}
