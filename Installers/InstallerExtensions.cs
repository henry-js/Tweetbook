namespace Tweetbook.Installers;

public static class InstallerExtensions
{
    public static void InstallServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
    {
        var installerTypes = typeof(Startup).Assembly.ExportedTypes
        .Where(predicate: t =>
            typeof(IInstaller).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
        .ToList();

        var installers = installerTypes.Select(Activator.CreateInstance)
                                       .Cast<IInstaller>()
                                       .ToList();
        installers.ForEach(installer => installer.InstallServices(services, configuration));
    }
}
