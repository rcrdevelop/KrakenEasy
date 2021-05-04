﻿using System;
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
using System.Diagnostics;

namespace KrakenEasy.HUDS
{
    /// <summary>
    /// Lógica de interacción para HUDControler.xaml
    /// </summary>
    public partial class HUDControler : Window
    {
        public HUDControler()
        {
            InitializeComponent();
            EstablecerMongo();
            Hilo();
        }
        private void EstablecerMongo()
        {
            MongoAccess _Access = new MongoAccess();
            _Access.Inicializar();
        }

        private void Focus_Window()
        {
            try
            {
                while (true)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                            this.Topmost = false;
                            this.Topmost = true;
                        if (!Propiedades.SystemActive)
                        {
                            this.Close();
                        }
                    });
                    Thread.Sleep(TimeSpan.FromSeconds(0.1));
                }
            }

            catch (Exception)
            {
            }
        }


        private void Opacidad_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Propiedades.Opacity = e.NewValue;
        }

        private void Size_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Propiedades.Size = e.NewValue;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        public void Hilo()
        {
            Thread _Hilo = new Thread(Focus_Window);
            _Hilo.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Propiedades.Change = true;
            if (Propiedades.Relative == 0)
            {
                Propiedades.Relative = 1;
            }
            if (Propiedades.Relative == 1)
            {
                Propiedades.Relative = 2;
            }
            if (Propiedades.Relative == 2)
            {
                Propiedades.Relative = 0;
            }
        }
    }
}