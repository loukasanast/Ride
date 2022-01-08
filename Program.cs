using System;
using System.Collections.Generic;
using System.IO;

namespace Ride
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("########## Receive directions between two locations ##########");
            Console.WriteLine("Please, enter your start location:");

            string tempFromLoc = Console.ReadLine().Trim();

            if(string.IsNullOrEmpty(tempFromLoc))
            {
                Console.WriteLine("You entered an invalid value.");
                Console.WriteLine("Press enter to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            Location fromLoc = new LocationParser().ParseLocation(tempFromLoc);

            Console.WriteLine("Please, enter your destination location:");

            string tempToLoc = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(tempToLoc))
            {
                Console.WriteLine("You entered an invalid value.");
                Console.WriteLine("Press enter to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            Location toLoc = new LocationParser().ParseLocation(tempToLoc);

            Route route = new RouteParser().ParseRoute(fromLoc, toLoc);

            try
            {
                WriteFile(route.Instructions);
            }
            catch(IOException e)
            {
                Console.WriteLine("An error occured");
                Console.WriteLine(e.Message);
                Console.WriteLine("Press enter to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            try
            {
                System.Diagnostics.Process.Start("notepad.exe", "instructions.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine(e.Message);
                Console.WriteLine("Press enter to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }

            Console.WriteLine("########## Thank you for using this program ##########");
            Console.WriteLine("Press enter to exit...");
            Console.ReadKey();
        }

        static void WriteFile(List<string> instructions)
        {
            using (FileStream file = File.Open("instructions.txt", FileMode.Create))
            using (StreamWriter writer = new StreamWriter(file))
            {
                for (int i = 0; i < instructions.Count; i++)
                {
                    if (instructions[i].ToLower().Contains("links"))
                    {
                        instructions[i] += " ←";
                    }
                    else if (instructions[i].ToLower().Contains("rechts"))
                    {
                        instructions[i] += " →";
                    }

                    writer.WriteLine(instructions[i]);
                }
            }
        }
    }
}