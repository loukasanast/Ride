namespace Ride
{
    interface IRouteParser<T>
    {
        Route ParseRoute(T from, T to);
    }
}