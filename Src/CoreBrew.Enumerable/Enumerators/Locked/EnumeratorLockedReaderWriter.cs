using System.Collections;

namespace CoreBrew.Enumerable.Enumerators.Locked;

public class EnumeratorLockedReaderWriter<T> : EnumeratorWrappedBase<T>
{
    private readonly ReaderWriterLock _readerWriterLock;

    public EnumeratorLockedReaderWriter(ReaderWriterLock readerWriterLock, IEnumerator<T> innerEnumerator): base(innerEnumerator)
    {
        _readerWriterLock = readerWriterLock;
        _readerWriterLock.AcquireReaderLock(-1);
    }

    public override void Dispose()
    {
        _readerWriterLock.ReleaseReaderLock();
    }
}