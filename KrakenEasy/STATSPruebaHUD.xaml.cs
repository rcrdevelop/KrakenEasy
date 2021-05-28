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
                _Access.Set_STAT_BET3((double)0, "PRUEBA");
                _Access.Set_STAT_PFR((double)0, "PRUEBA");
                _Access.Set_STAT_BET4((double)0, "PRUEBA");
                _Access.Set_STAT_BET5((double)0, "PRUEBA");
                _Access.Set_STAT_FCB((double)0, "PRUEBA");
                _Access.Set_STAT_TCB((double)0, "PRUEBA");
                _Access.Set_STAT_FoldFCB((double)0, "PRUEBA");
                _Access.Set_STAT_FoldTCB((double)0, "PRUEBA");
                _Access.Set_STAT_WSD((double)0, "PRUEBA");
                _Access.Set_STAT_WTSD((double)0, "PRUEBA");
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
            if (this.BET4.Text != "" && double.TryParse(this.VPIP.Text, out double STAT_BET4))
            {
                _Access.Set_STAT_BET4(double.Parse(this.BET4.Text), "PRUEBA");
            }
            if (this.BET5.Text != "" && double.TryParse(this.CC.Text, out double STAT_BET5))
            {
                _Access.Set_STAT_BET5(double.Parse(this.BET5.Text), "PRUEBA");
            }
            if (this.FCB.Text != "" && double.TryParse(this.FCB.Text, out double STAT_FCB))
            {
                _Access.Set_STAT_FCB( double.Parse(this.FCB.Text), "PRUEBA");
            }
            if (this.TCB.Text != "" && double.TryParse(this.TCB.Text, out double STAT_TCB))
            {
                _Access.Set_STAT_TCB(double.Parse(this.TCB.Text), "PRUEBA");
            }
            if (this.FFCB.Text != "" && double.TryParse(this.FFCB.Text, out double STAT_FFCB))
            {
                _Access.Set_STAT_FoldFCB(double.Parse(this.FFCB.Text), "PRUEBA");
            }
            if (this.FTCB.Text != "" && double.TryParse(this.FTCB.Text, out double STAT_FTCB))
            {
                _Access.Set_STAT_FoldTCB(double.Parse(this.FTCB.Text), "PRUEBA");
            }
            if (this.FBET3.Text != "" && double.TryParse(this.FBET3.Text, out double STAT_FBET3))
            {
                _Access.Set_STAT_FoldBET3(double.Parse(this.FBET3.Text), "PRUEBA");
            }
            if (this.WSD.Text != "" && double.TryParse(this.WSD.Text, out double STAT_WSD))
            {
                _Access.Set_STAT_WSD(double.Parse(this.WSD.Text), "PRUEBA");
            }
            if (this.WTSD.Text != "" && double.TryParse(this.WTSD.Text, out double STAT_WTSD))
            {
                _Access.Set_STAT_WTSD(double.Parse(this.WTSD.Text), "PRUEBA");
            }
        }
    }
}
