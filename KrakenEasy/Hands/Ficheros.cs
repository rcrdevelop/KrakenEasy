using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using KrakenEasy.Casinos;
using KrakenEasy.KrakenBD;
using Notification.Wpf;

namespace KrakenEasy.Hands
{
    public class Ficheros
    {
        public void ImportarHands() {
            try
            {
                MongoAccess _Access = new MongoAccess();
                Winamax _Winamax = new Winamax();
                Poker888 _888Poker = new Poker888();
                PokerStars _PokerStars = new PokerStars();
                string folder = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands";
                string folderName = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents";
                string folderWinamax = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/Winamax";
                string folder888Poker = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/888Poker";
                string folderPokerStars = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/PokerStars";
                if (!Directory.Exists(folder))
                {
                    string pathString = System.IO.Path.Combine(folderName, "KrakenHands");
                    System.IO.Directory.CreateDirectory(pathString);
                }
                string rootFolderPathWinamax = Winamax._Ruta;
                string rootFolderPath888Poker = Poker888._Ruta; 
                string rootFolderPathPokerStars = PokerStars._Ruta;
                string destinationPathWinamax = folderWinamax;
                string destinationPath888Poker = folder888Poker;
                string destinationPathPokerStars = folderPokerStars;
                string[] FolderList = System.IO.Directory.GetDirectories(rootFolderPathWinamax);
                foreach (var Folder in FolderList)
                {
                    string[] ReadFolder = System.IO.Directory.GetFiles(Folder + "/history");
                    foreach (var file in ReadFolder)
                    {
                        string fileToMove = file;
                        string moveTo = destinationPathWinamax + System.IO.Path.GetFileName(file);
                        File.Copy(fileToMove, moveTo, true);
                    }
                }

                FolderList = System.IO.Directory.GetDirectories(rootFolderPath888Poker);
                foreach (var Folder in FolderList)
                {
                    string[] ReadFolder = System.IO.Directory.GetFiles(Folder);
                    foreach (var file in ReadFolder)
                    {
                        string fileToMove = file;
                        string moveTo = destinationPath888Poker + System.IO.Path.GetFileName(file);
                        File.Copy(fileToMove, moveTo, true);
                    }
                }
                FolderList = System.IO.Directory.GetDirectories(rootFolderPathPokerStars);
                foreach (var Folder in FolderList)
                {
                    string[] ReadFolder = System.IO.Directory.GetFiles(Folder);
                    foreach (var file in ReadFolder)
                    {
                        string fileToMove = file;
                        string moveTo = destinationPathPokerStars + System.IO.Path.GetFileName(file);
                        File.Copy(fileToMove, moveTo, true);
                    }
                }

            }
            catch (Exception ex)
            {
                
            }


        }
        public void Recorrer_Registros() 
        {
            string folder = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands";

            string[] ReadFolder = System.IO.Directory.GetFiles(folder);
            if (Casinos.Poker888.Habilitado || Casinos.Winamax.Habilitado || Casinos.PokerStars.Habilitado) {
                foreach (var file in ReadFolder)
                {
                    Analizador _Analizador = new Analizador();
                    _Analizador.Recorrer_Fichero(Path.GetFullPath(file));
                }
            }
        }
    }
}
