using Xunit;
using Rhino.Mocks.Exceptions;

namespace Rhino.Mocks.Tests
{
	public class PropertySetterFixture
	{
		[Fact]
		public void Setter_Expectation_With_Custom_Ignore_Arguments()
		{
			IBar bar = MockRepository.GenerateStrictMock<IBar>();

			bar.Expect(x => x.Foo).SetPropertyAndIgnoreArgument();

			bar.Foo = 2;

			bar.VerifyAllExpectations();
		}

		[Fact]
		public void Setter_Expectation_Not_Fullfilled()
		{
			IBar bar = MockRepository.GenerateStrictMock<IBar>();

			bar.Expect(x => x.Foo).SetPropertyAndIgnoreArgument();

			var ex = Assert.Throws<ExpectationViolationException>(() => bar.VerifyAllExpectations());
			Assert.Equal("IBar.set_Foo(any); Expected #1, Actual #0.", ex.Message);
		}

		[Fact]
		public void Setter_Expectation_With_Correct_Argument()
		{
			IBar bar = MockRepository.GenerateStrictMock<IBar>();

			bar.Expect(x => x.Foo).SetPropertyWithArgument(1);

			bar.Foo = 1;

			bar.VerifyAllExpectations();
		}

		[Fact]
		public void Setter_Expectation_With_Wrong_Argument()
		{
			IBar bar = MockRepository.GenerateStrictMock<IBar>();

			bar.Expect(x => x.Foo).SetPropertyWithArgument(1);

			var ex = Assert.Throws<ExpectationViolationException>(() => { bar.Foo = 0; });
			Assert.Equal("IBar.set_Foo(0); Expected #0, Actual #1.\r\nIBar.set_Foo(1); Expected #1, Actual #0.", ex.Message);
		}
	}

	public interface IBar
	{
		int Foo { get; set; }
	}
}