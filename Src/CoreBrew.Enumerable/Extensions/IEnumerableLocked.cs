using System.Collections;
using CoreBrew.Enumerable.Locked;

namespace CoreBrew.Enumerable.Extensions;

public static class EnumerableLocked
{
    public static IEnumerable<T> AsLocked<T>(this IEnumerable<T> enumerable, ReaderWriterLock readerWriterLock)
    {
        return new EnumerableLockedReaderWriter<T>(enumerable, readerWriterLock);
    }
    
    public static IEnumerable<T> AsLocked<T>(this IEnumerable<T> enumerable, ReaderWriterLockSlim readerWriterLockSlim)
    {
        return new EnumerableLockedReaderWriterSlim<T>(enumerable, readerWriterLockSlim);
    }    
}