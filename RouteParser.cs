using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net;

namespace Ride
{
    class RouteParser : IRouteParser<Location>
    {
        private readonly WebClient _client = new WebClient();

        public Route ParseRoute(Location from, Location to)
        {
            var result = new Route($"{from.Name} to {to.Name}");

            Uri routeUri = new Uri(FormattableString.Invariant($"https://graphhopper.com/api/1/route?point={from.Lat}%2C{from.Lng}&point={to.Lat}%2C{to.Lng}&vehicle=car&locale=de&debug=true&points_encoded=false&key=744a2a39-b0a5-4aca-81d8-9c8afd078b79"));

            try
            {
                dynamic routeJson = JObject.Parse(_client.DownloadString(routeUri));

                JArray tempRouteArray = routeJson["paths"][0]["instructions"];
                result.Instructions = tempRouteArray.Select(x => (string)x["text"]).ToList();
            }
            catch (Exception e)
            {
                ErrorHandler.HandleError(e.Message);
            }

            return result;
        }
    }
}