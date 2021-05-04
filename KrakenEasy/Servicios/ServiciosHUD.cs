using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Windows.Threading;
using KrakenEasy.KrakenBD;
using KrakenEasy.HUDS;
namespace KrakenEasy.Servicios
{
    class ServiciosHUD : BackgroundService
    {
        private string _Id_Ventana;
        private readonly ILogger<ServiciosCasinos> _logger;

        public ServiciosHUD(ILogger<ServiciosCasinos> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                _logger.LogInformation("Worker running at: {time} HUDS", DateTimeOffset.Now);
                    MongoAccess _Access = new MongoAccess();
                    if (KrakenEasy.HUDS.Propiedades.SystemActive)
                    {
                        Limpiar_Lista_HUDS();
                    }
                    //Seguir_Ventana();
                }
                catch (Exception)
                {
                }

                await Task.Delay(8000, stoppingToken);
            }
        }
        private void Limpiar_Lista_HUDS()
        {
            MongoAccess _Access = new MongoAccess();
            foreach (string Ventana in _Access.Get_Ventanas_Inactivas_Kraken())
            {
                //_Access.Set_Mostrar_False(Ventana);
                _Access.HUDS_Delete(Ventana);
            } 
                
        }
        //private void Seguir_Ventana()
        //{
        //    MongoAccess _Access = new MongoAccess();
        //    foreach (string[] Ventana in _Access.Get_HUDS_List_Kraken())
        //    {
        //        Console.WriteLine("Seguir ventana");
        //        string Posicion = _Access.Get_Ventana_Posicion_Kraken(Ventana[1]);
        //        Console.WriteLine("Asignada posicion");
        //        int x = int.Parse(Posicion.Split(" ")[0]);
        //        int y = int.Parse(Posicion.Split(" ")[1]);
        //        int w = int.Parse(Posicion.Split(" ")[3]) + x;
        //        int h = int.Parse(Posicion.Split(" ")[4]) + y;
        //        //Win32.MoveWindow((IntPtr)Int32.Parse(Ventana[0]), x, y, (int)System.Windows.SystemParameters.PrimaryScreenWidth, (int)System.Windows.SystemParameters.PrimaryScreenHeight, true);
        //        //Win32.MoveWindow(new IntPtr(long.Parse(Ventana[0])), x, y, w, h, false);
        //        Console.WriteLine("No entiendo");
        //    }

        //}
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
