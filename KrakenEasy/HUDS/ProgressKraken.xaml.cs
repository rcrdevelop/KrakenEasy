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
        public ProgressKraken(HUDS_Kraken STATS)
        {
            InitializeComponent();
            Data(STATS);

        }
        void Data(HUDS_Kraken STATS)
        {
            var path = Environment.CurrentDirectory;
            MongoAccess _Access = new MongoAccess();
            Application.Current.Dispatcher.Invoke(() =>
            {
                

                        if (STATS.VPIP == 0)
                        {
                            this.VPIP_Positivo.EndAngle = 270;
                        }
                        else if (STATS.VPIP >= 10)
                        {
                            this.VPIP_Positivo.EndAngle= 270 + STATS.VPIP * 2 - 20;
                        }
                        if (STATS.VPIP >= 0 && STATS.VPIP < 10)
                        {
                            this.VPIP_Positivo.EndAngle = 270 + STATS.VPIP * 1.5;
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/SeaHorse.png"));
                        }
                        if (STATS.VPIP >= 10 && STATS.VPIP < 20)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Erizo.png"));
                        }
                        if (STATS.VPIP >= 20 && STATS.VPIP < 30)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Raspa.png"));
                        }
                        if (STATS.VPIP >= 30 && STATS.VPIP < 40)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Whale.png"));
                        }
                        if (STATS.VPIP >= 40 && STATS.VPIP < 50)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Erizo.png"));
                        }
                        if (STATS.VPIP >= 50 && STATS.VPIP < 60)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Worm.png"));
                        }
                        if (STATS.VPIP >= 60 && STATS.VPIP < 75)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/Shark.png"));
                        }
                        if (STATS.VPIP >= 75)
                        {
                            this.Badge.ImageSource = new BitmapImage(new Uri(path + "/HUDS/Badges/pulpo.png"));
                        }


                        if (STATS.CC == 0)
                        {
                            this.Fondo_VPIP_Negativo.EndAngle = 270;

                        }
                        else
                        {
                            this.Fondo_VPIP_Negativo.EndAngle = 270 + 20 - STATS.CC * 2;

                        }


                        this.BET3.Content = STATS.BET3;

                        this.BET4.Content = STATS.BET4;

                        this.BET5.Content = STATS.BET5;
                    
            });
        }
    }
}