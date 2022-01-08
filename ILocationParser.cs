namespace Ride
{
    interface ILocationParser<T>
    {
        Location ParseLocation(T name);
        decimal ParseLat(T lat);
        decimal ParseLng(T lng);
    }
}
