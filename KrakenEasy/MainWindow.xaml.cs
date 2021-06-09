using System;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using KrakenEasy.KrakenBD;
using KrakenEasy.Hands;
using KrakenEasy.HUDS;
using Notification.Wpf;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using MongoDB.Bson;
using System.Windows.Forms;
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows.Media.Imaging;
using Application = System.Windows.Application;
using NuGet.Protocol.Plugins;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Drawing;
namespace KrakenEasy
{

    //Ventana Principal
    public partial class MainWindow : Window
    {

        static string path = @"c:/Users/" + (WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/";
        NotifyIcon icon = new NotifyIcon();
        static Servicios.Servicios _Servicios = new Servicios.Servicios();
        static List<string> Mesas_HUDS_Abiertos = new List<string>();
        static List<string> HUDS_Abiertos = new List<string>();
        static string Casino_HUD = ""; 

        public MainWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri(Environment.CurrentDirectory + "/HUDS/Badges/pulpo.ico"));
            Registros_Data.InicializarRecursos();
            KrakenBD.Settings.Inicializar();
            IconProgram();
        }
        private void HUD_Click(object sender, RoutedEventArgs e)
        {
            MongoAccess _Access = new MongoAccess(); ;


            try
            {

                _Access.InicializarMain();

   

            }
            catch (Exception ex)
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
        private void IconProgram()
        {            
            icon.Icon = new Icon(Environment.CurrentDirectory + "/HUDS/Badges/pulpo.ico");
            icon.Visible = true;
            icon.ContextMenuStrip = new ContextMenuStrip();
            icon.ContextMenuStrip.Items.Add("Abrir", null, Abrir);
            icon.ContextMenuStrip.Items.Add("Iniciar Kraken", null, Iniciar_HUDS);
            icon.ContextMenuStrip.Items.Add("Iniciar Replayer", null, Iniciar_Replayer);
            icon.ContextMenuStrip.Items.Add("Salir", null, Salir);
        }

        void Salir (object sender, EventArgs e)
        {
            this.Close();
        }
        private void Abrir(object sender, EventArgs e)
        {
            WindowState = WindowState.Normal;
            this.Topmost = false;
            this.Topmost = true;
        }
        private void Iniciar_HUDS(object sender, EventArgs e)
        {


            if (!SystemKraken.HUDS)
            {
                Iniciar_Kraken();
                icon.ContextMenuStrip = new ContextMenuStrip();
                icon.ContextMenuStrip.Items.Add("Abrir", null, Abrir);
                icon.ContextMenuStrip.Items.Add("Detener Kraken", null, Iniciar_HUDS);
                icon.ContextMenuStrip.Items.Add("Iniciar Replayer", null, Iniciar_Replayer);
                icon.ContextMenuStrip.Items.Add("Salir", null, Salir);
            }
            else
            {
                Detener_Kraken();
                icon.ContextMenuStrip = new ContextMenuStrip();
                icon.ContextMenuStrip.Items.Add("Abrir", null, Abrir);
                icon.ContextMenuStrip.Items.Add("Iniciar Kraken", null, Iniciar_HUDS);
                icon.ContextMenuStrip.Items.Add("Iniciar Replayer", null, Iniciar_Replayer);
                icon.ContextMenuStrip.Items.Add("Salir", null, Salir);
            }


        }
        void Iniciar_Replayer(object sender, EventArgs e)
        {

                Replayer.MainWindow _Replayer = new Replayer.MainWindow();
                _Replayer.Show();

        }

        private void Iniciar_Kraken()
        {
            if (!SystemKraken.HUDS)
            {
                Thread _Hilo_Monitor = new Thread(Monitor_HUDS);
                Thread _Hilo_Servicio = new Thread(Iniciar_Servicio);
                Thread _Hilo_HUDS = new Thread(Iniciar_HUD);
                this.HUD.Content = "Detener Kraken";
                _Hilo_Servicio.Start();
                _Hilo_Monitor.Start();
                _Hilo_HUDS.Start();
                SystemKraken.HUDS = true;
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "KrakenEasy",
                    Message = "Kraken iniciado... HUDS listos para mostrarse.",
                    Type = NotificationType.Notification
                });

            }
            
        }
        private void Detener_Kraken()
        {
            if (SystemKraken.HUDS)
            { 
                this.HUD.Content = "Iniciar Kraken";
                Mesas_HUDS_Abiertos = new List<string>();
                HUDS_Abiertos = new List<string>();
                SystemKraken.HUDS = false;
                var notificationManager = new NotificationManager();

                notificationManager.Show(new NotificationContent
                {
                    Title = "KrakenEasy",
                    Message = "Kraken detenido.",
                    Type = NotificationType.Notification
                });
 
            }
        }
        public static void Detener_Sistema() 
        {

        }
        //Iniciar RePlayer
        

        private void Window_Closed(object sender, EventArgs e)
        {
            Detener_Servicio();
            Process.GetCurrentProcess().Kill();
        }
        
        //Llevar ficheros a KrakenHands manualmente
        private void Importar_Hands(object sender, RoutedEventArgs e)
        {
            ImportarHands Importar = new ImportarHands();
            Importar.Show();
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
        }
        private static void Iniciar_HUD()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                HUDControler _Controler = new HUDControler();
                _Controler.Show();
            });
            MongoAccess _Access = new MongoAccess();
                //Thread hilo = new Thread(() => { _Access.Obtener_Jugadores(); });
                //hilo.Start();

                //Thread _Hilo_Monito_HUDS = new Thread(Monitor_HUDS);
                //_Hilo_Monito_HUDS.Start();

        }
        //Procedimiento para mostras HUDS
        private static void Monitor_HUDS()
        {

            
            while (true)
            {
                
                Casinos.Mesas.Abiertas = Mesas_HUDS_Abiertos;
                string folder = @"c:/Users/" + (WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands";

                string[] ReadFolder = Directory.GetFiles(folder);
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
                    string Casino = "";
                    string Hand = "";
                    List<string> Jugadores = new List<string>();
                    string Nombre = "";
                    foreach (var file in ReadFolder)
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

                        
                        

                        using (StreamReader jsonStream = File.OpenText(file))
                        {
                            var json = jsonStream.ReadToEnd();
                            jsonStream.Close();
                            Mesa HUD_JSON = Newtonsoft.Json.JsonConvert.DeserializeObject<Mesa>(json);
                            if (HUD_JSON.Read == true)
                            {
                                foreach (var Jugador in HUD_JSON.Jugadores)
                                {
                                    HUDS(Jugador, Nombre_File);
                                }
                                Casino = HUD_JSON.Casino;
                                Hand = HUD_JSON.Hand;
                                Jugadores = HUD_JSON.Jugadores;
                                Nombre = HUD_JSON.Nombre;
                                Hands.Mesa MesaFile = new Hands.Mesa
                                {
                                    Casino = Casino,
                                    Hand = Hand,
                                    Jugadores = Jugadores,
                                    Nombre = Nombre,
                                    Read = false
                                };
                                string json2 = JsonConvert.SerializeObject(MesaFile);
                                System.IO.File.WriteAllText(file, json2);
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
                File.Copy(Nombre_File, Environment.CurrentDirectory + "/swap/mainwindow/");
                using (StreamReader jsonStream = File.OpenText(Environment.CurrentDirectory + "/swap/mainwindow/"+Path.GetFileName(Nombre_File)))
                {
                    var json = jsonStream.ReadToEnd();
                    jsonStream.Close();
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
                File.Delete(Environment.CurrentDirectory + "/swap/mainwindow/" + Path.GetFileName(Nombre_File));
            }
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Settings _Settings = new Settings();
            _Settings.Show();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                var notificationManager = new NotificationManager();
                notificationManager.Show(new NotificationContent
                {
                    Title = "KrakenEasy",
                    Message = "Aplicacion oculta en barra de tareas",
                    Type = NotificationType.Notification
                });
            }
            else
            {
                this.ShowInTaskbar = true;
            }
        }
    }
}
