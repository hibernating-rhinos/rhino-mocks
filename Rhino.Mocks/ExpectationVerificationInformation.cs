using System.Collections.Generic;
using Rhino.Mocks.Generated;
using Rhino.Mocks.Interfaces;

namespace Rhino.Mocks
{
	internal class ExpectationVerificationInformation
	{
	    private IExpectation expected;
		private IList<object[]> argumentsForAllCalls;
		
		public IExpectation Expected { get { return expected; } set { expected = value; } }
        public ICollection<CallRecord> AllCallRecords { get; set; }
        public IList<object[]> ArgumentsForAllCalls
	    {
	        get
	        {
                if (argumentsForAllCalls==null)
                {
                    var allCalls = AllCallRecords;
                    var a = new List<object[]>(allCalls.Count);
                    foreach (var call in allCalls)
                    {
                        a.Add(call.Arguments);
                    }
                    argumentsForAllCalls = a;
                }
	            return argumentsForAllCalls;
	        }
	    }
	}
}
