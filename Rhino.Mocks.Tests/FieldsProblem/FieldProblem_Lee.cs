namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System;
	using System.Collections.Generic;
	using Xunit;
	using Rhino.Mocks.Constraints;

	public class FieldProblem_Lee
	{
		[Fact]
		public void IgnoringArgumentsOnGenericMethod()
		{
			IHaveGenericMethod mock = MockRepository.GenerateStrictMock<IHaveGenericMethod>();
			
			mock.Expect(x => x.Foo(15)).IgnoreArguments().Return(true);

			bool result = mock.Foo(16);
			Assert.True(result);
			mock.VerifyAllExpectations();
		}

		[Fact]
		public void WithGenericMethods()
		{
			IFunkyList<int> list = MockRepository.GenerateMock<IFunkyList<int>>();
			Assert.NotNull(list);
			List<Guid> results = new List<Guid>();
			list.Expect(x => x.FunkItUp<Guid>(null, null)).IgnoreArguments().Constraints(Is.Equal("1"), Is.Equal(2)).Return(results);
			Assert.Same(results, list.FunkItUp<Guid>("1", 2));
		}
	}

	public interface IFunkyList<T> : IList<T>
	{
		ICollection<T2> FunkItUp<T2>(object arg1, object arg2);
	}

	public interface IHaveGenericMethod
	{
		bool Foo<T>(T obj);
	}
}