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
        public Progress(HUDS_Kraken STATS)
        {
            InitializeComponent();
            Data(STATS);
        }
        public void Data(HUDS_Kraken STATS)
        {

                    var VPIP = new Controls.ProgressControl("VPIP", STATS.VPIP);
                    Grid.SetColumn(VPIP, 0);
                    Grid.SetRow(VPIP, 0);
                    this.Contenedor.Children.Add(VPIP);

                    var CC = new Controls.ProgressControl("CC", STATS.CC);
                    Grid.SetColumn(CC, 0);
                    Grid.SetRow(CC, 1);
                    this.Contenedor.Children.Add(CC);

                    var BET3 = new Controls.ProgressControl("3B", STATS.BET3);
                    Grid.SetColumn(BET3, 0);
                    Grid.SetRow(BET3, 2);
                    this.Contenedor.Children.Add(BET3);

                    var FCB = new Controls.ProgressControl("FCB", STATS.FCB);
                    Grid.SetColumn(FCB, 0);
                    Grid.SetRow(FCB, 3);
                    this.Contenedor.Children.Add(FCB);

                    var TCB = new Controls.ProgressControl("TCB", STATS.TCB);
                    Grid.SetColumn(TCB, 0);
                    Grid.SetRow(TCB, 4);
                    this.Contenedor.Children.Add(TCB);

                    var FFCB = new Controls.ProgressControl("FFCB", STATS.FFCB);
                    Grid.SetColumn(FFCB, 1);
                    Grid.SetRow(FFCB, 0);
                    this.Contenedor.Children.Add(FFCB);

                    var FTCB = new Controls.ProgressControl("FTCB", STATS.FTCB);
                    Grid.SetColumn(FTCB, 1);
                    Grid.SetRow(FTCB, 1);
                    this.Contenedor.Children.Add(FTCB);

                    var Limp = new Controls.ProgressControl("Limp", STATS.Limp);
                    Grid.SetColumn(Limp, 1);
                    Grid.SetRow(Limp, 2);
                    this.Contenedor.Children.Add(Limp);

                    var RB = new Controls.ProgressControl("RB", STATS.RB);
                    Grid.SetColumn(RB, 1);
                    Grid.SetRow(RB, 3);
                    this.Contenedor.Children.Add(RB);

                    var PFR = new Controls.ProgressControl("PFR", STATS.PFR);
                    Grid.SetColumn(PFR, 1);
                    Grid.SetRow(PFR, 4);
                    this.Contenedor.Children.Add(PFR);

                    this.WSD.Content = STATS.WSD + "%";

                    this.WTSD.Content = STATS.WTSD + "%";

                    this.Hands.Content = STATS.Hands;
                

            
        }
    }
}
