using System;
using System.IO;
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
            var CurrentDirectory = Environment.CurrentDirectory;
            var path = CurrentDirectory;
                    if (Registros_Data.Hole_Cards)
                    {

                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            try
                            {
   
                                this.Card_Left.Source = new BitmapImage(new Uri(path+ "/Replayer/Cartas/Baraja Dorso2.png"));
                                this.Card_Right.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/Baraja Dorso2.png"));
                            }
                            catch {
                                throw;
                            }
                        });
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.Card_Right.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/Papel 10c.png", UriKind.Relative));
                            this.Card_Left.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/Papel 2h.png", UriKind.Relative));
                        });
                    }
          
        }
    }
}
