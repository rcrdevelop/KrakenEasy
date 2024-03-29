﻿using System;
using System.Collections.Generic;
using System.Text;
using KrakenEasy.KrakenBD;

namespace KrakenEasy.Casinos
{
    public class Poker888
    {
        public static string _Ruta { get; set; }
        public static bool Habilitado { get; set; }
        public static string Proceso()
        {
            string _Proceso = "QtWebEngineProcess";
            return _Proceso;
        }
        public string[] Ventanas()
        {
            string[] _Ventana = { "OMAHA", "NLH" };
            return _Ventana;
        }
        public bool Identificador_HandID(String _Line)
        {
            if (_Line.Contains("Game NO".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static string Id()
        {
            return "Game NO";
        }
        public bool Identificador_Mesa(string _Line)
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
        public bool WinHand(String _Line)
        {
            if (_Line.Contains("collected".ToUpper()))
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
            if (_Line.Contains("blind [".ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Identificador_Pre(String _Line)
        {
            if (_Line.Contains("Dealing down cards".ToUpper()))
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
            if (_Line.Contains("flop".ToUpper()))
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
            if (_Line.Contains("turn".ToUpper()))
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
            if (_Line.Contains("river".ToUpper()))
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
            if (_Line.Contains("shows".ToUpper()))
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
            if (_Line.Contains("Summary".ToUpper()))
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
