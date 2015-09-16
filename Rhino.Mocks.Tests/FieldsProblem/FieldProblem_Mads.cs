using System.Collections.Generic;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_Mads
	{
		[Fact]
		public void Unresolable_Framework_Bug_With_Generic_Method_On_Generic_Interface_With_Conditions_On_Both_Generics()
		{
			TestInterface<List<string>> mockedInterface = MockRepository.GenerateStrictMock<TestInterface<List<string>>>();
		}
	}

	public interface TestInterface<T> where T : IEnumerable<string>
	{
		string TestMethod<T2>(T2 obj) where T2 : T, ICollection<string>;
	}
}