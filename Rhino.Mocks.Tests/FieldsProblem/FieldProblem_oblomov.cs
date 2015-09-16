using System;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	using Xunit;

	public interface IMyService
	{
		void Func1();
		void Func2();
		void Func3();
	}

	public class FieldProblem_oblomov : IDisposable
	{
		IMyService service;

		public FieldProblem_oblomov()
		{
			service = MockRepository.GenerateStrictMock<IMyService>();
		}
		public void Dispose()
		{
			service.VerifyAllExpectations();
		}

		[Fact]
		public void TestWorks()
		{
			service.Expect(x => x.Func1());
			service.Expect(x => x.Func2());
			service.Expect(x => x.Func3());

			service.Func2();
			service.Func1();
			service.Func3();

			service.AssertWasCalled(x => x.Func3()).After(service.AssertWasCalled(x => x.Func1()));
			service.AssertWasCalled(x => x.Func3()).After(service.AssertWasCalled(x => x.Func2()));
		}

		[Fact]
		public void TestDoesnotWork()
		{
			service.Expect(x => x.Func3());
			service.Func3();
		}
	}
}
