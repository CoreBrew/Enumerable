using CoreBrew.Enumerable.Enumerators.Locked;

namespace CoreBrew.Enumerable.Locked;

public class EnumerableLockedReaderWriterSlim<T> : EnumerableWrappedBase<T>
{
    private readonly ReaderWriterLockSlim _readerWriterLockSlim;
    public EnumerableLockedReaderWriterSlim(IEnumerable<T> innerEnumerable, ReaderWriterLockSlim readerWriterLockSlim) : base(innerEnumerable)
    {
        _readerWriterLockSlim = readerWriterLockSlim;
    }

    public override IEnumerator<T> GetEnumerator()
    {
        return new EnumeratorLockedReaderWriterSlim<T>(_readerWriterLockSlim, InnerEnumerable.GetEnumerator());
    }
}