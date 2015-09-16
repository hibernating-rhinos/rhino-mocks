using Xunit;
using Rhino.Mocks.Exceptions;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_Robert
	{
		public interface IView
		{
			void RedrawDisplay(object something);
		}

		[Fact]
		public void CorrectResultForExpectedWhenUsingTimes()
		{
			IView view = MockRepository.GenerateStrictMock<IView>();
			view.Expect(x => x.RedrawDisplay(null)).Repeat.Times(4).IgnoreArguments();
			var ex = Assert.Throws<ExpectationViolationException>(() =>
			                                                      	{
			                                                      		for (int i = 0; i < 5; i++)
			                                                      		{
			                                                      			view.RedrawDisplay("blah");
			                                                      		}
			                                                      	});
			Assert.Equal("IView.RedrawDisplay(\"blah\"); Expected #4, Actual #5.", ex.Message);
		}

		[Fact]
		public void CorrectResultForExpectedWhenUsingTimesWithRange()
		{
			IView view = MockRepository.GenerateStrictMock<IView>();
			view.Expect(x => x.RedrawDisplay(null)).Repeat.Times(3,4).IgnoreArguments();
			var ex = Assert.Throws<ExpectationViolationException>(() =>
			                                                      	{
			                                                      		for (int i = 0; i < 5; i++)
			                                                      		{
			                                                      			view.RedrawDisplay("blah");
			                                                      		}
			                                                      	});
			Assert.Equal("IView.RedrawDisplay(\"blah\"); Expected #3 - 4, Actual #5.", ex.Message);
		}
	}
}