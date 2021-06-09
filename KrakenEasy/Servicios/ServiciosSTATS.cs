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
    public class ServiciosSTATS
    {

        public static void main()
        {
            MongoAccess _Access = new MongoAccess();
            _Access.Set_Hand_STAT_Actualizar();
        }
    }
}
