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
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace KrakenEasy
{

    //Ventana Principal
    public partial class MainWindow : Window
    {

        static string path = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/";

        static Servicios.Servicios _Servicios = new Servicios.Servicios();
        static List<string> Mesas_HUDS_Abiertos = new List<string>();
        static List<string> HUDS_Abiertos = new List<string>();
        static string Casino_HUD = ""; 

        public MainWindow()
        {
            InitializeComponent();
            Registros_Data.InicializarRecursos();
            KrakenBD.Settings.Inicializar();
           
        }
        private void HUD_Click(object sender, RoutedEventArgs e)
        {
            MongoAccess _Access = new MongoAccess();
            //_Access.Set_VPIP("POKER0545");
            //_Access.Set_CC("POKER0545");
            //_Access.Set_BET3("POKER0545");
            //_Access.Set_BET4("POKER0545");
            //_Access.Set_BET5("POKER0545");
            //_Access.Set_Limp("POKER0545");
            //_Access.Set_RB("POKER0545");
            //_Access.Set_PFR("POKER0545");
            //_Access.Set_FCB("POKER0545");
            //_Access.Set_FoldFCB("POKER0545");
            //_Access.Set_TCB("POKER0545");
            //_Access.Set_FoldTCB("POKER0545");
            //_Access.Set_FoldBET3("POKER0545");
            //_Access.Set_WSD("POKER0545");
            //_Access.Set_WTSD("POKER0545");

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
                    //HUDS(" POKER0545 ", "");
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
                MongoAccess _Access = new MongoAccess();
                _Access.Obtener_Jugadores();
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
                    List<string> Lista_Nueva = new List<string>(); 
                    foreach (var Mesa in Mesas_HUDS_Abiertos)
                    {
                        foreach (var file in ReadFolder)
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
                            if (Nombre_File.ToUpper().Trim() == Mesa.ToUpper().Trim())
                            {
                                Lista_Nueva.Add(Nombre_File.Trim());
                            }
                            
                        }
                    }

                    foreach (var file in ReadFolder)
                    {   
                        
                        
                            if (!HUD_Duplicado(file)) 
                            {
                                string Nombre_File = "";
                                if (file.Contains("WINAMAX"))
                                {
                                    Nombre_File = Path.GetFileName(file).Split("_")[1].Split("_")[0];
                                    Casino_HUD = "Winamax";
                                }
                                else if (file.Contains("888POKER"))
                                {
                                    Nombre_File = Path.GetFileName(file).Split(" ")[1].Split(" ")[0];
                                    Casino_HUD = "888Poker";
                                }
                                else if (file.Contains("POKERSTARS"))
                                {
                                    Nombre_File = Path.GetFileName(file).Split(" ")[1] + " " + Path.GetFileName(file).Split(" ")[2];
                                    if (Nombre_File.Contains("-"))
                                    {
                                        Nombre_File = Nombre_File.Replace("-", "");
                                    }
                                    Casino_HUD = "PokerStars";
                                }
                                bool ProcesoHUDListo = false;
                                Abrir_HUDS(file);
                                
                                if (ProcesoHUDListo)
                                {
                                    NotificationManager notificationManager = new NotificationManager();
                                    notificationManager.Show(new NotificationContent
                                    {
                                        Title = "KrakenEasy",
                                        Message = "Cargando HUDS para la mesa '" + Nombre_File + "'",
                                        Type = NotificationType.Information
                                    });
                                }
  

                                Mesas_HUDS_Abiertos.Add(Path.GetFileName(file));
                

                        }
                    }
                }
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
            void HUDS(string _Id_Jugador, string _Id_Ventana)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ContenedorHUD _HUD = new ContenedorHUD(_Id_Jugador, _Id_Ventana, false);
                    _HUD.Show();
                });
            }
            bool HUD_Duplicado(string file)
            {
                List<string> Mesas_HUDS = Mesas_HUDS_Abiertos;
                bool Condicion_HUD_Abierto = false;
                foreach (var Mesa in Mesas_HUDS)
                {
                    //string Nombre_File = " ";
                    //if (file.Contains("WINAMAX"))
                    //{

                    //    Nombre_File = Path.GetFileName(file).Split("_")[1].Split("_")[0];
                    //}
                    //else if (file.Contains("888POKER"))
                    //{

                    //    Nombre_File = Path.GetFileName(file).Split(" ")[1];
                    //}
                    //else if (file.Contains("POKERSTARS"))
                    //{

                    //    Nombre_File = Path.GetFileName(file).Split(" ")[1] + " " + Path.GetFileName(file).Split(" ")[2];
                    //    if (Nombre_File.Contains("-"))
                    //    {
                    //        Nombre_File = Nombre_File.Replace("-", "");
                    //    }

                    //}
                    if (Mesa == Path.GetFileName(file))
                    {
                        Condicion_HUD_Abierto = true;
                    }
                }
                return Condicion_HUD_Abierto;
            }

            void Abrir_HUDS(string Nombre_File)
            {

                using (StreamReader jsonStream = File.OpenText(Nombre_File))
                {
                    var json = jsonStream.ReadToEnd();
                    Mesa HUD_JSON = Newtonsoft.Json.JsonConvert.DeserializeObject<Mesa>(json);
                    MongoAccess _Access = new MongoAccess();
                    foreach (var Jugador in HUD_JSON.Jugadores)
                    {
                        _Access.STATS(Jugador);
                    }
                    foreach (var Jugador in HUD_JSON.Jugadores)
                    {
                        HUDS(Jugador, Nombre_File);

                    }
                }
  
            }
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings _Settings = new Settings();
            _Settings.Show();
        }
    }
}
