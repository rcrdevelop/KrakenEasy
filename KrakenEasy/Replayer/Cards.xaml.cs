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
            MongoAccess _Access = new MongoAccess();
            Thread _Hilo = new Thread(() =>
            {
                while (true) { 
                    if (_Access.Get_Hole_Cards())
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.Card_Right.Source = new BitmapImage(new Uri(@"C:\Users\contr\source\repos\KrakenEasy\Replayer\Cartas\Baraja Dorso1.png"));
                            this.Card_Left.Source = new BitmapImage(new Uri(@"C:\Users\contr\source\repos\KrakenEasy\Replayer\Cartas\Baraja Dorso1.png"));
                        });
                    }
                    else
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            this.Card_Right.Source = new BitmapImage(new Uri(@"C:\Users\contr\source\repos\KrakenEasy\Replayer\Cartas\Papel 10c.png"));
                            this.Card_Left.Source = new BitmapImage(new Uri(@"C:\Users\contr\source\repos\KrakenEasy\Replayer\Cartas\Papel 2h.png"));
                        });
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(0.2));
                }
            });
            _Hilo.Start();
        }
    }
}
