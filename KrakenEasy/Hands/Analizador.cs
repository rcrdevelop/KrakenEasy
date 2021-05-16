using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using KrakenEasy.Casinos;
using KrakenEasy.KrakenBD;
using MongoDB.Bson;
namespace KrakenEasy.Hands
{
    public class Analizador
    {
        public void Recorrer_Fichero(string fichero)
        {
            try
            {
                List<string> _Hand_Set = new List<string>();
                List<string> _Hand = new List<string>();
                using (StreamReader lector = new StreamReader(fichero))
                {
                    List<string> _Fichero = new List<string>();
                    bool _Inicializar = false;
                    bool _Condicion_Primera_Linea = true;
                    while (lector.Peek() > -1)
                    {
                        string linea = lector.ReadLine().ToUpper();
                        if (!String.IsNullOrEmpty(linea))
                        {
                            if (fichero.ToUpper().Contains("Winamax".ToUpper()) && (!(fichero.ToUpper().Contains("dat".ToUpper()))))
                            {
                                _Hand.Add(linea);
                                Winamax _Winamax = new Winamax();
                                if (_Winamax.Identificador_HandID(linea))
                                {
                                    if (_Condicion_Primera_Linea)
                                    {
                                        _Inicializar = false;
                                    }
                                    else
                                    {
                                        _Inicializar = true;
                                    }
                                }
                                if (_Inicializar)
                                {
                                    _Hand_Set = _Hand;
                                    Extraer_Etapas_Winamax(_Hand);
                                    _Hand = new List<string>();
                                    _Inicializar = false;
                                }
                                _Condicion_Primera_Linea = false;
                            }
                            if (fichero.ToUpper().Contains("888".ToUpper()) && (!(fichero.ToUpper().Contains("dat".ToUpper()))))
                            {
                                _Hand.Add(linea);
                                Poker888 _888Poker = new Poker888();
                                if (_888Poker.Identificador_HandID(linea))
                                {

                                    if (_Condicion_Primera_Linea)
                                    {
                                        _Inicializar = false;
                                    }
                                    else
                                    {
                                        _Inicializar = true;
                                    }
                                }
                                if (_Inicializar)
                                {
                                    _Hand_Set = _Hand;
                                    Extraer_Etapas_888Poker(_Hand);
                                    _Hand = new List<string>();
                                    _Inicializar = false;
                                }
                                _Condicion_Primera_Linea = false;
                            }
                            if (fichero.ToUpper().Contains("PokerStars".ToUpper()) && (!(fichero.ToUpper().Contains("dat".ToUpper()))))
                            {
                                _Hand.Add(linea);
                                PokerStars _PokerStars = new PokerStars();
                                if (_PokerStars.Identificador_HandID(linea))
                                {
                                    if (_Condicion_Primera_Linea)
                                    {
                                        _Inicializar = false;
                                    }
                                    else
                                    {
                                        _Inicializar = true;
                                    }
                                }
                                if (_Inicializar)
                                {
                                    _Hand_Set = _Hand;
                                    Extraer_Etapas_PokerStars(_Hand);
                                    _Hand = new List<string>();
                                    _Inicializar = false;
                                }
                                _Condicion_Primera_Linea = false;
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                
            }
        }
        public void Extraer_Etapas_Winamax(List<string> _Hand)
        {
            bool _Blinds_Line = false;
            bool _Pre_Line = false;
            bool _Flop_Line = false;
            bool _Turn_Line = false;
            bool _River_Line = false;
            bool _Showdown_Line = false;
            bool _Summary_Line = false;
            string _Hand_ID = "";
            string _Mesa = "";
            string _Button = "";
            List<string> _Cards = new List<string>();
            List<string> _Blinds = new List<string>();
            List<string> _Pre = new List<string>();
            List<string> _Flop = new List<string>();
            List<string> _Turn = new List<string>();
            List<string> _River = new List<string>();
            List<string> _Showdown = new List<string>();
            List<string> _Summary = new List<string>();
            List<string> _Players = new List<string>();
            List<string> _SB = new List<string>();
            List<string> _BB = new List<string>();
            List<string> _UTG = new List<string>();
            List<string> _UTG1 = new List<string>();
            List<string> _MP = new List<string>();
            List<string> _MP1 = new List<string>();
            List<string> _CO = new List<string>();
            List<string> _HJ = new List<string>();
            List<string> _BTN = new List<string>();

            string[] _Players_Type = { "SB", "BB", "UTG", "UTG1", "MP", "MP1", "CO", "HJ", "BTN" };
            Winamax _Winamax = new Winamax();
            foreach (string linea in _Hand)
            {
                if (_Winamax.Identificador_HandID(linea))
                {
                    _Hand_ID = linea;
                }
                if (_Winamax.Identificador_Mesa(linea))
                {
                    _Mesa = linea;
                }
                if (_Winamax.Identificador_Button(linea))
                {
                    _Button = linea;
                }
                if (linea.Contains("ANTE/BLINDS") || linea.Contains("PRE") || linea.Contains("FLOP") || 
                    linea.Contains("TURN") || linea.Contains("RIVER") || linea.Contains("SHOW DOWN") || linea.Contains("SUMMARY")) {
                    _Blinds_Line = _Winamax.Identificador_Blinds(linea);
                    _Pre_Line = _Winamax.Identificador_Pre(linea);
                    _Flop_Line = _Winamax.Identificador_Flop(linea);
                    _Turn_Line = _Winamax.Identificador_Turn(linea);
                    _River_Line = _Winamax.Identificador_River(linea);
                    _Showdown_Line = _Winamax.Identificador_Showdown(linea);
                    _Summary_Line = _Winamax.Identificador_Summary(linea);
                    Validar_Etapas();
                }
                else
                {
                    Almacenar_Etapas(linea);
                }
            }
            Almacenar_Players(_Hand);
            Organizar_Players();
            Almacenar_Actions();
            Almacenar_Cards();
            Set_Data();
            //Console.WriteLine(_Hand_ID);
            //Console.WriteLine(_Button);
            //foreach (var Player in _Players)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Blinds***");
            //foreach (var Player in _Blinds)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Pre***");
            //foreach (var Player in _Pre)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Flop***");
            //foreach (var Player in _Flop)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Turn***");
            //foreach (var Player in _Turn)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***River***");
            //foreach (var Player in _River)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Showdown***");
            //foreach (var Player in _Showdown)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Summary***");
            //foreach (var Player in _Summary)
            //{
            //    Console.WriteLine(Player);
            //}
            //foreach (var Action in _SB)
            //{
            //    Console.WriteLine(Action + "/ SB action");
            //}
            //foreach (var Action in _BB)
            //{
            //    Console.WriteLine(Action + "/BB action");
            //}
            //foreach (var Action in _UTG)
            //{
            //    Console.WriteLine(Action + "/ UTG action");
            //}
            //foreach (var Action in _UTG1)
            //{
            //    Console.WriteLine(Action + "/ UTG1 action");
            //}
            //foreach (var Action in _MP)
            //{
            //    Console.WriteLine(Action + "/ MP action");
            //}
            //foreach (var Action in _MP1)
            //{
            //    Console.WriteLine(Action + "/ MP1 action");
            //}
            //foreach (var Action in _CO)
            //{
            //    Console.WriteLine(Action + "/ CO action");
            //}
            //foreach (var Action in _HJ)
            //{
            //    Console.WriteLine(Action + "/ HJ action");
            //}
            //foreach (var Action in _BTN)
            //{
            //    Console.WriteLine(Action + "/ BTN action");
            //}
            //foreach (var Card in _Cards)
            //{
            //    Console.WriteLine(Card + "/ Card");
            //}

            void Validar_Etapas() {
                if (_Blinds_Line && _Pre_Line)
                {
                    _Blinds_Line = false;
                }
                if (_Pre_Line && _Flop_Line)
                {
                    _Pre_Line = false;
                }
                if (_Flop_Line && _Turn_Line)
                {
                    _Flop_Line = false;
                }
                if (_Turn_Line && _River_Line)
                {
                    _Turn_Line = false;
                }
                if (_River_Line && _Showdown_Line)
                {
                    _River_Line = false;
                }
                if (_Showdown_Line && _Summary_Line)
                {
                    _Showdown_Line = false;
                }
            }
            void Organizar_Players()
            {
                {
                    int Position_Type = 0;
                    int Player_BTN = 0;
                    List<string> _Players_Organizado = new List<string>();
                    Player_BTN = Last_Player(_Players.Count);
                    Adaptar_Players_Type(_Players.Count);
                    for (int i = Player_BTN; i < _Players.Count; i++)
                    {
                        _Players_Organizado.Add(_Players[i]);
                    }

                    for (int i = 0; i < Player_BTN; i++)
                    {
                        _Players_Organizado.Add(_Players[i]);
                    }
                    foreach (var _Type in _Players_Type)
                    {
                        _Players_Organizado[Position_Type] = _Players_Organizado[Position_Type] + " /Position: " + _Type;
                        Position_Type++;
                    }

                    _Players = _Players_Organizado;
                }
            }
            int Last_Player(int N_Player)
            {
                int _Resultado = 0;
                foreach (var item in _Players)
                {
                    if (_Button.Split("#")[1].Split(" ")[0] == (item.Split(":")[0].Split(" ")[1]))
                    {
                        return _Resultado;
                    }
                    _Resultado++;
                }
                return _Resultado;
            }
            void Almacenar_Etapas(string linea) 
            {
                    if (_Blinds_Line)
                    {
                        _Blinds.Add(linea);
                    }
                    if (_Pre_Line)
                    {
                        _Pre.Add(linea);
                    }
                    if (_Flop_Line)
                    {
                        _Flop.Add(linea);
                    }
                    if (_Turn_Line)
                    {
                        _Turn.Add(linea);
                    }
                    if (_River_Line)
                    {
                        _River.Add(linea);
                    }
                    if (_Showdown_Line)
                    {
                        _Showdown.Add(linea);
                    }
                    if (_Summary_Line)
                    {
                        _Summary.Add(linea);
                    }
            }
            void Almacenar_Players(List<string> _Hand)
            {
                foreach (var linea in _Hand)
                {
                    if (_Winamax.Identificador_Players(linea))
                    {

                        _Players.Add(linea);
                    }
                    if (_Winamax.Identificador_Button(linea))
                    {
                        _Button = linea;
                    }
                }
            }
            void Almacenar_Actions()
            {
                foreach (var linea in _Blinds)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Pre)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Flop)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Turn)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _River)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Showdown)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Summary)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
            }
            void Almacenar_Cards()
            {
                foreach (var linea in _Summary)
                {
                    if (linea.Contains("board".ToUpper()))
                    {
                        _Cards.Add(linea);
                    }
                    if (linea.Contains("show".ToUpper()))
                    {
                        _Cards.Add(linea);
                    }
                }
                foreach (var linea in _Blinds)
                {
                    if (linea.Contains("dealt to".ToUpper()))
                    {
                        _Cards.Add(linea);
                    }
                }
            }
            void Set_Data()
            {
                MongoAccess _Access = new MongoAccess();
                _Access.Set_Hand_Winamax(_BTN,
                                         _SB,
                                         _BB,
                                         _UTG,
                                         _UTG1,
                                         _MP,
                                         _MP1,
                                         _CO,
                                         _HJ,
                                         _Blinds,
                                         _Flop,
                                         _Pre,
                                         _Turn,
                                         _River,
                                         _Showdown,
                                         _Summary,
                                         _Cards,
                                         _Hand_ID,
                                         _Mesa,
                                         _Players);
            }
            void Type_Jugador(string linea, string Player)
            {
                if (Player.Contains("SB"))
                {
                    _SB.Add(linea);
                }
                if (Player.Contains("BB"))
                {
                    _BB.Add(linea);
                }
                if (Player.Contains("UTG"))
                {
                    _UTG.Add(linea);
                }
                if (Player.Contains("UTG1"))
                {
                    _UTG1.Add(linea);
                }
                if (Player.Contains("MP"))
                {
                    _MP.Add(linea);
                }
                if (Player.Contains("MP1"))
                {
                    _MP1.Add(linea);
                }
                if (Player.Contains("CO"))
                {
                    _CO.Add(linea);
                }
                if (Player.Contains("HJ"))
                {
                    _HJ.Add(linea);
                }
                if (Player.Contains("BTN"))
                {
                    _BTN.Add(linea);
                }
            }
            void Adaptar_Players_Type(int N_Players)
            {
                if (N_Players == 2)
                {
                    _Players_Type = new string[2];
                    _Players_Type[0] = "SB / BTN";
                    _Players_Type[1] = "BB";
                }
                if (N_Players == 3)
                {
                    _Players_Type = new string[3];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                }
                if (N_Players == 4)
                {
                    _Players_Type = new string[4];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "CO";
                }
                if (N_Players == 5)
                {
                    _Players_Type = new string[5];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "CO";
                }
                if (N_Players == 6)
                {
                    _Players_Type = new string[6];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "MP";
                    _Players_Type[5] = "CO";
                }
                if (N_Players == 7)
                {
                    _Players_Type = new string[6];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "MP";
                    _Players_Type[5] = "CO";
                    _Players_Type[6] = "HJ";
                }
                if (N_Players == 8)
                {
                    _Players_Type = new string[7];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "MP";
                    _Players_Type[5] = "MP1";
                    _Players_Type[6] = "CO";
                    _Players_Type[7] = "HJ";
                }
                if (N_Players == 9)
                {
                    _Players_Type = new string[7];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "UTG1";
                    _Players_Type[5] = "MP";
                    _Players_Type[6] = "MP1";
                    _Players_Type[7] = "CO";
                    _Players_Type[8] = "HJ";
                }
            }
        }
        public void Extraer_Etapas_888Poker(List<string> _Hand)
        {
            bool _Blinds_Line = false;
            bool _Pre_Line = false;
            bool _Flop_Line = false;
            bool _Turn_Line = false;
            bool _River_Line = false;
            bool _Showdown_Line = false;
            bool _Summary_Line = false;
            string _Hand_ID = "";
            string _Button = "";
            string _Mesa = "";
            List<string> _Cards = new List<string>();
            List<string> _Blinds = new List<string>();
            List<string> _Pre = new List<string>();
            List<string> _Flop = new List<string>();
            List<string> _Turn = new List<string>();
            List<string> _River = new List<string>();
            List<string> _Showdown = new List<string>();
            List<string> _Summary = new List<string>();
            List<string> _Players = new List<string>();
            List<string> _SB = new List<string>();
            List<string> _BB = new List<string>();
            List<string> _UTG = new List<string>();
            List<string> _UTG1 = new List<string>();
            List<string> _MP = new List<string>();
            List<string> _MP1 = new List<string>();
            List<string> _CO = new List<string>();
            List<string> _HJ = new List<string>();
            List<string> _BTN = new List<string>();

            string[] _Players_Type = { "SB", "BB", "UTG", "UTG1", "MP", "MP1", "CO", "HJ", "BTN" };
            Poker888 _888Poker = new Poker888();
            foreach (string linea in _Hand)
            {
                if (_888Poker.Identificador_HandID(linea))
                {
                    _Hand_ID = "888POKER "+ linea;
                }
                if (_888Poker.Identificador_Mesa(linea))
                {
                    _Mesa = linea;
                }
                if (_888Poker.Identificador_Button(linea))
                {
                    _Button = linea;
                }
                if (_888Poker.Identificador_Pre(linea) || _888Poker.Identificador_Flop(linea)
                    || _888Poker.Identificador_Turn(linea) || _888Poker.Identificador_River(linea) || _888Poker.Identificador_Summary(linea))
                {
                    _Blinds_Line = _888Poker.Identificador_Blinds(linea);
                    _Pre_Line = _888Poker.Identificador_Pre(linea);
                    _Flop_Line = _888Poker.Identificador_Flop(linea);
                    _Turn_Line = _888Poker.Identificador_Turn(linea);
                    _River_Line = _888Poker.Identificador_River(linea);
                    _Summary_Line = _888Poker.Identificador_Summary(linea);
                    if (_Blinds_Line)
                    {
                        Almacenar_Etapas(linea);
                    }
                    Validar_Etapas();
                }
                else
                {
                    Almacenar_Etapas(linea);
                }
            }
            Almacenar_Players(_Hand);
            Organizar_Players();
            Almacenar_Actions();
            Almacenar_Cards();
            Set_Data();
            //Console.WriteLine(_Hand_ID);
            //Console.WriteLine(_Button);
            //foreach (var Player in _Players)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Blinds***");
            //foreach (var Player in _Blinds)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Pre***");
            //foreach (var Player in _Pre)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Flop***");
            //foreach (var Player in _Flop)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Turn***");
            //foreach (var Player in _Turn)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***River***");
            //foreach (var Player in _River)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Showdown***");
            //foreach (var Player in _Showdown)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Summary***");
            //foreach (var Player in _Summary)
            //{
            //    Console.WriteLine(Player);
            //}
            //foreach (var Action in _SB)
            //{
            //    Console.WriteLine(Action + "/ SB action");
            //}
            //foreach (var Action in _BB)
            //{
            //    Console.WriteLine(Action + "/BB action");
            //}
            //foreach (var Action in _UTG)
            //{
            //    Console.WriteLine(Action + "/ UTG action");
            //}
            //foreach (var Action in _UTG1)
            //{
            //    Console.WriteLine(Action + "/ UTG1 action");
            //}
            //foreach (var Action in _MP)
            //{
            //    Console.WriteLine(Action + "/ MP action");
            //}
            //foreach (var Action in _MP1)
            //{
            //    Console.WriteLine(Action + "/ MP1 action");
            //}
            //foreach (var Action in _CO)
            //{
            //    Console.WriteLine(Action + "/ CO action");
            //}
            //foreach (var Action in _HJ)
            //{
            //    Console.WriteLine(Action + "/ HJ action");
            //}
            //foreach (var Action in _BTN)
            //{
            //    Console.WriteLine(Action + "/ BTN action");
            //}
            //foreach (var Card in _Cards)
            //{
            //    Console.WriteLine(Card + "/ Card");
            //}

            void Validar_Etapas()
            {
                if (_Blinds_Line && _Pre_Line)
                {
                    _Blinds_Line = false;
                }
                if (_Pre_Line && _Flop_Line)
                {
                    _Pre_Line = false;
                }
                if (_Flop_Line && _Turn_Line)
                {
                    _Flop_Line = false;
                }
                if (_Turn_Line && _River_Line)
                {
                    _Turn_Line = false;
                }
                if (_River_Line && _Showdown_Line)
                {
                    _River_Line = false;
                }
                if (_Showdown_Line && _Summary_Line)
                {
                    _Showdown_Line = false;
                }
            }
            void Organizar_Players()
            {
                {
                    int Position_Type = 0;
                    int Player_BTN = 0;
                    List<string> _Players_Organizado = new List<string>();
                    Player_BTN = Last_Player(_Players.Count);
                    Adaptar_Players_Type(_Players.Count);
                    for (int i = Player_BTN; i < _Players.Count; i++)
                    {
                        _Players_Organizado.Add(_Players[i]);
                    }

                    for (int i = 0; i < Player_BTN; i++)
                    {
                        _Players_Organizado.Add(_Players[i]);
                    }
                    foreach (var _Type in _Players_Type)
                    {
                        _Players_Organizado[Position_Type] = _Players_Organizado[Position_Type] + " /Position: " + _Type;
                        Position_Type++;
                    }

                    _Players = _Players_Organizado;
                }
            }
            int Last_Player(int N_Player)
            {
                int _Resultado = 0;
                foreach (var item in _Players)
                {
                    if (_Button.Split(" ")[1] == (item.Split(":")[0].Split(" ")[1]))
                    {
                        return _Resultado;
                    }
                    _Resultado++;
                }
                return _Resultado;
            }
            void Almacenar_Etapas(string linea)
            {
                if (!_Pre_Line && !_Turn_Line && !_River_Line && !_Flop_Line && !_Showdown_Line && !_Summary_Line && _888Poker.Identificador_Blinds(linea))
                {
                    _Blinds.Add(linea);
                }
                if (_Pre_Line)
                {
                    _Pre.Add(linea);
                }
                if (_Flop_Line)
                {
                    _Flop.Add(linea);
                }
                if (_Turn_Line)
                {
                    _Turn.Add(linea);
                }
                if (_River_Line)
                {
                    _River.Add(linea);
                }
                if (_Showdown_Line)
                {
                    _Showdown.Add(linea);
                }
                if (_Summary_Line)
                {
                    _Summary.Add(linea);
                }
            }
            void Almacenar_Players(List<string> _Hand)
            {
                foreach (var linea in _Hand)
                {
                    if (_888Poker.Identificador_Players(linea))
                    {

                        _Players.Add(linea);
                    }
                    if (_888Poker.Identificador_Button(linea))
                    {
                        _Button = linea;
                    }
                }
            }
            void Almacenar_Actions()
            {
                foreach (var linea in _Blinds)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Pre)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Flop)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Turn)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _River)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Showdown)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Summary)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
            }
            void Almacenar_Cards()
            {
                string Cards = "";
                foreach (string linea in _Hand)
                {
                    if (linea.ToUpper().Contains("Dealing river".ToUpper()) || linea.ToUpper().Contains("Dealing flop".ToUpper()) || linea.ToUpper().Contains("Dealing turn".ToUpper()))
                    {
                        Cards = Cards + linea.Split("** [")[1].Split(" ]")[0];
                    }
                }
            }
            void Set_Data()
            {
                MongoAccess _Access = new MongoAccess();
                _Access.Set_Hand_888Poker(_BTN,
                                         _SB,
                                         _BB,
                                         _UTG,
                                         _UTG1,
                                         _MP,
                                         _MP1,
                                         _CO,
                                         _HJ,
                                         _Blinds,
                                         _Flop,
                                         _Pre,
                                         _Turn,
                                         _River,
                                         _Showdown,
                                         _Summary,
                                         _Cards,
                                         _Hand_ID,
                                         _Mesa,
                                         _Players);
            }
            void Type_Jugador(string linea, string Player)
            {
                if (Player.Contains("SB"))
                {
                    _SB.Add(linea);
                }
                if (Player.Contains("BB"))
                {
                    _BB.Add(linea);
                }
                if (Player.Contains("UTG"))
                {
                    _UTG.Add(linea);
                }
                if (Player.Contains("UTG1"))
                {
                    _UTG1.Add(linea);
                }
                if (Player.Contains("MP"))
                {
                    _MP.Add(linea);
                }
                if (Player.Contains("MP1"))
                {
                    _MP1.Add(linea);
                }
                if (Player.Contains("CO"))
                {
                    _CO.Add(linea);
                }
                if (Player.Contains("HJ"))
                {
                    _HJ.Add(linea);
                }
                if (Player.Contains("BTN"))
                {
                    _BTN.Add(linea);
                }
            }
            void Adaptar_Players_Type(int N_Players)
            {
                if (N_Players == 2)
                {
                    _Players_Type = new string[2];
                    _Players_Type[0] = "SB / BTN";
                    _Players_Type[1] = "BB";
                }
                if (N_Players == 3)
                {
                    _Players_Type = new string[3];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                }
                if (N_Players == 4)
                {
                    _Players_Type = new string[4];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "CO";
                }
                if (N_Players == 5)
                {
                    _Players_Type = new string[5];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "CO";
                }
                if (N_Players == 6)
                {
                    _Players_Type = new string[6];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "MP";
                    _Players_Type[5] = "CO";
                }
                if (N_Players == 7)
                {
                    _Players_Type = new string[6];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "MP";
                    _Players_Type[5] = "CO";
                    _Players_Type[6] = "HJ";
                }
                if (N_Players == 8)
                {
                    _Players_Type = new string[7];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "MP";
                    _Players_Type[5] = "MP1";
                    _Players_Type[6] = "CO";
                    _Players_Type[7] = "HJ";
                }
                if (N_Players == 9)
                {
                    _Players_Type = new string[7];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "UTG1";
                    _Players_Type[5] = "MP";
                    _Players_Type[6] = "MP1";
                    _Players_Type[7] = "CO";
                    _Players_Type[8] = "HJ";
                }
            }
        }

        public void Extraer_Etapas_PokerStars(List<string> _Hand)
        {
            bool _Blinds_Line = false;
            bool _Pre_Line = false;
            bool _Flop_Line = false;
            bool _Turn_Line = false;
            bool _River_Line = false;
            bool _Summary_Line = false;
            string _Hand_ID = "";
            string _Button = "";
            string _Mesa = "";
            List<string> _Cards = new List<string>();
            List<string> _Blinds = new List<string>();
            List<string> _Pre = new List<string>();
            List<string> _Flop = new List<string>();
            List<string> _Turn = new List<string>();
            List<string> _River = new List<string>();
            List<string> _Showdown = new List<string>();
            List<string> _Summary = new List<string>();
            List<string> _Players = new List<string>();
            List<string> _SB = new List<string>();
            List<string> _BB = new List<string>();
            List<string> _UTG = new List<string>();
            List<string> _UTG1 = new List<string>();
            List<string> _MP = new List<string>();
            List<string> _MP1 = new List<string>();
            List<string> _CO = new List<string>();
            List<string> _HJ = new List<string>();
            List<string> _BTN = new List<string>();

            string[] _Players_Type = { "SB", "BB", "UTG", "UTG1", "MP", "MP1", "CO", "HJ", "BTN" };
            PokerStars _PokerStars = new PokerStars();
            foreach (string linea in _Hand)
            {
                if (_PokerStars.Identificador_HandID(linea))
                {
                    _Hand_ID = linea;
                }
                if (_PokerStars.Identificador_Mesa(linea))
                {
                    _Mesa = linea;
                }
                if (_PokerStars.Identificador_Button(linea))
                {
                    _Button = linea;
                }
                if (_PokerStars.Identificador_Pre(linea) || _PokerStars.Identificador_Flop(linea)
                    || _PokerStars.Identificador_Turn(linea) || _PokerStars.Identificador_River(linea) || _PokerStars.Identificador_Summary(linea))
                {
                    _Blinds_Line = _PokerStars.Identificador_Blinds(linea);
                    _Pre_Line = _PokerStars.Identificador_Pre(linea);
                    _Flop_Line = _PokerStars.Identificador_Flop(linea);
                    _Turn_Line = _PokerStars.Identificador_Turn(linea);
                    _River_Line = _PokerStars.Identificador_River(linea);
                    _Summary_Line = _PokerStars.Identificador_Summary(linea);
                    if (_Blinds_Line)
                    {
                        Almacenar_Etapas(linea);
                    }
                    Validar_Etapas();
                }
                else
                {
                    Almacenar_Etapas(linea);
                }
            }
            Almacenar_Players(_Hand);
            Organizar_Players();
            Almacenar_Actions();
            Almacenar_Cards();
            Set_Data();
            //Console.WriteLine(_Hand_ID);
            //Console.WriteLine(_Button);
            //foreach (var Player in _Players)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Blinds***");
            //foreach (var Player in _Blinds)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Pre***");
            //foreach (var Player in _Pre)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Flop***");
            //foreach (var Player in _Flop)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Turn***");
            //foreach (var Player in _Turn)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***River***");
            //foreach (var Player in _River)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Showdown***");
            //foreach (var Player in _Showdown)
            //{
            //    Console.WriteLine(Player);
            //}
            //Console.WriteLine("***Summary***");
            //foreach (var Player in _Summary)
            //{
            //    Console.WriteLine(Player);
            //}
            //foreach (var Action in _SB)
            //{
            //    Console.WriteLine(Action + "/ SB action");
            //}
            //foreach (var Action in _BB)
            //{
            //    Console.WriteLine(Action + "/BB action");
            //}
            //foreach (var Action in _UTG)
            //{
            //    Console.WriteLine(Action + "/ UTG action");
            //}
            //foreach (var Action in _UTG1)
            //{
            //    Console.WriteLine(Action + "/ UTG1 action");
            //}
            //foreach (var Action in _MP)
            //{
            //    Console.WriteLine(Action + "/ MP action");
            //}
            //foreach (var Action in _MP1)
            //{
            //    Console.WriteLine(Action + "/ MP1 action");
            //}
            //foreach (var Action in _CO)
            //{
            //    Console.WriteLine(Action + "/ CO action");
            //}
            //foreach (var Action in _HJ)
            //{
            //    Console.WriteLine(Action + "/ HJ action");
            //}
            //foreach (var Action in _BTN)
            //{
            //    Console.WriteLine(Action + "/ BTN action");
            //}
            //foreach (var Card in _Cards)
            //{
            //    Console.WriteLine(Card + "/ Card");
            //}

            void Validar_Etapas()
            {
                if (_Blinds_Line && _Pre_Line)
                {
                    _Blinds_Line = false;
                }
                if (_Pre_Line && _Flop_Line)
                {
                    _Pre_Line = false;
                }
                if (_Flop_Line && _Turn_Line)
                {
                    _Flop_Line = false;
                }
                if (_Turn_Line && _River_Line)
                {
                    _Turn_Line = false;
                }
                if (_River_Line && _Summary_Line)
                {
                    _River_Line = false;
                }
            }
            void Organizar_Players()
            {
                {
                    int Position_Type = 0;
                    int Player_BTN = 0;
                    List<string> _Players_Organizado = new List<string>();
                    Player_BTN = Last_Player(_Players.Count);
                    Adaptar_Players_Type(_Players.Count);
                    for (int i = Player_BTN; i < _Players.Count; i++)
                    {
                        _Players_Organizado.Add(_Players[i]);
                    }

                    for (int i = 0; i < Player_BTN; i++)
                    {
                        _Players_Organizado.Add(_Players[i]);
                    }
                    foreach (var _Type in _Players_Type)
                    {
                        _Players_Organizado[Position_Type] = _Players_Organizado[Position_Type] + " /Position: " + _Type;
                        Position_Type++;
                    }

                    _Players = _Players_Organizado;
                }
            }
            int Last_Player(int N_Player)
            {
                int _Resultado = 0;
                foreach (var item in _Players)
                {
                    if (_Button.Split(" ")[1] == (item.Split(":")[0].Split(" ")[1]))
                    {
                        return _Resultado;
                    }
                    _Resultado++;
                }
                return _Resultado;
            }
            void Almacenar_Etapas(string linea)
            {
                if (!_Pre_Line && !_Turn_Line && !_River_Line && !_Flop_Line && !_Summary_Line && _PokerStars.Identificador_Blinds(linea))
                {
                    _Blinds.Add(linea);
                }
                if (_Pre_Line)
                {
                    _Pre.Add(linea);
                }
                if (_Flop_Line)
                {
                    _Flop.Add(linea);
                }
                if (_Turn_Line)
                {
                    _Turn.Add(linea);
                }
                if (_River_Line)
                {
                    _River.Add(linea);
                }
                if (_Summary_Line)
                {
                    _Summary.Add(linea);
                }
            }
            void Almacenar_Players(List<string> _Hand)
            {
                _Summary_Line = false;
                foreach (var linea in _Hand)
                {
                    if (_PokerStars.Identificador_Summary(linea))
                    {
                        _Summary_Line = true;
                    }
                    if (!_Summary_Line)
                    {
                        if (_PokerStars.Identificador_Players(linea))
                        {

                            _Players.Add(linea);
                        }
                    }

                    if (_PokerStars.Identificador_Button(linea))
                    {
                        _Button = linea;
                    }
                }
            }
            void Almacenar_Actions()
            {
                foreach (var linea in _Blinds)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Pre)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Flop)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Turn)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _River)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Showdown)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
                foreach (var linea in _Summary)
                {
                    foreach (var Player in _Players)
                    {
                        if (linea.Contains(Player.Split(":")[1].Split(" ")[1].Trim()))
                        {
                            Type_Jugador(linea, Player);
                        }
                    }
                }
            }
            void Almacenar_Cards()
            {
                foreach (var linea in _Summary)
                {
                    if (linea.Contains("board".ToUpper()))
                    {
                        _Cards.Add(linea);
                    }
                    if (linea.Contains("show".ToUpper()))
                    {
                        _Cards.Add(linea);
                    }
                }
                foreach (var linea in _Blinds)
                {
                    if (linea.Contains("dealt to".ToUpper()))
                    {
                        _Cards.Add(linea);
                    }
                }
            }
            void Set_Data()
            {
                MongoAccess _Access = new MongoAccess();
                _Access.Set_Hand_PokerStars(_BTN,
                                         _SB,
                                         _BB,
                                         _UTG,
                                         _UTG1,
                                         _MP,
                                         _MP1,
                                         _CO,
                                         _HJ,
                                         _Blinds,
                                         _Flop,
                                         _Pre,
                                         _Turn,
                                         _River,
                                         _Showdown,
                                         _Summary,
                                         _Cards,
                                         _Hand_ID,
                                         _Mesa,
                                         _Players);
            }
            void Type_Jugador(string linea, string Player)
            {
                if (Player.Contains("SB"))
                {
                    _SB.Add(linea);
                }
                if (Player.Contains("BB"))
                {
                    _BB.Add(linea);
                }
                if (Player.Contains("UTG"))
                {
                    _UTG.Add(linea);
                }
                if (Player.Contains("UTG1"))
                {
                    _UTG1.Add(linea);
                }
                if (Player.Contains("MP"))
                {
                    _MP.Add(linea);
                }
                if (Player.Contains("MP1"))
                {
                    _MP1.Add(linea);
                }
                if (Player.Contains("CO"))
                {
                    _CO.Add(linea);
                }
                if (Player.Contains("HJ"))
                {
                    _HJ.Add(linea);
                }
                if (Player.Contains("BTN"))
                {
                    _BTN.Add(linea);
                }
            }
            void Adaptar_Players_Type(int N_Players)
            {
                if (N_Players == 2)
                {
                    _Players_Type = new string[2];
                    _Players_Type[0] = "SB / BTN";
                    _Players_Type[1] = "BB";
                }
                if (N_Players == 3)
                {
                    _Players_Type = new string[3];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                }
                if (N_Players == 4)
                {
                    _Players_Type = new string[4];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "CO";
                }
                if (N_Players == 5)
                {
                    _Players_Type = new string[5];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "CO";
                }
                if (N_Players == 6)
                {
                    _Players_Type = new string[6];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "MP";
                    _Players_Type[5] = "CO";
                }
                if (N_Players == 7)
                {
                    _Players_Type = new string[6];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "MP";
                    _Players_Type[5] = "CO";
                    _Players_Type[6] = "HJ";
                }
                if (N_Players == 8)
                {
                    _Players_Type = new string[7];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "MP";
                    _Players_Type[5] = "MP1";
                    _Players_Type[6] = "CO";
                    _Players_Type[7] = "HJ";
                }
                if (N_Players == 9)
                {
                    _Players_Type = new string[7];
                    _Players_Type[0] = "BTN";
                    _Players_Type[1] = "SB";
                    _Players_Type[2] = "BB";
                    _Players_Type[3] = "UTG";
                    _Players_Type[4] = "UTG1";
                    _Players_Type[5] = "MP";
                    _Players_Type[6] = "MP1";
                    _Players_Type[7] = "CO";
                    _Players_Type[8] = "HJ";
                }
            }
        }
    }

}
