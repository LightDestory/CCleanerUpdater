using System;
namespace CCleanerUpdater
{
    class Program
    {
        private static String action;
        static void Main(string[] args)
        {
            Updater Tool = new Updater();
            //Title
            Tool.WriteLineColored(ConsoleColor.Blue, ConsoleColor.Red, Tool.getTitle());
            //Checking Update
            Tool.CheckForUpdate();
            //Checking Status
            if (args.Length != 0 && (args[0].Equals("/help") || args[0].Equals("/h")))
            {
                //Requesting Help
                Console.Out.WriteLine(Tool.getUsage());
                Console.Out.WriteLine("\nLanguages:\n\n" +Tool.getLangList());
            }
            else if ((args.Length !=0 && args.Length!=3) || (args.Length == 3 && (!args[0].Contains("path=") || !args[1].Contains("lang=") || !args[2].Contains("winapp2="))))
            {
                //Arguments Missing
                Tool.WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Wrong arguments!, try CCleanerUpdater.exe /help");
            }
            else
            {
                if (args.Length == 0)
                {
                    //INSTALL CCLEANER
                    Console.Out.WriteLine("Opening this tool without arguments will start a installation wizard:\nIf you want to install CCleaner say 'y'\n" +
                        "If you want to update CCleaner, check argument '/help' ('n' to close):");
                    RequestInput();
                    if (action.Equals("y"))
                    {
                        if (!Tool.CheckExist(Tool.getCommonDir(), "CCleaner.exe"))
                        {
                            Console.Out.WriteLine("Choose a Language (Insert ID):");
                            Console.Out.WriteLine(Tool.getLangList());
                            String lang;
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
                                    action = "DownloadTrim";
                                }
                                else
                                {
                                    action = "Download";
                                }
                            }
                            else
                            {
                                action = "None";
                            }
                            Tool.Job("newinstall", lang, action);
                        }
                        else
                        {
                            Tool.WriteLineColored(ConsoleColor.Green, ConsoleColor.Blue, "CCleaner is already installed!\n Please run this tool as 'updater'!");
                        }
                        Tool.Exit();
                    }
                    else
                    {
                        Tool.Exit();
                    }
                }
                else
                {
                    //UPDATE CCLEANER
                    if (Tool.CheckArg("Language", args[1].Replace("lang=", "")) && Tool.CheckArg("Winapp2", args[2].Replace("winapp2=", "").ToLower()))
                    {
                        Tool.Job(args[0].Replace("path=", ""), args[1].Replace("lang=", ""), args[2].Replace("winapp2=", ""));
                    }
                    else
                    {
                        Tool.WriteLineColored(ConsoleColor.Red, ConsoleColor.Black, "Invalid Arguments. Try CCleanerUpdater.exe /h");
                        Tool.Exit();
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
    }
}
