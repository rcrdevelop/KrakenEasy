using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrakenEasy.Modals
{
    public class Carpetas
    {
        public Carpetas() 
        { 
        
        }
        public void CarpetaDiagnostico(string Casino)
        {
            var notificationManager = new NotificationManager();
            if (Casino == "KrakenHands")
            {
                 
                notificationManager.Show(new NotificationContent
                {
                    Title = "KrakenEasy",
                    Message = "Carpeta KrakenHands no se ha encontrado, inicia KrakenEasy para generarla.",
                    Type = NotificationType.Warning
                });
            }
            else
            {

                notificationManager.Show(new NotificationContent
                {
                    Title = "KrakenEasy",
                    Message = "Carpeta " + Casino + " no se ha podido encontrar por favor actualice la ruta del mismo",
                    Type = NotificationType.Warning
                });
            }
        }
    }
}
