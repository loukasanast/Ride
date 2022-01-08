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
                ErrorHandler.HandleInvalidInput();
            }

            Location fromLoc = new LocationParser().ParseLocation(tempFromLoc);

            Console.WriteLine("Please, enter your destination location:");

            string tempToLoc = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(tempToLoc))
            {
                ErrorHandler.HandleInvalidInput();
            }

            Location toLoc = new LocationParser().ParseLocation(tempToLoc);

            Route route = new RouteParser().ParseRoute(fromLoc, toLoc);

            try
            {
                WriteFile(route.Instructions);
            }
            catch(IOException e)
            {
                ErrorHandler.HandleError(e.Message);
            }

            try
            {
                System.Diagnostics.Process.Start("notepad.exe", "instructions.txt");
            }
            catch (Exception e)
            {
                ErrorHandler.HandleError(e.Message);
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