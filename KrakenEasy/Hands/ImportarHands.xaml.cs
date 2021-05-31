using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace KrakenEasy.Hands
{
    /// <summary>
    /// Lógica de interacción para ImportarHands.xaml
    /// </summary>
    public partial class ImportarHands : Window
    {
        public ImportarHands()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Casino = "PokerStars";
            if (this.RutaCarpeta.Text == "")
            {

            }
            else if (Directory.Exists(this.RutaCarpeta.Text))
            {
                string folder = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHistory/";
                foreach (var file in Directory.GetFiles(this.RutaCarpeta.Text))
                {
                    string fichero = "";
                    if (this.CheckWinamax.IsChecked.Value)
                    {
                        fichero = "WINAMAX" + Path.GetFileName(file);
                    }
                    if (this.Check888Poker.IsChecked.Value)
                    {
                        fichero = "888POKER" + Path.GetFileName(file);
                    }
                    if (this.CheckPokerStars.IsChecked.Value)
                    {
                        fichero = "POKERSTARS" + Path.GetFileName(file);
                    }
                    File.Copy(file, folder+fichero);
                }
            }
            else 
            { 
            
            }
        }

    }
}
