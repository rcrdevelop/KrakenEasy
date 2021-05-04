using System;
using System.Collections.Generic;
using System.Text;
using KrakenEasy.KrakenBD;

namespace KrakenEasy.Casinos
{
        public class Winamax
        {
            bool _HandRead = false;
            bool _PreFlop = false;
            bool _Flop = false;
            bool _Turn = false;
            bool _River = false;
            bool _ShowDown = false;
            bool _Summary = false;
            bool _Organizar = false;
            bool _Analizar = false;
            bool BB_Read = false;
            string _HandID = "";
            string _WinHand = "";
            int Numero_PLayer = 0;
            List<string> _Players_Info = new List<string>();
            List<string> _Player_HJ = new List<string>();
            List<string> _Player_CO = new List<string>();
            List<string> _Player_MP = new List<string>();
            List<string> _Player_MP1 = new List<string>();
            List<string> _Player_UTG = new List<string>();
            List<string> _Player_UTG1 = new List<string>();
            List<string> _Player_BTN = new List<string>();
            List<string> _Player_BB = new List<string>();
            List<string> _Player_SB = new List<string>();
            List<string> PreFlop_Actions = new List<string>();
            List<string> Flop_Actions = new List<string>();
            List<string> Turn_Actions = new List<string>();
            List<string> River_Actions = new List<string>();
            List<string> ShowDown_Actions = new List<string>();
            List<string> Summary_Actions = new List<string>();
            string[] _Cards_Board = new string[5];
        public List<string> AnalizarFichero(List<string> _Fichero)
        {
            return _Players_Info;
        }
            public String Proceso()
            {
                String _Proceso = "Winamax";
                return _Proceso;
            }
            public String[] Ventanas()
            {
                String[] _Ventana = { "Hold", "Omaha" };
                return _Ventana;
            }
            public String Ruta()
            {
                MongoAccess _Access = new MongoAccess();
                String _userName = (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1];
                String _Ruta = _Access.Get_Ruta_Casino("WINAMAX");
                return _Ruta;
            }
            public bool Identificador_HandID(string _Line)
            {
                if (_Line.Contains("HandId".ToUpper()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
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
        public bool Identificador_BET(string _Line)
        {
            if (_Line.Contains("BETS"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool Identificador_RAISES(string _Line)
        {
            if (_Line.Contains("RAISES"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Identificador_WinHand(String _Line)
            {
                if (_Line.Contains("won"))
                {
                    _WinHand = _Line;
                }
            }
            public bool Identificador_Button(String _Line)
            {
                if (_Line.Contains("is the button".ToUpper()) && !(_Line.Contains("won".ToUpper())) && (!(_Line.Contains("lost".ToUpper()))))
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
                if (!_Line.Contains("#".ToUpper()) && !_Line.Contains("won".ToUpper()) && !_Line.Contains("lost".ToUpper()) && _Line.Contains("Seat".ToUpper()) && !_Line.Contains("posts".ToUpper()) && !_Line.Contains("big blind".ToUpper()) && !_Line.Contains("small blind".ToUpper()))
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
                if (_Line.Contains("ANTE/BLIND"))
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
                if (_Line.Contains("PRE"))
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
                if (_Line.Contains("*** FLOP ***"))
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
        public void Set_Hand(List<string>_BTN,
                             List<string>_SB,
                             List<string>_BB,
                             List<string>_UTG,
                             List<string>_UTG1,
                             List<string>_MP,
                             List<string>_MP1,
                             List<string>_CO,
                             List<string>_HJ,
                             List<string>_Blinds,
                             List<string>_Flop,
                             List<string>_Pre,
                             List<string>_Turn,
                             List<string>_River,
                             List<string>_Showdown,
                             List<string>_Summary,
                             List<string>_Cards,
                             string _Hand_ID,
                             List<string>_Players)
        {
            
        }

        public void Reiniciar_Entorno()
            {
                _HandID = "";
                _WinHand = "";
                Numero_PLayer = 0;
                _Turn = false;
                _PreFlop = false;
                 _Flop = false;
                _River = false;
                _Summary = false;
                _ShowDown = false;
                _Players_Info = new List<string>();
                _Player_HJ = new List<string>();
                _Player_CO = new List<string>();
                _Player_MP = new List<string>();
                _Player_MP1 = new List<string>();
                _Player_UTG = new List<string>();
                _Player_UTG1 = new List<string>();
                _Player_BTN = new List<string>();
                _Player_BB = new List<string>();
                _Player_SB = new List<string>();
                PreFlop_Actions = new List<string>();
                Flop_Actions = new List<string>();
                Turn_Actions = new List<string>();
                River_Actions = new List<string>();
                ShowDown_Actions = new List<string>();
                Summary_Actions = new List<string>();
                _Cards_Board = new string[5];
            }
    }
}
