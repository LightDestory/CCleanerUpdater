using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace CCleanerUpdater
{
    class Updater
    {
        private const String title = "--- CCleaner Updater by LightDestory ---";
        private const String USAGE = "Usage:\n\n" +
            "CCleanerUpdater.exe path=\"[CCleaner's Install Dir]\" lang=\"[Language]\" winapp2=\"[Option]\" service=\"[option]\"\n\n" +
            "    # Common Install Dir: \"C:/Program Files/CCleaner\" - Use 'Common' if you want use this path\n"+
            "    # WinApp2 Option:\n      # None - Don't install WinApp2\n      # Download - install the latest version\n      # DownloadTrim - Install Winapp2 and Run the Trimmer script\n" +
            "    # Service Option:\n      # None - Don't set-up the on startup service\n      # Install - Set-up the on startup service\n\n" +
            "Example: CCleanerUpdater.exe path=\"Common\" lang=\"1040\" winapp2=\"Download\" service=\"install\"";
        private readonly String[,] Languages =
        {
            {"Albanian","1052"},{"Arabic","1025"},{"Armenian","1067"},{"Azeri Latin","1068"},{"Belarusian","1059"},{"Bosnian","5146"},{"Portuguese(Brazil)","1046"},
            {"Bulgarian","1026"},{"Catalan","1027"},{"Chinese-Simplified","2052"},{"Chinese-Traditional","1028"},{"Croatian"," 1050"},{"Czech","1029"},
            {"Danish","1030"},{"Dutch","1043"},{"English","1033"},{"Estonian","1061"},{"Farsi","1065"},{"Finnish","1035"},{"French","1036"},{"Galician","1110"},
            {"Georgian","1079"},{"German","1031"},{"Greek","1032"},{"Hebrew","1037"},{"Hungarian","1038"},{"Italian","1040"},{"Japanese","1041"},{"Kazakh","1087"},
            {"Korean","1042"},{"Lithuanian","1063"},{"Macedonian","1071"},{"Norwegian","1044"},{"Polish","1045"},{"Portuguese","2070"},{"Romanian","1048"},
            {"Russian","1049"},{"Serbian-Cyrillic","3098"},{"Serbian-Latin","2074"},{"Slovak","1051"},{"Slovenian","1060"},{"Spanish","1034"},
            {"Swedish","1053"},{"Turkish","1055"},{"Ukrainian","1058"},{"Vietnamese","1066"}
        };
        private readonly String[] Links =
            {
                "https://www.piriform.com/ccleaner/download","http://download.piriform.com/ccsetup", "https://raw.githubusercontent.com/LightDestory/CCleanerUpdater/master/version.txt",
                "https://raw.githubusercontent.com/MoscaDotTo/Winapp2/master/Winapp2.ini","http://www.winapp2.com/trim.bat","http://lightdestoryweb.altervista.org/"
            };
        private readonly String[] Winapp2Options =
        {
            "none", "download", "downloadtrim"
        };
        private readonly String[] ServiceOptions =
        {
            "none", "install"
        };
        private WebClient webby;
        private const String HTMLPrefix ="<p><strong>v";
        private const String HTMLSuffix = "</strong> (";
        private const String FileName = "CCleanerUpdate.exe";
        private String CommonDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\CCleaner\\";
        private String OnlineVersion;
        private String CurrentVersion;

        public Updater()
        {
            CurrentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            webby = new WebClient();
        }

        public void Job(String InstallDir, String Lang, String Winapp2, String Service)
        {
            if (InstallDir.Equals("newinstall"))
            {
                getLatestVersionOnline();
                Download(Links[1] + OnlineVersion.Substring(0, 4).Replace(".", "") + ".exe", System.IO.Path.GetTempPath() + "\\" + FileName, "CCleaner");
                Install(Lang);
                SetupWinapp2(CommonDirectory, Winapp2);
                SetUpService(CommonDirectory, Lang, Winapp2, Service);
            }
            else
            {
                if (InstallDir.Equals("common"))
                {
                    InstallDir = CommonDirectory;
                }
                else if (InstallDir.ToCharArray()[InstallDir.Length - 1] != '/')
                {
                    InstallDir = InstallDir + "\\";
                }
                getCurrentVersionFromExe(InstallDir);
                getLatestVersionOnline();
                if (Compare())
                {
                    Download(Links[1] + OnlineVersion.Substring(0, 4).Replace(".", "") + ".exe", System.IO.Path.GetTempPath() + "\\" + FileName, "CCleaner");
                    Install(Lang);
                }
                else
                {
                    Console.WriteLine("You are using the latest version!");
                }
                SetupWinapp2(InstallDir, Winapp2);
                SetUpService(InstallDir, Lang, Winapp2, Service);
            }
            WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "Done!");
        }

        private Boolean Compare()
        {
            Console.WriteLine("Installed Version: {0}\nOnline Version: {1}", CurrentVersion, OnlineVersion);
            return (!CurrentVersion.Equals(OnlineVersion));
        }

        private void Download(String link, String saveto, String name)
        {
            try
            {
                Console.Out.Write("Downloading latest version of {0}...", name);
                webby.DownloadFile(link, saveto);
                WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "Download completed");
            }
            catch (Exception ex)
            {
                WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Error on downloading!");
                Console.Error.WriteLine(ex);
                Exit();
            }
        }

        public Boolean CheckExist(String path, String file)
        {
            return File.Exists(path + file);
        }

        private void Install(String Lang)
        {
            Console.Out.Write("Installing CCleaner... ");
            try
            {
                var Installation = Process.Start(System.IO.Path.GetTempPath() + "\\" + FileName, "/S /L=" + Lang);
                Installation.WaitForExit();
                File.Delete(System.IO.Path.GetTempPath() + "\\" + FileName);
                WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "Install completed!");
            }
            catch (Exception ex)
            {
                WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Error on installing...");
                Console.Error.WriteLine(ex.ToString());
                Exit();
            }
        }

        private void SetupWinapp2(String path, String Winapp2)
        {
            try
            {
                if (Winapp2.Contains("download"))
                {
                    if (!CheckExist(path, "Winapp2.ini"))
                    {
                        Download(Links[3], path + "Winapp2.ini", "Winapp2");
                        if (Winapp2.Contains("trim"))
                        {
                            TrimWinapp2(path);
                        }
                    }
                    else
                    {
                        OnlineVersion = new StringReader(webby.DownloadString(Links[3])).ReadLine().Replace("; Version: ", "");
                        CurrentVersion = new StringReader(File.ReadAllText(path + "Winapp2.ini")).ReadLine().Replace("; Version: ", "");
                        if (Compare())
                        {
                            Download(Links[3], path + "Winapp2.ini", "Winapp2");
                            if (Winapp2.Contains("trim"))
                            {
                                TrimWinapp2(path);
                            }
                        }
                        else
                        {
                            Console.WriteLine("You are using the latest version!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Error on Setup of Winapp2");
                Console.WriteLine(ex);
                Exit();
            }
        }

        private void TrimWinapp2(String path)
        {
            try
            {
                if (!CheckExist(path, "Trimmer.bat"))
                {
                    Download(Links[4], path + "Trimmer.bat", "Trimmer Script");
                }
                Console.Out.Write("Running Trimmer Script, follow the instructions!");
                var TrimmerScript = Process.Start(path + "Trimmer.bat");
                TrimmerScript.WaitForExit();
                WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "Trimmed Successfull");
            }
            catch (WebException wex)
            {
                WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Unable to Download the Trimmer Script");
                Console.Error.WriteLine(wex);
                Exit();
            }
        }

        public void SetUpService(String path, string lang, string winapp2, string action)
        {
            if (action.Equals("install"))
            {
                if (System.Environment.OSVersion.Version.Major == 5)
                {
                    WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Windows XP isn't supported for daily Service Setup!");
                }
                else
                {
                    Console.Out.Write("Setting up Daily Service...");
                    String Path2 = Environment.CurrentDirectory + "\\" + System.AppDomain.CurrentDomain.FriendlyName;
                    String Arg = "path=\"" + path + "\\\"" + " lang=\"" + lang + "\"" + " winapp2=\"" + winapp2 + "\"" + " service=\"None\"";
                    File.WriteAllText("Service.xml", CCleanerUpdater.Properties.Resources.CCleanerUpdaterService.ToString().Replace("%PATH_TO_TOOL%", Path2).Replace("%ARG_TO_USE%", Arg));
                    var x = Process.Start("schtasks.exe", "/create /tn \"CCleanerUpdater\" /XML \"" + Environment.CurrentDirectory + "\\" + "Service.xml\"");
                    WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "Done! Remember: Don't move the tool from the actual folder!");
                    x.WaitForExit();
                    File.Delete("Service.xml");
                }
            }
        }

        public void CheckForUpdate()
        {
            Console.Out.Write("Checking for Tool's Update... ");
            try
            {
                OnlineVersion = webby.DownloadString(Links[2]);
                if (!OnlineVersion.Equals(CurrentVersion))
                {
                    WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "New Update available! Opening my website...");
                    Process.Start(Links[5]);
                    Exit();
                }
                else
                {
                    WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "No Update available");
                }
            }
            catch (WebException wex)
            {
                WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Error on checking for update!");
                Console.Error.WriteLine(wex.ToString());
            }
        }

        public Boolean CheckArg(String type, String data)
        {
            Boolean valid = false;
            switch (type)
            {
                case "Language":
                    for (int i = 0; i < Languages.GetLength(0); i++)
                    {
                        if (data.Equals(Languages[i, 1]))
                        {
                            valid = true;
                            break;
                        }
                    }
                    break;
                case "Winapp2":
                    for (int i = 0; i < Winapp2Options.Length; i++)
                    {
                        if (data.Equals(Winapp2Options[i]))
                        {
                            valid = true;
                            break;
                        }
                    }
                    break;
                case "Service":
                    for (int i = 0; i < ServiceOptions.Length; i++)
                    {
                        if (data.Equals(ServiceOptions[i]))
                        {
                            valid = true;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
            return valid;
        }

        public String getUsage() => USAGE;

        public String getTitle() => title;

        public String getLangList()
        {
            String List = Languages[0, 0] + ":" + Languages[0, 1] + "\t\t";
            for (int i = 1; i < Languages.GetLength(0); i++)
            {
                if (i % 2 == 0)
                {
                    List = List + "\n";
                }
                List = List + Languages[i, 0] + ":" + Languages[i, 1] + "\t\t";
            }
            return List;
        }

        public String getCommonDir() => CommonDirectory;

        private void getCurrentVersionFromExe(string path)
        {
            try
            {
                CurrentVersion = FileVersionInfo.GetVersionInfo(path + "CCleaner.exe").FileVersion.Replace("00", "").Replace(" ", "").Replace(",", ".").Replace("..", ".");
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException)
                {
                    WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Can't find CCleaner's exe inside \"" + path + "\"");
                }
                else
                {
                    WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Error retriving local version!");
                }
                Console.Error.WriteLine(ex.ToString());
                Exit();
            }
        }

        private void getLatestVersionOnline()
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string html = webby.DownloadString(Links[0]);
                int start = html.IndexOf(HTMLPrefix) + HTMLPrefix.Length;
                int end = html.IndexOf(HTMLSuffix) - start;
                OnlineVersion = html.Substring(start, end);
            }
            catch (Exception ex)
            {
                WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Error retriving the latest version online!");
                Console.Error.WriteLine(ex.ToString());
                Exit();
            }
        }

        public void WriteLineColored(ConsoleColor Background, ConsoleColor FontColor, String Text)
        {
            Console.BackgroundColor = Background;
            Console.ForegroundColor = FontColor;
            Console.Out.WriteLine(" " + Text + " ");
            Console.ResetColor();
        }

        public void Exit()
        {
            Console.Out.WriteLine("Press Any Key to Exit...");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
