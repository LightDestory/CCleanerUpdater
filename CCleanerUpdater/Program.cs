using System;
namespace CCleanerUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length==0 || args.Length > 1)
            {
                Console.Out.WriteLine("Wrong arguments!\nUsage: CcleanerUpdater.exe [\"CCleaner's Path\"]");
            }
            else
            {
                Updater program = new Updater();
                Console.Out.WriteLine("Getting installed version...");
                if (program.getCurrentVersionFromExe(args[0]))
                {
                    Console.Out.WriteLine("Getting online version...");
                    if (program.getLatestVersionOnline())
                    {
                        Console.Out.WriteLine("Comparing Version...");
                        if (program.Compare())
                        {
                            Console.Out.WriteLine("New update found!\nDownloading update...");
                            program.Download();
                            Console.Out.WriteLine("Installing update...");
                            program.Install();
                            Exit();
                        }
                        else
                        {
                            Console.Out.WriteLine("You are using the latest Version!");
                            Exit();
                        }
                    }
                    else
                    {
                        Exit();
                    }
                }
                else
                {
                    Exit();
                }

            }
        }

        private static void Exit()
        {
            Console.Out.WriteLine("Press Any Key to Exit...");
            Console.ReadKey();
        }
    }
}
