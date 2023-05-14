using CoreBrew.Enumerable.Enumerators.Locked;

namespace CoreBrew.Enumerable.Locked;

public class EnumerableLockedReaderWriter<T> : EnumerableWrappedBase<T>
{
    private readonly ReaderWriterLock _readerWriterLock;
    public EnumerableLockedReaderWriter(IEnumerable<T> innerEnumerable, ReaderWriterLock readerWriterLock) : base(innerEnumerable)
    {
        _readerWriterLock = readerWriterLock;
    }

    public override IEnumerator<T> GetEnumerator()
    {
        return new EnumeratorLockedReaderWriter<T>(_readerWriterLock, InnerEnumerable.GetEnumerator());
    }
}