using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KrakenEasy.HUDS
{
    public class Propiedades
    {
        public static double Opacity { get; set; }
        public static double Size { get; set; }
        public static int Relative { get; set; }
        public static bool Change { get; set; }
        public static bool SystemActive { get; set; }
        public static bool Actualizar { get; set; }
    }
}
