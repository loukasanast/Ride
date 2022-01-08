using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Net;

namespace Ride
{
    class LocationParser : ILocationParser<string>
    {
        private readonly WebClient _client = new WebClient();
        public Location ParseLocation(string name)
        {
            var result = new Location(name);
            Uri toUri = new Uri($"https://graphhopper.com/api/1/geocode?q={result.Name}&debug=true&key=744a2a39-b0a5-4aca-81d8-9c8afd078b79");

            try
            {
                dynamic toJson = JObject.Parse(_client.DownloadString(toUri));

                result.Lat = ParseLat((string)toJson["hits"][0]["point"]["lat"]);
                result.Lng = ParseLng((string)toJson["hits"][0]["point"]["lng"]);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured.");
                Console.WriteLine(e.Message);
                Environment.Exit(1);
            }

            return result;
        }

        public decimal ParseLat(string lat)
        {
            return Decimal.Parse(lat, NumberStyles.Float, CultureInfo.InvariantCulture);
        }

        public decimal ParseLng(string lng)
        {
            return Decimal.Parse(lng, NumberStyles.Float, CultureInfo.InvariantCulture);
        }
    }
}
