using System.Collections;

namespace CoreBrew.Enumerable.Enumerators.Locked;

public class EnumeratorLockedReaderWriterSlim<T> : EnumeratorWrappedBase<T>
{
    private readonly ReaderWriterLockSlim _readerWriterLockSlim;

    public EnumeratorLockedReaderWriterSlim(ReaderWriterLockSlim readerWriterLockSlim, IEnumerator<T> innerEnumerator): base(innerEnumerator)
    {
        _readerWriterLockSlim = readerWriterLockSlim;
        _readerWriterLockSlim.EnterReadLock();
    }
    
    public override void Dispose()
    {
        _readerWriterLockSlim.ExitReadLock();
    }
}