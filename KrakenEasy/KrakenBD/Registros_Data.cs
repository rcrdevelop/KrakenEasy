using System;
using System.Collections.Generic;
using System.Text;
using KrakenEasy.Casinos;
using System.IO;
using System.Windows;
using System.Security.Principal;
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
                Winamax._Ruta = @"c:/Users/" + (WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/Winamax Poker/accounts/";
                Poker888._Ruta = @"c:/Users/" + (WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/888poker/HandHistory/";
                PokerStars._Ruta = @"c:/Users/" + (WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/AppData/Local/PokerStars/HandHistory/";
                var Casino = new Casinos.Casinos
                {
                    Winamax_Ruta = Winamax._Ruta,
                    Poker888_Ruta = Poker888._Ruta,
                    PokerStars_Ruta = PokerStars._Ruta,
                };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(Casino);
                File.WriteAllText(path, json);
                Winamax.Habilitado = false;
                Poker888.Habilitado = false;
                PokerStars.Habilitado = false;
            }
            

            KrakenEasy.HUDS.HUDS.Lista = new BsonArray();
            
            Hole_Cards = true;
            HUDS.Propiedades.Relative = 1;
 
  
        }
    }
    

}
