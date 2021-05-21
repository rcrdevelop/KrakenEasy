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
            MongoAccess _Access = new MongoAccess();
            Application.Current.Dispatcher.Invoke(() =>
            {
                for (var i = 0; i < _STATS.Count; i++ )
                {
                    if (i == 0) // VPIP 
                    {
                        if (_STATS[i] == 0)
                        {
                            this.VPIP_Positivo.StartAngle = 270;
                        }
                        else
                        {
                            this.VPIP_Positivo.StartAngle= 270 + _STATS[i] * 2 - 22;
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
                            this.Fondo_VPIP_Negativo.EndAngle = _STATS[i] * 2 - 270 - 10;

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