using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace CCleanerUpdater
{
    class Updater
    {
        private const String USAGE = "Usage:\nCCleanerUpdater.exe path=\"[CCleaner's Install Dir]\" lang=\"[Language]\"\n" +
                    "Common Install Dir: \"%ProgramFiles%\\CCleaner\"\n"+
                    "Example: CCleanerUpdater.exe path=\"%ProgramFiles%\\CCleaner\" lang=\"1040\"";
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
                "https://forums.mydigitallife.net/threads/1-0-ccleaner-updater.74503/", "https://github.com/LightDestory/CCleanerUpdater", "http://lightdestoryweb.altervista.org/"
            };
        private WebClient webby;
        private const String HTMLPrefix ="<p><strong>v";
        private const String HTMLSuffix = "</strong> (";
        private const String FileName = "CCleanerUpdate.exe";
        private String OnlineVersion;
        private String CurrentVersion;

        public Updater()
        {
            webby = new WebClient();
        }

        public void Job(String InstallDir, String Lang)
        {
            if (InstallDir.Equals(""))
            {
                getLatestVersionOnline();
                Download();
                Install(true, Lang);
            }
            else
            {
                getCurrentVersionFromExe(InstallDir);
                getLatestVersionOnline();
                if (Compare())
                {
                    Download();
                    Install(false, Lang);
                }
                else
                {
                    Console.WriteLine("You are using the latest version!");
                }
                
            }
        }

        private void getCurrentVersionFromExe(string path)
        {
            try
            {
                CurrentVersion = FileVersionInfo.GetVersionInfo(path + "\\CCleaner.exe").FileVersion.Replace("00", "").Replace(" ", "").Replace(",",".").Replace("..",".");
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine("Can't find CCleaner's exe inside \"{0}\"...", path);
                Exit(true);
            }
        }

        private void getLatestVersionOnline()
        {
            try
            {
                string html = webby.DownloadString(Links[0]);
                int start = html.IndexOf(HTMLPrefix) + HTMLPrefix.Length;
                int end = html.IndexOf(HTMLSuffix) - start;
                OnlineVersion = html.Substring(start, end);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unable to access Internet!\nError:\n{0}", ex.ToString());
                Exit(true);
            }
        }

        private Boolean Compare()
        {
            Boolean Update = false;
            Console.WriteLine("Installed Version: {0}\nOnline Version: {1}", CurrentVersion, OnlineVersion);
            if (!CurrentVersion.Equals(OnlineVersion))
            {
                Update = true;
            }
            return Update;
        }

        private void Download()
        {
            try
            {
                Console.Out.WriteLine("Downloading latest version from Piriform...");
                webby.DownloadFile(Links[1] + OnlineVersion.Substring(0,4).Replace(".","") + ".exe", "\\" + FileName);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                Exit(true);
            }
        }

        public void CheckForUpdate(String currver)
        {
            Console.Out.WriteLine(currver);
            Console.Out.WriteLine("Checking for Tool's Update...");
            String ver = "";
            try
            {
                ver = webby.DownloadString(Links[2]);
                if (!ver.Equals(currver))
                {
                    Console.Out.WriteLine("New Update avaible! Opening forum's thread...");
                    Process.Start(Links[3]);
                }
                else
                {
                    Console.Out.WriteLine("No Update avaible...");
                }
            }
            catch (WebException wex)
            {
                Console.Error.WriteLine("Error: {0}", wex);
            }
        }

        private void Install(Boolean visual, String Lang)
        {
            Console.Out.WriteLine("Installing...");
            try
            {
                var Installation = Process.Start("C://" + FileName, "/S /L=" + Lang);
                Installation.WaitForExit();
                File.Delete("C://" + FileName);
                if (visual)
                {
                    Console.Out.WriteLine("Done!");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: {0}", ex.ToString());
            }
        }

        public String getUsage()
        {
            return USAGE;
        }

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

        public Boolean CheckLang(String lang)
        {
            Boolean valid = false;
            for (int i = 0; i < Languages.GetLength(0); i++)
            {
                if (lang.Equals(Languages[i, 1]))
                {
                    valid = true;
                    break;
                }
            }
            return valid;
        }

        public void Exit(Boolean visual)
        {
            if (visual)
            {
                Console.Out.WriteLine("Press Any Key to Exit...");
                Console.ReadKey();
            }
        }

        public Boolean CheckInstall()
        {
            Boolean installed = false;
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\CCleaner\\CCleaner.exe"))
            {
                installed = true;
            }
            return installed;
        }
    }
}
