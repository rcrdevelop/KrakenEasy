using System.IO;
using System.Windows;
using KrakenEasy.Modals;
using KrakenEasy.KrakenBD;

namespace KrakenEasy
{
    /// <summary>
    /// Lógica de interacción para Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
            Rutas();
        }
        void Rutas() 
        {
            MongoAccess _Access = new MongoAccess();
            Application.Current.Dispatcher.Invoke(() =>
            {
                this.Casino_Winamax.Text = _Access.Get_Ruta_Casino("WINAMAX");
                this.Casino_888Poker.Text = _Access.Get_Ruta_Casino("888POKER");
                this.Casino_PokerStars.Text = _Access.Get_Ruta_Casino("POKERSTARS");
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MongoAccess _Access = new MongoAccess();
            _Access.Set_Ruta_Casino(this.Casino_Winamax.Text, "WINAMAX");
            _Access.Set_Ruta_Casino(this.Casino_888Poker.Text, "888POKER");
            _Access.Set_Ruta_Casino(this.Casino_PokerStars.Text, "POKERSTARS");
            bool[] Casinos = new bool[3];
            Casinos[0] = this.Winamax_Status.IsChecked.Value;
            Casinos[1] = this.Poker888_Status.IsChecked.Value;
            Casinos[2] = this.PokerStars_Status.IsChecked.Value;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Carpetas();
            void Carpetas()
            {
                MongoAccess _Access = new MongoAccess();
                if (!Directory.Exists(_Access.Get_Ruta_Casino("WINAMAX")))
                {
                    Carpetas _Ventana = new Carpetas("WINAMAX");
                    _Ventana.ShowDialog();
                }
                if (!Directory.Exists(_Access.Get_Ruta_Casino("888POKER")))
                {
                    Carpetas _Ventana = new Carpetas("888POKER");
                    _Ventana.ShowDialog();
                }
                if (!Directory.Exists(_Access.Get_Ruta_Casino("POKERSTARS")))
                {
                    Carpetas _Ventana = new Carpetas("POKERSTARS");
                    _Ventana.ShowDialog();
                }
                if (!Directory.Exists("c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands"))
                {
                    Carpetas _Ventana = new Carpetas("KrakenHands");
                    _Ventana.ShowDialog();
                }
            }
        }

    }
}
