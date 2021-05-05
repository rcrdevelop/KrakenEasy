using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using KrakenEasy.Casinos;
using KrakenEasy.KrakenBD;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace KrakenEasy.Servicios
{
    class main_Servicios: BackgroundService
    {
        private readonly ILogger<ServiciosCasinos> _logger;

        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        public main_Servicios(IHostApplicationLifetime hostApplicationLifetime, ILogger<ServiciosCasinos> logger)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (HUDS.Propiedades.SystemActive)
                    {
                        ServiciosCasinos.main();
                        ServiciosFicheros.main();
                    }
                }
                catch (Exception ex)
                {

                }
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
        
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            return base.StopAsync(cancellationToken);
        }
  
    }
}

