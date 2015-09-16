using System;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	using Exceptions;

	public class FieldProblem_Stefan
	{
		// This test fixture relates to ploblems when ignoring arguments on generic method calls when the type is a struct (aka value type).
		// With reference types - such as String - there is no problem.
		// It has nothing to do with ordering or not -> but if you do not use an ordered mock recorder, then the error msg is not helpful.

		[Fact]
		public void ShouldIgnoreArgumentsOnGenericCallWhenTypeIsStruct()
		{
			// setup
			ISomeService m_SomeServiceMock = MockRepository.GenerateStrictMock<ISomeService>();
			SomeClient sut = new SomeClient(m_SomeServiceMock);

			m_SomeServiceMock.Expect(x => x.DoSomething(null, default(string))).IgnoreArguments();
			m_SomeServiceMock.Expect(x => x.DoSomething(null, default(DateTime))).IgnoreArguments();

			// test
			sut.DoSomething();

			// verification
			m_SomeServiceMock.AssertWasCalled(x => x.DoSomething(null, default(string)), o => o.IgnoreArguments())
				.Before(m_SomeServiceMock.AssertWasCalled(x => x.DoSomething(null, default(DateTime)), o => o.IgnoreArguments()));
			m_SomeServiceMock.VerifyAllExpectations();
		}

		[Fact]
		public void UnexpectedCallToGenericMethod()
		{
			ISomeService m_SomeServiceMock = MockRepository.GenerateStrictMock<ISomeService>();
			m_SomeServiceMock.Expect(x => x.DoSomething(null, "foo"));
			var ex = Assert.Throws<ExpectationViolationException>(() => m_SomeServiceMock.DoSomething(null, 5));
			Assert.Equal(@"ISomeService.DoSomething<System.Int32>(null, 5); Expected #0, Actual #1.
ISomeService.DoSomething<System.String>(null, ""foo""); Expected #1, Actual #0.", ex.Message);
		}

		[Fact]
		public void IgnoreArgumentsAfterDo()
		{
			IDemo demo = MockRepository.GenerateMock<IDemo>();
			bool didDo = false;
			demo.Expect(x => x.VoidNoArgs()).Do(SetToTrue(out didDo)).IgnoreArguments();

			demo.VoidNoArgs();
			Assert.True(didDo, "Do has not been executed!");

			demo.VerifyAllExpectations();
		}
		
		private delegate void PlaceHolder();

				private PlaceHolder SetToTrue(out bool didDo)
				{
			didDo = true;
						return delegate { };
				}
	}

	public interface ISomeService
	{
		void DoSomething<T>(string key, T someObj);
	}

	internal class SomeClient
	{
		private readonly ISomeService m_SomeSvc;

		public SomeClient(ISomeService someSvc)
		{
			m_SomeSvc = someSvc;
		}

		public void DoSomething()
		{
			m_SomeSvc.DoSomething<string>("string.test", "some string");

			m_SomeSvc.DoSomething<DateTime>("struct.test", DateTime.Now);
		}
	}
}