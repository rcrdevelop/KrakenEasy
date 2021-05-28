using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using KrakenEasy.Casinos;
using System.Windows;
using Notification.Wpf;
using Newtonsoft.Json;

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
                //Mesas.Abiertas = new BsonArray();
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
                        _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores").InsertOne(new BsonDocument("_id", "PRUEBA"));
                        _Access.Set_STAT_VPIP((double)0, "PRUEBA");
                        _Access.Set_STAT_CC((double)0, "PRUEBA");
                        _Access.Set_STAT_BET3((double)0, "PRUEBA");
                        _Access.Set_STAT_PFR((double)0, "PRUEBA");
                        _Access.Set_STAT_RB((double)0, "PRUEBA");
                        _Access.Set_STAT_BET5((double)0, "PRUEBA");
                        _Access.Set_STAT_BET4((double)0, "PRUEBA");
                        _Access.Set_STAT_FoldBET3((double)0, "PRUEBA");
                        _Access.Set_STAT_FoldTCB((double)0, "PRUEBA");
                        _Access.Set_STAT_FoldFCB((double)0, "PRUEBA");
                        _Access.Set_STAT_FCB((double)0, "PRUEBA");
                        _Access.Set_STAT_TCB((double)0, "PRUEBA");
                        _Access.Set_STAT_WSD((double)0, "PRUEBA");
                        _Access.Set_STAT_WTSD((double)0, "PRUEBA");
                        _Access.Set_STAT_Limp((double)0, "PRUEBA");
                        _Access.Set_STAT_Hand((double)0, "PRUEBA");
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
        public bool Jugador_STAT(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands");
            if (_Collection.Find(new BsonDocument("_id", Jugador)).CountDocuments() == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<string> Get_Jugadores()
        {
            MongoAccess _Access = new MongoAccess();
            List<string> _Resultado = new List<string>();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");

            foreach (var Jugadores in _Collection.Find(new BsonDocument()).ToList())
            {
                bool Condicion_Agregar = true;
                foreach (var Jugador in _Resultado)
                {
                    if (Jugador.Contains(Jugadores.GetElement("_id").Value.AsString.Trim()))
                    {
                        Condicion_Agregar = false;
                    }
                }
                if (Condicion_Agregar)
                {
                    _Resultado.Add(Jugadores.GetElement("_id").Value.AsString.Trim());
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
        public void Set_Mesas_Activas(string _Mesa)
        {


            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("MesasActivas");
            _Collection.InsertOne(new BsonDocument(new BsonElement("Mesa", _Mesa)));

        }

        public void Set_Mesas_Inactiva(string _Mesa)
        {


            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("MesasActivas");
            _Collection.DeleteOne(new BsonDocument(new BsonElement("Mesa", _Mesa)));

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
        public List<string> Get_Players_Kraken(string _Id_Mesa)
        {

            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken");
            List<string> _Resultado = new List<string>();
            var filter = Builders<BsonDocument>.Filter.Eq("Mesa", _Id_Mesa.Trim());
            var notificationManager = new NotificationManager();

            foreach (var hands in _Collection.GetCollection<BsonDocument>("Hands").Find(filter).ToList())
            {

                var Time = DateTime.Now;
                var HoraActual = Time.Year * 10000000000 + Time.Month * 1000000 + Time.Day * 10000 + Time.Hour * 100 + Time.Minute;
               
                if (hands.GetElement("Date").Value.AsInt64 + 5 >= HoraActual) 
                {
                    

                    foreach (var Players in hands.GetElement("Players").Value.AsBsonArray)
                        {
                        notificationManager = new NotificationManager();
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "KrakenEasy",
                            Message = "Leyendo datos de '" + _Id_Mesa + "'",
                            Type = NotificationType.Information
                        });
                        _Resultado.Add(Players.AsString.Split(": ")[1].Split(" ")[0]);
                        }


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
        public IFindFluent<BsonDocument, BsonDocument> Get_Hands_Jugador(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands");
            var _Hand = _Collection.Find(new BsonDocument("Players",new Regex(Jugador)));
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
            
            
        }
        public void Set_STAT_Hand(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("Hands", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_VPIP(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("VPIP", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_CC(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("CC", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_Limp(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("Limp", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_PFR(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("PFR", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_BET3(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("BET3", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_BET4(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("BET4", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_BET5(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("BET5", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_WSD(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("WSD", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_WTSD(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("WTSD", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_FCB(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("FCB", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_FoldFCB(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("FFCB", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_TCB(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("TCB", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_FoldTCB(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("FTCB", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_FoldBET3(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("FBET3", STAT);
            _Collection.UpdateOne(_Filter, update);
        }
        public void Set_STAT_RB(double STAT, string _Id_Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Id_Jugador);
            var update = Builders<BsonDocument>.Update.Set("RB", STAT);
            _Collection.UpdateOne(_Filter, update);
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
        public double Get_BET3(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("BET3").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_FBET3(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("FBET3").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_BET4(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("BET4").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_BET5(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("BET5").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_PFR(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("PFR").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_Limp(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("Limp").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_WSD(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("WSD").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_WTSD(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("WTSD").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_FCB(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("FCB").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_TCB(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("TCB").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_FFCB(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("FFCB").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_FTCB(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("FTCB").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_RB(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double Resultado = 0;
            foreach (var STATS in _Collection.Find(_Filter).ToList())
            {
                Resultado = STATS.GetElement("RB").Value.AsDouble;
            }
            return Resultado;
        }
        public double Get_Hands(string Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            var _Filter = Builders<BsonDocument>.Filter.Eq("_id", Jugador);
            double _Resultado = 0;
            foreach (var item in _Collection.Find(new BsonDocument("_id", new Regex(Jugador))).ToList())
            {
                _Resultado = item.GetElement("Hands").Value.AsDouble;
            }

            return _Resultado;
        }
        public void Set_VPIP(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players",new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action = 0;
            double Oportunity = 0;
            double VPIP = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                foreach (var pre in hand.GetElement("Pre").Value.AsBsonArray)
                {
                    if (Action_Raises(pre.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                    if (Action_BET(pre.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                    if (Action_Fold(pre.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Check(pre.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Call(pre.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                }
                foreach (var flop in hand.GetElement("Flop").Value.AsBsonArray)
                {
                    if (Action_Raises(flop.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                    if (Action_BET(flop.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                    if (Action_Fold(flop.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Check(flop.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Call(flop.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                }
                foreach (var turn in hand.GetElement("Turn").Value.AsBsonArray)
                {
                    if (Action_Raises(turn.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                    if (Action_BET(turn.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                    if (Action_Fold(turn.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Check(turn.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Call(turn.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                }
                foreach (var river in hand.GetElement("River").Value.AsBsonArray)
                {
                    if (Action_Raises(river.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                    if (Action_BET(river.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                    if (Action_Fold(river.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Check(river.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Call(river.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                }
            }
            VPIP = Action / Oportunity * 100;
            var update = Builders<BsonDocument>.Update.Set("VPIP", VPIP);
            bool Nuevo_Jugador = true;
            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement vpip = new BsonElement("VPIP", VPIP);
                stat.Add(jugador);
                stat.Add(vpip);
                _CollectionSTAT.InsertOne(stat);
            }
        }
        public void Set_CC(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action = 0;
            double Oportunity = 0;
            double CC = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                foreach (var pre in hand.GetElement("Pre").Value.AsBsonArray)
                {
                    if (Action_Raises(pre.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Fold(pre.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Call(pre.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                }
                foreach (var flop in hand.GetElement("Flop").Value.AsBsonArray)
                {
                    if (Action_Raises(flop.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Fold(flop.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Call(flop.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                }
                foreach (var turn in hand.GetElement("Turn").Value.AsBsonArray)
                {
                    if (Action_Raises(turn.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Fold(turn.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Call(turn.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                }
                foreach (var river in hand.GetElement("River").Value.AsBsonArray)
                {
                    if (Action_Raises(river.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Fold(river.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Call(river.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                }
            }
            CC = Action - Oportunity;
            var update = Builders<BsonDocument>.Update.Set("CC", CC);
            bool Nuevo_Jugador = true;

            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement cc = new BsonElement("CC", CC);
                stat.Add(jugador);
                stat.Add(cc);
                _CollectionSTAT.InsertOne(stat);
            };
        }
        public void Set_BET3(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action = 0;
            double Oportunity = 0;
            double BET3 = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                Action_BET3(hand, "Pre");
                Action_BET3(hand, "Flop");
                Action_BET3(hand, "Turn");
                Action_BET3(hand, "River");
            }
            BET3 = Action - Oportunity;
            var update = Builders<BsonDocument>.Update.Set("BET3", BET3);
            bool Nuevo_Jugador = true;

            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement bet3 = new BsonElement("BET3", BET3);
                stat.Add(jugador);
                stat.Add(bet3);
                _CollectionSTAT.InsertOne(stat);
            }
            void Action_BET3(BsonDocument hand, string etapa)
            {
                double BET = 1;
                bool condicion = false;
                foreach (var linea in hand.GetElement(etapa).Value.AsBsonArray)
                {
                    if (linea.AsString.Contains("BET".ToUpper()))
                    {
                        BET++;

                    }
                    if (linea.AsString.Contains("raises".ToUpper()))
                    {
                        BET++;
                        if (linea.AsString.Contains(_Jugador) && BET == 3)
                        {
                            Window window = new Window();
                            window.Show();
                            Action++;
                            condicion = true;
                        }
                        
                    }
                    if (Action_Fold(linea.AsString, _Jugador))
                    {
                        if (linea.AsString.Contains(_Jugador) && BET == 2)
                        {
                            Oportunity++;
                        }
                    }
                    if (Action_Call(linea.AsString, _Jugador))
                    {
                        if (linea.AsString.Contains(_Jugador) && BET == 2)
                        {
                            Oportunity++;
                        }
                    }

                }
            }
        }
        public void Set_BET4(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action = 0;
            double Oportunity = 0;
            double BET4 = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                Action_BET4(hand, "Pre");
                Action_BET4(hand, "Flop");
                Action_BET4(hand, "Turn");
                Action_BET4(hand, "River");
            }
            BET4 = Action / Oportunity * 100;
            var update = Builders<BsonDocument>.Update.Set("BET4", BET4);
            bool Nuevo_Jugador = true;

            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement bet4 = new BsonElement("BET4", BET4);
                stat.Add(jugador);
                stat.Add(bet4);
                _CollectionSTAT.InsertOne(stat);
            }
            void Action_BET4(BsonDocument hand, string etapa)
            {
                double BET = 4;
                bool condicion = false;
                foreach (var linea in hand.GetElement(etapa).Value.AsBsonArray)
                {
                    if (linea.AsString.Contains("BET".ToUpper()))
                    {
                        BET++;

                    }
                    if (linea.AsString.Contains("raises".ToUpper()))
                    {
                        BET++;
                        if (linea.AsString.Contains(_Jugador) && BET == 4)
                        {
                            Action++;
                            condicion = true;
                        }

                    }
                    if (Action_Fold(linea.AsString, _Jugador))
                    {
                        if (linea.AsString.Contains(_Jugador) && BET == 4)
                        {
                            Oportunity++;
                        }
                    }
                    if (Action_Call(linea.AsString, _Jugador))
                    {
                        if (linea.AsString.Contains(_Jugador) && BET == 4)
                        {
                            Oportunity++;
                        }
                    }

                }
            }
        }
        public void Set_BET5(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action = 0;
            double Oportunity = 0;
            double BET5 = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                Action_BET5(hand, "Pre");
                Action_BET5(hand, "Flop");
                Action_BET5(hand, "Turn");
                Action_BET5(hand, "River");
            }
            BET5 = Action - Oportunity;
            var update = Builders<BsonDocument>.Update.Set("BET5", BET5);
            bool Nuevo_Jugador = true;

            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement bet5 = new BsonElement("BET5", BET5);
                stat.Add(jugador);
                stat.Add(bet5);
                _CollectionSTAT.InsertOne(stat);
            }
            void Action_BET5(BsonDocument hand, string etapa)
            {
                double BET = 5;
                bool condicion = false;
                foreach (var linea in hand.GetElement(etapa).Value.AsBsonArray)
                {
                    if (linea.AsString.Contains("BET".ToUpper()))
                    {
                        BET++;

                    }
                    if (linea.AsString.Contains("raises".ToUpper()))
                    {
                        BET++;
                        if (linea.AsString.Contains(_Jugador) && BET == 5)
                        {
                            Action++;
                            condicion = true;
                        }

                    }
                    if (Action_Fold(linea.AsString, _Jugador))
                    {
                        if (linea.AsString.Contains(_Jugador) && BET == 5)
                        {
                            Oportunity++;
                        }
                    }
                    if (Action_Call(linea.AsString, _Jugador))
                    {
                        if (linea.AsString.Contains(_Jugador) && BET == 5)
                        {
                            Oportunity++;
                        }
                    }

                }
            }
        }
        public void Set_Limp(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action = 0;
            double Oportunity = 0;
            double Limp = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                string BB = "";
                bool Condicion = false;
                foreach (var players in hand.GetElement("Players").Value.AsBsonArray)
                {
                    if (players.AsString.Contains("Position: BB"))
                    { 
                        BB = players.AsString.Split(": ")[0].Split(" ")[0];
                    }
                }
                foreach (var pre in hand.GetElement("Pre").Value.AsBsonArray)
                {
                    if (Action_BET(pre.AsString, _Jugador) && pre.AsString.Contains(BB))
                    {
                        Condicion = true;
                    }
                    if (pre.AsString.Contains("raises".ToUpper()) && !pre.AsString.Contains(_Jugador))
                    {
                        Condicion = false;
                    }
                    if (Action_Raises(pre.AsString, _Jugador) && Condicion)
                    {
                        Oportunity++;
                    }

                    if (Action_Fold(pre.AsString, _Jugador) && Condicion)
                    {
                        Oportunity++;
                    }
                    if (Action_Call(pre.AsString, _Jugador) && Condicion)
                    {
                        Action++;
                        Oportunity++;
                    }
                }
            }
            Limp = Action - Oportunity;
            if((Action == 0) && (Oportunity == 0))
            {
                Limp = 0;
            }
            var update = Builders<BsonDocument>.Update.Set("Limp", Limp);
            bool Nuevo_Jugador = true;
            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement limp = new BsonElement("Limp", Limp);
                stat.Add(jugador);
                stat.Add(limp);
                _CollectionSTAT.InsertOne(stat);
            }
        }
        public void Set_RB(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action = 0;
            double Oportunity = 0;
            double RB = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                foreach (var pre in hand.GetElement("River").Value.AsBsonArray)
                {
                    if (Action_BET(pre.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                    if (Action_Check(pre.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                }
            }
            RB = Action - Oportunity;
            if ((Action == 0) && (Oportunity == 0))
            {
                RB = 0;
            }
            var update = Builders<BsonDocument>.Update.Set("RB", RB);
            bool Nuevo_Jugador = true;
            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement rb = new BsonElement("RB", RB);
                stat.Add(jugador);
                stat.Add(rb);
                _CollectionSTAT.InsertOne(stat);
            }
        }
        public void Set_PFR(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action = 0;
            double Oportunity = 0;
            double PFR = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                foreach (var pre in hand.GetElement("Pre").Value.AsBsonArray)
                {
                    if (Action_Raises(pre.AsString, _Jugador))
                    {
                        Action++;
                        Oportunity++;
                    }
                    if (Action_Check(pre.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Call(pre.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                    if (Action_Fold(pre.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                }
            }
            PFR = Action - Oportunity;
            if ((Action == 0) && (Oportunity == 0))
            {
                PFR = 0;
            }
            var update = Builders<BsonDocument>.Update.Set("PFR", PFR);
            bool Nuevo_Jugador = true;
            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement pfr = new BsonElement("PFR", PFR);
                stat.Add(jugador);
                stat.Add(pfr);
                _CollectionSTAT.InsertOne(stat);
            }
        }
        public void Set_FCB(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action = 0;
            double Oportunity = 0;
            double FCB = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                bool Condicion = false;
                foreach (var pre in hand.GetElement("Pre").Value.AsBsonArray)
                {
                    if (Action_BET(pre.AsString, _Jugador))
                    {
                        Condicion = true;
                    }
                }
                foreach (var flop in hand.GetElement("Flop").Value.AsBsonArray)
                {
                    if (Action_BET(flop.AsString, _Jugador))
                    {
                        if (Condicion)
                        { 
                            Action++;
                            Oportunity++;
                        }
                    }
                    if (Action_Check(flop.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                }
                
            }
            FCB = Action - Oportunity;
            var update = Builders<BsonDocument>.Update.Set("FCB", FCB);
            bool Nuevo_Jugador = true;

            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement fcb = new BsonElement("FCB", FCB);
                stat.Add(jugador);
                stat.Add(fcb);
                _CollectionSTAT.InsertOne(stat);
            };

        }
        public void Set_TCB(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action = 0;
            double Oportunity = 0;
            double TCB = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                bool Condicion = false;
                foreach (var pre in hand.GetElement("Flop").Value.AsBsonArray)
                {
                    if (Action_BET(pre.AsString, _Jugador))
                    {
                        Condicion = true;
                    }
                }
                foreach (var flop in hand.GetElement("Turn").Value.AsBsonArray)
                {
                    if (Action_BET(flop.AsString, _Jugador))
                    {
                        if (Condicion)
                        {
                            Action++;
                            Oportunity++;
                        }
                    }
                    if (Action_Check(flop.AsString, _Jugador))
                    {
                        Oportunity++;
                    }
                }

            }
            TCB = Action - Oportunity;
            var update = Builders<BsonDocument>.Update.Set("TCB", TCB);
            bool Nuevo_Jugador = true;

            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement tcb = new BsonElement("TCB", TCB);
                stat.Add(jugador);
                stat.Add(tcb);
                _CollectionSTAT.InsertOne(stat);
            };

        }
        public void Set_FoldFCB(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action_Raise = 0;
            double Action_Calls = 0;
            double Action = 0;
            double Oportunity = 0;
            double FFCB = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                bool Condicion = false;
                bool CondicionFCB = false;
                string JugadorBET = "";
                foreach (var pre in hand.GetElement("Pre").Value.AsBsonArray)
                {
                    if (pre.AsString.Contains("BET".ToUpper()))
                    {
                        Condicion = true;
                        JugadorBET = pre.AsString.Split(": ")[0].Split(" ")[0];
                    }
                }
                foreach (var flop in hand.GetElement("Flop").Value.AsBsonArray)
                {
                    if (flop.AsString.Contains("BET".ToUpper()) && flop.AsString.Contains(JugadorBET))
                    {
                        CondicionFCB = true;
                    }
                    if (CondicionFCB)
                    { 
                        if (Action_Fold(flop.AsString, _Jugador))
                        {
                            Action++;
                        }
                        if (Action_Raises(flop.AsString, _Jugador))
                        {

                            Action_Raise++;
                        }
                        if (Action_Call(flop.AsString, _Jugador))
                        {
                            Action_Calls++;
                        }
                    }
                }

            }
            FFCB = Action - Action_Calls + Action_Raise;
            var update = Builders<BsonDocument>.Update.Set("FFCB", FFCB);
            bool Nuevo_Jugador = true;

            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement ffcb = new BsonElement("FFCB", FFCB);
                stat.Add(jugador);
                stat.Add(ffcb);
                _CollectionSTAT.InsertOne(stat);
            };

        }
        public void Set_FoldTCB(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action_Raise = 0;
            double Action_Calls = 0;
            double Action = 0;
            double Oportunity = 0;
            double FTCB = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                bool Condicion = false;
                bool CondicionFCB = false;
                string JugadorBET = "";
                foreach (var flop in hand.GetElement("Flop").Value.AsBsonArray)
                {
                    if (flop.AsString.Contains("BET".ToUpper()))
                    {
                        Condicion = true;
                        JugadorBET = flop.AsString.Split(": ")[0].Split(" ")[0];
                    }
                }
                foreach (var turn in hand.GetElement("Turn").Value.AsBsonArray)
                {
                    if (turn.AsString.Contains("BET".ToUpper()) && turn.AsString.Contains(JugadorBET))
                    {
                        CondicionFCB = true;
                    }
                    if (CondicionFCB)
                    {
                        if (Action_Fold(turn.AsString, _Jugador))
                        {
                            Action++;
                        }
                        if (Action_Raises(turn.AsString, _Jugador))
                        {

                            Action_Raise++;
                        }
                        if (Action_Call(turn.AsString, _Jugador))
                        {
                            Action_Calls++;
                        }
                    }
                }

            }
            FTCB = Action - Action_Calls + Action_Raise;
            var update = Builders<BsonDocument>.Update.Set("FTCB", FTCB);
            bool Nuevo_Jugador = true;

            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement ftcb = new BsonElement("FTCB", FTCB);
                stat.Add(jugador);
                stat.Add(ftcb);
                _CollectionSTAT.InsertOne(stat);
            };

        }
        public void Set_FoldBET3(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action = 0;
            double Oportunity = 0;
            double FoldBET3 = 0;
            double hands = 0;
            foreach (var hand in _Collection.ToList())
            {
                hands++;
                Action_BET3(hand, "Pre");
                Action_BET3(hand, "Flop");
                Action_BET3(hand, "Turn");
                Action_BET3(hand, "River");
            }
            FoldBET3 = Action - Oportunity;
            var update = Builders<BsonDocument>.Update.Set("FBET3", FoldBET3);
            bool Nuevo_Jugador = true;

            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement foldbet3 = new BsonElement("FBET3", FoldBET3);
                stat.Add(jugador);
                stat.Add(foldbet3);
                _CollectionSTAT.InsertOne(stat);
            }
            void Action_BET3(BsonDocument hand, string etapa)
            {
                double BET = 1;
                bool condicion = false;
                foreach (var linea in hand.GetElement(etapa).Value.AsBsonArray)
                {
                    if (linea.AsString.Contains("BET".ToUpper()))
                    {
                        BET++;

                    }
                    if (linea.AsString.Contains("raises".ToUpper()))
                    {
                        BET++;
                        if (linea.AsString.Contains(_Jugador) && BET == 4)
                        {
                            Oportunity++;
                        }

                    }
                    if (Action_Fold(linea.AsString, _Jugador))
                    {
                        if (linea.AsString.Contains(_Jugador) && BET == 3)
                        {
                            Action++;
                            Oportunity++;
                        }
                    }
                    if (Action_Call(linea.AsString, _Jugador))
                    {
                        if (linea.AsString.Contains(_Jugador) && BET == 3)
                        {
                            Oportunity++;
                        }
                    }

                }
            }

        }
        public void Set_WTSD(string _Jugador)
        {

            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Action_Calls = 0;
            double ShowDown = 0;
            double WTSD = 0;
            
            foreach (var hand in _Collection.ToList())
            {
                foreach (var flop in hand.GetElement("Flop").Value.AsBsonArray)
                {
                    if (Action_Call(flop.AsString, _Jugador))
                    {
                        Action_Calls++;                       
                    }
                }
                foreach (var summary in hand.GetElement("Summary").Value.AsBsonArray)
                {
                    if (summary.AsString.Contains(_Jugador))
                    {
                        ShowDown++;
                    }
                }

            }
            WTSD = ShowDown / Action_Calls * 100;
            var update = Builders<BsonDocument>.Update.Set("WTSD", WTSD);
            bool Nuevo_Jugador = true;
            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement wtsd = new BsonElement("WTSD", WTSD);
                stat.Add(jugador);
                stat.Add(wtsd);
                _CollectionSTAT.InsertOne(stat);
            }
        }
        public void Set_WSD(string _Jugador)
        {
            MongoAccess _Access = new MongoAccess();
            var _Session = _Access._Client.StartSession();
            var _Filter = Builders<BsonDocument>.Filter.Eq("Players", _Jugador);
            var _Collection = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Hands").Find(new BsonDocument(new BsonElement("Players", new Regex(_Jugador))));
            var _CollectionSTAT = _Session.Client.GetDatabase("Kraken").GetCollection<BsonDocument>("Jugadores");
            double Win = 0;
            double ShowDown = 0;
            double WSD = 0;

            foreach (var hand in _Collection.ToList())
            {
                foreach (var summary in hand.GetElement("Summary").Value.AsBsonArray)
                {
                    if (summary.AsString.Contains(_Jugador))
                    {
                        ShowDown++;
                        if (summary.AsString.Contains("won".ToUpper()) || summary.AsString.Contains("collected".ToUpper()))
                        {
                            Win++;
                        }
                    }
                }

            }
            WSD = Win / ShowDown * 100;
            var update = Builders<BsonDocument>.Update.Set("WSD", WSD);
            bool Nuevo_Jugador = true;
            _Filter = Builders<BsonDocument>.Filter.Eq("_id", _Jugador);
            foreach (var Jugador in _CollectionSTAT.Find(_Filter).ToList())
            {
                _CollectionSTAT.UpdateOne(_Filter, update);
                Nuevo_Jugador = false;
            }
            if (Nuevo_Jugador)
            {
                BsonDocument stat = new BsonDocument();
                BsonElement jugador = new BsonElement("_id", _Jugador);
                BsonElement wsd = new BsonElement("WSD", WSD);
                stat.Add(jugador);
                stat.Add(wsd);
                _CollectionSTAT.InsertOne(stat);
            }
        }
        bool Action_Raises(string linea, string Jugador)
        {
            if (linea.Contains("raises".ToUpper()) && linea.Contains(Jugador.ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool Action_Fold(string linea, string Jugador)
        {
            if (linea.Contains("fold".ToUpper()) && linea.Contains(Jugador.ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool Action_BET(string linea, string Jugador)
        {
            if (linea.Contains("BET".ToUpper()) && linea.Contains(Jugador.ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool Action_Check(string linea, string Jugador)
        {
            if (linea.Contains("check".ToUpper()) && linea.Contains(Jugador.ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        bool Action_Call(string linea, string Jugador)
        {
            if (linea.Contains("call".ToUpper()) && linea.Contains(Jugador.ToUpper()))
            {
                return true;
            }
            else
            {
                return false;
            }
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
                var Time = DateTime.Now;
                var HoraActual = Time.Year * 10000000000 + Time.Month * 1000000 + Time.Day * 10000 + Time.Hour * 100 + Time.Minute;
                
                _Hand.Add(new BsonElement("Casino", "WINAMAX"));
                _Hand.Add(new BsonElement("Mesa", (BsonValue)_Mesa));
                _Hand.Add(new BsonElement("Date", HoraActual));
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
                    HUDS.Propiedades.Actualizar = true;
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
                var Time = DateTime.Now;
                var HoraActual = Time.Year * 10000000000 + Time.Month * 1000000 + Time.Day * 10000 + Time.Hour * 100 + Time.Minute;
                _Hand.Add(new BsonElement("Date", HoraActual));
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
                    HUDS.Propiedades.Actualizar = true;
                }
                catch (Exception)
                {

                    
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

                BsonDocument _Hand = new BsonDocument(
                                    new BsonElement("_id", (BsonValue)_Hand_ID));
                var Time = DateTime.Now;
                var HoraActual = Time.Year * 10000000000 + Time.Month * 1000000 + Time.Day * 10000 + Time.Hour * 100 + Time.Minute;
                _Hand.Add(new BsonElement("Date", HoraActual));
                _Hand.Add(new BsonElement("Casino", "PokerStars"));
                _Hand.Add(new BsonElement("Mesa", _Mesa));
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
                        HUDS.Propiedades.Actualizar = true;
                    
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}