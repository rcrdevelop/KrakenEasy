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
using System.Windows.Shapes;
using KrakenEasy.KrakenBD;

namespace KrakenEasy
{
    /// <summary>
    /// Lógica de interacción para STATSPruebaHUD.xaml
    /// </summary>
    public partial class STATSPruebaHUD : Window
    {
        public STATSPruebaHUD()
        {
            InitializeComponent();
            Inicializar();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public void Inicializar()
        {

            MongoAccess _Access = new MongoAccess();
            _Access.InicializarMain();
            if (!_Access.Jugador_STAT("PRUEBA"))
            {
                _Access.Set_STAT_VPIP((double)0, "PRUEBA");
                _Access.Set_STAT_CC((double)0, "PRUEBA");
                _Access.Set_STAT_BET3((double)0, "PRUEBA");
                _Access.Set_STAT_Limp((double)0, "PRUEBA");
                _Access.Set_STAT_RB((double)0, "PRUEBA");
                _Access.Set_STAT_PFR((double)0, "PRUEBA");
            }
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MongoAccess _Access = new MongoAccess();
            if (this.VPIP.Text != "" && double.TryParse(this.VPIP.Text, out double STAT_VPIP))
            {
                _Access.Set_STAT_VPIP(double.Parse(this.VPIP.Text), "PRUEBA");
            }
            if (this.CC.Text != "" && double.TryParse(this.CC.Text, out double STAT_CC))
            {
                _Access.Set_STAT_CC(double.Parse(this.CC.Text), "PRUEBA");
            }
            if (this.BET3.Text != "" && double.TryParse(this.BET3.Text, out double STAT_BET3))
            {
                _Access.Set_STAT_BET3(double.Parse(this.BET3.Text), "PRUEBA");
            }
            if (this.Limp.Text != "" && double.TryParse(this.Limp.Text, out double STAT_Limp))
            {
                _Access.Set_STAT_Limp(double.Parse(this.Limp.Text), "PRUEBA");
            }
            if (this.RB.Text != "" && double.TryParse(this.RB.Text, out double STAT_RB))
            {
                _Access.Set_STAT_RB(double.Parse(this.RB.Text), "PRUEBA");
            }
            if (this.PFR.Text != "" && double.TryParse(this.PFR.Text, out double STAT_PFR))
            {
                _Access.Set_STAT_PFR(double.Parse(this.PFR.Text), "PRUEBA");
            }
        }
    }
}
