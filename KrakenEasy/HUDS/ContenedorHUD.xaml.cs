using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading;
using KrakenEasy.KrakenBD;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using MongoDB.Bson;
using System.IO;
using Newtonsoft.Json;

namespace KrakenEasy.HUDS
{
    /// <summary>
    /// Lógica de interacción para ContenedorHUD.xaml
    /// </summary>
    public partial class ContenedorHUD : Window
    {
        string _Id_Ventana;
        string _Id_Jugador;
        static bool _Replayer;
        HUDS_Kraken Static_STATS_Kraken { get; set; }
        public ContenedorHUD(string Id_Jugador, string Id_Ventana, bool Replayer)
        { 
            InitializeComponent();
            _Id_Jugador = "";
            _Id_Jugador = Id_Jugador.Trim();
            _Id_Ventana = Id_Ventana;
            _Replayer = Replayer;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_Replayer) 
            { 
                this.DragMove();
            }
        }
        private void Opacidad()
        {
            MongoAccess _Access = new MongoAccess();
            while (true)
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Opacity = Propiedades.Opacity;
                    }));
                    Thread.Sleep(TimeSpan.FromSeconds(0.2));
                }
                catch (Exception)
                {

                    
                }
            }

        }
        private void Size()
        {
            double _OriginalWidth = this.ActualWidth;
            double _OriginalHeight = this.ActualHeight;
            try
            {
                while (true)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Width = _OriginalWidth * Propiedades.Size;
                        this.Height = _OriginalHeight * Propiedades.Size;
                    }));
                    Thread.Sleep(TimeSpan.FromSeconds(0.2));
                }
            }
           
            
            catch (Exception)
            {

            }

        }

        private void HUD_Relative(bool hilo)
        {
            HUDS_Kraken STATS_Kraken = Static_STATS_Kraken;
            bool condicion = true;
            while (condicion)
            {
                if (!hilo)
                {
                    condicion = false;
                }
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Propiedades.Actualizar = false;
                    if ((Propiedades.Relative == 0))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            ProgressKraken _Kraken = new ProgressKraken(STATS_Kraken);
                            this.Contenedor.Children.Clear();
                            this.Contenedor.Children.Add(_Kraken);
                        });
                    }
                    if ((Propiedades.Relative == 1))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Progress _Progress = new Progress(STATS_Kraken);
                            this.Contenedor.Children.Clear();
                            this.Contenedor.Children.Add(_Progress);
                        });
                    }
                    if ((Propiedades.Relative == 2))
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            FullHUD _HUD = new FullHUD(STATS_Kraken, _Id_Ventana);
                            this.Contenedor.Children.Clear();
                            this.Contenedor.Children.Add(_HUD);
                        });
                    }
                });
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
        }
        private void HUD_Data()
        {




            Thread _HiloHUD = new Thread(
                () => {

                    bool Actualizar = true;
                    HUDS_Kraken Referencia = new HUDS_Kraken();

                    while (true)
                    {
                        var Time = File.GetLastWriteTime(_Id_Ventana);
                        var HoraFichero = Time.Year * 10000000000 + Time.Month * 1000000 + Time.Day * 10000 + Time.Hour * 100 + Time.Minute;
                        Time = DateTime.Now;
                        var HoraActual = Time.Year * 10000000000 + Time.Month * 1000000 + Time.Day * 10000 + Time.Hour * 100 + Time.Minute;
                        if (HoraActual == HoraFichero)
                        {
                            Actualizar = true;
                        }
                        if (Actualizar)
                        { 
                            MongoAccess _Access = new MongoAccess();

                       
                            HUDS_Kraken STATS_Kraken = new HUDS_Kraken
                            {
                                VPIP = _Access.Get_VPIP(_Id_Jugador),
                                CC = _Access.Get_CC(_Id_Jugador),
                                BET3 = _Access.Get_BET3(_Id_Jugador),
                                BET4 = _Access.Get_BET4(_Id_Jugador),
                                BET5 = _Access.Get_BET5(_Id_Jugador),
                                FBET3 = _Access.Get_FBET3(_Id_Jugador),
                                FCB = _Access.Get_FCB(_Id_Jugador),
                                TCB = _Access.Get_TCB(_Id_Jugador),
                                FFCB = _Access.Get_FFCB(_Id_Jugador),
                                FTCB = _Access.Get_FTCB(_Id_Jugador),
                                PFR = _Access.Get_PFR(_Id_Jugador),
                                Limp = _Access.Get_Limp(_Id_Jugador),
                                RB = _Access.Get_RB(_Id_Jugador),
                                WSD = _Access.Get_WSD(_Id_Jugador),
                                WTSD = _Access.Get_WTSD(_Id_Jugador),
                                Hands = _Access.Get_Hands(_Id_Jugador)
                            };
                            
                            Referencia = STATS_Kraken;
                            Static_STATS_Kraken = STATS_Kraken;
                            if (STATS_Kraken.Equals(Referencia))
                            {
                                HUD_Relative(false);
                            }

                            Referencia = STATS_Kraken;

                            Thread.Sleep(TimeSpan.FromSeconds(2));
                        }
                    }
                });
            _HiloHUD.Start();
            Thread _HiloRelative = new Thread(() => { HUD_Relative(true); });
            Dispatcher.Invoke(new Action(() =>
            {
                this.Name_Jugador.Content = _Id_Jugador;
                this.Title = _Id_Jugador;

            }));

        }
        private void HUD_Status()
        {
            try
            {
                Propiedades.SystemActive = true;
                while (true)
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        if (this.WindowState == WindowState.Minimized)
                        {
                            this.Activate();
                        }
                        this.Topmost = true;
                        this.Topmost = false;
                        
                        if (!Propiedades.SystemActive)
                        {
                            this.Close();
                        }
                        //foreach (var item in Casinos.Mesas.Abiertas)
                        //{
                        //    bool Cerrar = true;
                        //    if (_Id_Ventana == item)
                        //    {
                        //        Cerrar = false;
                        //    }
                        //    if (Cerrar)
                        //    {
                        //        this.Close();
                        //    }
                        //}

                    }));
                    Thread.Sleep(TimeSpan.FromSeconds(0.2));
                }

            }
            catch (Exception)
            {
            }

        }
        private void Hilo()
        {
            if (!_Replayer)
            {
                Thread _HiloSize = new Thread(Size);
                _HiloSize.Start();

                Thread _HiloStatus = new Thread(HUD_Status);
                _HiloStatus.Start();

                Thread _HiloOpacidad = new Thread(Opacidad);
                _HiloOpacidad.Start();
            }
 

            Thread _HiloData= new Thread(HUD_Data);
            _HiloData.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {   
            Hilo();
        }

    }
}
