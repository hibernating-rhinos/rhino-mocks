namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System;
	using Xunit;

	public class FieldProblem_Libardo
	{
		[Fact]
		public void Can_mix_assert_was_call_with_verify_all()
		{
			var errorHandler = MockRepository.GenerateMock<IErrorHandler>();

			var ex = new Exception("Take this");
			errorHandler.HandleError(ex);

			errorHandler.AssertWasCalled(eh => eh.HandleError(ex));

			errorHandler.Replay();
			errorHandler.VerifyAllExpectations(); // Can I still keep this somehow?
		}
	}

	public interface IErrorHandler
	{
		void HandleError(Exception e);
	}
}