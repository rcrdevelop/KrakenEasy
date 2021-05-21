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

namespace KrakenEasy.HUDS.Controls
{
    /// <summary>
    /// Lógica de interacción para ProgressControl.xaml
    /// </summary>
    public partial class ProgressControl : UserControl
    {
        public ProgressControl(string Name, double valor)
        {
            InitializeComponent();
            Application.Current.Dispatcher.Invoke(() => {
                this.STAT_NAME.Text = Name;
                this.Positivo.Value = valor;
            });
        }
    }
}
