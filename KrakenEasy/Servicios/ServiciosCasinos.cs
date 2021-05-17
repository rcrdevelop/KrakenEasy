using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using KrakenEasy.Casinos;
using KrakenEasy.KrakenBD;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace KrakenEasy.Servicios
{
    class ServiciosCasinos
    {

        public static void main()
        {
                try
                {
                    MongoAccess _Access = new MongoAccess();
                    if (KrakenEasy.HUDS.Propiedades.SystemActive)
                    {
                        Actualizar_Casino_Status();
                        
                        try
                        {
                            if (KrakenEasy.Casinos.Winamax.Habilitado)
                            {
                                AnalizarMesaWinamax("C:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands");

                            }
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            if (KrakenEasy.Casinos.Winamax.Habilitado)
                            {
                                AnalizarMesa888Poker("C:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands");

                            }
                        }
                        catch (Exception ex)
                        {



                        }
                        try
                        {
                            if (KrakenEasy.Casinos.Winamax.Habilitado)
                            {
                                AnalizarMesaPokerStars("C:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands");

                            }
                        }
                        catch (Exception ex)
                        {



                        }
                }
                }
                catch(Exception ex) {
                    
                }
            }
        public static void AnalizarMesaWinamax(String _Ruta)
        {
            String[] _Archivos = Directory.GetFiles(_Ruta);
            foreach (IntPtr _Ventana in Mesas_Winamax())
            {
                foreach (string _Element in _Archivos)
                {
                    if (_Element.ToUpper().Contains("Winamax".ToUpper()))
                    {
                        if (_Element.ToUpper().Split("_")[1].Trim().ToUpper() == (Win32.GetWindowText(_Ventana).Split('/')[0].Trim().ToUpper()))
                        {
                            string[] _Data = new string[5];
                            RECT rcWindow = new RECT();
                            MongoAccess _Access = new MongoAccess();
                            Win32.GetWindowRect(_Ventana, out rcWindow);
                            _Data[0] = Win32.GetWindowText(_Ventana).Split('/')[0].Trim();
                            _Data[1] = rcWindow.Bottom.ToString() + " " + rcWindow.Left.ToString() + " " + rcWindow.Right.ToString() + " " + rcWindow.Top.ToString();
                            _Data[2] = "true";
                            _Data[3] = "false";
                            _Data[4] = "Winamax";
                            _Access.Set_Mesas(_Data);
                        }
                    }
                }
            }
        }
        public static void AnalizarMesa888Poker(String _Ruta)
        {
            String[] _Archivos = Directory.GetFiles(_Ruta);
            foreach (IntPtr _Ventana in Mesas_888Poker())
            {
                foreach (string _Element in _Archivos)
                {
                    if (_Element.ToUpper().Contains("888Poker".ToUpper()))
                    {
                        if (_Element.ToUpper().Contains(Win32.GetWindowText(_Ventana).Split(' ')[0].Trim().ToUpper()))
                        {
                            string[] _Data = new string[5];
                            var rcWindow = new RECT();
                            MongoAccess _Access = new MongoAccess();
                            Win32.GetWindowRect(_Ventana, out rcWindow);
                            _Data[0] = Win32.GetWindowText(_Ventana).Split(' ')[0].Trim();
                            _Data[1] = rcWindow.Bottom.ToString() + " " + rcWindow.Left.ToString() + " " + rcWindow.Right.ToString() + " " + rcWindow.Top.ToString();
                            _Data[2] = "true";
                            _Data[3] = "false";
                            _Data[4] = "888Poker";
                            _Access.Set_Mesas(_Data);
                        }
                    }
                }
            }
        }
        public static void AnalizarMesaPokerStars(String _Ruta)
        {
            String[] _Archivos = Directory.GetFiles(_Ruta);
            foreach (IntPtr _Ventana in Mesas_PokerStars())
            {
                foreach (string _Element in _Archivos)
                {
                    if (_Element.ToUpper().Contains("PokerStars".ToUpper()))
                    {
                        if (_Element.Split(" ")[1].Trim().ToUpper() == Win32.GetWindowText(_Ventana).Split('-')[0].Trim().ToUpper())
                        {
                            string[] _Data = new string[5];
                            var rcWindow = new RECT();
                            MongoAccess _Access = new MongoAccess();
                            Win32.GetWindowRect(_Ventana, out rcWindow);
                            _Data[0] = Win32.GetWindowText(_Ventana).Split('-')[0].Trim();
                            _Data[1] = rcWindow.Bottom.ToString() + " " + rcWindow.Left.ToString() + " " + rcWindow.Right.ToString() + " " + rcWindow.Top.ToString();
                            _Data[2] = "true";
                            _Data[3] = "false";
                            _Data[4] = "PokerStars";
                            _Access.Set_Mesas(_Data);
                        }
                    }
                }
            }
        }
        public static void Actualizar_Casino_Status()
        {

            foreach (Process ProcesoSystem in Process.GetProcesses())
            {
                if (ProcesoSystem.ProcessName.ToUpper().Contains(Winamax.Proceso().ToUpper()))
                {

                    //Procedimiento par);a casino activo
                    KrakenEasy.Casinos.Casinos.Winamax = true;
                }
                else
                {
                    if (KrakenEasy.Casinos.Casinos.Winamax)
                    {
                        KrakenEasy.Casinos.Casinos.Winamax = false;
                    }
                }
            }
            foreach (Process ProcesoSystem in Process.GetProcesses())
            {
                if (ProcesoSystem.ProcessName.ToUpper().Contains(Poker888.Proceso().ToUpper()))
                {
                    //Procedimiento para casino activo
                    KrakenEasy.Casinos.Casinos.Poker888 = true;
                }
                else
                {
                    if (KrakenEasy.Casinos.Casinos.Poker888)
                    {
                        KrakenEasy.Casinos.Casinos.Poker888 = false;
                    }
                }
            }
            foreach (Process ProcesoSystem in Process.GetProcesses())
            {
                if (ProcesoSystem.ProcessName.ToUpper().Contains(PokerStars.Proceso().ToUpper()))
                {
                    //Procedimiento para casino activo
                    KrakenEasy.Casinos.Casinos.PokerStars = true;
                }
                else
                {
                    if (KrakenEasy.Casinos.Casinos.PokerStars)
                    {
                        KrakenEasy.Casinos.Casinos.PokerStars = false;
                    }
                }
            }
        }

        
        public static List<IntPtr> Mesas_Winamax()
        {
            Winamax _Winamax = new Winamax();
            List<System.IntPtr> _Ventanas = new List<IntPtr>();
            foreach (String KeyVentana in _Winamax.Ventanas())
            {
                Console.WriteLine(KeyVentana);
                foreach (var Id_Ventana in Win32.FindWindowsWithText(KeyVentana))
                {
                    
                    _Ventanas.Add(Id_Ventana);
                }
            }
            return _Ventanas;
        }
        public static List<IntPtr> Mesas_888Poker()
        {
            Poker888 _888Poker = new Poker888();
            List<System.IntPtr> _Ventanas = new List<IntPtr>();
            foreach (String KeyVentana in _888Poker.Ventanas())
            {
                foreach (var Id_Ventana in Win32.FindWindowsWithText(KeyVentana))
                {
                    _Ventanas.Add(Id_Ventana);
                }
            }
            return _Ventanas;

        }
        public static List<IntPtr> Mesas_PokerStars()
        {
            PokerStars _PokerStars = new PokerStars();
            List<System.IntPtr> _Ventanas = new List<IntPtr>();
            foreach (String KeyVentana in _PokerStars.Ventanas())
            {
                foreach (var Id_Ventana in Win32.FindWindowsWithText(KeyVentana))
                {
                    _Ventanas.Add(Id_Ventana);
                }
            }
            return _Ventanas;

        }
    }
}
