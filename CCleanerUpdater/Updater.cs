using System;
using System.Collections.Generic;
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
        
        private readonly Dictionary<string,string> Links = new Dictionary<string,string>()
            {
                {"Version", "https://raw.githubusercontent.com/LightDestory/CCleanerUpdater/master/version.txt" },
                {"Website", "http://lightdestoryweb.altervista.org/" }
            };
        private readonly String[] ServiceOptions =
        {
            "none", "install"
        };
        private WebClient webby;
        private String OnlineVersion;
        private String CurrentVersion;

        public Updater()
        {
            CurrentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            webby = new WebClient();
        }

        public void Job(String InstallDir, String Lang, String Winapp2, String Service)
        {
            getLatestVersionOnline();
            if (InstallDir.Equals("newinstall"))
            {
                Download(CCleaner.Links["Download"] + OnlineVersion.Substring(0, 4).Replace(".", "") + ".exe", Path.GetTempPath() + "\\" + CCleaner.FileName, "CCleaner");
                Install(Lang);
            }
            else
            {
                if (InstallDir.Equals("common"))
                {
                    InstallDir = CCleaner.CommonDirectory;
                }
                else if (InstallDir.ToCharArray()[InstallDir.Length - 1] != '/')
                {
                    InstallDir = InstallDir + "\\";
                }
                getCurrentVersionFromExe(InstallDir);
                if (isUpdateAvailable(CurrentVersion, OnlineVersion))
                {
                    Download(CCleaner.Links["Download"] + OnlineVersion.Substring(0, 4).Replace(".", "") + ".exe", Path.GetTempPath() + "\\" + CCleaner.FileName, "CCleaner");
                    Install(Lang);
                }
                else
                {
                    Console.WriteLine("You are using the latest version!");
                }
            }
            SetupWinapp2(CCleaner.CommonDirectory, Winapp2);
            SetUpService(CCleaner.CommonDirectory, Lang, Winapp2, Service);
            WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "Done!");
        }

        private Boolean isUpdateAvailable(String local, String online)
        {
            Console.WriteLine("Installed Version: {0}\nOnline Version: {1}", local, online);
            return (!local.Equals(online));
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
                Exit(10);
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
                var Installation = Process.Start(Path.GetTempPath() + "\\" + CCleaner.FileName, "/S /L=" + Lang);
                Installation.WaitForExit();
                File.Delete(Path.GetTempPath() + "\\" + CCleaner.FileName);
                WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "Install completed!");
            }
            catch (Exception ex)
            {
                WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Error on installing...");
                Console.Error.WriteLine(ex.ToString());
                Exit(10);
            }
        }

        private void SetupWinapp2(String path, String Option)
        {
            try
            {
                if (Option.Contains("download"))
                {
                    if (!CheckExist(path, "Winapp2.ini"))
                    {
                        Download(Winapp2.Links["File"], path + "Winapp2.ini", "Winapp2");
                        if (Option.Contains("trim"))
                        {
                            TrimWinapp2(path);
                        }
                    }
                    else
                    {
                        OnlineVersion = new StringReader(webby.DownloadString(Winapp2.Links["File"])).ReadLine().Replace("; Version: ", "");
                        CurrentVersion = new StringReader(File.ReadAllText(path + "Winapp2.ini")).ReadLine().Replace("; Version: ", "");
                        if (isUpdateAvailable(CurrentVersion, OnlineVersion))
                        {
                            Download(Winapp2.Links["File"], path + "Winapp2.ini", "Winapp2");
                            if (Option.Contains("trim"))
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
                Exit(10);
            }
        }

        private void TrimWinapp2(String path)
        {
            try
            {
                if (!CheckExist(path, "Trimmer.bat"))
                {
                    Download(Winapp2.Links["Trimmer"], path + "Trimmer.bat", "Trimmer Script");
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
                Exit(10);
            }
        }

        public void SetUpService(String path, string lang, string winapp2, string action)
        {
            if (action.Equals("install"))
            {
                if (Environment.OSVersion.Version.Major == 5)
                {
                    WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Windows XP isn't supported for daily Service Setup!");
                }
                else
                {
                    Console.Out.Write("Setting up Daily Service...");
                    String Path2 = Environment.CurrentDirectory + "\\" + AppDomain.CurrentDomain.FriendlyName;
                    String Arg = "path=\"" + path + "\\\"" + " lang=\"" + lang + "\"" + " winapp2=\"" + winapp2 + "\"" + " service=\"None\"";
                    File.WriteAllText("Service.xml", Properties.Resources.CCleanerUpdaterService.ToString().Replace("%PATH_TO_TOOL%", Path2).Replace("%ARG_TO_USE%", Arg));
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
                OnlineVersion = webby.DownloadString(Links["Version"]);
                if (!OnlineVersion.Equals(CurrentVersion))
                {
                    WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "New Update available! Opening my website...");
                    Process.Start(Links["Website"]);
                    Exit(0);
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
                Exit(10);
            }
        }

        public Boolean CheckArg(String type, String data)
        {
            Boolean valid = false;
            switch (type)
            {
                case "Language":
                    foreach(KeyValuePair<string, string> d in CCleaner.Languages)
                    {
                        if(data.Equals(d.Value))
                        {
                            valid = true;
                            break;
                        }
                    }
                    break;
                case "Winapp2":
                    for (int i = 0; i < Winapp2.Winapp2Options.Length; i++)
                    {
                        if (data.Equals(Winapp2.Winapp2Options[i]))
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
            String List = "";
            foreach(KeyValuePair<string,string> d in CCleaner.Languages)
            {
                List += d.Key + ":" + d.Value + "\n";
            }
            return List;
        }

        private void getCurrentVersionFromExe(string path)
        {
            try
            {
                CurrentVersion = FileVersionInfo.GetVersionInfo(path + "CCleaner.exe").FileVersion.Substring(0, 4).Replace(",", ".");
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
                Exit(10);
            }
        }

        private void getLatestVersionOnline()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                string html = webby.DownloadString(CCleaner.Links["Version"]);
                int start = html.IndexOf(CCleaner.HTMLPrefix) + CCleaner.HTMLPrefix.Length;
                int end = html.IndexOf(CCleaner.HTMLSuffix) - start;
                OnlineVersion = html.Substring(start, end).Substring(0,4);
            }
            catch (Exception ex)
            {
                WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Error retriving the latest version online!");
                Console.Error.WriteLine(ex.ToString());
                Exit(10);
            }
        }

        public void WriteLineColored(ConsoleColor Background, ConsoleColor FontColor, String Text)
        {
            Console.BackgroundColor = Background;
            Console.ForegroundColor = FontColor;
            Console.Out.WriteLine(" " + Text + " ");
            Console.ResetColor();
        }

        public void Exit(int mode)
        {
            Console.Out.WriteLine("Press Any Key to Exit...");
            Console.ReadKey();
            Environment.Exit(mode);
        }
    }
}
