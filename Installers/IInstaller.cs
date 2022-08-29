namespace Tweetbook.Installers;
internal interface IInstaller
{
    void InstallServices(IServiceCollection services, IConfiguration configuration);
}
