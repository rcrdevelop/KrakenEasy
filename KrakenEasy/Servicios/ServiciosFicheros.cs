using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Security.Principal;
using KrakenEasy.Casinos;
using KrakenEasy.KrakenBD;
using KrakenEasy.Hands;
using Notification.Wpf;

namespace KrakenEasy.Servicios
{
    public class ServiciosFicheros
    {

        public static void main()
        {

                    MongoAccess _Access = new MongoAccess();
                    if (SystemKraken.HUDS)
                    {
                        Ficheros _Ficheros = new Ficheros();
                        Recorrer_Directorios_Casino();
                        _Ficheros.Recorrer_Registros();
                    }
                    //_Access.Hands();
            }


        public static Boolean Mostrar_HUDS(String Fichero)
        {
            MongoAccess _Access = new MongoAccess();
            if (Winamax.Habilitado && Fichero.Contains("WINAMAX"))
            {
                DateTime dt = File.GetLastWriteTime(Fichero);
                int _Hora_Fichero = dt.Hour * 100 + dt.Minute;
                int _Hora_Actual = DateTime.Now.Hour * 100 + DateTime.Now.Minute;
                if (_Hora_Fichero >= _Hora_Actual)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (Poker888.Habilitado && Fichero.Contains("888POKER"))
            {
                DateTime dt = File.GetLastWriteTime(Fichero);
                int _Hora_Fichero = dt.Hour * 100 + dt.Minute;
                int _Hora_Actual = DateTime.Now.Hour * 100 + DateTime.Now.Minute;
                if (_Hora_Fichero >= _Hora_Actual)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (PokerStars.Habilitado && Fichero.Contains("POKERSTARS"))
            {
                DateTime dt = File.GetLastWriteTime(Fichero);
                int _Hora_Fichero = dt.Hour * 100 + dt.Minute;
                int _Hora_Actual = DateTime.Now.Hour * 100 + DateTime.Now.Minute;
                if (_Hora_Fichero >= _Hora_Actual)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;

        }
        public static void Recorrer_Directorios_Casino()
        {
            try
            {
                string folder = @"c:/Users/" + (WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHistory";
                string folderMesas = @"c:/Users/" + (WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands";
                string folderName = @"c:/Users/" + (WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents";
                string folderWinamax = @"c:/Users/" + (WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHistory/";
                string folder888Poker = @"c:/Users/" + (WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHistory/";
                string folderPokerStars = @"c:/Users/" + (WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHistory/";
                if (!Directory.Exists(folder))
                {
                    string pathString = Path.Combine(folderName, "KrakenHistory");
                    Directory.CreateDirectory(pathString);
                }
                if (!Directory.Exists(folderMesas))
                {
                    string pathString = Path.Combine(folderName, "KrakenHands");
                    Directory.CreateDirectory(pathString);
                }
                if (!Directory.Exists(Environment.CurrentDirectory+"/Mesas/Winamax"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/Mesas/Winamax");
                }
                if (!Directory.Exists(Environment.CurrentDirectory + "/swap/stats_actualizar"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/swap/stats_actualizar");
                }
                if (!Directory.Exists(Environment.CurrentDirectory + "/swap/mainwindow"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/swap/mainwindow");
                }
                if (!Directory.Exists(Environment.CurrentDirectory + "/Mesas/888Poker"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/Mesas/PokerStars");
                }
                if (!Directory.Exists(Environment.CurrentDirectory + "/HUDS_Kraken/Winamax"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/HUDS_Kraken/Winamax");
                }
                if (!Directory.Exists(Environment.CurrentDirectory + "/HUDs_Kraken/888Poker"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/HUDS_Kraken/888Poker");
                }
                if (!Directory.Exists(Environment.CurrentDirectory + "/HUDS_Kraken/PokerStars"))
                {
                    Directory.CreateDirectory(Environment.CurrentDirectory + "/HUDS_Kraken/PokerStars");
                }

                string rootFolderPathWinamax = Winamax._Ruta;
                string rootFolderPath888Poker = Poker888._Ruta;
                string rootFolderPathPokerStars = PokerStars._Ruta;
                string destinationPathWinamax = folderWinamax;
                string destinationPath888Poker = folder888Poker;
                string destinationPathPokerStars = folderPokerStars;
                try
                {

                    if (KrakenEasy.Casinos.Winamax.Habilitado)
                    {
                        string[] FolderList = Directory.GetDirectories(rootFolderPathWinamax);
                        foreach (string Folder in FolderList)
                        {
                            string[] ReadFolder = Directory.GetFiles(Folder + "/history");
                            foreach (string file in ReadFolder)
                            {
                                if (file.Contains(".txt")) 
                                {
                                    string fileToMove = file;
                                    string moveTo = destinationPathWinamax + "WINAMAX" + Path.GetFileName(file);
                                    var Time = File.GetLastAccessTime(file);
                                    var TimeFile = Time.Year * 10000 + Time.Month * 10 + Time.Day * 10 + Time.Hour * 10 + Time.Minute * 10 + Time.Second * 10;
                                    Time = DateTime.Now;
                                    var TimeActual = Time.Year * 10000 + Time.Month * 10 + Time.Day * 10 + Time.Hour * 10 + Time.Minute * 10 + Time.Second * 10;
                                    
                                    if (!(TimeFile == TimeActual)) { 
                                        File.Copy(fileToMove, moveTo, true);
                                    }
                                }
                                
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Modals.Carpetas carpeta = new Modals.Carpetas();

                    carpeta.CarpetaDiagnostico("WINAMAX");
                }
                try
                {
                    if (KrakenEasy.Casinos.Poker888.Habilitado)
                    {
                        string[] FolderList = Directory.GetDirectories(rootFolderPath888Poker);
                        foreach (string Folder in FolderList)
                        {
                            string[] ReadFolder = Directory.GetFiles(Folder);
                            foreach (string file in ReadFolder)
                            {
                                if (file.Contains(".txt"))
                                {
                                    string fileToMove = file;
                                    string moveTo = destinationPath888Poker + "888POKER" + Path.GetFileName(file);
                                    File.Copy(fileToMove, moveTo, true);
                                }
                            }
                        }
                    }
            }
                catch(Exception ex) {
                    Modals.Carpetas carpeta = new Modals.Carpetas();

                    carpeta.CarpetaDiagnostico("888POKER");
                }
                try
                {

                    if (KrakenEasy.Casinos.PokerStars.Habilitado)
                    {
                        string[] FolderList = Directory.GetDirectories(rootFolderPathPokerStars);

                        foreach (string Folder in FolderList)
                        {
                            string[] ReadFolder = Directory.GetFiles(Folder);
                            foreach (string file in ReadFolder)
                            {
                                if (file.Contains(".txt"))
                                {
                                    string fileToMove = file;
                                    string moveTo = destinationPathPokerStars + "POKERSTARS" + Path.GetFileName(file);
                                    File.Copy(fileToMove, moveTo, true);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Modals.Carpetas carpeta = new Modals.Carpetas();

                    carpeta.CarpetaDiagnostico("POKERSTARS");
                }
            }
            catch (Exception ex) {
            }

        }

    }
}
