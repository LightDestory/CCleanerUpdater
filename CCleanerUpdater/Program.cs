using System;
namespace CCleanerUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            Updater tool = new Updater();
            tool.CheckForUpdate(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            if (args.Length > 0 && (args[0] == "/help" || args[0] == "/h"))
            {
                Console.Out.WriteLine(tool.getUsage());
                Console.Out.WriteLine("Languages:\n" +tool.getLangList());
            }
            else if (args.Length > 0 && (!args[0].Contains("path=")) && (!args[1].Contains("lang=")))
            {
                Console.Out.WriteLine("Wrong arguments!, try CCleanerUpdater.exe /help");
            }
            else
            {
                if (args.Length == 0)
                {
                    //INSTALL CCLEANER
                    Console.Out.WriteLine("Opening this tool without arguments will start a installation wizard.\nIf you want to install CCleaner say 'y'\n" +
                        "If you want to update CCleaner, close the tool and start again with argument '/help' ('n' to close):");
                    String answer;
                    do
                    {
                         answer = Console.ReadLine().ToLower();
                    } while (!(answer.Equals("y") || answer.Equals("n")));

                    if (answer.Equals("y"))
                    {
                        if (!tool.CheckInstall())
                        {
                            Console.Out.WriteLine("Choose a Language (Insert ID):");
                            Console.Out.WriteLine(tool.getLangList());
                            String lang;
                            do
                            {
                                Console.Out.Write("ID:");
                                lang = Console.ReadLine();
                            } while (!tool.CheckLang(lang));
                            tool.Job("", lang);
                        }
                        else
                        {
                            Console.Out.WriteLine("CCleaner is already installed!\nPlease run this tool as 'updater'!");
                        }
                        tool.Exit(true);
                    }
                    else
                    {
                        tool.Exit(true);
                    }
                }
                else
                {
                    //UPDATE CCLEANER
                    tool.Job(args[0].Replace("path=", ""), args[1].Replace("lang=", ""));
                    tool.Exit(false);
                }
            }
        }
    }
}
