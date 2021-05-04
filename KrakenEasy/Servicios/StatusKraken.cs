using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Diagnostics;
using KrakenEasy.Casinos;
using KrakenEasy.KrakenBD;
namespace KrakenEasy.Servicios
{
    public class StatusKraken : BackgroundService
    {
        private readonly ILogger<StatusKraken> _logger;

        public StatusKraken(ILogger<StatusKraken> logger)
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
                }
                catch (Exception)
                {

                    
                }
                await Task.Delay(TimeSpan.FromSeconds(4), stoppingToken);
            }
        }


        public List<IntPtr> Mesas_Winamax()
        {
            Win32 _Win32 = new Win32();
            Winamax _Winamax = new Winamax();
            List<System.IntPtr> _Ventanas = new List<IntPtr>();
            foreach (String KeyVentana in _Winamax.Ventanas())
            {
                foreach (var Id_Ventana in Win32.FindWindowsWithText(KeyVentana))
                {
                    _Ventanas.Add(Id_Ventana);
                }
            }
            return _Ventanas;
        }
        public List<System.IntPtr> Mesas_888Poker()
        {
            Win32 _Win32 = new Win32();
            Poker888 _888Poker = new Poker888();
            List<System.IntPtr> _Ventanas = new List<IntPtr>();
            foreach (String KeyVentana in _888Poker.Ventanas())
            {
                foreach (var Id_Ventana in Win32.FindWindowsWithText(KeyVentana))
                {
                    _Ventanas.Add(Id_Ventana);
                }
            }
            return _Ventanas;

        }
        public List<System.IntPtr> Mesas_PokerStars()
        {
            Win32 _Win32 = new Win32();
            PokerStars _PokerStars = new PokerStars();
            List<System.IntPtr> _Ventanas = new List<IntPtr>();
            foreach (String KeyVentana in _PokerStars.Ventanas())
            {
                foreach (var Id_Ventana in Win32.FindWindowsWithText(KeyVentana))
                {
                    _Ventanas.Add(Id_Ventana);
                }
            }
            return _Ventanas;

        }
    }

}
