using System;
namespace CCleanerUpdater
{
    class Program
    {
        static String lang, winapp2, action;
        static Updater Tool;
        static void Main(string[] args)
        {
            Tool = new Updater();
           //Title
            Console.Title = "CCleanerUpdater by LightDestory";
            Tool.WriteLineColored(ConsoleColor.Blue, ConsoleColor.Red, Tool.getTitle());
            //Checking Update
            Tool.CheckForUpdate();
            //Checking Status
            if (args.Length == 1 && (args[0].Equals("/help") || args[0].Equals("/h")))
            {
                //Requesting Help
                Console.Out.WriteLine(Tool.getUsage());
                Console.Out.WriteLine("\nLanguages:\n\n" + Tool.getLangList());
            }
            else if ((args.Length != 0 && args.Length!= 4) || (args.Length == 4 && (!args[0].Contains("path=") || !args[1].Contains("lang=") || !args[2].Contains("winapp2=") || !args[3].Contains("service="))))
            {
                //Arguments Missing
                Tool.WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Wrong arguments!, try CCleanerUpdater.exe /help");
                Tool.Exit(0);
            }
            else
            {
                if (args.Length == 0)
                {
                    //INSTALL CCLEANER
                    Boolean isInstalled = Tool.CheckExist(CCleaner.CommonDirectory, "CCleaner.exe");
                    Console.Out.WriteLine("Opening this tool without arguments will start a installation wizard:\nIf you want to install CCleaner say 'y'\n" +
                        "If you want to update CCleaner and set-up the daily service, check argument '/help' ('n' to close):");
                    RequestInput();
                    if (action.Equals("y") && isInstalled == false)
                    {
                        PerformInstall();
                    }
                    else
                    {
                        if (isInstalled)
                        {
                            Tool.WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "CCleaner is already installed. Please run this tool as 'updater'!");
                        }
                        Tool.Exit(0);
                    }
                }
                else
                {
                    //UPDATE CCLEANER
                    lang = args[1].Replace("lang=", "");
                    winapp2 = args[2].Replace("winapp2=", "").ToLower();
                    action = args[3].Replace("service=", "").ToLower();
                    if (Tool.CheckArg("Language", lang) && Tool.CheckArg("Winapp2", winapp2) && Tool.CheckArg("Service", action))
                    {
                        Tool.Job(args[0].Replace("path=", ""), lang, winapp2, action);
                        Environment.Exit(0);
                    }
                    else
                    {
                        Tool.WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Invalid Arguments. Try CCleanerUpdater.exe /h");
                        Tool.Exit(10);
                    }
                }
            }
        }

        static void RequestInput()
        {
            do
            {
                Console.Out.WriteLine("What do you want to do?");
                action = Console.ReadLine().ToLower();
            } while (!(action.Equals("y") || action.Equals("n")));
        }

        static void PerformInstall()
        {
            Console.Out.WriteLine("Choose a Language (Insert ID):");
            Console.Out.WriteLine(Tool.getLangList());
            do
            {
                Console.Out.Write("ID:");
                lang = Console.ReadLine();
            } while (!Tool.CheckArg("Language", lang));
            Console.Out.WriteLine("Do you want to install Winapp2? (y/n)");
            RequestInput();
            if (action.Equals("y"))
            {
                Console.Out.WriteLine("Do you want to Trim Winapp2? (y/n)");
                RequestInput();
                if (action.Equals("y"))
                {
                    winapp2 = "downloadtrim";
                }
                else
                {
                    winapp2 = "download";
                }
            }
            else
            {
                winapp2 = "none";
            }
            Console.Out.WriteLine("Do you want to set-up on startup service? (y/n)");
            RequestInput();
            if (action.Equals("y"))
            {
                action = "install";
            }
            else
            {
                action = "none";
            }
                Tool.Job("newinstall", lang, winapp2, action);
        }
    }
}
