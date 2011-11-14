using Xunit;
using RhinoMocksCPPInterfaces;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem_Shanon
	{
		[Fact]
		public void CanMockInterfaceWithMethodsHavingModOpt()
		{
			MockRepository mocks = new MockRepository();
			IHaveMethodWithModOpts mock = mocks.StrictMock<IHaveMethodWithModOpts>();
			Assert.NotNull(mock);
		}
	}
}
