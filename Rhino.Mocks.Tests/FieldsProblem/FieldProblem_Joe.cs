using System.Windows.Forms;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_Joe
	{
		[Fact]
		public void MockingConcreteForm()
		{
			Form frm = MockRepository.GeneratePartialMock<Form>();
			Assert.NotNull(frm);
		}
	}
}
