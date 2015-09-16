using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	using Exceptions;

	public class FieldProblem_Sam
	{
		[Fact]
		public void Test()
		{
			SimpleOperations myMock = MockRepository.GenerateStrictMock<SimpleOperations>();
			myMock.Expect(x => x.AddTwoValues(1, 2)).Return(3);
			Assert.Equal(3, myMock.AddTwoValues(1, 2));
			myMock.VerifyAllExpectations();
		}

		[Fact]
		public void WillRememberExceptionInsideOrderRecorderEvenIfInsideCatchBlock()
		{
			IInterfaceWithThreeMethods interfaceWithThreeMethods = MockRepository.GenerateStrictMock<IInterfaceWithThreeMethods>();

			interfaceWithThreeMethods.Expect(x => x.A());
			interfaceWithThreeMethods.Expect(x => x.C());

			interfaceWithThreeMethods.A();
			try
			{
				interfaceWithThreeMethods.B();
			}
			catch { /* valid for code under test to catch all */ }
			interfaceWithThreeMethods.C();

			interfaceWithThreeMethods.AssertWasCalled(x => x.C()).After(interfaceWithThreeMethods.AssertWasCalled(x => x.A()));
			var ex = Assert.Throws<ExpectationViolationException>(() => interfaceWithThreeMethods.VerifyAllExpectations());
			Assert.Equal("IInterfaceWithThreeMethods.B(); Expected #0, Actual #1.", ex.Message);
		}
	}

	public interface IInterfaceWithThreeMethods
	{
		void A();
		void B();
		void C();
	}

	public class SimpleOperations
	{
		public virtual int AddTwoValues(int x, int y)
		{
			return x + y;
		}
	}
}