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

    internal CallRecord()
    {
      Sequence = Interlocked.Increment(ref sequencer);
    }
    internal object[] Arguments { get; set; }
    internal long Sequence { get; private set; }
    internal MethodInfo Method { get; set; }
  }
}