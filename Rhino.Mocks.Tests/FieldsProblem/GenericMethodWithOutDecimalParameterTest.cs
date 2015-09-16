namespace Rhino.Mocks.Tests.FieldsProblem
{
	using Xunit;

	public class GenericMethodWithOutDecimalParameterTest
	{
		public interface IMyInterface
		{
			void GenericMethod<T>(out T parameter);
		}

		[Fact]
		public void GenericMethodWithOutDecimalParameter()
		{
			IMyInterface mock = MockRepository.GenerateStrictMock<IMyInterface>();

			decimal expectedOutParameter = 1.234M;
			decimal emptyOutParameter;
			mock.Expect(x => x.GenericMethod(out emptyOutParameter)).OutRef(expectedOutParameter);

			decimal outParameter;
			mock.GenericMethod(out outParameter);
			Assert.Equal(expectedOutParameter, outParameter);
			mock.VerifyAllExpectations();
		}

		public static void Foo(out decimal d)
		{
			d = 1.234M;
		}

		public static void Foo(out int d)
		{
			d = 1;
		}
	}
}