using System.Collections.Generic;
using System;
namespace CCleanerUpdater
{
    static class Winapp2
    {
        public static readonly Dictionary<string,string> Links = new Dictionary<string, string>()
            {
                {"Trimmer", "http://www.winapp2.com/trim.bat"},
                {"File", "https://raw.githubusercontent.com/MoscaDotTo/Winapp2/master/Winapp2.ini"}
            };

        public static readonly String[] Winapp2Options =
            {
            "none", "download", "downloadtrim"
            };
    }
}
