using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;
using KrakenEasy.Casinos;
using KrakenEasy.KrakenBD;
using Notification.Wpf;
using System.Linq;

namespace KrakenEasy.Hands
{
    public class Ficheros
    {
        public void ImportarHands()
        {
            try
            {
                //if (HUDS.Propiedades.SystemActive)
                //{
                //    MongoAccess _Access = new MongoAccess();
                //    Winamax _Winamax = new Winamax();
                //    Poker888 _888Poker = new Poker888();
                //    PokerStars _PokerStars = new PokerStars();
                //    string folder = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands";
                //    string folderName = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents";
                //    string folderWinamax = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/WINAMAX";
                //    string folder888Poker = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/888POKER";
                //    string folderPokerStars = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/POKERSTARS";
                //    if (!Directory.Exists(folder))
                //    {
                //        string pathString = System.IO.Path.Combine(folderName, "KrakenHistory");
                //        System.IO.Directory.CreateDirectory(pathString);
                //    }
                //    string rootFolderPathWinamax = Winamax._Ruta;
                //    string rootFolderPath888Poker = Poker888._Ruta;
                //    string rootFolderPathPokerStars = PokerStars._Ruta;
                //    string destinationPathWinamax = folderWinamax;
                //    string destinationPath888Poker = folder888Poker;
                //    string destinationPathPokerStars = folderPokerStars;
                //    if (Casinos.Winamax.Habilitado)
                //    {
                //        string[] FolderList = System.IO.Directory.GetDirectories(rootFolderPathWinamax);
                //        foreach (var Folder in FolderList)
                //        {
                //            string[] ReadFolder = System.IO.Directory.GetFiles(Folder + "/history");
                //            foreach (var file in ReadFolder)
                //            {
                //                string fileToMove = file;
                //                string moveTo = destinationPathWinamax + System.IO.Path.GetFileName(file);
                //                File.Copy(fileToMove, moveTo, true);
                //            }
                //        }
                //    }
                //    if (Casinos.Poker888.Habilitado)
                //    {
                //        string[] FolderList = System.IO.Directory.GetDirectories(rootFolderPath888Poker);
                //        foreach (var Folder in FolderList)
                //        {
                //            string[] ReadFolder = System.IO.Directory.GetFiles(Folder);
                //            foreach (var file in ReadFolder)
                //            {
                //                string fileToMove = file;
                //                string moveTo = destinationPath888Poker + System.IO.Path.GetFileName(file);
                //                File.Copy(fileToMove, moveTo, true);
                //            }
                //        }
                //    }
                //    if (Casinos.PokerStars.Habilitado)
                //    {
                //        string[] FolderList = System.IO.Directory.GetDirectories(rootFolderPathPokerStars);
                //        foreach (var Folder in FolderList)
                //        {
                //            string[] ReadFolder = System.IO.Directory.GetFiles(Folder);
                //            foreach (var file in ReadFolder)
                //            {
                //                string fileToMove = file;
                //                string moveTo = destinationPathPokerStars + System.IO.Path.GetFileName(file);
                //                File.Copy(fileToMove, moveTo, true);
                //            }
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {

            }


        }
        public void Recorrer_Registros()
        {
            string folder = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHistory";

            string[] ReadFolder = System.IO.Directory.GetFiles(folder);
            string folderMesas = @"c:/Users/" + (System.Security.Principal.WindowsIdentity.GetCurrent().Name).Split('\\')[1] + "/Documents/KrakenHands/";
            Thread _KrakenHands = new Thread(() => { KrakenHands(ReadFolder, folderMesas); });
            _KrakenHands.Start();
        }
        void KrakenHands(string[] ReadFolder, string folderMesas)
        {
            if (Casinos.Poker888.Habilitado || Casinos.Winamax.Habilitado || Casinos.PokerStars.Habilitado)
            {
                foreach (var file in ReadFolder)
                {
                    if (!file.Contains(".dat"))
                    {
                        Analizador _Analizador = new Analizador();
                        _Analizador.Recorrer_Fichero(Path.GetFullPath(file));
                        var Time = File.GetLastWriteTime(file);
                        var HoraFichero = Time.Year * 10000000000 + Time.Month * 1000000 + Time.Day * 10000 + Time.Hour * 100 + Time.Minute;
                        Time = DateTime.Now;
                        var HoraActual = Time.Year * 10000000000 + Time.Month * 1000000 + Time.Day * 10000 + Time.Hour * 100 + Time.Minute;
                        if (HoraActual <= HoraFichero + 1)
                        {
                            string Casino = " ";
                            var moveTo = folderMesas + System.IO.Path.GetFileName(file);
                            string Nombre_File = "";
                            if (file.Contains("WINAMAX"))
                            {
                                Casino = "Winamax";
                                Nombre_File = Path.GetFileName(file).Split("_")[1].Split("_")[0];
                            }
                            else if (file.Contains("888POKER"))
                            {
                                Casino = "888Poker";
                                Nombre_File = Path.GetFileName(file).Split(" ")[1].Split(" ")[0];
                            }
                            else if (file.Contains("POKERSTARS"))
                            {
                                Casino = "PokerStars";
                                Nombre_File = Path.GetFileName(file).Split(" ")[1] + " " + Path.GetFileName(file).Split(" ")[2];
                                Nombre_File = Nombre_File.Replace("-", "");
                            }

                            var notificationManager = new NotificationManager();
                            notificationManager.Show(new NotificationContent
                            {
                                Title = "KrakenEasy",
                                Message = "Se ha detectado una mesa con nombre de '" + Nombre_File + "' del casino " + Casino,
                                Type = NotificationType.Notification
                            });
                            string line;
                            MongoAccess _Access = new MongoAccess();
                            StreamReader fileRead = new System.IO.StreamReader(file);
                            List<string> ids = new List<string>();
                            while ((line = fileRead.ReadLine()) != null)
                            {

                                if (Casino == "Winamax")
                                {
                                    if (line.Contains(Winamax.Id()))
                                    {
                                        ids.Add(line.Split("#")[1].Split(" ")[0]);
                                    }

                                }
                                if (Casino == "888Poker")
                                {
                                    if (line.Contains(Poker888.Id()))
                                    {
                                        ids.Add(line.Split(": ")[1]);
                                    }
                                }
                                if (Casino == "PokerStars")
                                {
                                    if (line.Contains(PokerStars.Id()))
                                    {
                                        ids.Add(line.Split("#")[1].Split(":")[0]);
                                    }
                                }
                            }
                            fileRead.Close();
                            List<string> jugadores = new List<string>();
                            while (0 == jugadores.Count)
                            {
                                jugadores = _Access.Get_Players(ids[ids.Count - 1]);
                                Thread.Sleep(TimeSpan.FromSeconds(0.2));
                            }
                            Mesa _Mesa = new Mesa
                            {
                                Hand = ids[ids.Count - 1],
                                Casino = Casino,
                                Nombre = Path.GetFileName(file).Replace("txt", "json"),
                                Jugadores = jugadores

                            };
                            string json = Newtonsoft.Json.JsonConvert.SerializeObject(_Mesa);
                            System.IO.File.WriteAllText(folderMesas + Path.GetFileName(file).Replace("txt", "json"), json);

                        }
                        else
                        {
                            ReadFolder = System.IO.Directory.GetFiles(folderMesas);
                            foreach (var file2 in ReadFolder)
                            {

                                if (file2.Contains(Path.GetFileName(file).Replace("txt", "json")))
                                {

                                    string Nombre_File2 = " ";
                                    MongoAccess _Access = new MongoAccess();
                                    if (file.Contains("WINAMAX"))
                                    {
                                        Nombre_File2 = Path.GetFileName(file).Split("_")[1].Split("_")[0];
                                    }
                                    else if (file.Contains("888POKER"))
                                    {
                                        Nombre_File2 = Path.GetFileName(file).Split(" ")[1].Split(" ")[0];
                                    }
                                    else if (file.Contains("POKERSTARS"))
                                    {
                                        Nombre_File2 = Path.GetFileName(file).Split(" ")[1] + " " + Path.GetFileName(file).Split(" ")[2];
                                        Nombre_File2 = Nombre_File2.Replace("-", "");
                                    }

                                    File.Delete(folderMesas + Path.GetFileName(file).Replace("txt", "json"));
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
