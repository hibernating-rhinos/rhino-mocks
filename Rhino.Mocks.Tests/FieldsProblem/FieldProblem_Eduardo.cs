namespace Rhino.Mocks.Tests.FieldsProblem
{
	using Xunit;

	public class FieldProblem_Eduardo
	{
		[Fact]
		public void CanSetExpectationOnReadWritePropertyUsingAAASyntax()
		{
			var demo = MockRepository.GenerateMock<IDemo>();

			demo.Expect(x => x.Prop).SetPropertyWithArgument("Eduardo");

			demo.Prop = "Eduardo";

			demo.VerifyAllExpectations();
		}
	}
}
