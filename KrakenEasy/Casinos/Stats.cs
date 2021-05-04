using System;
using System.Collections.Generic;
using System.Text;

namespace KrakenEasy.Casinos
{
    class Stats
    {
        public void STAT_VPIP(string _line) // Apuesta voluntaria
        {
            //( Total Times Voluntarily Put Money in the Pot / Total Hands Played ) * 100
        }
        public void STAT_3BET(string _line) // Aumento de la apuesta ( Total Times 3Bet on PF|F|T|R) / Total 3Bet Opportunities on PF|F|T|R ) * 100
        {

        }
        public void STAT_4BET(string _line) // Aumento de la apuesta despues de un 3BET 
        {

        }
        public void STAT_5BET(string _line) // Aumento de la apuesta despues de un 4BET
        {

        }
        public void STAT_WSD(string _line) // Ganar la mano en la muestra de Cartas [ Total Times Won Money at Showdown after Turn|River Raise / Total Times Went to Showdown after Turn|River Raise ] * 100
        {

        }
        public void STAT_WTSD(string _line) // LLegar a Showdown haciendo check en flop ( Total Times Went to Showdown / Total Times Saw The Flop ) * 100
        {

        }
        public void STAT_CC(string _line)  // Igualar apuesta (call) ( Total Hands Called / Total Hands Oportunity Raise ) * 100
        {

        }
        public void STAT_PFR(string _line) // Apuesta antes del Flop ( Total Hands Raised Pre-Flop / Total Hands Played ) * 100
        {

        }
        public void STAT_Limp(string _line) // Igualar apuestas de los juegadores denominados como ciegas  ( Total Hands Raised to Blinds/ Total oportunity Raises to blind ) * 100
        {

        }
        public void STAT_LimpR(string _line) // Resubir una apuesta de una ciega luego que ha ocurrido un Limp
        {

        }
        public void STAT_FoldFCB(string _line) //  Abandonar luego de un FCB
        {

        }
        public void STAT_FoldTCB(string _line) // Abandonar luego de un TCB
        {

        }
        public void STAT_RB(string _line) // Hacer un raises en el River ( total times raised F|T|R bet / total times faced F|T|R bet ) * 100
        {

        }
        public void STAT_FCB(string _line) //Hacer la primera apuesta del Flop luego de un PFR hecho por ti.  ( Total Times Raised Flop CBet / Total Times Faced Flop CBet ) * 100
        {

        }
        public void STAT_TCB(string _line) //Hacer la primera apuesta del Turn luego de un Raise hecho en el Flop por ti. ( Total Times Raised Turn CBet / Total Times Faced Turn CBet ) * 100
        {

        }
        public void STAT_Hands(string _line) // Cantidad de Hands almacenados por jugador
        {

        }
        public void STAT_Fold3BET(string _line) // Abandonar luego de un Re - Raises
        {

        }
        public void STAT_Fold4BET(string _line) // Abandonar luego de un 4Bet
        {

        }
        public void STAT_Fold5BET(string _line) // Abandonar luego de un 5Bet
        {

        }
        public void STAT_FDBET(string _line) // Interrumpir un Continuation Bet con un Raise / interrumpir FCBET / Oportunidad de interrumpir FCBET / 100  
        {

        }
    }
}
