using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace KrakenEasy.Servicios
{
    public class Servicios
    {
        string[] args = new string[0];
        public void main()
        {

            CreateHostBuilder(args).Build().Run();

        }


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHostedService<main_Servicios>();
                });
        public void Detener()
        {
        }
    }
}