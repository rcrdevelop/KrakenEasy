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
                if (valor >= 0)
                {
                    this.Positivo.Value = valor;
                    if (valor >= 0 && 20 < valor)
                    {
                        this.Positivo.Foreground = new SolidColorBrush(Colors.Yellow);
                    }
                    if (valor >= 20 && 40 < valor)
                    {
                        this.Positivo.Foreground = new SolidColorBrush(Colors.GreenYellow);
                    }
                    if (valor >= 40 && 70 < valor)
                    {
                        this.Positivo.Foreground = new SolidColorBrush(Colors.Green);
                    }
                    if (valor >= 70)
                    {
                        this.Positivo.Foreground = new SolidColorBrush(Colors.Blue);
                    }
                }
                else
                {
                    valor = valor * -1;
                    this.Negativo.Value = valor;
                    this.Negativo.Foreground = new SolidColorBrush(Colors.Red);
                    this.STAT_NAME.Foreground = new SolidColorBrush(Colors.White);
                }
                
            });
        }
    }
}
