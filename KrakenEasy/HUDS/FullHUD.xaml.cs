using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using KrakenEasy.KrakenBD;

namespace KrakenEasy.HUDS
{
    /// <summary>
    /// Lógica de interacción para FullHUD.xaml
    /// </summary>
    public partial class FullHUD : UserControl
    {
        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public FullHUD(HUDS_Kraken STAT, string Mesa)
        {
           
                InitializeComponent();
            MongoAccess _Access = new MongoAccess();
                SeriesCollection _Collection = new SeriesCollection();
                LineSeries _Line = new LineSeries();
            List<double> valuesVPIP = new List<double>();
            List<double> valuesCC = new List<double>();
            List<double> valuesPFR = new List<double>();
            foreach (var Jugador in _Access.Get_Players_Kraken(Mesa))
            {
                valuesVPIP.Add(_Access.Get_VPIP(Jugador));
            }
            foreach (var Jugador in _Access.Get_Players_Kraken(Mesa))
            {
                valuesCC.Add(_Access.Get_CC(Jugador));
            }
            foreach (var Jugador in _Access.Get_Players_Kraken(Mesa))
            {
                valuesPFR.Add(_Access.Get_PFR(Jugador));
            }


            _Collection.Add(
                    new LineSeries
                    {
                        Title = "VPIP",
                        Values = valuesVPIP.AsChartValues(),
                    }
                    );
            _Collection.Add(
                new LineSeries
                {
                    Title = "PFR",
                    Values = valuesPFR.AsChartValues(),
                }
                );
            _Collection.Add(
                    new LineSeries
                    {
                        Title = "CC",
                        Values = valuesCC.AsChartValues(),
                    }
                    );
            this.STAT_CHARTS.Series = _Collection;
                
                
                DataContext = this;
                Controls.StatControl control1 = new Controls.StatControl(STAT.VPIP);
                Controls.StatControl control2 = new Controls.StatControl(STAT.CC);
                Controls.StatControl control3 = new Controls.StatControl(STAT.BET3);
                Controls.StatControl control4 = new Controls.StatControl(STAT.BET4);
                Controls.StatControl control5 = new Controls.StatControl(STAT.BET5);
                Controls.StatControl control6 = new Controls.StatControl(STAT.PFR);
                Controls.StatControl control7 = new Controls.StatControl(STAT.TCB);
                Controls.StatControl control8 = new Controls.StatControl(STAT.FCB);
                Controls.StatControl control9 = new Controls.StatControl(STAT.FTCB);
                Controls.StatControl control10 = new Controls.StatControl(STAT.FFCB);
                Controls.StatControl control11 = new Controls.StatControl(STAT.Limp);
                Controls.StatControl control12 = new Controls.StatControl(STAT.RB);
                Controls.StatControl control13 = new Controls.StatControl(STAT.FBET3);
                Controls.StatControl control14 = new Controls.StatControl(STAT.WSD);
                Controls.StatControl control15 = new Controls.StatControl(STAT.WTSD);
                Controls.StatControl control16 = new Controls.StatControl(STAT.Hands);

                Grid.SetRow(control1, 0);
                Grid.SetRow(control2, 0);
                Grid.SetRow(control3, 0);
                Grid.SetRow(control4, 0);
                Grid.SetColumn(control1, 0);
                Grid.SetColumn(control2, 1);
                Grid.SetColumn(control3, 2);
                Grid.SetColumn(control4, 3);

                Grid.SetRow(control5, 1);
                Grid.SetRow(control6, 1);
                Grid.SetRow(control7, 1);
                Grid.SetRow(control8, 1);
                Grid.SetColumn(control5, 0);
                Grid.SetColumn(control6, 1);
                Grid.SetColumn(control7, 2);
                Grid.SetColumn(control8, 3);

                Grid.SetRow(control9, 2);
                Grid.SetRow(control10, 2);
                Grid.SetRow(control11, 2);
                Grid.SetRow(control12, 2);
                Grid.SetColumn(control9, 0);
                Grid.SetColumn(control10, 1);
                Grid.SetColumn(control11, 2);
                Grid.SetColumn(control12, 3);

                Grid.SetRow(control13, 3);
                Grid.SetRow(control14, 3);
                Grid.SetRow(control15, 3);
                Grid.SetRow(control16, 3);
                Grid.SetColumn(control13, 0);
                Grid.SetColumn(control14, 1);
                Grid.SetColumn(control15, 2);
                Grid.SetColumn(control16, 3);


            this.Controls_STATS.Children.Add(control1);
            this.Controls_STATS.Children.Add(control2);
            this.Controls_STATS.Children.Add(control3);
            this.Controls_STATS.Children.Add(control4);
            this.Controls_STATS.Children.Add(control5);
            this.Controls_STATS.Children.Add(control6);
            this.Controls_STATS.Children.Add(control7);
            this.Controls_STATS.Children.Add(control8);
            this.Controls_STATS.Children.Add(control9);
            this.Controls_STATS.Children.Add(control10);
            this.Controls_STATS.Children.Add(control11);
            this.Controls_STATS.Children.Add(control12);
            this.Controls_STATS.Children.Add(control13);
            this.Controls_STATS.Children.Add(control14);
            this.Controls_STATS.Children.Add(control15);
            this.Controls_STATS.Children.Add(control16);
        }

    }
}
