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
        string _Jugador = "";
        public ProgressKraken(string Jugador)
        {
            _Jugador = "UBET1967";
            InitializeComponent();
            Thread _Data = new Thread(Data);
            _Data.Start();

        }
        public void Data()
        {
            MongoAccess _Access = new MongoAccess();
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_Access.Get_VPIP("UBET1967") == 0)
                {
                    this.VPIP_Positivo.EndAngle = 270;
                }
                else
                {
                    this.VPIP_Positivo.EndAngle = 270 + _Access.Get_VPIP(_Jugador) * 2 - 20;
                }
                if (_Access.Get_CC("UBET1967") == 0)
                {
                    this.Fondo_VPIP_Negativo.EndAngle = 270;

                }
                else
                {
                    this.Fondo_VPIP_Negativo.EndAngle = _Access.Get_CC("UBET1967") * 2 - 270 - 20;

                }

                this.BET3.Content = _Access.Get_VPIP("UBET1967");
                this.BET4.Content = _Access.Get_CC("UBET1967");
            }
            );
        }
    }
}