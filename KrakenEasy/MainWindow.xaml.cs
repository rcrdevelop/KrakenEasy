﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Collections.ObjectModel;
using System.Management.Automation;
using KrakenEasy.KrakenBD;
using KrakenEasy.Hands;
using KrakenEasy.Replayer;
using KrakenEasy.Servicios;
using System.Windows.Threading;
using System.IO;
using KrakenEasy.HUDS;
using MongoDB.Bson;

namespace KrakenEasy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Servicios.Servicios _Servicios = new Servicios.Servicios();
        public MainWindow()
        {
            InitializeComponent();
        }
        private void HUD_Click(object sender, RoutedEventArgs e)
        {
            MongoAccess _Access = new MongoAccess();
            _Access.InicializarMain();
            KrakenEasy.HUDS.HUDS.Lista = new BsonArray();
            Casinos.Mesas.Abiertas = new BsonArray();
            Thread _Hilo_Servicio = new Thread(Iniciar_Servicio);
            Thread _Hilo_HUDS = new Thread(Iniciar_HUDS);

            if (this.HUD.Content.ToString().ToUpper() == "Iniciar Kraken".ToUpper())
            {
                Propiedades.SystemActive = true;
                this.HUD.Content = "Detener Kraken";
                _Hilo_Servicio.Start();
                _Hilo_HUDS.Start();
            }
            else
            {

                _Hilo_HUDS.Interrupt();
                this.HUD.Content = "Iniciar Kraken";
                Detener_Servicio();
            }
        }

        private void Replayer_Click(object sender, RoutedEventArgs e)
        {
            Replayer.MainWindow _Replayer = new Replayer.MainWindow();
            _Replayer.Show();

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Detener_Servicio();
            Process.GetCurrentProcess().Kill();
        }

        private void Importar_Hands(object sender, RoutedEventArgs e)
        {
            main_Manejeador_Manos _Hands = new main_Manejeador_Manos();
            _Hands.Iniciar();
        }
        private static void Detener_Servicio()
        {
            MongoAccess _Access = new MongoAccess();
            Propiedades.SystemActive = false;


        }
        private static void Iniciar_Servicio()
        {
            Propiedades.SystemActive = true;
            _Servicios.main();
            //string imagepath =  Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ManejadorServicios.exe");
            //Process p = new Process();
            //ProcessStartInfo psi = new ProcessStartInfo(imagepath);
            //p.StartInfo = psi;
            //p.Start();

        }
        private static void Iniciar_HUDS()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                HUDControler _Controler = new HUDControler();
                _Controler.Show();
                Thread _Hilo_Monito_HUDS = new Thread(Monitor_HUDS);
                _Hilo_Monito_HUDS.Start();
            });
        }
        private static void Monitor_HUDS()
        {

              
            while (true)
            {
                if (Casinos.Mesas.Abiertas != null)
                {

                
                    for (int i = 0; i > Casinos.Mesas.Abiertas; i++)
                    {
                        if (Casinos.Mesas.Abiertas.Count > 0)
                        { 
                        
                        MongoAccess _Access = new MongoAccess();

                        List<string> Players = _Access.Get_Players(Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("_id").Value.AsString);
                    
                        if (Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("Activa").Value.AsBoolean && !Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("Ready").Value.AsBoolean)
                        {
                            foreach (string Player in Players)
                            {
                                    Thread _Hilo_Contenedor = new Thread(() => HUDS(Player.Split(" ")[2], Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("_id").Value.AsString));
                                    _Hilo_Contenedor.Start();
                            }
                                BsonDocument _Data = new BsonDocument();
                                _Data.Add(new BsonElement("_id", Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("_id").Value));
                                _Data.Add(new BsonElement("Dimensiones", Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("Dimensiones").Value));
                                _Data.Add(new BsonElement("Activa", Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("Activa").Value));
                                _Data.Add(new BsonElement("Ready", true));
                                _Data.Add(new BsonElement("Casino", Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("Casino").Value));
                                _Data.Add(new BsonElement("_Last_Hand", Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("_Last_Hand").Value));
                                Window window = new Window();
                                window.Show();

                                Casinos.Mesas.Abiertas[i] = _Data;
                        }

                            //for (int i = 0; i < Players.Count; i++)
                            //{
                            //    Thread _Hilo_Contenedor = new Thread(() => HUDS(Players[i], items[0]));
                            //    _Hilo_Contenedor.Start();
                            //}
                        }
                    }
                }
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        private static void HUDS(string _Id_Jugador, string _Id_Ventana)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressKraken _KrakenHUD = new ProgressKraken(_Id_Jugador);
                ContenedorHUD _HUD = new ContenedorHUD(_Id_Jugador, _Id_Ventana);
                _HUD.Contenedor.Children.Add(_KrakenHUD);
                _HUD.Show();
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings _Settings = new Settings();
            _Settings.Show();
        }
    }
}