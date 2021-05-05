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

namespace KrakenEasy.HUDS
{
    /// <summary>
    /// Lógica de interacción para ContenedorHUD.xaml
    /// </summary>
    public partial class ContenedorHUD : Window
    {
        static string _Id_Ventana;
        static string _Id_Jugador;
        public ContenedorHUD(string Id_Jugador, string Id_Ventana)
        { 
            InitializeComponent();
            _Id_Jugador = Id_Jugador;
            _Id_Ventana = Id_Ventana;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
            
        }
        private void Opacidad()
        {
                try
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Opacity = Propiedades.Opacity;
                    }));
                }
                catch (Exception)
                {

                    
                }

        }
        private void Size()
        {
            double _OriginalWidth = this.ActualWidth;
            double _OriginalHeight = this.ActualHeight;
            try
            {

                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Width = _OriginalWidth * Propiedades.Size;
                        this.Height = _OriginalHeight * Propiedades.Size;
                    }));

            }
           
            
            catch (Exception)
            {

            }

        }

        private void HUD_Relative(string _Id_Jugador)
        {
            int _HUD = new int();
            bool _Change = new bool();

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    _HUD = Propiedades.Relative;
                    _Change = Propiedades.Change;

                    if ((_HUD == 0) && (_Change))
                    {
                        Progress _Progress = new Progress(_Id_Jugador);
                        this.Contenedor.Children.Clear();
                        this.Contenedor.Children.Add(_Progress);
                        Propiedades.Change = false;
                    }
                    if ((_HUD == 1) && (_Change))
                    {
                        ProgressKraken _Kraken = new ProgressKraken(_Id_Jugador);
                        this.Contenedor.Children.Clear();
                        this.Contenedor.Children.Add(_Kraken);
                        Propiedades.Change = false;
                    }
                    if ((_HUD == 2) && (_Change))
                    {
                        FullHUD _Full = new FullHUD();
                        this.Contenedor.Children.Clear();
                        this.Contenedor.Children.Add(_Full);
                        Propiedades.Change = false;
                    }
                }));
        }
        private void HUD_Data()
        {
            BsonDocument _HUD = new BsonDocument();
            _HUD.Add(new BsonElement("_id", _Id_Jugador));
            _HUD.Add(new BsonElement("_Mesa", _Id_Ventana));
            HUDS.Lista.Add(_HUD);
                    Thread _HiloHUD = new Thread(
                        ()=> {
                            HUD_Relative(_Id_Jugador);
                            });
                    _HiloHUD.Start();
                        Dispatcher.Invoke(new Action(() =>
                        {
                            this.Name_Jugador.Content = _Id_Jugador;
                    
                        }));

        }
        private void HUD_Status()
        {
            try
            {
                HUD_Relative(_Id_Jugador);
                Opacidad();
                Size();
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (this.WindowState == WindowState.Minimized)
                    {
                        this.Activate();
                    }
                    this.Topmost = true;
                    this.Topmost = false;
                    if (Casinos.Casinos.Winamax)                    {
                        this.Close();
                    }

                }));
                Thread.Sleep(TimeSpan.FromSeconds(0.2));
            }
            catch (Exception)
            {
            }

        }
        private void Hilo()
        {
            Thread _HiloData= new Thread(HUD_Data);
            Thread _HiloStatus = new Thread(HUD_Status);
            _HiloData.Start();
            _HiloStatus.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {   
            Hilo();
        }

    }
}
