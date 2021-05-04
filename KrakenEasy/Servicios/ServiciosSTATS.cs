using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Windows.Threading;
using KrakenEasy.KrakenBD;

namespace KrakenEasy.Servicios
{
    class ServiciosSTATS: BackgroundService
    {

        private readonly ILogger<ServiciosSTATS> _logger;

        public ServiciosSTATS(ILogger<ServiciosSTATS> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Worker running at: {time} Estatus Kraken", DateTimeOffset.Now);
                    MongoAccess _Access = new MongoAccess();
                    _Access.Obtener_Jugadores();
                    _Access.STATS();
                }
                catch (Exception)
                {


                }
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
        }
    }
}
