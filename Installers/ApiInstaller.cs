using Microsoft.OpenApi.Models;

namespace Tweetbook.Installers;

public class ApiInstaller : IInstaller
{
    public void InstallServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers(); // TODO : switch back to using AddControllersWithViews if bugs occur
        services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo { Title = "Tweetbook API", Version = "v1" }));
    }
}
