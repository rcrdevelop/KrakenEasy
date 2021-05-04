﻿using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using KrakenEasy.Casinos;
using System.IO;
using System.Windows;

namespace KrakenEasy.KrakenBD
{
    public class Registros_Data
    {
        public static void Ventanas(string[] _Mesa)
        {
                if (Mesas.Abiertas.Count != 0)
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
                            _Data.Add(new BsonElement("Mostrar", _Mesa[3]));
                            _Data.Add(new BsonElement("Casino", _Mesa[4]));
                            _Data.Add(new BsonElement("_Last_Hand", MongoAccess.Get_Last_Hand(_Mesa[0])));
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
                            _Datos.Add(new BsonElement("Mostrar", false));
                        }
                        if (i == 4)
                        {
                            _Datos.Add(new BsonElement("Casino", Element));
                        }
                    }
                    Mesas.Abiertas.Add(_Datos);

                //_Collection.GetCollection<BsonDocument>("Ventanas").InsertOne(_Datos);
                //Set_Last_Hand(_Mesa[0]);
            }
            }

        public static void Set_Last_Hand(string _Id_Mesa)
        {
            var dt = DateTime.Now;
            long _Hora_Actual = dt.Year * 1000000000000 + dt.Month * 1000000 + dt.Day * 10000 + dt.Hour * 100 + dt.Minute;
            var Folder = @"C:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands";
            string[] ReadFolder = System.IO.Directory.GetFiles(Folder);
            foreach (string file in ReadFolder)
            {
                if (System.IO.Path.GetFileName(file).Contains(_Id_Mesa))
                {
                    DateTime dtF = File.GetLastWriteTime(file);
                    long _Hora_Fichero = dt.Year * 1000000000000 + dt.Month * 1000000 + dt.Day * 10000 + dt.Hour * 100 + dt.Minute;
                    if (_Hora_Fichero >= _Hora_Actual - 1)
                    {
                        if (System.IO.Path.GetFileName(file).Contains("888Poker".ToUpper()))
                        {
                            if (System.IO.Path.GetFileName(file).Split(" ")[1] == _Id_Mesa.ToUpper())
                            {
                                for (var i = 0; i >= Mesas.Abiertas.Count; i++)
                                {
                                    if (Mesas.Abiertas[i].AsBsonDocument.GetElement("_id").Value.AsString == _Id_Mesa)
                                    {
                                        var _Name_Mesa = System.IO.Path.GetFileName(file).Split(" ")[1];
                                        BsonDocument _Data = Mesas.Abiertas[i].AsBsonDocument;
                                        _Data.Add(new BsonElement("_Last_Hand", MongoAccess.Get_Last_Hand(_Name_Mesa)));
                                        Mesas.Abiertas[i] = _Data;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (System.IO.Path.GetFileName(file).Split("'")[1] == _Id_Mesa.ToUpper())
                            {
                                for (var i = 0; i >= Mesas.Abiertas.Count; i++)
                                {
                                    if (Mesas.Abiertas[i].AsBsonDocument.GetElement("_id").Value.AsString == _Id_Mesa)
                                    {
                                        var _Name_Mesa = System.IO.Path.GetFileName(file).Split("_")[1];
                                        BsonDocument _Data = Mesas.Abiertas[i].AsBsonDocument;
                                        _Data.Add(new BsonElement("_Last_Hand", MongoAccess.Get_Last_Hand(_Name_Mesa)));
                                        Mesas.Abiertas[i] = _Data;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

}