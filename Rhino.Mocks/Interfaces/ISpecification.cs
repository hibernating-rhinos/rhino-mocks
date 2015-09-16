namespace Rhino.Mocks.Impl.Invocation.Specifications
{
    ///<summary>
    ///</summary>
    public interface ISpecification<T>
    {
        ///<summary>
        ///</summary>
        bool IsSatisfiedBy(T item);
    }
}