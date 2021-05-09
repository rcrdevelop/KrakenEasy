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
namespace KrakenEasy.Replayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Jugador;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                Jugadores();
            }
            catch (Exception ex)
            {

                Application.Current.Dispatcher.Invoke(() => {
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
                if (_Access.Get_Hands(Name) < 5)
                {
                    item.Foreground = Brushes.Red;
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
            _Access.InicializarMain();
            Progress _HUD = new Progress(Jugador);
            Cards _Cards = new Cards();
            this.SB_HUD.Children.Add(_HUD);
            this.SB_Cards.Children.Add(_Cards);
            _HUD = new Progress(Jugador);
            _Cards = new Cards();
            this.BB_HUD.Children.Add(_HUD);
            this.BB_Cards.Children.Add(_Cards);
            _HUD = new Progress(Jugador);
            _Cards = new Cards();
            this.UTG_HUD.Children.Add(_HUD);
            this.UTG_Cards.Children.Add(_Cards);
            _HUD = new Progress(Jugador);
            _Cards = new Cards();
            this.UTG1_HUD.Children.Add(_HUD);
            this.UTG1_Cards.Children.Add(_Cards);
            _HUD = new Progress(Jugador);
            _Cards = new Cards();
            this.MP_HUD.Children.Add(_HUD);
            this.MP_Cards.Children.Add(_Cards);
            _HUD = new Progress(Jugador);
            _Cards = new Cards();
            this.MP1_HUD.Children.Add(_HUD);
            this.MP1_Cards.Children.Add(_Cards);
            _HUD = new Progress(Jugador);
            _Cards = new Cards();
            this.HJ_HUD.Children.Add(_HUD);
            this.HJ_Cards.Children.Add(_Cards);
            _HUD = new Progress(Jugador);
            _Cards = new Cards();
            this.CO_HUD.Children.Add(_HUD);
            this.CO_Cards.Children.Add(_Cards);
            _HUD = new Progress(Jugador);
            _Cards = new Cards();
            this.BTN_HUD.Children.Add(_HUD);
            this.BTN_Cards.Children.Add(_Cards);
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
                        this.SB_HUD.Children.Clear();
                        this.SB_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new Progress(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.BB_HUD.Children.Clear();
                        this.BB_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new Progress(Jugador);
                        _Cards = new Cards();
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.UTG_HUD.Children.Clear();
                        this.UTG_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new Progress(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.UTG1_HUD.Children.Clear();
                        this.UTG1_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new Progress(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.MP_HUD.Children.Clear();
                        this.MP_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new Progress(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.MP1_HUD.Children.Clear();
                        this.MP1_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new Progress(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.HJ_HUD.Children.Clear();
                        this.HJ_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new Progress(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.CO_HUD.Children.Clear();
                        this.CO_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new Progress(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.BTN_HUD.Children.Clear();
                        this.BTN_HUD.Children.Add(_HUD);
                    }
                    if ((_HUD == 1) && (_Change))
                    {
                        Cards _Cards = new Cards();
                        ProgressKraken _HUD = new ProgressKraken(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.SB_HUD.Children.Clear();
                        this.SB_HUD.Children.Add(_HUD);
                        _HUD = new ProgressKraken(Jugador);
                        _Cards = new Cards();
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.BB_HUD.Children.Clear();
                        this.BB_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new ProgressKraken(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.UTG_HUD.Children.Clear();
                        this.UTG_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new ProgressKraken(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.UTG1_HUD.Children.Clear();
                        this.UTG1_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new ProgressKraken(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.MP_HUD.Children.Clear();
                        this.MP_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new ProgressKraken(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.MP1_HUD.Children.Clear();
                        this.MP1_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new ProgressKraken(Jugador);
                        this.HJ_HUD.Children.Clear();
                        this.HJ_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new ProgressKraken(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.CO_HUD.Children.Clear();
                        this.CO_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new ProgressKraken(Jugador);
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.BTN_HUD.Children.Clear();
                        this.BTN_HUD.Children.Add(_HUD);
                    }
                    if ((_HUD == 2) && (_Change))
                    {
                        Cards _Cards = new Cards();
                        FullHUD _HUD = new FullHUD();
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.SB_HUD.Children.Clear();
                        this.SB_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new FullHUD();
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.BB_HUD.Children.Clear();
                        this.BB_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new FullHUD();
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.UTG_HUD.Children.Clear();
                        this.UTG_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new FullHUD();
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.UTG1_HUD.Children.Clear();
                        this.UTG1_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new FullHUD();
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.MP_HUD.Children.Clear();
                        this.MP_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new FullHUD();
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.MP1_HUD.Children.Clear();
                        this.MP1_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new FullHUD();
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.HJ_HUD.Children.Clear();
                        this.HJ_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new FullHUD();
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.CO_HUD.Children.Clear();
                        this.CO_HUD.Children.Add(_HUD);
                        _Cards = new Cards();
                        _HUD = new FullHUD();
                        this.SB_Cards.Children.Clear();
                        this.SB_Cards.Children.Add(_Cards);
                        this.BTN_HUD.Children.Clear();
                        this.BTN_HUD.Children.Add(_HUD);
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
                //if (ID_HAND.SelectedIndex == i)
                //{
                //    Window _Window = new Window();
                //    Label _Label = new Label();
                //    foreach (var item2 in _Access.Get_Cards_Hands_Board(item.ToString().Split(":")[1].Trim()))
                //    {
                //        _Label.Content = item2;
                //    }
                //    _Window.Content = _Label;
                //    _Window.Show();
                //    if (_Access.Get_Cards_Hands_Board(item.ToString().Split(":")[1].Trim()).Count > 0)
                //    {
                //        var Board = _Access.Get_Cards_Hands_Board(item.ToString().Split(":")[1].Trim()));
                //        for (int j = 0; j < Board.Split(" ").Count(); j++)
                //        {
                //            string card = "";
                //            string ruta = "C:/Users/contr/source/repos/KrakenEasy/KrakenEasy/Replayer/Cartas";
                //            string[] FolderList = System.IO.Directory.GetDirectories(ruta);
                //            foreach (var Folder in FolderList)
                //            {
                //                if (Folder.ToUpper().Contains(Board.Split(" ")[j].ToUpper()))
                //                {
                //                    CondicionHand = false;

                //                    card = Folder;
                //                    if (j == 0)
                //                    {
                //                        this.Card1.Source = new BitmapImage(new Uri(card));
                //                    }
                //                    if (j == 1)
                //                    {
                //                        this.Card2.Source = new BitmapImage(new Uri(card));
                //                    }
                //                    if (j == 2)
                //                    {
                //                        this.Card3.Source = new BitmapImage(new Uri(card));
                //                    }
                //                    if (j == 3)
                //                    {
                //                        this.Card4.Source = new BitmapImage(new Uri(card));
                //                    }
                //                    if (j == 4)
                //                    {
                //                        this.Card5.Source = new BitmapImage(new Uri(card));
                //                    }
                //                }
                //            }

                //        }
                //    }
                //    else 
                //    { 
                //        _Jugador = item.ToString().Split(":")[1].Trim();
                //    }
                //}
                //i++;

            }
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
    }
}