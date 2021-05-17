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

namespace KrakenEasy.Modals
{
    /// <summary>
    /// Lógica de interacción para Carpetas.xaml
    /// </summary>
    public partial class Carpetas : Window
    {
        public Carpetas(string Casino)
        {
            InitializeComponent();
            if (Casino == "KrakenHands")
            {
                this.Valor.Text = "Carpeta KrakenHands no se ha encontrado, inicia KrakenEasy para generarla.";
            }
            else 
            { 
               this.Valor.Text = "Carpeta " + Casino + " no se ha podido encontrar por favor actualice la ruta del mismo";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
