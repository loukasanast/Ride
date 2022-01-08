using System.Collections.Generic;

class Route
{
    public string Name { get; set; }
    public List<string> Instructions { get; set; }

    public Route(string name)
    {
        Name = name;
    }
}
