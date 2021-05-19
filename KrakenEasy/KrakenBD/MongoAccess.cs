﻿using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using KrakenEasy.Casinos;
using System.Windows;

namespace KrakenEasy.KrakenBD
{
    public class MongoAccess
    {
        static List<string> Lista_Jugadores = new List<string>();
        IMongoDatabase _DataBase;
        IMongoClient _Client;
        public MongoAccess()
        {

            if (false) //Settings.MongoCloud
            {
                _Client = new MongoClient("mongodb+srv://Kraken:HaMz4uGvG58BbhkP@cluster0.uo07x.mongodb.net/Kraken?retryWrites=true&w=majority");
                _DataBase = _Client.GetDatabase("Kraken");
            }
            else
            {
                _Client = new MongoClient();
                _DataBase = _Client.GetDatabase("Kraken");
            }
        }



        public static string Get_Last_Hand(string Fichero)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken");
            string Resultado = "";
            foreach (var Hand in _Collection.GetCollection<BsonDocument>("Hands").Find(new BsonDocument("Mesa", new Regex(Fichero.ToUpper()))).ToList())
            {
                Resultado = Hand.GetElement("_id").Value.AsString;
            }
            return Resultado;
        }

        public string[] Get_Cards_Player(string Player, string Id_Hand)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Resultado = new string[2];
            _Resultado[0] = "dorso1";
            _Resultado[1] = "dorso1";
            foreach (BsonDocument Hand in _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument("_id", new Regex(Id_Hand))).ToList())
            {
                foreach (var Jugador in Hand.GetElement("Players").Value.AsBsonArray)
                {
                    if (Jugador.AsString.Contains(Player))
                    {
                        foreach (var linea in Hand.GetElement(Jugador.AsString.Split("Position: ")[1]).Value.AsBsonArray)
                        {
                            if (linea.AsString.Contains("SHOWS"))
                            {
                                var cards = linea.AsString.Split("[")[1].Split("]")[0];
                                _Resultado[0] = cards.Split(" ")[0];
                                _Resultado[1] = cards.Split(" ")[1];
                            }
                        }
                    }
                }
            }
            return _Resultado;
        }
        public bool Get_Hole_Cards()
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", "Hole");

            foreach (BsonDocument Ventana in _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("HUD").Find(_Filter).ToList())
            {
                if (Ventana.GetElement("Hole").Value.AsBoolean == false)
                {
                    return false;
                }
            }
            return true;
        }
        public void Set_Hole_Cards()
        {
            Registros_Data.Hole_Cards = !Registros_Data.Hole_Cards;


        }
        //public List<string> Get_Cards_Hands_Jugador()
        //{

        //}
        public List<string> Get_Cards_Hands_Board(string ID_Hand)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            List<string> _Resultado = new List<string>();
            foreach (var Cards in _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument("_id", new Regex(ID_Hand.Trim()))).ToList())
            {
                    foreach (var element in Cards.GetElement("Cards").Value.AsBsonArray)
                    {
                            foreach (var item in element.AsString.Split(" "))
                            {
                                _Resultado.Add(item);
                            }
                    }
            }
            return _Resultado;
        }
        public void HUDS_Delete(string Ventana)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Ventana", Ventana);
            _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("HUD").FindOneAndDelete(_Filter);


        }

        public void InicializarMain()
        {
            try
            {
                Mesas.Abiertas = new BsonArray();
                MongoAccess _Access = new MongoAccess();
                var _Session = _Access._Client.StartSession();
                PrimeraVez();

                void PrimeraVez()
                {
                    if (_Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Casino").CountDocuments(new BsonDocument { }) == 0)
                    {
                        InicializarData();
                    }
                    else
                    {


                    }
                }
                static void InicializarData()
                {
                    MongoAccess _Access = new MongoAccess();
                    var _Session = _Access._Client.StartSession();
                    try
                    {
                        _Session.Client.GetDatabase("Kraken").CreateCollection("Jugadores");
                        _Session.Client.GetDatabase("Kraken").CreateCollection("Replayer");
                        _Session.Client.GetDatabase("Kraken").CreateCollection("Hands");
                    }
                    catch { };
                }

            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Window _Window = new Window();
                    _Window.Content = ex.Message;
                    _Window.Show();
                });
            }

        }

        public List<string> Get_Jugadores()
        {
            MongoAccess _Access = new MongoAccess();
            List<string> _Resultado = new List<string>();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands");

            foreach (var Jugadores in _Collection.Find(new BsonDocument()).ToList())
            {
                foreach (var item in Jugadores.GetElement("Players").Value.AsBsonArray)
                {
                    bool Condicion_Agregar = true;
                    foreach(var Jugador in _Resultado) 
                    {
                        if (Jugador.Contains(item.AsString.Split(":")[1].Split("(")[0]))
                        {
                            Condicion_Agregar = false;
                        }
                    }
                    if (Condicion_Agregar)
                    {
                        _Resultado.Add(item.AsString.Split(":")[1].Split("(")[0]);
                    }
                }
                
            }
            return _Resultado;
        }
        public List<string> Hands_ID(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands");
            List<string> _Resultado = new List<string>();
            foreach (var Hand in _Collection.Find(new BsonDocument("Players", new Regex(Jugador))).ToList())
            {
                _Resultado.Add(Hand.GetElement("_id").Value.AsString);
            }
            return _Resultado;
        }
        public void Set_Cards_Jugador(string _Id) 
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Replayer");
            if (_Collection.Find(new BsonDocument(new BsonElement("_id", _Id))).CountDocuments() == 0)
            {
                
            }
            else 
            { 

            }
        }


        public void Set_Mesas(string[] _Mesa)
        {
            if (Mesas.Abiertas.Count > 0)
            {
                Window _Window = new Window();
                _Window.Width = 10;
                _Window.Height = 10;
                _Window.Show();
                for (var i = 0; i >= Mesas.Abiertas.Count; i++)
                {
                    if (Mesas.Abiertas[i].AsBsonDocument.GetElement("_id").Value.AsString == _Mesa[0])
                    {
                        BsonDocument _Data = new BsonDocument();
                        _Data.Add(new BsonElement("_id", _Mesa[0]));
                        _Data.Add(new BsonElement("Dimensiones", _Mesa[1]));
                        _Data.Add(new BsonElement("Activa", _Mesa[2]));
                        _Data.Add(new BsonElement("Ready", _Mesa[3]));
                        _Data.Add(new BsonElement("Casino", _Mesa[4]));
                        _Data.Add(new BsonElement("_Last_Hand", Get_Last_Hand(_Mesa[0])));
                        Mesas.Abiertas[i] = _Data;
                    }
                }
            }
            else
            {
                BsonDocument _Datos = new BsonDocument();

                for (int i = 0; i < _Mesa.Length; i++)
                {
                    string Element = _Mesa[i];
                    if (i == 0)
                    {
                        _Datos.Add(new BsonElement("_id", Element));
                    }
                    if (i == 1)
                    {
                        _Datos.Add(new BsonElement("Dimensiones", Element));
                    }
                    if (i == 2)
                    {
                        _Datos.Add(new BsonElement("Activa", true));
                    }
                    if (i == 3)
                    {
                        _Datos.Add(new BsonElement("Ready", false));
                    }
                    if (i == 4)
                    {
                        _Datos.Add(new BsonElement("Casino", Element));
                    }
                }
                _Datos.Add(new BsonElement("_Last_Hand", Get_Last_Hand(_Mesa[0])));            
                Mesas.Abiertas.Add(_Datos);

                //_Collection.GetCollection<BsonDocument>("Ventanas").InsertOne(_Datos);
                //Set_Last_Hand(_Mesa[0]);
            }
        }
        public void Set_Mesas_Activas(string _Nombre)
        {
            MongoAccess _Access = new MongoAccess();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", _Nombre);
            var update = Builders<BsonDocument>.Update.Set("Activa", true);
            _DataBase.GetCollection<BsonDocument>("Ventanas").UpdateOne(filter, update);
        }



        public void Set_Last_Hand(string _Id_Mesa)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken");
            var dt = DateTime.Now;
            long _Hora_Actual = dt.Year * 1000000000000 + dt.Month * 1000000 + dt.Day * 10000 + dt.Hour * 100 + dt.Minute;
            var filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Mesa);

            foreach (var fichero in _Collection.GetCollection<BsonDocument>("Ficheros").Find(new BsonDocument("_id", new Regex(_Id_Mesa))).ToList())
            {
                Console.WriteLine(_Hora_Actual.ToString() +"//"+ (long)fichero.GetElement("Last_Write").Value);

                if ((long)fichero.GetElement("Last_Write").Value >= _Hora_Actual -1)
                {
                    var N_Hands = (int)_Collection.GetCollection<BsonDocument>("Hands").Find(new BsonDocument("Mesa", new Regex(_Id_Mesa.Trim().ToUpper()))).CountDocuments();
                    foreach (var hand in _Collection.GetCollection<BsonDocument>("Hands").Find(new BsonDocument("Mesa", new Regex(_Id_Mesa.Trim().ToUpper()))).Skip(N_Hands - 1).ToList())
                    {

                        if (hand.GetElement("_id").Value.AsString.Contains("888Poker".ToUpper()))
                        {
                            if (hand.GetElement("Mesa").Value.AsString.Split(" ")[1] == _Id_Mesa.ToUpper())
                            {
                                for (var i = 0; i >= Mesas.Abiertas.Count; i++)
                                {
                                    if (Mesas.Abiertas[i].AsBsonDocument.GetElement("_id").Value.AsString == _Id_Mesa)
                                    {
                                        BsonDocument _Data = Mesas.Abiertas[i].AsBsonDocument;
                                        _Data.Add(new BsonElement("_Last_Hand", hand.GetElement("_id").Value.AsString));
                                        Mesas.Abiertas[i] = _Data;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (hand.GetElement("Mesa").Value.AsString.Split("'")[1] == _Id_Mesa.ToUpper())
                            {
                                for (var i = 0; i >= Mesas.Abiertas.Count; i++)
                                {
                                    if (Mesas.Abiertas[i].AsBsonDocument.GetElement("_id").Value.AsString == _Id_Mesa)
                                    {
                                        BsonDocument _Data = Mesas.Abiertas[i].AsBsonDocument;
                                        _Data.Add(new BsonElement("_Last_Hand", hand.GetElement("_id").Value.AsString));
                                        Mesas.Abiertas[i] = _Data;
                                    }
                                }
                            }
                        }

                    }
                    

                }
            }
        }
        public void Set_NPlayers(string _Id_Mesa)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Mesa);
            foreach (var item in _Collection.GetCollection<BsonDocument>("Ventanas").Find(filter).ToList())
            {
                var hand = item.GetElement("Last_Hand").Value.AsString;
                var filter_hand = Builders<BsonDocument>.Filter.Eq("_id", hand.ToUpper());
                foreach (var jugadores in _Collection.GetCollection<BsonDocument>("Hands").Find(filter_hand).ToList())
                {
                    var N_Jugadores = jugadores.GetElement("Players").Value.AsBsonArray;
                    var update = Builders<BsonDocument>.Update.Set("N_Players", N_Jugadores.Count);
                    _Collection.GetCollection<BsonDocument>("Ventanas").UpdateOne(filter, update);
                    update = Builders<BsonDocument>.Update.Set("Mostrar", true);
                    _Collection.GetCollection<BsonDocument>("Ventanas").UpdateOne(filter, update);
                }
            }
        }
        public string[] Get_NPlayers()
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken");
            var filter = Builders<BsonDocument>.Filter.Eq("Mostrar", true);
            string[] _Resultado = new string[2];
            foreach (var Ventana in _Collection.GetCollection<BsonDocument>("Ventanas").Find(filter).ToList())
            {
                Console.WriteLine("Mostrar");
                _Resultado[0] = Ventana.GetElement("N_Players").Value.ToString();
                _Resultado[1] = Ventana.GetElement("_id").Value.ToString();
            }
            return _Resultado;
        }
        public List<string> Get_Players(string _Id_Hand)
        {
            
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken");
            List<string> _Resultado = new List<string>();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Hand);
            foreach (var hand in _Collection.GetCollection<BsonDocument>("Hands").Find(filter).ToList())
            {
                foreach (var Players in hand.GetElement("Players").Value.AsBsonArray)
                {
                    _Resultado.Add(Players.AsString);
                }
            }
            return _Resultado;     
        }

        public List<string> Ventanas_Winamax()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Casino", "Winamax");
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken");
            var _Ventanas = _Collection.GetCollection<BsonDocument>("Ventanas").Find(filter).ToList();
            List<string> _Resultado = new List<string>();
            foreach (var _Element in _Ventanas)
            {
                _Resultado.Add(_Element.GetElement(0).Value.AsString);
            }
            return _Resultado;
        }
        public List<string> Ventanas_888Poker()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Casino", "888Poker");
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken");
            var _Ventanas = _Collection.GetCollection<BsonDocument>("Ventanas").Find(filter).ToList();
            List<string> _Resultado = new List<string>();
            foreach (var _Element in _Ventanas)
            {
                _Resultado.Add(_Element.GetElement(0).Value.AsString);
            }
            return _Resultado;
        }
        public List<string> Ventanas_PokerStars()
        {
            var filter = Builders<BsonDocument>.Filter.Eq("Casino", "PokerStars");
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken");
            var _Ventanas = _Collection.GetCollection<BsonDocument>("Ventanas").Find(filter).ToList();
            List<string> _Resultado = new List<string>();
            foreach (var _Element in _Ventanas)
            {
                _Resultado.Add(_Element.GetElement(0).Value.AsString);
            }
            return _Resultado;
        }

        public void Set_Ficheros(string _Fichero, string _Casino)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken");
            DateTime dt = File.GetLastWriteTime(_Fichero);
            long _Hora_Fichero = dt.Year * 1000000000000 + dt.Month * 1000000 + dt.Day * 10000 + dt.Hour * 100 + dt.Minute;
            BsonDocument _Element = new BsonDocument();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", Path.GetFileName(_Fichero));
            var update = Builders<BsonDocument>.Update.Set("Last_Write", _Hora_Fichero);
            _Element.Add(new BsonElement("_id", Path.GetFileName(_Fichero)));
            _Element.Add(new BsonElement("Last_Write", _Hora_Fichero));
            _Element.Add(new BsonElement("Casino", _Casino));
            if (_Collection.GetCollection<BsonDocument>("Ficheros").Find(filter).CountDocuments() == 0)
            {
                _Collection.GetCollection<BsonDocument>("Ficheros").InsertOne(_Element);
            }
            else
            {
                
                _Collection.GetCollection<BsonDocument>("Ficheros").UpdateOne(filter, update);
            }
        }

 
        public IFindFluent<BsonDocument, BsonDocument> Get_Hand()
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands");
            var _Hand = _Collection.Find(new BsonDocument());
            return _Hand;
        }
        public void Obtener_Jugadores()
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands");
            foreach (var Hand in _Access.Get_Hand().ToList())
            {
                foreach (var Jugador in Hand.GetElement("Players").Value.AsBsonArray)
                {
                    bool registrar = true;
                    foreach (var Lista in Lista_Jugadores)
                    {
                        if (Lista == Jugador.AsString.Split(" ")[2])
                        {
                            registrar = false;
                        }
                    }
                    if (registrar)
                    {
                        Lista_Jugadores.Add(Jugador.AsString.Split(" ")[2]);
                    }
                }
            }
            BsonArray Jugadores_Kraken = new BsonArray();
            foreach (var Jugador in Lista_Jugadores)
            {
                Jugadores_Kraken.Add(Jugador);
            }
            if (_Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores").Find(new BsonDocument(new BsonElement("_id", 0))).CountDocuments() == 0)
            {
                _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores").InsertOne(new BsonDocument(new BsonElement("_id", 0), new BsonElement("Jugadores", Jugadores_Kraken)));
            }
            else 
            {
                _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores").DeleteOne(new BsonDocument(new BsonElement("_id", 0)));

                _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores").InsertOne(new BsonDocument(new BsonElement("_id", 0), new BsonElement("Jugadores", Jugadores_Kraken)));
            }

        }
        public void STATS()
        {
            double _VPIP = 0;
            double _OVPIP = 0;
            double _CC = 0;
            double _OCC = 0;
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands");
            foreach (var Document in _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores").Find(new BsonDocument(new BsonElement("_id", 0))).ToList())
            {
                foreach (var Jugador in Document.GetElement("Jugadores").Value.AsBsonArray)
                {
                    foreach (var Hand in _Collection.Find(new BsonDocument("Players", new Regex(Jugador.AsString))).ToList())
                    {
                        string _Posicion = "";
                        foreach (var posicion in Hand.GetElement("Players").Value.AsBsonArray)
                        {
                            if (posicion.AsString.Contains(Jugador.AsString))
                            {
                                if (posicion.AsString.Contains("Position: SB / BTN"))
                                {
                                    _Posicion = "SB";
                                }
                                else
                                {
                                    _Posicion = posicion.AsString.Split("Position: ")[1];
                                }

                                foreach (var Accion in Hand.GetElement(_Posicion).Value.AsBsonArray)
                                {
                                    if (Accion.AsString.Contains("BET"))
                                    {
                                        _VPIP++;
                                        _OVPIP++;
                                    }
                                    if (Accion.AsString.Contains("RAISES"))
                                    {
                                        _VPIP++;
                                        _OVPIP++;
                                        _OCC++;
                                    }
                                    if (Accion.AsString.Contains("CALL"))
                                    {
                                        _CC++;
                                        _OCC++;
                                        _OVPIP++;
                                    }
                                    if (Accion.AsString.Contains("FOLD"))
                                    {
                                        _OCC++;
                                        _OVPIP++;
                                    }
                                }
                            }
                        }
                    }
                    var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador.AsString);
                    double Numero_Hands = _Collection.Find(new BsonDocument("Players", new Regex(Jugador.AsString))).CountDocuments();
                    if (_Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores").Find(new BsonDocument("_id", new Regex(Jugador.AsString))).CountDocuments() < 1)
                    {

                        BsonDocument _Data = new BsonDocument();
                        _Data.Add(new BsonElement("_id", Jugador.AsString));
                        _Data.Add(new BsonElement("Hands", Numero_Hands));
                        _Data.Add(new BsonElement("VPIP", VPIP(_VPIP, _OVPIP)));
                        _Data.Add(new BsonElement("CC", CC(_CC, _OCC)));
                        _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores").InsertOne(_Data);
                    }
                    else
                    {
                        var _Update = Builders<BsonDocument>.Update.Set("VPIP", VPIP(_VPIP, _OVPIP));
                        _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores").UpdateOne(_Filter, _Update);
                        _Update = Builders<BsonDocument>.Update.Set("CC", CC(_CC, _OCC));
                        _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores").UpdateOne(_Filter, _Update);
                        _Update = Builders<BsonDocument>.Update.Set("Hands", Numero_Hands);
                        _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores").UpdateOne(_Filter, _Update);


                    }
                    _VPIP = 0;
                    _OVPIP = 0;
                    _CC = 0;
                    _OCC = 0;
                }
                
            }
        }
        public double Get_VPIP(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("VPIP").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_CC(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("CC").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_Hands(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            return _Collection.Find(new BsonDocument("_id", new Regex(Jugador))).Count();
        }
        public static double VPIP(double VPIP, double OVPIP)
        {
            return VPIP / OVPIP * 100;
        }
        public static double CC(double CC, double OCC)
        {
            return CC / OCC * 100;
        }
        public static double BET3(double _3BET, double _O3BET)
        {
            return _3BET / _O3BET * 100;
        }
        public static double BET4(double _4BET, double _O4BET)
        {
            return _4BET / _O4BET * 100;
        }
        public static double BET5(double _5BET, double _O5BET)
        {
            return _5BET / _O5BET * 100;
        }
        public static double WSD(double _RaisesWinWSD, double _RaisesWSD)
        {
            return _RaisesWinWSD / _RaisesWSD * 100;
        }
        public static double WTSD(double Showdown, double Checks_Flop)
        {
            return Showdown / Checks_Flop * 100;
        }
        public static double Limp(double _ChecksBlinds, double _OChecksBlinds)
        {
            return _ChecksBlinds / _OChecksBlinds * 100;
        }
        public static double LimpR(double _RaisesBlinds, double _ORaisesBlinds)
        {
            return _RaisesBlinds / _ORaisesBlinds * 100;
        }
        public static double FCB(double _FCB, double _OFCB)
        {
            return _FCB / _OFCB * 100;
        }
        public static double TCB(double _TCB, double _OTCB)
        {
            return _TCB / _OTCB * 100;
        }
        public static double FoldFCB(double _FoldFCB, double _OFoldFCB)
        {
            return _FoldFCB / _OFoldFCB * 100;
        }
        public static double FoldTCB(double _FoldTCB, double _OFoldTCB)
        {
            return _FoldTCB / _OFoldTCB * 100;
        }
        public static double RB(double _RB, double _ORB)
        {
            return _RB / _ORB * 100;
        }
        public static double FDB(double _FDB, double _OFDB)
        {
            return _FDB / _OFDB * 100;
        }

        public void Set_Hand_Winamax(List<string> _BTN,
                                     List<string> _SB,
                                     List<string> _BB,
                                     List<string> _UTG,
                                     List<string> _UTG1,
                                     List<string> _MP,
                                     List<string> _MP1,
                                     List<string> _CO,
                                     List<string> _HJ,
                                     List<string> _Blinds,
                                     List<string> _Flop,
                                     List<string> _Pre,
                                     List<string> _Turn,
                                     List<string> _River,
                                     List<string> _Showdown,
                                     List<string> _Summary,
                                     List<string> _Cards,
                                     string _Hand_ID,
                                     string _Mesa,
                                     List<string> _Players)
        {
            MongoAccess _Access = new MongoAccess();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", _Hand_ID);
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands");
            if (_Collection.Find(new BsonDocument("_id", new Regex(_Hand_ID))).CountDocuments() == 0)
            {

                BsonArray _Bson_BTN = new BsonArray();
                BsonArray _Bson_SB = new BsonArray();
                BsonArray _Bson_BB = new BsonArray();
                BsonArray _Bson_UTG = new BsonArray();
                BsonArray _Bson_UTG1 = new BsonArray();
                BsonArray _Bson_MP = new BsonArray();
                BsonArray _Bson_MP1 = new BsonArray();
                BsonArray _Bson_CO = new BsonArray();
                BsonArray _Bson_HJ = new BsonArray();
                BsonArray _Bson_Blinds = new BsonArray();
                BsonArray _Bson_Flop = new BsonArray();
                BsonArray _Bson_Pre = new BsonArray();
                BsonArray _Bson_Turn = new BsonArray();
                BsonArray _Bson_River = new BsonArray();
                BsonArray _Bson_Showdown = new BsonArray();
                BsonArray _Bson_Summary = new BsonArray();
                BsonArray _Bson_Cards = new BsonArray();
                BsonArray _Bson_Players = new BsonArray();
                foreach (string item in _BTN)
                {
                    _Bson_BTN.Add(item);

                }
                foreach (string item in _SB)
                {
                    _Bson_SB.Add(item);
                }
                foreach (string item in _BB)
                {
                    _Bson_BB.Add(item);
                }
                foreach (string item in _UTG)
                {
                    _Bson_UTG.Add(item);
                }
                foreach (string item in _UTG1)
                {
                    _Bson_UTG1.Add(item);
                }
                foreach (string item in _MP)
                {
                    _Bson_MP.Add(item);
                }
                foreach (string item in _MP1)
                {
                    _Bson_MP1.Add(item);
                }
                foreach (string item in _CO)
                {
                    _Bson_CO.Add(item);
                }
                foreach (string item in _HJ)
                {
                    _Bson_HJ.Add(item);
                }
                foreach (string item in _Blinds)
                {

                    _Bson_Blinds.Add(item);
                }
                foreach (string item in _Pre)
                {

                    _Bson_Pre.Add(item);
                }
                foreach (string item in _Flop)
                {

                    _Bson_Flop.Add(item);
                }
                foreach (string item in _Turn)
                {

                    _Bson_Turn.Add(item);
                }
                foreach (string item in _River)
                {

                    _Bson_River.Add(item);
                }
                foreach (string item in _Showdown)
                {

                    _Bson_Showdown.Add(item);
                }
                foreach (string item in _Summary)
                {

                    _Bson_Summary.Add(item);
                }
                foreach (string item in _Cards)
                {

                    _Bson_Cards.Add(item);
                }
                foreach (string item in _Players)
                {

                    _Bson_Players.Add(item);
                }

                BsonDocument _Hand = new BsonDocument( new BsonElement("_id",(BsonValue)_Hand_ID));
                _Hand.Add(new BsonElement("Casino", "WINAMAX"));
                _Hand.Add(new BsonElement("Mesa", (BsonValue)_Mesa));
                _Hand.Add(new BsonElement("Players", _Bson_Players));
                _Hand.Add(new BsonElement("SB", _Bson_SB));
                _Hand.Add(new BsonElement("BB", _Bson_BB));
                _Hand.Add(new BsonElement("UTG", _Bson_UTG));
                _Hand.Add(new BsonElement("UTG1", _Bson_UTG1));
                _Hand.Add(new BsonElement("MP", _Bson_MP));
                _Hand.Add(new BsonElement("MP1", _Bson_MP1));
                _Hand.Add(new BsonElement("CO", _Bson_CO));
                _Hand.Add(new BsonElement("BTN", _Bson_BTN));
                _Hand.Add(new BsonElement("HJ", _Bson_HJ));
                _Hand.Add(new BsonElement("Cards", _Bson_Cards));
                _Hand.Add(new BsonElement("Showdown", _Bson_Showdown));
                _Hand.Add(new BsonElement("Pre", _Bson_Pre));
                _Hand.Add(new BsonElement("Flop", _Bson_Flop));
                _Hand.Add(new BsonElement("Turn", _Bson_Turn));
                _Hand.Add(new BsonElement("River", _Bson_River));
                _Hand.Add(new BsonElement("Summary", _Bson_Summary));
                _Hand.Add(new BsonElement("Blinds", _Bson_Blinds));
                string[] Update_Ventana = new string[3];
                Update_Ventana[0] = _Mesa.Split("'")[0];
                Update_Ventana[1] = "HAND";
                Update_Ventana[2] = _Hand_ID;
                try
                {
                    _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").InsertOne(_Hand);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public void Set_Hand_888Poker(List<string> _BTN,
                                    List<string> _SB,
                                    List<string> _BB,
                                    List<string> _UTG,
                                    List<string> _UTG1,
                                    List<string> _MP,
                                    List<string> _MP1,
                                    List<string> _CO,
                                    List<string> _HJ,
                                    List<string> _Blinds,
                                    List<string> _Flop,
                                    List<string> _Pre,
                                    List<string> _Turn,
                                    List<string> _River,
                                    List<string> _Showdown,
                                    List<string> _Summary,
                                    List<string> _Cards,
                                    string _Hand_ID,
                                    string _Mesa,
                                    List<string> _Players)
        {
            MongoAccess _Access = new MongoAccess();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", _Hand_ID.Trim());
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands");
            if (_Collection.Find(filter).CountDocuments() == 0)
            {

                BsonArray _Bson_BTN = new BsonArray();
                BsonArray _Bson_SB = new BsonArray();
                BsonArray _Bson_BB = new BsonArray();
                BsonArray _Bson_UTG = new BsonArray();
                BsonArray _Bson_UTG1 = new BsonArray();
                BsonArray _Bson_MP = new BsonArray();
                BsonArray _Bson_MP1 = new BsonArray();
                BsonArray _Bson_CO = new BsonArray();
                BsonArray _Bson_HJ = new BsonArray();
                BsonArray _Bson_Blinds = new BsonArray();
                BsonArray _Bson_Flop = new BsonArray();
                BsonArray _Bson_Pre = new BsonArray();
                BsonArray _Bson_Turn = new BsonArray();
                BsonArray _Bson_River = new BsonArray();
                BsonArray _Bson_Showdown = new BsonArray();
                BsonArray _Bson_Summary = new BsonArray();
                BsonArray _Bson_Cards = new BsonArray();
                BsonArray _Bson_Players = new BsonArray();
                foreach (string item in _BTN)
                {
                    _Bson_BTN.Add(item);

                }
                foreach (string item in _SB)
                {
                    _Bson_SB.Add(item);
                }
                foreach (string item in _BB)
                {
                    _Bson_BB.Add(item);
                }
                foreach (string item in _UTG)
                {
                    _Bson_UTG.Add(item);
                }
                foreach (string item in _UTG1)
                {
                    _Bson_UTG1.Add(item);
                }
                foreach (string item in _MP)
                {
                    _Bson_MP.Add(item);
                }
                foreach (string item in _MP1)
                {
                    _Bson_MP1.Add(item);
                }
                foreach (string item in _CO)
                {
                    _Bson_CO.Add(item);
                }
                foreach (string item in _HJ)
                {
                    _Bson_HJ.Add(item);
                }
                foreach (string item in _Blinds)
                {

                    _Bson_Blinds.Add(item);
                }
                foreach (string item in _Pre)
                {

                    _Bson_Pre.Add(item);
                }
                foreach (string item in _Flop)
                {

                    _Bson_Flop.Add(item);
                }
                foreach (string item in _Turn)
                {

                    _Bson_Turn.Add(item);
                }
                foreach (string item in _River)
                {

                    _Bson_River.Add(item);
                }
                foreach (string item in _Showdown)
                {

                    _Bson_Showdown.Add(item);
                }
                foreach (string item in _Summary)
                {

                    _Bson_Summary.Add(item);
                }
                foreach (string item in _Cards)
                {

                    _Bson_Cards.Add(item);
                }
                foreach (string item in _Players)
                {

                    _Bson_Players.Add(item);
                }

                BsonDocument _Hand = new BsonDocument( new BsonElement("_id", (BsonValue)_Hand_ID.Replace("888POKER", "").Trim()));
                _Hand.Add(new BsonElement("Casino", "888Poker"));
                _Hand.Add(new BsonElement("Mesa", _Mesa.Split("'")[0]));
                _Hand.Add(new BsonElement("Players", _Bson_Players));
                _Hand.Add(new BsonElement("SB", _Bson_SB));
                _Hand.Add(new BsonElement("BB", _Bson_BB));
                _Hand.Add(new BsonElement("UTG", _Bson_UTG));
                _Hand.Add(new BsonElement("UTG1", _Bson_UTG1));
                _Hand.Add(new BsonElement("MP", _Bson_MP));
                _Hand.Add(new BsonElement("MP1", _Bson_MP1));
                _Hand.Add(new BsonElement("CO", _Bson_CO));
                _Hand.Add(new BsonElement("BTN", _Bson_BTN));
                _Hand.Add(new BsonElement("HJ", _Bson_HJ));
                _Hand.Add(new BsonElement("Cards", _Bson_Cards));
                _Hand.Add(new BsonElement("Showdown", _Bson_Showdown));
                _Hand.Add(new BsonElement("Pre", _Bson_Pre));
                _Hand.Add(new BsonElement("Flop", _Bson_Flop));
                _Hand.Add(new BsonElement("Turn", _Bson_Turn));
                _Hand.Add(new BsonElement("River", _Bson_River));
                _Hand.Add(new BsonElement("Summary", _Bson_Summary));
                _Hand.Add(new BsonElement("Blinds", _Bson_Blinds));

                try
                {
                    _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").InsertOne(_Hand);
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        public void Set_Hand_PokerStars(List<string> _BTN,
                                    List<string> _SB,
                                    List<string> _BB,
                                    List<string> _UTG,
                                    List<string> _UTG1,
                                    List<string> _MP,
                                    List<string> _MP1,
                                    List<string> _CO,
                                    List<string> _HJ,
                                    List<string> _Blinds,
                                    List<string> _Flop,
                                    List<string> _Pre,
                                    List<string> _Turn,
                                    List<string> _River,
                                    List<string> _Showdown,
                                    List<string> _Summary,
                                    List<string> _Cards,
                                    string _Hand_ID,
                                    string _Mesa,
                                    List<string> _Players)
        {
            MongoAccess _Access = new MongoAccess();
            var filter = Builders<BsonDocument>.Filter.Eq("_id", _Hand_ID);
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands");
            if (_Collection.Find(new BsonDocument("_id", new Regex(_Hand_ID))).CountDocuments() == 0)
            {

                BsonArray _Bson_BTN = new BsonArray();
                BsonArray _Bson_SB = new BsonArray();
                BsonArray _Bson_BB = new BsonArray();
                BsonArray _Bson_UTG = new BsonArray();
                BsonArray _Bson_UTG1 = new BsonArray();
                BsonArray _Bson_MP = new BsonArray();
                BsonArray _Bson_MP1 = new BsonArray();
                BsonArray _Bson_CO = new BsonArray();
                BsonArray _Bson_HJ = new BsonArray();
                BsonArray _Bson_Blinds = new BsonArray();
                BsonArray _Bson_Flop = new BsonArray();
                BsonArray _Bson_Pre = new BsonArray();
                BsonArray _Bson_Turn = new BsonArray();
                BsonArray _Bson_River = new BsonArray();
                BsonArray _Bson_Showdown = new BsonArray();
                BsonArray _Bson_Summary = new BsonArray();
                BsonArray _Bson_Cards = new BsonArray();
                BsonArray _Bson_Players = new BsonArray();
                foreach (string item in _BTN)
                {
                    _Bson_BTN.Add(item);

                }
                foreach (string item in _SB)
                {
                    _Bson_SB.Add(item);
                }
                foreach (string item in _BB)
                {
                    _Bson_BB.Add(item);
                }
                foreach (string item in _UTG)
                {
                    _Bson_UTG.Add(item);
                }
                foreach (string item in _UTG1)
                {
                    _Bson_UTG1.Add(item);
                }
                foreach (string item in _MP)
                {
                    _Bson_MP.Add(item);
                }
                foreach (string item in _MP1)
                {
                    _Bson_MP1.Add(item);
                }
                foreach (string item in _CO)
                {
                    _Bson_CO.Add(item);
                }
                foreach (string item in _HJ)
                {
                    _Bson_HJ.Add(item);
                }
                foreach (string item in _Blinds)
                {

                    _Bson_Blinds.Add(item);
                }
                foreach (string item in _Pre)
                {

                    _Bson_Pre.Add(item);
                }
                foreach (string item in _Flop)
                {

                    _Bson_Flop.Add(item);
                }
                foreach (string item in _Turn)
                {

                    _Bson_Turn.Add(item);
                }
                foreach (string item in _River)
                {

                    _Bson_River.Add(item);
                }
                foreach (string item in _Showdown)
                {

                    _Bson_Showdown.Add(item);
                }
                foreach (string item in _Summary)
                {

                    _Bson_Summary.Add(item);
                }
                foreach (string item in _Cards)
                {

                    _Bson_Cards.Add(item);
                }
                foreach (string item in _Players)
                {

                    _Bson_Players.Add(item);
                }

                BsonDocument _Hand = new BsonDocument(
                                    new BsonElement("_id", (BsonValue)_Hand_ID));
                _Hand.Add(new BsonElement("Casino", "PokerStars"));
                _Hand.Add(new BsonElement("Mesa", (BsonValue)_Mesa));
                _Hand.Add(new BsonElement("Players", _Bson_Players));
                _Hand.Add(new BsonElement("SB", _Bson_SB));
                _Hand.Add(new BsonElement("BB", _Bson_BB));
                _Hand.Add(new BsonElement("UTG", _Bson_UTG));
                _Hand.Add(new BsonElement("UTG1", _Bson_UTG1));
                _Hand.Add(new BsonElement("MP", _Bson_MP));
                _Hand.Add(new BsonElement("MP1", _Bson_MP1));
                _Hand.Add(new BsonElement("CO", _Bson_CO));
                _Hand.Add(new BsonElement("BTN", _Bson_BTN));
                _Hand.Add(new BsonElement("HJ", _Bson_HJ));
                _Hand.Add(new BsonElement("Cards", _Bson_Cards));
                _Hand.Add(new BsonElement("Showdown", _Bson_Showdown));
                _Hand.Add(new BsonElement("Pre", _Bson_Pre));
                _Hand.Add(new BsonElement("Flop", _Bson_Flop));
                _Hand.Add(new BsonElement("Turn", _Bson_Turn));
                _Hand.Add(new BsonElement("River", _Bson_River));
                _Hand.Add(new BsonElement("Summary", _Bson_Summary));
                _Hand.Add(new BsonElement("Blinds", _Bson_Blinds));
                try
                {

                        _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").InsertOne(_Hand);
                    
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}