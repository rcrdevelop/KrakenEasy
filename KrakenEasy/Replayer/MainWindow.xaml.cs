using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KrakenEasy.HUDS;
using KrakenEasy.KrakenBD;
using MongoDB.Bson;
using Notification.Wpf;

namespace KrakenEasy.Replayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string Jugador;
        string path = Environment.CurrentDirectory;
        // Distrubicion de datos del array [0] = NombreJugador, [1] = PosicionMesa, [2] = PosicionPoker, [3] = CardLeft, [4] = CardRight, [5] = Hero 

        public MainWindow()
        {
            try
            {
                InitializeComponent();
              
            }
            catch (Exception ex)
            {

                Application.Current.Dispatcher.Invoke(() =>
                {
                    Window _Error = new Window();
                    _Error.Width = 200;
                    _Error.Height = 150;
                    Grid _Grid = new Grid();
                    _Error.Content = _Grid;
                    Label _Label = new Label();
                    _Label.Content = ex.Message;
                    _Grid.Children.Add(_Label);
                    _Error.Show();
                });
            }

        }
        private void Jugadores()
        {
            MongoAccess _Access = new MongoAccess();

            foreach (var Name in _Access.Get_Jugadores())
            {
                Label item = new Label();
                item.Content = Name;
                item.FontSize = 9;
                item.Foreground = Brushes.Black;
                item.MinWidth = 120;
                item.MaxHeight = 50;
                bool Condicion = _Access.Get_Hands(Name) < 5;
                if (Condicion)
                {
                    item.Foreground = new SolidColorBrush(Colors.Red);
                }
                if(!Condicion)
                {
                    item.Foreground = new SolidColorBrush(Colors.Green);
                }
                ID_HAND.Items.Add(item);
            }
        }

        private void Hands_ID(string Jugador)
        {

            MongoAccess _Access = new MongoAccess();
            List<string> Hands = _Access.Hands_ID(Jugador);
            if (Hands.Count > 5)
            {
                ID_HAND.Items.Clear();
                foreach (string Name in Hands)
                {
                    Label item = new Label();
                    item.Content = Name;
                    item.FontSize = 9;
                    ID_HAND.Items.Add(item);
                }
            }
            else
            {
                Modals.HandsError _Error = new Modals.HandsError();
                _Error.Error();
            }


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Jugadores();
        }


        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int _Condicion = Propiedades.Relative;
            if (_Condicion == 0)
            {
                Propiedades.Relative = 1;
            }
            if (_Condicion == 1)
            {
                Propiedades.Relative = 2;
            }
            if (_Condicion == 2)
            {
                Propiedades.Relative = 3;
            }
        }

        private void ID_HAND_Selected(object sender, RoutedEventArgs e)
        {
            if (ID_HAND.SelectedIndex >= 0)
            {

            int _ID = ID_HAND.SelectedIndex;
            MongoAccess _Access = new MongoAccess();

            string _Jugador = ID_HAND.Items[_ID].ToString().Split(":")[1].Trim();
            bool CondicionHand = true;

                    var o = 0;

                    foreach (var item2 in _Access.Get_Cards_Hands_Board(_Jugador))
                    {
                        if (o == 0)
                        {
                            
                            this.Card1.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + item2 + ".png"));
                        }
                        if (o == 1)
                        {
                            this.Card2.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + item2 + ".png"));
                        }
                        if (o == 2)
                        {
                            this.Card3.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + item2 + ".png"));
                        }
                        if (o == 3)
                        {
                            this.Card4.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + item2 + ".png"));
                        }
                        if (o == 4)
                        {
                            this.Card5.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + item2 + ".png"));
                        }

                        o++;
                        CondicionHand = false;


                    }
                if (!CondicionHand)
                {
                    Info_Load((string)_Jugador); ;
                }
                if (CondicionHand)
                    {
                        Hands_ID(_Jugador);
                    }


            }



        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ID_HAND.Items.Clear();
            Jugadores();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MongoAccess _Access = new MongoAccess();
            _Access.Set_Hole_Cards();
        }
        public void Info_Load(string Id_Hand)
        {
            try
            {

                MongoAccess _Access = new MongoAccess();
                foreach (var item in _Access.Get_Players(Id_Hand))
                {
                    var Jugador = item.Split(": ")[0].Split(" ")[0];
                    var PosicionPoker = item.Split("Position: ")[1];
                    var PosicionMesa = item.Split("SEAT ")[1].Split(":")[0];
                    string[] cards = _Access.Get_Cards_Player(Jugador, Id_Hand);
                    Player_Cards(cards, PosicionMesa, PosicionPoker);
                    Titulo_Info(Jugador, PosicionMesa, PosicionPoker);
                    //HUD_Info(Jugador, PosicionPoker);
                }
                var notificationManager = new NotificationManager();
                notificationManager.Show(new NotificationContent
                {
                    Title = "KrakenEasy",
                    Message = "Datos cargados correctamente.",
                    Type = NotificationType.Success
                });
            }
            catch (Exception ex)
            {
                var notificationManager = new NotificationManager();
                notificationManager.Show(new NotificationContent
                {
                    Title = "KrakenEasy",
                    Message = ex.Message,
                    Type = NotificationType.Error
                });
            }
        }
        void Titulo_Info(string NombreJugador, string PosicionMesa, string PosicionPoker) 
        {
            Label label = new Label();
            label.Content = PosicionPoker + " " + PosicionMesa + " " + NombreJugador;
            label.FontSize = 10;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.Foreground = new SolidColorBrush(Colors.White);
            Rectangle rectangle = new Rectangle();
            rectangle.Fill = Color_Titulo();
            Grid.SetColumn(label, Seleccion_Posicion(PosicionPoker.Trim()));
            Grid.SetRow(label, 2);
            Grid.SetColumn(rectangle, Seleccion_Posicion(PosicionPoker.Trim()));
            Grid.SetRow(rectangle, 2);
            this.Ïnfo_Players.Children.Add(rectangle);
            this.Ïnfo_Players.Children.Add(label);

            SolidColorBrush Color_Titulo() 
            {
                if (PosicionPoker == "SB")
                {
                    return new SolidColorBrush(Colors.Purple);
                }
                if (PosicionPoker == "BB")
                {
                    return new SolidColorBrush(Colors.OrangeRed);
                }
                if (PosicionPoker == "UTG")
                {
                    return new SolidColorBrush(Colors.DarkRed);
                }
                if (PosicionPoker == "UTG1")
                {
                    return new SolidColorBrush(Colors.DarkSlateGray);
                }
                if (PosicionPoker == "MP")
                {
                    return new SolidColorBrush(Colors.DarkCyan);
                }
                if (PosicionPoker == "MP1")
                {
                    return new SolidColorBrush(Colors.Blue);
                }
                if (PosicionPoker == "CO")
                {
                    return new SolidColorBrush(Colors.Orange);
                }
                if (PosicionPoker == "HJ")
                {
                    return new SolidColorBrush(Colors.ForestGreen);
                }
                if (PosicionPoker == "BTN")
                {
                    return new SolidColorBrush(Colors.Cyan);
                }
                else
                {
                    return new SolidColorBrush(Colors.White);
                }
            }
        }
        void Player_Cards(string[] Cartas, string PosicionMesa, string PosicionPoker)
        {
            Cards cards = new Cards(Cartas[0],Cartas[1]);
            cards.Width = 200;            
            Grid.SetColumn(cards, Seleccion_Posicion(PosicionPoker));
            Grid.SetRow(cards, 0);
            this.Ïnfo_Players.Children.Add(cards);
        }
        void HUD_Info(string NombreJugador, string PosicionPoker)
        {
            ContenedorHUD contendor = new ContenedorHUD(NombreJugador, "", true);
            Grid grid = new Grid();
            Grid.SetColumn(grid,Seleccion_Posicion(PosicionPoker));
            Grid.SetRow(grid, 1);
            grid.Children.Add(contendor);
            Window window = new Window();
            window.Content = grid;
            this.Ïnfo_Players.Children.Add(window);
        }
        int Seleccion_Posicion(string PosicionPoker) 
        {
            if (PosicionPoker == "SB")
            {
                return 8;
            }
            if (PosicionPoker == "BB")
            {
                return 7;
            }
            if (PosicionPoker == "UTG")
            {
                return 6;
            }
            if (PosicionPoker == "UTG1")
            {
                return 5;
            }
            if (PosicionPoker == "MP")
            {
                return 4;
            }
            if (PosicionPoker == "MP1")
            {
                return 3;
            }
            if (PosicionPoker == "CO")
            {
                return 2;
            }
            if (PosicionPoker == "HJ")
            {
                return 1;
            }
            if (PosicionPoker == "BTN")
            {
                return 0;
            }
            else {
                return 0;
            }
        }
    }
}