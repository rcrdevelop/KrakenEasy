using System;
using System.Collections.Generic;
using System.Text;

namespace KrakenEasy.KrakenBD
{
    public class Settings
    {
        public static bool MongoCloud { get; set; }
        public static void Inicializar() 
        {
            MongoCloud = true;
        }
    }
}
