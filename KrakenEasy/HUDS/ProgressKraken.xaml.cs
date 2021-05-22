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
namespace KrakenEasy.HUDS
{
    /// <summary>
    /// Lógica de interacción para ProgressKraken.xaml
    /// </summary>
    public partial class ProgressKraken : UserControl
    {
        static List<double> _STATS { get; set; }
        public ProgressKraken(List<double> STATS)
        {
            _STATS = STATS;
            InitializeComponent();
            Data();

        }
        public void Data()
        {
            var path = Environment.CurrentDirectory;
            MongoAccess _Access = new MongoAccess();
            Application.Current.Dispatcher.Invoke(() =>
            {
                for (var i = 0; i < _STATS.Count; i++ )
                {
                    if (i == 0) // VPIP 
                    {
                        if (_STATS[i] == 0)
                        {
                            this.VPIP_Positivo.EndAngle = 270;
                        }
                        else
                        {
                            this.VPIP_Positivo.EndAngle= 270 + _STATS[i] * 2 - 20;
                        }
                        if (_STATS[i] >= 0 && _STATS[i] < 10)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/SeaHorse.png"));
                        }
                        if (_STATS[i] >= 10 && _STATS[i] < 20)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Erizo.png"));
                        }
                        if (_STATS[i] >= 20 && _STATS[i] < 30)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Raspa.png"));
                        }
                        if (_STATS[i] >= 30 && _STATS[i] < 40)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Whale.png"));
                        }
                        if (_STATS[i] >= 40 && _STATS[i] < 50)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Erizo.png"));
                        }
                        if (_STATS[i] >= 50 && _STATS[i] < 60)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Worm.png"));
                        }
                        if (_STATS[i] >= 60 && _STATS[i] < 75)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Shark.png"));
                        }
                        if (_STATS[i] >= 75)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/pulpo.png"));
                        }



                    }
                    if (i == 1) // CC
                    {
                        if (_STATS[i] == 0)
                        {
                            this.Fondo_VPIP_Negativo.EndAngle = 270;

                        }
                        else
                        {
                            this.Fondo_VPIP_Negativo.EndAngle = 270 + 20 - _STATS[i] * 2;

                        }
                    }
                    if (i == 2) // BET3
                    {
                        this.BET3.Content = _STATS[i];
                    }
                    if (i == 3) // BET4
                    {
                        this.BET4.Content = _STATS[i];
                    }
                }
            });
        }
    }
}