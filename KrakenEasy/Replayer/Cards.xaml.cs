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
            var CardDorso = new BitmapImage(new Uri(path + "/Replayer/Cartas/Baraja Dorso2.png"));
            var Card_Right = new BitmapImage(new Uri(path + "/Replayer/Cartas/TC.png"));
            var Card_Left = new BitmapImage(new Uri(path + "/Replayer/Cartas/2H.png"));
            Thread thread = new Thread(() => { 
                while (true) {
            
                        if (!Registros_Data.Hole_Cards)
                        {

                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                    this.Card_Left.Source = CardDorso;
                                    this.Card_Right.Source = CardDorso;
                            });
                        }
                        else
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                this.Card_Right.Source = Card_Right;
                                this.Card_Left.Source = Card_Left;
                            });
                        }
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            });
            thread.Start();
        }
    }
}
