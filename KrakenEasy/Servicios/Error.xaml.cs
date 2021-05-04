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
using System.Windows.Shapes;

namespace KrakenEasy.Servicios
{
    /// <summary>
    /// Lógica de interacción para Error.xaml
    /// </summary>
    public partial class Error : Window
    {
        public Error(string error)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                InitializeComponent();
            
                Label _Label = new Label();
                _Label.Content = error;
                _Label.HorizontalAlignment = HorizontalAlignment.Center;
                _Label.VerticalAlignment = VerticalAlignment.Center;
                Content.Children.Add(_Label);
            }));
        }
        public void Iniciar(string error)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                Label _Label = new Label();
                _Label.Content = error;
                _Label.HorizontalAlignment = HorizontalAlignment.Center;
                _Label.VerticalAlignment = VerticalAlignment.Center;
                Content.Children.Add(_Label);
            }));

        }
    }
}
