using System.Collections.Generic;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	using Exceptions;
	
	public class FieldProblem_Rob
	{
		[Fact]
		public void CanFailIfCalledMoreThanOnceUsingDynamicMock()
		{
			IDemo demo = MockRepository.GenerateMock<IDemo>();
			demo.Expect(x => x.VoidNoArgs()).Repeat.Once(); // doesn't realy matter
			demo.Expect(x => x.VoidNoArgs()).Repeat.Never();

			var ex = Assert.Throws<ExpectationViolationException>(() => demo.VoidNoArgs());
			Assert.Equal("IDemo.VoidNoArgs(); Expected #0, Actual #1.", ex.Message);
		}

		[Fact]
		public void Ayende_View_On_Mocking()
		{
			ISomeSystem mockSomeSystem = MockRepository.GenerateStrictMock<ISomeSystem>();

			mockSomeSystem.Expect(x => x.GetFooFor<ExpectedBar>("foo")).Return(new List<ExpectedBar>());

			var ex = Assert.Throws<ExpectationViolationException>(() =>
																															{
																																ExpectedBarPerformer cut = new ExpectedBarPerformer(mockSomeSystem);
																																cut.DoStuffWithExpectedBar("foo");
																																mockSomeSystem.VerifyAllExpectations();
																															});
			Assert.Equal(@"ISomeSystem.GetFooFor<Rhino.Mocks.Tests.FieldsProblem.UnexpectedBar>(""foo""); Expected #1, Actual #1.
ISomeSystem.GetFooFor<Rhino.Mocks.Tests.FieldsProblem.ExpectedBar>(""foo""); Expected #1, Actual #0.", ex.Message);
		}
	}

	public interface ISomeSystem
	{
		List<TBar> GetFooFor<TBar>(string key) where TBar : Bar;
	}
	public class Bar { }
	public class ExpectedBar : Bar { }
	public class UnexpectedBar : Bar { }

	public class ExpectedBarPerformer
	{
		ISomeSystem system;
		
		public ExpectedBarPerformer(ISomeSystem system)
		{
			this.system = system;
		}

		public void DoStuffWithExpectedBar(string p)
		{
			IList<UnexpectedBar> list = system.GetFooFor<UnexpectedBar>(p);
		}
	}
}
