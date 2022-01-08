using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace Ride
{
    class Program
    {
        static WebClient client = new WebClient();

        static void Main(string[] args)
        {
            Console.WriteLine("########## Receive directions between two locations ##########");
            Console.WriteLine("Please, enter your start location:");

            string tempFromLoc = Console.ReadLine().Trim();

            if(string.IsNullOrEmpty(tempFromLoc))
            {
                Console.WriteLine("You entered an invalid value.");
                return;
            }

            Location fromLoc = new Location(tempFromLoc);
            Uri fromUri = new Uri($"https://graphhopper.com/api/1/geocode?q={fromLoc.Name}&debug=true&key=744a2a39-b0a5-4aca-81d8-9c8afd078b79");

            try
            {
                dynamic fromJson = JObject.Parse(client.DownloadString(fromUri));

                fromLoc.Lat = ParseLat((string)fromJson["hits"][0]["point"]["lat"]);
                fromLoc.Lng = ParseLng((string)fromJson["hits"][0]["point"]["lng"]);
            }
            catch(Exception e)
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("Please, enter your destination location:");

            string tempToLoc = Console.ReadLine().Trim();

            if (string.IsNullOrEmpty(tempToLoc))
            {
                Console.WriteLine("You entered an invalid value.");
                return;
            }

            Location toLoc = new Location(tempToLoc);
            Uri toUri = new Uri($"https://graphhopper.com/api/1/geocode?q={toLoc.Name}&debug=true&key=744a2a39-b0a5-4aca-81d8-9c8afd078b79");

            try
            {
                dynamic toJson = JObject.Parse(client.DownloadString(toUri));

                toLoc.Lat = ParseLat((string)toJson["hits"][0]["point"]["lat"]);
                toLoc.Lng = ParseLng((string)toJson["hits"][0]["point"]["lng"]);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine(e.Message);
                return;
            }

            Route route = new Route($"{fromLoc.Name} to {toLoc.Name}");

            Uri routeUri = new Uri(FormattableString.Invariant($"https://graphhopper.com/api/1/route?point={fromLoc.Lat}%2C{fromLoc.Lng}&point={toLoc.Lat}%2C{toLoc.Lng}&vehicle=car&locale=de&debug=true&points_encoded=false&key=744a2a39-b0a5-4aca-81d8-9c8afd078b79"));

            try
            {
                dynamic routeJson = JObject.Parse(client.DownloadString(routeUri));

                JArray tempRouteArray = routeJson["paths"][0]["instructions"];
                route.Instructions = tempRouteArray.Select(x => (string)x["text"]).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine(e.Message);
                return;
            }

            try
            {
                WriteFile(route.Instructions);
            }
            catch(IOException e)
            {
                Console.WriteLine("An error occured");
                Console.WriteLine(e.Message);
                return;
            }

            try
            {
                System.Diagnostics.Process.Start("notepad.exe", "instructions.txt");
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine(e.Message);
                return;
            }

            Console.WriteLine("########## Thank you for using this program ##########");
        }

        static decimal ParseLat(string lat)
        {
            return Decimal.Parse(lat, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        static decimal ParseLng(string lng)
        {
            return Decimal.Parse(lng, NumberStyles.Float, CultureInfo.InvariantCulture);
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
