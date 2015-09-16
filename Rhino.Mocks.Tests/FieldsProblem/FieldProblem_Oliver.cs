using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_Oliver
	{
		public interface ITestGen<T>
		{
			int Foo { get; set; }
		}

		public interface ITestNormal
		{
			int Foo { get; set;}
		}

		[Fact]
		public void StubGenericInterface_CanReadWriteProperties()
		{
			ITestGen<int> test = MockRepository.GenerateStub<ITestGen<int>>();

			test.Foo = 10;
			Assert.Equal(10, test.Foo);

			test.VerifyAllExpectations();
		}

		[Fact]
		public void StubInterface_CanReadWriteProperties()
		{
			ITestNormal test = MockRepository.GenerateStub<ITestNormal>();

			test.Foo = 10;
			Assert.Equal(10, test.Foo);

			test.VerifyAllExpectations();
		}

		[Fact]
		public void MockGenericInterface_CanSetProperties()
		{
			ITestGen<int> test = MockRepository.GenerateStrictMock<ITestGen<int>>();

			test.Expect(x => x.Foo).PropertyBehavior();

			test.Foo = 10;
			Assert.Equal(10, test.Foo);

			test.VerifyAllExpectations();
		}

		[Fact]
		public void MockNormalInterface_CanSetProperties()
		{
			ITestNormal test = MockRepository.GenerateStrictMock<ITestNormal>();

			test.Expect(x => x.Foo).PropertyBehavior();

			test.Foo = 10;
			Assert.Equal(10, test.Foo);

			test.VerifyAllExpectations();
		}
	}
}
