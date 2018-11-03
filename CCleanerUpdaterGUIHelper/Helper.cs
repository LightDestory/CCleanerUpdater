using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace CCleanerUpdaterGUIHelper
{
    public partial class Helper : Form
    {
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

        private readonly String[,] Winapp2Options =
        {
            {"No", "none" }, {"Download Only", "download" }, {"Download and Trim", "downloadtrim" }
        };

        private readonly String[,] ServiceOptions =
        {
            {"No", "none" }, {"Install Startup Service", "install" }
        };

        public Helper()
        {
            InitializeComponent();
            Init();
           
            string x64DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "\\CCleaner";
            string x86DefaultPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) + "\\CCleaner";
            if (System.IO.Directory.Exists(x64DefaultPath)) {
                CCPath.Text = x64DefaultPath;
            }
            else if (System.IO.Directory.Exists(x86DefaultPath)) {
                CCPath.Text = x86DefaultPath;
            }
        }

        private void Init()
        {
            FillItems(Lang, Languages);
            FillItems(WinApp2, Winapp2Options);
            FillItems(Service, ServiceOptions);
        }

        private void FillItems(ComboBox Bucket, String[,] Data)
        {
            for(int i = 0; i<Data.GetLength(0); i++)
            {
                Bucket.Items.Add(Data[i,0]);
            }
            Bucket.SelectedIndex= 0;
        }

        private void CCPathSelect_Click(object sender, EventArgs e)
        {
            if(Selector.ShowDialog() != DialogResult.Cancel)
            {
                CCPath.Text = Selector.SelectedPath;
            }
        }

        private void HelpMe_Click(object sender, EventArgs e)
        {
            if(CCPath.Text != "")
            {
                try
                {
                    Process.Start("CCleanerUpdater.exe", "path=\"" + CCPath.Text + "\" lang=\"" + Languages[Lang.SelectedIndex, 1] + "\" winapp2=\"" + Winapp2Options[WinApp2.SelectedIndex, 1] + "\" service=\"" + ServiceOptions[Service.SelectedIndex, 1] + "\"");
                }
                catch(Exception)
                {
                    MessageBox.Show("CCleanerUpdater.exe not found!\nBe sure to don't rename the tool.\nBe sure to place this .exe on the same folder of tool!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Select CCleaner's Path!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
