using Xunit;
using RhinoMocksCPPInterfaces;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_Shanon
	{
		[Fact]
		public void CanMockInterfaceWithMethodsHavingModOpt()
		{
			IHaveMethodWithModOpts mock = MockRepository.GenerateStrictMock<IHaveMethodWithModOpts>();
			Assert.NotNull(mock);
		}
	}
}
