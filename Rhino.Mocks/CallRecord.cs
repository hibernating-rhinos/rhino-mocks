using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Rhino.Mocks
{
  /// <summary>
  /// Record about information of a specific method invocation.
  /// </summary>
  public class CallRecord
  {
    private static long sequencer = long.MinValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallRecord"/> class.
    /// </summary>
    internal CallRecord()
    {
      this.Sequence = Interlocked.Increment(ref sequencer);
    }

    internal object[] Arguments { get; set; }
    internal long Sequence { get; private set; }
    internal MethodInfo Method { get; set; }
  }

  /// <summary>
  /// Encapsulates a collection of <see cref="CallRecord"/>'s.
  /// </summary>
  public class CallRecordCollection
  {
    /// <summary>
    /// Encapsulated list of <see cref="CallRecord"/>.
    /// </summary>
    private readonly List<CallRecord> list;

    /// <summary>
    /// Initializes a new instance of the <see cref="CallRecordCollection"/> class.
    /// </summary>
    internal CallRecordCollection()
    {
      this.list = new List<CallRecord>();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CallRecordCollection"/> class.
    /// </summary>
    /// <param name="collection">The collection to copy.</param>
    internal CallRecordCollection(CallRecordCollection collection)
    {
      this.list = new List<CallRecord>(collection.list);
    }

    /// <summary>
    /// Gets the number of elements actually contained in the collection.
    /// </summary>
    internal int Count
    {
      get { return this.list.Count; }
    }

    /// <summary>
    /// Gets the <see cref="CallRecord"/> at the specified <paramref name="index"/>.
    /// </summary>
    /// <param name="index">The zero-based <paramref name="index"/> of the record to get.</param>
    /// <returns>
    /// The record at the specified <paramref name="index"/>.
    /// </returns>
    internal CallRecord this[int index]
    {
      get { return this.list[index]; }
    }

    /// <summary>
    /// Adds the specified <paramref name="record"/> to the end of the collection.
    /// </summary>
    /// <param name="record">The <paramref name="record"/> to be added to the end of the collection.</param>
    internal void Add(CallRecord record)
    {
      this.list.Add(record);
    }

    /// <summary>
    /// Gets the record enumerable.
    /// </summary>
    /// <returns>A instance of <see cref="IEnumerable{CallRecord}"/>.</returns>
    internal IEnumerable<CallRecord> GetRecordEnumerable()
    {
      return this.list;
    }
  }
}