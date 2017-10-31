using System;
using System.Collections.Generic;

namespace CCleanerUpdater
{
    static class CCleaner
    {
        public static readonly Dictionary<string,string> Languages = new Dictionary<string,string>()
            {
            {"Albanian","1052"},{"Arabic","1025"},{"Armenian","1067"},{"Azeri Latin","1068"},{"Belarusian","1059"},{"Bosnian","5146"},{"Portuguese(Brazil)","1046"},
            {"Bulgarian","1026"},{"Catalan","1027"},{"Chinese-Simplified","2052"},{"Chinese-Traditional","1028"},{"Croatian"," 1050"},{"Czech","1029"},
            {"Danish","1030"},{"Dutch","1043"},{"English","1033"},{"Estonian","1061"},{"Farsi","1065"},{"Finnish","1035"},{"French","1036"},{"Galician","1110"},
            {"Georgian","1079"},{"German","1031"},{"Greek","1032"},{"Hebrew","1037"},{"Hungarian","1038"},{"Italian","1040"},{"Japanese","1041"},{"Kazakh","1087"},
            {"Korean","1042"},{"Lithuanian","1063"},{"Macedonian","1071"},{"Norwegian","1044"},{"Polish","1045"},{"Portuguese","2070"},{"Romanian","1048"},
            {"Russian","1049"},{"Serbian-Cyrillic","3098"},{"Serbian-Latin","2074"},{"Slovak","1051"},{"Slovenian","1060"},{"Spanish","1034"},
            {"Swedish","1053"},{"Turkish","1055"},{"Ukrainian","1058"},{"Vietnamese","1066"}
        };

        public static readonly Dictionary<string, string> Links = new Dictionary<string, string>()
            {
                {"Version", "https://www.piriform.com/ccleaner/download"},{"Download", "http://download.piriform.com/ccsetup"}
            };

        public const String HTMLPrefix = "<p><strong>v";
        public const String HTMLSuffix = "</strong> (";
        public const String FileName = "CCleanerUpdate.exe";
        public static readonly String CommonDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\CCleaner\";
    }
}
