using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using KrakenEasy.Casinos;
using KrakenEasy.KrakenBD;
using KrakenEasy.Hands;

namespace KrakenEasy.Servicios
{
    public class ServiciosFicheros
    {

        public static void main()
        {
                try
                {
                    MongoAccess _Access = new MongoAccess();
                    if (KrakenEasy.HUDS.Propiedades.SystemActive)
                    {
                        Ficheros _Ficheros = new Ficheros();
                        Recorrer_Directorios_Casino();
                        _Ficheros.Recorrer_Registros();
                    }
                    //_Access.Hands();
                }
                catch (Exception ex)
                {
                }
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
                string folder = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands";
                string folderName = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents";
                string folderWinamax = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/";
                string folder888Poker = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/";
                string folderPokerStars = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/";
                if (!Directory.Exists(folder))
                {
                    string pathString = System.IO.Path.Combine(folderName, "KrakenHands");
                    Directory.CreateDirectory(pathString);
                }
                string rootFolderPathWinamax = Winamax._Ruta;
                string rootFolderPath888Poker = Poker888._Ruta;
                string rootFolderPathPokerStars = PokerStars._Ruta;
                string destinationPathWinamax = folderWinamax;
                string destinationPath888Poker = folder888Poker;
                string destinationPathPokerStars = folderPokerStars;
                try
                {

                
                string[] FolderList = Directory.GetDirectories(rootFolderPathWinamax);
                foreach (string Folder in FolderList)
                {
                    string[] ReadFolder = Directory.GetFiles(Folder + "/history");
                    foreach (string file in ReadFolder)
                    {
                        string fileToMove = file;
                        string moveTo = destinationPathWinamax + "WINAMAX" + System.IO.Path.GetFileName(file);
                        File.Copy(fileToMove, moveTo, true);
                        Registros_Data._Last_Hand(System.IO.Path.GetFileName(file).Split("_")[1]);
                    }
                }
                }
                catch(Exception ex) {
                    throw;
                }
                try
                {

                    string[]  FolderList = System.IO.Directory.GetDirectories(rootFolderPath888Poker);
                foreach (string Folder in FolderList)
                {
                    string[] ReadFolder = System.IO.Directory.GetFiles(Folder);
                    foreach (string file in ReadFolder)
                    {
                        string fileToMove = file;
                        string moveTo = destinationPath888Poker + "888POKER" + System.IO.Path.GetFileName(file);
                        File.Copy(fileToMove, moveTo, true);
                        MongoAccess _Access = new MongoAccess();
                        Registros_Data._Last_Hand(System.IO.Path.GetFileName(file).Split(" ")[1]);
                    }
                }
            }
                catch(Exception ex) {
                    throw;
                }
                try
                {

                
                string[]  FolderList = System.IO.Directory.GetDirectories(rootFolderPathPokerStars);
                foreach (string Folder in FolderList)
                {
                    string[] ReadFolder = System.IO.Directory.GetFiles(Folder);
                    foreach (string file in ReadFolder)
                    {
                        string fileToMove = file;
                        string moveTo = destinationPathPokerStars + "POKERSTARS" + System.IO.Path.GetFileName(file);
                        File.Copy(fileToMove, moveTo, true);
                        MongoAccess _Access = new MongoAccess();
                        Registros_Data._Last_Hand(System.IO.Path.GetFileName(file).Split("_")[1]);
                        }
                }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
            catch (Exception ex) {
                throw;
            }

        }

    }
}
