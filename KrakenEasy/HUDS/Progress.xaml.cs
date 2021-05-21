using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KrakenEasy.HUDS
{
    /// <summary>
    /// Lógica de interacción para Progress.xaml
    /// </summary>
    public partial class Progress : UserControl
    {
        static List<double> _STATS { get; set; }
        public Progress(string Jugador, List<double> STATS, double Hands)
        {
            _STATS = STATS;
            InitializeComponent();
            Data(Hands);
        }
        public void Data(double Hands)
        {
            this.Hands.Content = Hands;
            for(var i = 0; i < _STATS.Count; i++)
            {
                if (i == 0)
                {
                    var VPIP = new Controls.ProgressControl("VPIP", _STATS[i]);
                    Grid.SetColumn(VPIP, 0);
                    Grid.SetRow(VPIP, 1);
                    this.Contenedor.Children.Add(VPIP);
                }
                if (i == 1)
                {
                    var CC = new Controls.ProgressControl("CC", _STATS[i]);
                    Grid.SetColumn(CC, 0);
                    Grid.SetRow(CC, 2);
                    this.Contenedor.Children.Add(CC);
                }
                if (i == 2)
                {
                    var BET3 = new Controls.ProgressControl("3B", _STATS[i]);
                    Grid.SetColumn(BET3, 0);
                    Grid.SetRow(BET3, 3);
                    this.Contenedor.Children.Add(BET3);
                }
                if (i == 3)
                {
                    var PFR = new Controls.ProgressControl("PFR", _STATS[i]);
                    Grid.SetColumn(PFR, 1);
                    Grid.SetRow(PFR, 0);
                    this.Contenedor.Children.Add(PFR);
                }
                if (i == 4)
                {
                    var RBET = new Controls.ProgressControl("RB", _STATS[i]);
                    Grid.SetColumn(RBET, 1);
                    Grid.SetRow(RBET, 1);
                    this.Contenedor.Children.Add(RBET);
                }
                if (i == 5)
                {
                    var Limp = new Controls.ProgressControl("Limp", _STATS[i]);
                    Grid.SetColumn(Limp, 1);
                    Grid.SetRow(Limp, 2);
                    this.Contenedor.Children.Add(Limp);
                }
            }
        }
    }
}
