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
                this.Casino_Winamax.Text = Casinos.Winamax._Ruta;
                this.Casino_888Poker.Text = Casinos.Poker888._Ruta;
                this.Casino_PokerStars.Text = Casinos.PokerStars._Ruta;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MongoAccess _Access = new MongoAccess();
            KrakenEasy.Casinos.Winamax._Ruta = this.Casino_Winamax.Text;
            KrakenEasy.Casinos.Poker888._Ruta = this.Casino_888Poker.Text;
            KrakenEasy.Casinos.PokerStars._Ruta = this.Casino_PokerStars.Text;
            KrakenEasy.Casinos.Winamax.Habilitado = this.Winamax_Status.IsChecked.Value;
            KrakenEasy.Casinos.Poker888.Habilitado = this.Poker888_Status.IsChecked.Value;
            KrakenEasy.Casinos.PokerStars.Habilitado = this.PokerStars_Status.IsChecked.Value;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Carpetas();
            void Carpetas()
            {
                MongoAccess _Access = new MongoAccess();
                if (!Directory.Exists(Casinos.Winamax._Ruta))
                {
                    Carpetas _Ventana = new Carpetas("WINAMAX");
                    _Ventana.ShowDialog();
                }
                if (!Directory.Exists(Casinos.Poker888._Ruta))
                {
                    Carpetas _Ventana = new Carpetas("888POKER");
                    _Ventana.ShowDialog();
                }
                if (!Directory.Exists(Casinos.PokerStars._Ruta))
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

        private void HandsLocal(object sender, RoutedEventArgs e)
        {
            KrakenBD.Settings.MongoCloud = !HandsLocal_Status.IsChecked.Value; 
        }
    }
}
