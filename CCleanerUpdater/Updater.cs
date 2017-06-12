using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace CCleanerUpdater
{
    class Updater
    {
        private String OnlineVersion;
        private String CurrentVersion;
        private const String CcleanerWeb = "https://www.piriform.com/ccleaner/download";
        private const String DownloadLink = "http://download.piriform.com/ccsetup";
        private WebClient webby;
        private String HTMLPrefix ="<p><strong>v";
        private String HTMLSuffix = "</strong> (";
        private Boolean Done;

        public Updater()
        {
            webby = new WebClient();
        }

        public Boolean getCurrentVersionFromExe(string path)
        {
            Done = false;
            try
            {
                CurrentVersion = FileVersionInfo.GetVersionInfo(path + "\\CCleaner.exe").FileVersion.Replace("00", "").Replace(" ", "").Replace(",",".").Replace("..",".");
                Done = true;
            }
            catch (FileNotFoundException)
            {
                Console.Error.WriteLine("Can't find CCleaner's exe inside \"{0}\"...", path);
            }
            return Done;
        }

        public Boolean getLatestVersionOnline()
        {
            Done = false;
            try
            {
                string html = webby.DownloadString(CcleanerWeb);
                int start = html.IndexOf(HTMLPrefix) + HTMLPrefix.Length;
                int end = html.IndexOf(HTMLSuffix) - start;
                OnlineVersion = html.Substring(start, end);
                Done = true;
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Unable to access Internet!");
            }
            return Done;
        }

        public Boolean Compare()
        {
            Done = false;
            Console.WriteLine("Installed Version: {0}\nOnline Version: {1}", CurrentVersion, OnlineVersion);
            if (!CurrentVersion.Equals(OnlineVersion))
            {
                Done = true;
            }
            return Done;
        }

        public void Download()
        {
            try
            {
                webby.DownloadFile(DownloadLink + OnlineVersion.Substring(0,4).Replace(".","") + ".exe", "\\update.exe");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
            }
        }

        public void Install()
        {
            var x = Process.Start("C://update.exe", "/S");
            x.WaitForExit();
            Console.Out.WriteLine("Done!");
            File.Delete("C://update.exe");
        }
    }
}
