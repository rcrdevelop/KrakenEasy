using Notification.Wpf;
using System;

namespace KrakenEasy.Hands
{
    public class main_Manejeador_Manos
    {
        public void Iniciar()
        {
            Ficheros _Ficheros = new Ficheros();
            _Ficheros.ImportarHands();
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = "KrakenEasy",
                Message = "Manos importadas correctamente",
                Type = NotificationType.Success
            });
        }
    }
}
