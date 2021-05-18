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
        public static BsonDocument SB = new BsonDocument();
        public static BsonDocument BB = new BsonDocument();
        public static BsonDocument UTG = new BsonDocument();
        public static BsonDocument UTG1 = new BsonDocument();
        public static BsonDocument MP = new BsonDocument();
        public static BsonDocument MP1 = new BsonDocument();
        public static BsonDocument CO = new BsonDocument();
        public static BsonDocument HJ = new BsonDocument();
        public static BsonDocument BTN = new BsonDocument();
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Jugadores();
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
                if (_Access.Get_Hands(Name) < 5)
                {

                }
                else
                {

                }
                ID_HAND.Items.Add(item);
            }
        }

        private void Hands_ID(string Jugador)
        {

            MongoAccess _Access = new MongoAccess();
            if (_Access.Hands_ID(Jugador).Count > 5)
            {
                ID_HAND.Items.Clear();
                foreach (var Name in _Access.Hands_ID(Jugador))
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
                _Error.Show();
            }


        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MongoAccess _Access = new MongoAccess();
            Progress _HUD = new Progress(Jugador);
            Cards _Cards = new Cards();
            Hilo();
        }

        private void HUD_Relative()
        {
            int _HUD = new int();
            bool _Change = new bool();
            MongoAccess _Access = new MongoAccess();
            while (true)
            {
                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    _HUD = Propiedades.Relative;
                    _Change = Propiedades.Change;

                    if ((_HUD == 0) && (_Change))
                    {
                        Cards _Cards = new Cards();
                        Progress _HUD = new Progress(Jugador);
                    }
                    if ((_HUD == 1) && (_Change))
                    {
                        Cards _Cards = new Cards();
                        ProgressKraken _HUD = new ProgressKraken(Jugador);

                    }
                    if ((_HUD == 2) && (_Change))
                    {
                        Cards _Cards = new Cards();
                        FullHUD _HUD = new FullHUD();

                    }
                }));
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }
        }
        private void Hilo()
        {
            Thread _HiloHUD = new Thread(HUD_Relative);
            _HiloHUD.Start();

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MongoAccess _Access = new MongoAccess();
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
            MongoAccess _Access = new MongoAccess();
            int i = 0;
            string _Jugador = "";
            bool CondicionHand = true;
            foreach (var item in ID_HAND.Items)
            {
                if (ID_HAND.SelectedIndex == i)
                {
                    Label _Label1 = new Label();
                    Label _Label2 = new Label();
                    Label _Label3 = new Label();
                    Label _Label4 = new Label();
                    Label _Label5 = new Label();
                    Grid _Grid = new Grid();
                    var o = 0;
                    foreach (var item2 in _Access.Get_Cards_Hands_Board(item.ToString()))
                    {
                        _Label1 = new Label();

                        _Label1.Content = item2;

                        if (o == 1)
                        {
                            this.Card1.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + item2 + ".png"));
                        }
                        if (o == 2)
                        {
                            this.Card2.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + item2 + ".png"));
                        }
                        if (o == 3)
                        {
                            this.Card3.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + item2 + ".png"));
                        }
                        if (o == 4)
                        {
                            this.Card4.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + item2 + ".png"));
                        }
                        if (o == 5)
                        {
                            this.Card5.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + item2 + ".png"));
                        }
                        _Grid.RowDefinitions.Add(new RowDefinition());
                        Grid.SetRow(_Label1, o);
                        _Grid.Children.Add(_Label1);
                        o++;
                    }
                    Info_Load((string)item);

                    
                    }
                    else
                    {
                        _Jugador = item.ToString().Split(":")[1].Trim();
                    }
                }
                i++;

            if (CondicionHand)
            {
                Hands_ID(_Jugador);
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
        private void Players_Insert()
        {
            var CardsSB = "";
            var CurrentDirectory = Environment.CurrentDirectory;
            var path = CurrentDirectory;
            var CardDorso = new BitmapImage(new Uri(path + "/Replayer/Cartas/Baraja Dorso2.png"));
            var Card_Right = new BitmapImage(new Uri(path + "/Replayer/Cartas"));
            var Card_Left = new BitmapImage(new Uri(path + "/Replayer/Cartas/2H.png"));
            Cards _Card = new Cards();
            for (var count = 0; count <= 8; count++)
            {
                this.Ïnfo_Players.Children.Add(_Card);
                _Card.Card_Left.Source = Card_Left;
                _Card.Card_Left.Source = Card_Right;
                Grid.SetRow(_Card, count);
                Grid.SetColumn(_Card, count);
            }
            for (var count = 0; count <= 2; count++)
            {
                Grid _Grid = new Grid();
                Grid.SetRow(_Card, count);
                Grid.SetColumn(_Card, count);
            }
        }
        public void Info_Load(string Id_Hand)
        {
            this.Ïnfo_Players.RowDefinitions.Add(new RowDefinition());
            this.Ïnfo_Players.RowDefinitions.Add(new RowDefinition());
            this.Ïnfo_Players.RowDefinitions.Add(new RowDefinition());
            this.Ïnfo_Players.ColumnDefinitions.Add(new ColumnDefinition());
            this.Ïnfo_Players.ColumnDefinitions.Add(new ColumnDefinition());
            this.Ïnfo_Players.ColumnDefinitions.Add(new ColumnDefinition());
            this.Ïnfo_Players.ColumnDefinitions.Add(new ColumnDefinition());
            this.Ïnfo_Players.ColumnDefinitions.Add(new ColumnDefinition());
            this.Ïnfo_Players.ColumnDefinitions.Add(new ColumnDefinition());
            this.Ïnfo_Players.ColumnDefinitions.Add(new ColumnDefinition());
            this.Ïnfo_Players.ColumnDefinitions.Add(new ColumnDefinition());
            this.Ïnfo_Players.ColumnDefinitions.Add(new ColumnDefinition());
            MongoAccess _Access = new MongoAccess();
            foreach (var item in _Access.Get_Players(Id_Hand))
            {
                string[] cards = new string[2];
                cards[0] = item.Split("")[0];
                cards[1] = item.Split("")[1];
                Player_Cards(cards, item.Split(" ")[0], item.Split(" ")[0]);
                Titulo_Info(item.Split(" ")[0], item.Split(" ")[0], item.Split(" ")[0]);
                HUD_Info(item.Split(" ")[0], item.Split(" ")[0]);
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
            Grid grid = new Grid();
            Grid.SetColumn(grid, Seleccion_Posicion(PosicionPoker));
            Grid.SetRow(grid, 2);
            grid.Children.Add(label);
            grid.Children.Add(rectangle);
            this.Ïnfo_Players.Children.Add(grid);
            
            SolidColorBrush Color_Titulo() 
            {
                if (PosicionPoker == "SB")
                {
                    return new SolidColorBrush(Colors.White);
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
            MongoAccess _Access = new MongoAccess();
            Cards cards = new Cards();
            cards.Card_Left.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + Cartas[0] + ".png"));
            cards.Card_Right.Source = new BitmapImage(new Uri(path + "/Replayer/Cartas/" + Cartas[1] + ".png"));
            Grid grid = new Grid();
            Grid.SetColumn(grid, Seleccion_Posicion(PosicionPoker));
            Grid.SetRow(grid, 0);
            this.Ïnfo_Players.Children.Add(grid);
        }
        void HUD_Info(string NombreJugador, string PosicionPoker)
        {
            ContenedorHUD contendor = new ContenedorHUD(NombreJugador, "", true);
            Grid grid = new Grid();
            Grid.SetColumn(grid,Seleccion_Posicion(PosicionPoker));
            Grid.SetRow(grid, 1);
            grid.Children.Add(contendor);
            this.Ïnfo_Players.Children.Add(grid);
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
                return -1;
            }
        }
    }
}