using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrakenEasy.Modals
{
    public class HandsError
    {
        public HandsError()
        {

        }
        public void Error() 
        {
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = "KrakenEasy",
                Message = "El jugador seleccionado no tiene suficientes manos jugadas",
                Type = NotificationType.Error
            });
        }
    }
}
