using System.Collections;

namespace CoreBrew.Enumerable.Enumerators.Locked;

public class EnumeratorLockedReaderWriter<T> : IEnumerator<T>
{
    private readonly ReaderWriterLock _readerWriterLock;
    private readonly IEnumerator<T> _innerEnumerator;

    public EnumeratorLockedReaderWriter(ReaderWriterLock readerWriterLock, IEnumerator<T> innerEnumerator)
    {
        _readerWriterLock = readerWriterLock;
        _innerEnumerator = innerEnumerator;
        _readerWriterLock.AcquireReaderLock(-1);
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

    public void Dispose()
    {
        _readerWriterLock.ReleaseReaderLock();
    }
}