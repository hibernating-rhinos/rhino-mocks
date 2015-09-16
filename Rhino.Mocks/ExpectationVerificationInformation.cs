using System.Collections.Generic;
using System.Linq;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks
{
  internal class ExpectationVerificationInformation
  {
    private IList<object[]> argumentsForAllCalls;

    public ExpectationVerificationInformation(CallRecordCollection allCallRecords, IExpectation expected)
    {
      this.AllCallRecords = allCallRecords;
      this.Expected = expected;
    }

    public IExpectation Expected { get; private set; }
    public CallRecordCollection AllCallRecords { get; private set; }
    public IList<object[]> ArgumentsForAllCalls
    {
      get
      {
        return this.argumentsForAllCalls ??
               (this.argumentsForAllCalls =
                new List<object[]>(this.AllCallRecords.GetRecordEnumerable().Select(call => call.Arguments)));
      }
    }
  }
}
