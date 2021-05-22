using System.IO;
using System.Windows;
using KrakenEasy.Modals;
using KrakenEasy.KrakenBD;
using Notification.Wpf;
using Newtonsoft.Json;

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
            string path = System.Environment.CurrentDirectory + "/rutas.json";
            using (StreamReader jsonStream = File.OpenText(path))
            {
                var json = jsonStream.ReadToEnd();
                Casinos.Casinos Casino = JsonConvert.DeserializeObject<Casinos.Casinos>(json);
                Application.Current.Dispatcher.Invoke(() =>
                {
                    this.Casino_Winamax.Text = Casino.Winamax_Ruta;
                    this.Casino_888Poker.Text = Casino.Poker888_Ruta;
                    this.Casino_PokerStars.Text = Casino.PokerStars_Ruta;
                    this.Winamax_Status.IsChecked = Casino.Winamax_Habilitado;
                    this.Poker888_Status.IsChecked = Casino.Poker888_Habilitado;
                    this.PokerStars_Status.IsChecked = Casino.PokerStars_Habilitado;
                });

            }

            
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
            var Casino = new Casinos.Casinos
            {
                Winamax_Ruta = Casinos.Winamax._Ruta,
                Poker888_Ruta = Casinos.Poker888._Ruta,
                PokerStars_Ruta = Casinos.PokerStars._Ruta,
                Winamax_Habilitado = Casinos.Winamax.Habilitado,
                PokerStars_Habilitado = Casinos.PokerStars.Habilitado,
                Poker888_Habilitado = Casinos.Poker888.Habilitado
            };
            string path = System.Environment.CurrentDirectory + "/rutas.json";
            string json = JsonConvert.SerializeObject(Casino);
            System.IO.File.WriteAllText(path, json);
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = "KrakenEasy",
                Message = "Datos actualizados.",
                Type = NotificationType.Notification
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Carpetas();
            void Carpetas()
            {
                bool TodoCorrecto = true;
                MongoAccess _Access = new MongoAccess();
                if (!Directory.Exists(Casinos.Winamax._Ruta) && Casinos.Winamax.Habilitado)
                {
                    Carpetas carpeta = new Carpetas();

                    carpeta.CarpetaDiagnostico("WINAMAX");
                    TodoCorrecto = false;
                }
                if (!Directory.Exists(Casinos.Poker888._Ruta) && Casinos.Poker888.Habilitado)
                {
                    Carpetas carpeta = new Carpetas();

                    carpeta.CarpetaDiagnostico("888POKER");
                    TodoCorrecto = false;
                }
                if (!Directory.Exists(Casinos.PokerStars._Ruta) && Casinos.PokerStars.Habilitado)
                {
                    Carpetas carpeta = new Carpetas();

                    carpeta.CarpetaDiagnostico("POKERSTARS");
                    TodoCorrecto = false;
                }
                if (!Directory.Exists("c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands"))
                {
                    Carpetas carpeta = new Carpetas();

                    carpeta.CarpetaDiagnostico("KrakenHands");
                    TodoCorrecto = false;
                }
                if (TodoCorrecto)
                {
                    var notificationManager = new NotificationManager();
                    notificationManager.Show(new NotificationContent
                    {
                        Title = "KrakenEasy",
                        Message = "No se encontraron problemas.",
                        Type = NotificationType.Success
                    });
                }
            }
        }

        private void HandsLocal(object sender, RoutedEventArgs e)
        {
            KrakenBD.Settings.MongoCloud = !HandsLocal_Status.IsChecked.Value; 
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            HUDS.PruebaHUDControler Controler_HUD = new HUDS.PruebaHUDControler();
            Controler_HUD.Show();
            HUDS.PruebaHUD HUD = new HUDS.PruebaHUD("Prueba", "Mesa_Prueba", false);
            HUD.Show();
        }
    }
}
