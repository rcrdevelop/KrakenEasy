using System;

namespace KrakenEasy.Hands
{
    public class main_Manejeador_Manos
    {
        public void Iniciar()
        {
            Ficheros _Ficheros = new Ficheros();
            _Ficheros.ImportarHands();
        }
    }
}
