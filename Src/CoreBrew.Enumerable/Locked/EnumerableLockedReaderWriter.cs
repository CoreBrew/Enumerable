using System.Collections;
using CoreBrew.Enumerable.Enumerators.Locked;

namespace CoreBrew.Enumerable.Locked;

public class EnumerableLockedReaderWriter<T> : IEnumerable<T>
{
    private readonly IEnumerable<T> _innerEnumerable;
    private readonly ReaderWriterLock _readerWriterLock;

    public EnumerableLockedReaderWriter(IEnumerable<T> innerEnumerable, ReaderWriterLock readerWriterLock)
    {
        _innerEnumerable = innerEnumerable;
        _readerWriterLock = readerWriterLock;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return new EnumeratorLockedReaderWriter<T>(_readerWriterLock, _innerEnumerable.GetEnumerator());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}