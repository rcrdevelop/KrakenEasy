using System;
using System.Collections.Generic;
using System.Text;
using KrakenEasy.KrakenBD;

namespace KrakenEasy.Casinos
{
    public class PokerStars
    {
        public static string _Ruta { get; set; }
        public static bool Habilitado { get; set; }
        public static string Proceso()
        {
            string _Proceso = "PokerStars";
            return _Proceso;
        }
        public string[] Ventanas()
        {
            string[] _Ventanas = { "Omaha", "Hold" };
            return _Ventanas;
        }
        public static string Id()
        {
            return "Hand #";
        }

        public bool Identificador_HandID(String _Line)
        {
            if (_Line.Contains("Hand #".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Identificador_Mesa(String _Line)
        {
            if (_Line.Contains("Table".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Identificador_WinHand(String _Line)
        {
            if (_Line.Contains("won".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Identificador_Button(String _Line)
        {
            if (_Line.Contains("is the button".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Identificador_Players(String _Line)
        {
            if (!(_Line.Contains("is the button".ToUpper())) && _Line.Contains("Seat".ToUpper()))
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        public bool Identificador_Blinds(String _Line)
        {
            if (_Line.Contains("posts".ToUpper()) && _Line.Contains("blind".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean Identificador_Pre(String _Line)
        {
            if (_Line.Contains("HOLE CARDS".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Identificador_Flop(String _Line)
        {
            if (_Line.Contains("FLOP".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Identificador_Turn(String _Line)
        {
            if (_Line.Contains("TURN".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Identificador_River(String _Line)
        {
            if (_Line.Contains("RIVER".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Identificador_Showdown(String _Line)
        {
            if (_Line.Contains("SHOW DOWN".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Identificador_Summary(String _Line)
        {
            if (_Line.Contains("SUMMARY".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
