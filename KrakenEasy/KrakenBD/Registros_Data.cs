using System;
using System.Collections.Generic;
using System.Text;
using KrakenEasy.Casinos;
using System.IO;
using System.Windows;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization.Attributes;

using Newtonsoft.Json;

namespace KrakenEasy.KrakenBD
{
    public static class Registros_Data
    {


        public static bool Hole_Cards { get; set; }


        public static void InicializarRecursos()
        {

            var path = Environment.CurrentDirectory+"/rutas.json";




            if (File.Exists(path))
            {
                using (StreamReader jsonStream = File.OpenText(path))
                {
                    var json = jsonStream.ReadToEnd();
                    Casinos.Casinos Casino = Newtonsoft.Json.JsonConvert.DeserializeObject<Casinos.Casinos>(json);
                    Winamax._Ruta = Casino.Winamax_Ruta;
                    Poker888._Ruta = Casino.Poker888_Ruta;
                    PokerStars._Ruta = Casino.PokerStars_Ruta;
                    Winamax.Habilitado = Casino.Winamax_Habilitado;
                    Poker888.Habilitado = Casino.Poker888_Habilitado;
                    PokerStars.Habilitado = Casino.PokerStars_Habilitado;
                }
                
            }
            else 
            {
                Winamax._Ruta = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/Winamax Poker/accounts/";
                Poker888._Ruta = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/888poker/HandHistory/";
                PokerStars._Ruta = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/AppData/Local/PokerStars/HandHistory/";
                var Casino = new Casinos.Casinos
                {
                    Winamax_Ruta = Winamax._Ruta,
                    Poker888_Ruta = Poker888._Ruta,
                    PokerStars_Ruta = PokerStars._Ruta,
                };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(Casino);
                System.IO.File.WriteAllText(path, json);
                Winamax.Habilitado = false;
                Poker888.Habilitado = false;
                PokerStars.Habilitado = false;
            }
            

            KrakenEasy.HUDS.HUDS.Lista = new BsonArray();
            
            Hole_Cards = true;
            HUDS.Propiedades.Relative = 1;
 
  
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
        public static void STAT(BsonDocument Datos)
        {

            //string json = MongoDB.Bson.IO.JsonConvert. SerializeObject(Casino);
            //System.IO.File.WriteAllText(path, json);


        }
        [BsonIgnoreExtraElements]
        public class RestaurantGradeDb
        {
            [BsonElement(elementName: "date")]
            public DateTime InsertedUtc { get; set; }
            [BsonElement(elementName: "grade")]
            public string Grade { get; set; }
            [BsonElement(elementName: "score")]
            public object Score { get; set; }
        }
        [BsonIgnoreExtraElements]
        public class STATS
        {
            [BsonElement(elementName: "_id")]
            public string _id { get; set; }
            [BsonElement(elementName: "Estructura")]
            public BsonArray Estructura { get; set; }
            [BsonElement(elementName: "STAT_Name")]
            public BsonDocument STAT_Name { get; set; }
            [BsonElement(elementName: "Formula")]
            public BsonDocument Formula { get; set; }
            //BsonDocument STAT_Name = new(
            //  new BsonElement("Oportunidad_Action", new BsonArray()),
            //  new BsonElement("Action", new BsonArray()),
            //  new BsonElement("Formula_STAT", new BsonDocument(
            //      new BsonElement("Variables", new BsonArray()),
            //      new BsonElement("Formula", new BsonDocument(
            //        new BsonElement("Orden", new BsonArray()),
            //        new BsonElement("Operadores", new BsonArray())
            //      ))
            //  ))
            //);
        }

    }

}
