using System;

namespace Ride
{
    class ErrorHandler
    {
        public static void HandleError(string message)
        {
            Console.WriteLine("An error occured");
            Console.WriteLine(message);
            Console.WriteLine("Press enter to exit...");
            Console.ReadKey();
            Environment.Exit(1);
        }

        public static void HandleInvalidInput()
        {
            Console.WriteLine("You entered an invalid value.");
            Console.WriteLine("Press enter to exit...");
            Console.ReadKey();
            Environment.Exit(1);
        }
    }
}