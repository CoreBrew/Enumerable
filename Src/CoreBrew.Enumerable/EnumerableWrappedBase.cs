using System.Collections;

namespace CoreBrew.Enumerable;

public abstract class EnumerableWrappedBase<T>: IEnumerable<T>
{
    protected readonly IEnumerable<T> InnerEnumerable;

    protected EnumerableWrappedBase(IEnumerable<T> innerEnumerable)
    {
        InnerEnumerable = innerEnumerable;
    }

    public abstract IEnumerator<T> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}