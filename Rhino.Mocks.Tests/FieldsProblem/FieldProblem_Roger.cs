using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_Roger
	{
		[Fact]
		public void VerifyMockCanBeSetupWhenExternalInterfaceUsingInnerClassWithInternalScope()
		{
			ISomeInterface<InnerClass> target = MockRepository.GenerateStrictMock<ISomeInterface<InnerClass>>();
		}
	}

	internal class InnerClass
	{
	}

	public interface ISomeInterface<T> { }
}
