using Xunit;
using Rhino.Mocks.Tests.Model;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_Rabashani
	{
		[Fact]
		public void CanMockInternalInterface()
		{
			IInternal mock = MockRepository.GenerateStrictMock<IInternal>();
			mock.Expect(x => x.Foo());
			mock.Foo();
			mock.VerifyAllExpectations();
		}

		[Fact]
		public void CanMockInternalClass()
		{
			Internal mock = MockRepository.GenerateStrictMock<Internal>();
			mock.Expect(x => x.Bar()).Return("blah");
			Assert.Equal("blah", mock.Bar());
			mock.VerifyAllExpectations();
		}

		[Fact]
		public void CanPartialMockInternalClass()
		{
			Internal mock = MockRepository.GeneratePartialMock<Internal>();
			mock.Expect(x => x.Foo()).Return("blah");
			Assert.Equal("blah", mock.Foo());
			Assert.Equal("abc", mock.Bar());
			mock.VerifyAllExpectations();
		}
	}
}
