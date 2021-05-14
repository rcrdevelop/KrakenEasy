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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using KrakenEasy.KrakenBD;

namespace KrakenEasy.Replayer
{
    /// <summary>
    /// Lógica de interacción para Cards.xaml
    /// </summary>
    public partial class Cards : UserControl
    {
        public Cards()
        {
            InitializeComponent();
            HoleCards();
        }
        private void HoleCards() 
        {

                    if (Registros_Data.Hole_Cards)
                    {

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            try
                            {

                                Uri _uri = new Uri("Baraja Dorso1.png", UriKind.Relative);
                                this.Card_Left.Source = new BitmapImage(_uri);
                                _uri = new Uri("Baraja Dorso1.png", UriKind.Relative);
                                this.Card_Right.Source = new BitmapImage(_uri);
                            }
                            catch {
                                Window _Window = new Window();
                                _Window.Show();
                            }
                        });
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.Card_Right.Source = new BitmapImage(new Uri("/Cartas/Papel 10c.png", UriKind.Relative));
                            this.Card_Left.Source = new BitmapImage(new Uri("/Cartas/Papel 2h.png", UriKind.Relative));
                        });
                    }
          
        }
    }
}
