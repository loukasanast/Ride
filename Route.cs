using System.Collections.Generic;

namespace Ride
{
    class Route
    {
        public string Name { get; set; }
        public List<string> Instructions { get; set; }

        public Route(string name)
        {
            Name = name;
        }
    }
}
