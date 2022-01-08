namespace Ride
{
    class Location
    {
        public string Name { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }

        public Location(string name)
        {
            Name = name;
        }
    }
}
