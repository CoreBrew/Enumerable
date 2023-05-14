using System.Collections;

namespace CoreBrew.Enumerable.Enumerators;

public abstract class EnumeratorWrappedBase<T> : IEnumerator<T>
{
    private readonly IEnumerator<T> _innerEnumerator;

    protected EnumeratorWrappedBase(IEnumerator<T> innerEnumerator)
    {
        _innerEnumerator = innerEnumerator;
    }

    public bool MoveNext()
    {
        return _innerEnumerator.MoveNext();
    }

    public void Reset()
    {
        _innerEnumerator.Reset();
    }

    public T Current => _innerEnumerator.Current;

    object? IEnumerator.Current => Current;

    public abstract void Dispose();

}