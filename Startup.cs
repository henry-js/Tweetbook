using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Tweetbook.Data;
using Tweetbook.Installers;
using Tweetbook.Options;

namespace Tweetbook;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

        services.InstallServicesInAssembly(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseHsts();
        }

        var swaggerConfig = new SwaggerConfig();
        Configuration.GetSection(nameof(SwaggerConfig)).Bind(swaggerConfig);

        app.UseSwagger(option => option.RouteTemplate = swaggerConfig.JsonRoute);
        app.UseSwaggerUI(option => option.SwaggerEndpoint(swaggerConfig.UIEndpoint, swaggerConfig.Description));

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        // app.UseAuthentication();

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}
