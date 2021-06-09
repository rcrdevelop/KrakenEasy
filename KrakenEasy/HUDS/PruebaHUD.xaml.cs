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
using System.IO;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Diagnostics;

namespace KrakenEasy.HUDS
{
    /// <summary>
    /// Lógica de interacción para ContenedorHUD.xaml
    /// </summary>
    public partial class PruebaHUD : Window
    {
        string _Id_Ventana;
        string _Id_Jugador;
        bool _Replayer;
        bool _Click = false;
        List<double> _STATS = new List<double>();
        RECT dimensionesHUD = new RECT();
        int[] Posiciones { get; set; }
        public PruebaHUD(string Id_Jugador, string Id_Ventana, bool Replayer)
        {
            InitializeComponent();
            _Id_Jugador = Id_Jugador;
            _Id_Ventana = Id_Ventana;
            _Replayer = Replayer;



        }
        private void Posicion_HUD(string Posicion) 
        {
            
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_Replayer)
            {
                _Click = true;
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
                
                Win32 win32 = new Win32();
                foreach (var ventana in win32.FindWindowsWithText(_Id_Ventana))
                {
                    foreach (var HUD in win32.FindWindowsWithText(_Id_Jugador))
                    {
                        var Dimensiones = new RECT();
                        var DimensionesHUD = new RECT();
                        Win32.GetWindowRect(ventana, out Dimensiones);
                        Win32.GetWindowRect(HUD, out DimensionesHUD);
                        dimensionesHUD.Left = DimensionesHUD.Left - Dimensiones.Left;
                        dimensionesHUD.Top = DimensionesHUD.Top - Dimensiones.Top;
                    }
                }
            }
        }
        private void Opacidad()
        {
            MongoAccess _Access = new MongoAccess();
            while (true)
            {
                try
                {
                    this.Dispatcher.Invoke(new Action(() =>
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
                    this.Dispatcher.Invoke(new Action(() =>
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


        
        private void HUD_Data()
        {
            this.Dispatcher.Invoke(() =>
            {
                var path = Environment.CurrentDirectory + "/HUDS-Kraken/" + this.Window.Title + "-Mesa-" + _Id_Ventana + ".json";
            });
            if (!Directory.Exists(Environment.CurrentDirectory + "/HUDS-Kraken"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "/HUDS-Kraken");
            }
            if (!Directory.Exists(Environment.CurrentDirectory + "/"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "/HUDS-Kraken");
            }


            Thread _HiloHUD = new Thread(
                () => {
                    
                    Propiedades.Actualizar = true;
                    HUDS_Kraken Referencia = new HUDS_Kraken();
                    while (true)
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
                        var NHands = _Access.Get_Hands(_Id_Jugador);
                        if (STATS_Kraken.Equals(Referencia))
                        {

                                
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

                            }

                        Referencia = STATS_Kraken;
                        
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                    }
                });
            _HiloHUD.Start();
            this.Dispatcher.Invoke(new Action(() =>
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
                bool condicion = true;
                bool PrimerPlano = false;
                while (condicion)
                {
                    while (_Click)
                    {
                        Thread.Sleep(TimeSpan.FromMilliseconds(500));
                    }
                    this.Dispatcher.Invoke(new Action(() =>
                    {
                        



                        Win32 win32 = new Win32();
                        foreach (var ventana in win32.FindWindowsWithText(_Id_Ventana))
                        {
                            foreach (var HUD in win32.FindWindowsWithText(this.Window.Title))
                            {


                                //Remove WS_POPUP style and add WS_CHILD style 

                                //Win32.SetWindowLongA(ventana, -16, 0x40000000L);
                                //Win32.SetWindowLongPtrA(HUD, -16, 0x40000000L);
                                //IntPtr NewVentana = Win32.SetParent(HUD, ventana);
                                var Ventana = new RECT();
                                var Dimensiones = new RECT();
                                var DimensionesHUD = new RECT();
                                Win32.GetWindowRect(ventana, out Dimensiones);
                                Win32.GetWindowRect(HUD, out DimensionesHUD);
                                Win32.GetWindowRect(ventana, out Ventana);
                                this.Dispatcher.Invoke(() =>
                                {
                                    if (Ventana.Left < 0)
                                    {
                                        this.Hide();
                                    }
                                    else 
                                    {
                                        try
                                        {
                                            this.Show();
                                        }
                                        catch 
                                        { 
   
                                        }


                                    }
                                    
                                    if (Win32.GetForegroundWindow() == ventana && !PrimerPlano)
                                    {
                                        Win32.MoveWindow(HUD, Ventana.Left + dimensionesHUD.Left, Ventana.Top + dimensionesHUD.Top, (int)this.Width, (int)this.Height, false);
                                        PrimerPlano = true;
                                        this.Topmost = false;
                                        this.Topmost = true;



                                    }
                                    else 
                                    {
                                        PrimerPlano = false;
                                        this.Topmost = false;
                                    }
                                    
                                });

                            }
                        }

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
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
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


            Thread _HiloData = new Thread(HUD_Data);
            _HiloData.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Hilo();
            //Window window = new Window();
            //Grid grid = new Grid();
            //grid.RowDefinitions.Add(new RowDefinition());
            //grid.RowDefinitions.Add(new RowDefinition());
            //grid.ColumnDefinitions.Add(new ColumnDefinition());
            //grid.ColumnDefinitions.Add(new ColumnDefinition());

            //Label leftventana = new Label();
            //Label topventana = new Label();
            //Label lefthud = new Label();
            //Label tophud = new Label();

            //Thread windowhilo = new Thread(() => {
            //    while (true)
            //    {


            //        Win32 win32 = new Win32();
            //        foreach (var ventana in win32.FindWindowsWithText(_Id_Ventana))
            //        {
            //            foreach (var HUD in win32.FindWindowsWithText(_Id_Jugador))
            //            {

            //                var Dimensiones = new RECT();
            //                var DimensionesHUD = new RECT();
            //                Win32.GetWindowRect(ventana, out Dimensiones);
            //                Win32.GetWindowRect(HUD, out DimensionesHUD);
                            
            //                Application.Current.Dispatcher.Invoke(() => {
            //                    leftventana.Content = Dimensiones.Left + "Ventana " + DimensionesHUD.Left + "HUD " + "Diferencia: " + (DimensionesHUD.Left - Dimensiones.Left);
            //                    topventana.Content = Dimensiones.Top + "Ventana " + DimensionesHUD.Top + "HUD " + "Diferencia: " + (DimensionesHUD.Top - Dimensiones.Top);
                                
            //                });
                            

            //            }
            //        }
            //    }
            //    Thread.Sleep(TimeSpan.FromMilliseconds(500));
            //});
            //windowhilo.Start();
            //Grid.SetColumn(leftventana, 0);
            //Grid.SetRow(leftventana, 0);

            //Grid.SetColumn(topventana, 0);
            //Grid.SetRow(topventana, 1);

            //Grid.SetColumn(lefthud, 1);
            //Grid.SetRow(lefthud, 0);

            //Grid.SetColumn(tophud, 1);
            //Grid.SetRow(tophud, 1);

            //grid.Children.Add(leftventana);
            //grid.Children.Add(topventana);
            //grid.Children.Add(lefthud);
            //grid.Children.Add(tophud);

            //window.Content = grid;
            //window.Show();
        }

        private void Window_MouseLeave(object sender, MouseEventArgs e)
        {
           
            //Win32 win32 = new Win32();
            //RECT Dimensiones = new RECT();
            //foreach (var HUD in win32.FindWindowsWithText(_Id_Jugador))
            //{
            //    Win32.GetWindowRect(HUD, out Dimensiones);
            //}
            //int Left = Dimensiones.Left;
            //int Top = Dimensiones.Top;
            //foreach (var ventana in win32.FindWindowsWithText(_Id_Ventana))
            //{
            //    //Win32.GetWindowRect(ventana, out Dimensiones);
            //    //dimensionesHUD.Left = Left - Dimensiones.Left;
            //    //dimensionesHUD.Left = Top - Dimensiones.Top;


            //}
            _Click = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Propiedades.SystemActive = false;
        }
    }
}
