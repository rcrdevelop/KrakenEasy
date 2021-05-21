using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using KrakenEasy.KrakenBD;
using KrakenEasy.Hands;
using KrakenEasy.HUDS;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using Notify;
using Notification.Wpf;
using System.IO;
using System.Collections.Generic;

namespace KrakenEasy
{
    //Ventana Principal
    public partial class MainWindow : Window
    {



        static Servicios.Servicios _Servicios = new Servicios.Servicios();
        static List<string> Mesas_HUDS_Abiertos = new List<string>();
        static List<string> HUDS_Abiertos = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            Registros_Data.InicializarRecursos();
            KrakenBD.Settings.Inicializar();
        }
        private void HUD_Click(object sender, RoutedEventArgs e)
        {
            MongoAccess _Access = new MongoAccess();
            Thread _Hilo_Monitor = new Thread(Monitor_HUDS);
            Thread _Hilo_Servicio = new Thread(Iniciar_Servicio);
            Thread _Hilo_HUDS = new Thread(Iniciar_HUDS);

            if (this.HUD.Content.ToString().ToUpper() == "Iniciar Kraken".ToUpper())
            {
                try 
                { 
                
                _Access.InicializarMain();
                Propiedades.SystemActive = true;
                this.HUD.Content = "Detener Kraken";
                _Hilo_Servicio.Start();
                _Hilo_Monitor.Start();
                _Hilo_HUDS.Start();
                    var notificationManager = new NotificationManager();

                    notificationManager.Show(new NotificationContent
                    {
                        Title = "KrakenEasy",
                        Message = "Kraken iniciado... HUDS listos para mostrarse.",
                        Type = NotificationType.Notification
                    });
                }
                catch(Exception ex)
                {
                    var notificationManager = new NotificationManager();

                    notificationManager.Show(new NotificationContent
                    {
                        Title = "KrakenEasy",
                        Message = "Error al iniciar Kraken... //" + ex.Message,
                        Type = NotificationType.Error
                    });
                }
            }
            else
            {

                Propiedades.SystemActive = false;

                _Hilo_HUDS.Interrupt();
                this.HUD.Content = "Iniciar Kraken";
                Mesas_HUDS_Abiertos = new List<string>();
                HUDS_Abiertos = new List<string>();
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "KrakenEasy",
                    Message = "Kraken detenido.",
                    Type = NotificationType.Notification
                });
                Detener_Servicio();
            }
        }
        public static void Detener_Sistema() 
        {

        }
        //Iniciar RePlayer
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
        //Llevar ficheros a KrakenHands manualmente
        private void Importar_Hands(object sender, RoutedEventArgs e)
        {
            main_Manejeador_Manos _Hands = new main_Manejeador_Manos();
            _Hands.Iniciar();
        }
        private static void Detener_Servicio()
        {
            Propiedades.SystemActive = false;


        }
        //WorkServices
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
                //Thread _Hilo_Monito_HUDS = new Thread(Monitor_HUDS);
                //_Hilo_Monito_HUDS.Start();
            });
        }
        //Procedimiento para mostras HUDS
        private static void Monitor_HUDS()
        {


            while (true)
            {
                Casinos.Mesas.Abiertas = Mesas_HUDS_Abiertos;
                string folder = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands";

                string[] ReadFolder = System.IO.Directory.GetFiles(folder);
                if (Casinos.Poker888.Habilitado || Casinos.Winamax.Habilitado || Casinos.PokerStars.Habilitado)
                {
                    foreach (var Mesa in Mesas_HUDS_Abiertos)
                    {
                        foreach (var file in ReadFolder)
                        {

                            bool Remover = true;
                            string Nombre_File = " ";
                            if (file.Contains("WINAMAX"))
                            {

                                Nombre_File = Path.GetFileName(file).Split("_")[1].Split("_")[0];
                            }
                            else if (file.Contains("888POKER"))
                            {

                                Nombre_File = Path.GetFileName(file).Split(" ")[1];
                            }
                            else if (file.Contains("POKERSTARS"))
                            {

                                Nombre_File = Path.GetFileName(file).Split(" ")[1] + " " + Path.GetFileName(file).Split(" ")[2];
                                if (Nombre_File.Contains("-"))
                                {
                                    Nombre_File = Nombre_File.Replace("-", "");
                                }

                            }
                            if (Nombre_File.ToUpper().Trim() == Mesa.ToUpper().Trim())
                            {
                                Remover = false;
                            }
                            if (Remover) 
                            {
                                Mesas_HUDS_Abiertos.Remove(Mesa);
                            }
                            
                        }
                    }
                    foreach (var file in ReadFolder)
                    {   
                        
                        MongoAccess _Access = new MongoAccess();
  
                            bool Condicion_HUD_Abierto = false;
                            foreach (var Mesa in Mesas_HUDS_Abiertos)
                            {
                                string Nombre_File = " ";
                                if (file.Contains("WINAMAX"))
                                {

                                    Nombre_File = Path.GetFileName(file).Split("_")[1].Split("_")[0];
                                }
                                else if (file.Contains("888POKER"))
                                {

                                    Nombre_File = Path.GetFileName(file).Split(" ")[1];
                                }
                                else if (file.Contains("POKERSTARS"))
                                {

                                    Nombre_File = Path.GetFileName(file).Split(" ")[1] + " " + Path.GetFileName(file).Split(" ")[2];
                                    if (Nombre_File.Contains("-")) 
                                    {
                                        Nombre_File = Nombre_File.Replace("-", "");
                                    }

                                }
                                if (Mesa == Nombre_File) 
                                {
                                    Condicion_HUD_Abierto = true;
                                }                                
                            }
                            if (!Condicion_HUD_Abierto) 
                            {
                                string Casino = "";
                                string Nombre_File = "";
                                if (file.Contains("WINAMAX"))
                                {
                                    Casino = "Winamax";
                                    Nombre_File = Path.GetFileName(file).Split("_")[1].Split("_")[0];
                                }
                                else if (file.Contains("888POKER"))
                                {
                                    Casino = "888Poker";
                                    Nombre_File = Path.GetFileName(file).Split(" ")[1].Split(" ")[0];
                                }
                                else if (file.Contains("POKERSTARS"))
                                {
                                    Casino = "PokerStars";
                                    Nombre_File = Path.GetFileName(file).Split(" ")[1] + " " + Path.GetFileName(file).Split(" ")[2];
                                    if (Nombre_File.Contains("-"))
                                    {
                                        Nombre_File = Nombre_File.Replace("-", "");
                                    }
                                }
                                bool MostrarHUD = true;
                                foreach (var Jugador in _Access.Get_Players_Kraken(Nombre_File.ToUpper()))
                                {
                                    foreach (var HUD in HUDS_Abiertos)
                                    {
                                        if (HUD.Trim() == Jugador.Trim())
                                        {
                                            MostrarHUD = false;
                                        }
                                    }
                                    if (MostrarHUD)
                                    {
                                        HUDS(Jugador, Nombre_File);
                                        HUDS_Abiertos.Add(Jugador);
                                    }
                                    
                                }
                                NotificationManager notificationManager = new NotificationManager();
                                notificationManager.Show(new NotificationContent
                                {
                                    Title = "KrakenEasy",
                                    Message = "Cargando HUDS para la mesa '" + Nombre_File + "'",
                                    Type = NotificationType.Information
                                });
                                Mesas_HUDS_Abiertos.Add(Nombre_File);
                

                        }
                    }
                }
                //bool Condicion_Mostrar = true;
                //if (Mesas != null)
                //{
                //    MongoAccess _Access1 = new MongoAccess();
                //    foreach (var item in Mesas)
                //    {
                //        Application.Current.Dispatcher.Invoke(() =>
                //        {
                //            var GetPlayers = _Access1.Get_Players(item.AsBsonDocument.GetElement("_id").Value.AsString.ToUpper());
                //            foreach (var hud in HUD_List)
                //            {
                //                if (item.AsBsonDocument.GetElement("_id").Value.AsString == hud)
                //                {
                //                    Condicion_Mostrar = false;
                //                }
                //            }
                //            if (Condicion_Mostrar)
                //            {
                //                foreach (string Player in GetPlayers)
                //                {
                //                    Thread _Hilo_Contenedor = new Thread(() => HUDS(Player.Split(" ")[2], item.AsBsonDocument.GetElement("_id").Value.AsString));
                //                    _Hilo_Contenedor.Start();
                //                }
                //                Casinos.Mesas.HUDS_Abiertos.Add(item.AsBsonDocument.GetElement("_id").Value.AsString);
                //            }

                //        });
                //    }
                //for (int i = 0; i > Mesas; i++)
                //{

                //    if (Casinos.Mesas.Abiertas.Count > 0)
                //    { 

                //    MongoAccess _Access = new MongoAccess();

                //        var GetPlayers = _Access.Get_Players(Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("_id").Value.AsString);
                //    List<string> Players = GetPlayers;

                //    if (Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("Activa").Value.AsBoolean && !Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("Ready").Value.AsBoolean)
                //    {
                //        foreach (string Player in Players)
                //        {
                //                Thread _Hilo_Contenedor = new Thread(() => HUDS(Player.Split(" ")[2], Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("_id").Value.AsString));
                //                _Hilo_Contenedor.Start();
                //        }
                //            BsonDocument _Data = new BsonDocument();
                //            _Data.Add(new BsonElement("_id", Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("_id").Value));
                //            _Data.Add(new BsonElement("Dimensiones", Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("Dimensiones").Value));
                //            _Data.Add(new BsonElement("Activa", Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("Activa").Value));
                //            _Data.Add(new BsonElement("Ready", true));
                //            _Data.Add(new BsonElement("Casino", Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("Casino").Value));
                //            _Data.Add(new BsonElement("_Last_Hand", Casinos.Mesas.Abiertas[i].AsBsonDocument.GetElement("_Last_Hand").Value));


                //            Casinos.Mesas.Abiertas[i] = _Data;
                //    }

                //        //for (int i = 0; i < Players.Count; i++)
                //        //{
                //        //    Thread _Hilo_Contenedor = new Thread(() => HUDS(Players[i], items[0]));
                //        //    _Hilo_Contenedor.Start();
                //        //}
                //    }
                //}
                //}
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }

        private static void HUDS(string _Id_Jugador, string _Id_Ventana)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ProgressKraken _KrakenHUD = new ProgressKraken(_Id_Jugador);
                ContenedorHUD _HUD = new ContenedorHUD(_Id_Jugador, _Id_Ventana, false);
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
