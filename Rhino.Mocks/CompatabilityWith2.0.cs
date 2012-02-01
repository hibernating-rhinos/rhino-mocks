namespace Rhino.Mocks
{
    /// <summary>
    /// This delegate is compatible with the System.Func{T,R} signature
    /// We have to define our own to get compatability with 2.0
    /// </summary>
    public delegate R Function<T, R>(T t);
}