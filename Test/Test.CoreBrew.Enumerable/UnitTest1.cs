using System.Collections;
using CoreBrew.Enumerable.Enumerators.Locked;
using CoreBrew.Enumerable.Extensions;

namespace Test.CoreBrew.Enumerable;

public class TestConcurrentCollectionEnumerator<T> : IEnumerable<T>
{
    private readonly List<T> _innerList = new ();
    private readonly ReaderWriterLock _collectionLock = new();

    public void ClearAndAdd(IEnumerable<T> transportNotes)
    {
        _collectionLock.AcquireWriterLock(-1);
        try
        {
            _innerList.Clear();
            foreach (var transportNote in transportNotes)
            {
                _innerList.Add(transportNote);
            }
        }
        finally
        {
            _collectionLock.ReleaseWriterLock();
        }
    }     
    
    public IEnumerator<T> GetEnumerator()
    {
        return new EnumeratorLockedReaderWriter<T>(_collectionLock, _innerList.GetEnumerator());
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class TestConcurrentCollectionExtension<T>
{
    private readonly List<T> _innerList = new ();
    private readonly ReaderWriterLock _collectionLock = new();
    public IEnumerable<T> Items => _innerList.AsLocked(_collectionLock);

    public void ClearAndAdd(IEnumerable<T> transportNotes)
    {
        _collectionLock.AcquireWriterLock(-1);
        try
        {
            _innerList.Clear();
            foreach (var transportNote in transportNotes)
            {
                _innerList.Add(transportNote);
            }
        }
        finally
        {
            _collectionLock.ReleaseWriterLock();
        }
    }    
}


public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestConcurrencyExtension()
    {
        var firstListOfNotes = new List<object>{new (),new (),new (),new (),new ()};
        var secondListOfNotes = new List<object>{new (),new (),new (),new (),new ()};

        var testConcurrentCollection = new TestConcurrentCollectionExtension<object>();
        testConcurrentCollection.ClearAndAdd(firstListOfNotes);

        var autoResetEvent = new AutoResetEvent(initialState: false);

        var myTask = Task.Run(() =>
        {
            Assert.DoesNotThrow(() =>
            {
                
                var firstLoop = true;
                foreach (var item in testConcurrentCollection.Items)
                {
                    if (!firstLoop) continue;
                    autoResetEvent.Set();
                    firstLoop = false;
                    //Sleep for a "very" long time to let the main test thread reach the
                    //transportNote collection clear and add new transport notes function
                    Thread.Sleep(2000);
                }
            },"Expected no exception thrown. Most likely the collection was changed while enumerating");
        });

        autoResetEvent.WaitOne();
        testConcurrentCollection.ClearAndAdd(secondListOfNotes);

        await myTask;        
    }
    
    [Test]
    public async Task TestConcurrencyEnumerator()
    {
        var firstListOfNotes = new List<object>{new (),new (),new (),new (),new ()};
        var secondListOfNotes = new List<object>{new (),new (),new (),new (),new ()};

        var testConcurrentCollection = new TestConcurrentCollectionEnumerator<object>();
        testConcurrentCollection.ClearAndAdd(firstListOfNotes);

        var autoResetEvent = new AutoResetEvent(initialState: false);

        var myTask = Task.Run(() =>
        {
            Assert.DoesNotThrow(() =>
            {
                
                var firstLoop = true;
                foreach (var item in testConcurrentCollection)
                {
                    if (!firstLoop) continue;
                    autoResetEvent.Set();
                    firstLoop = false;
                    //Sleep for a "very" long time to let the main test thread reach the
                    //transportNote collection clear and add new transport notes function
                    Thread.Sleep(2000);
                }
            },"Expected no exception thrown. Most likely the collection was changed while enumerating");
        });

        autoResetEvent.WaitOne();
        testConcurrentCollection.ClearAndAdd(secondListOfNotes);

        await myTask;        
    }
}