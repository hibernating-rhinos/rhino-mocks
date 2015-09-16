
namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System;
	using System.Data.SqlClient;
	using Exceptions;
	using Xunit;

	public interface ITestInterface
	{
		ITestInterface AddService<TService, TComponent>() where TComponent : TService;
	}

	public class TestInterface : MarshalByRefObject
	{
		public virtual TestInterface AddService<TService, TComponent>() where TComponent : TService
		{
			return this;
		}
	}

	public class FieldProblem_Alexey
	{
		[Fact]
		public void MockInterfaceWithGenericMethodWithConstraints()
		{
			ITestInterface mockObj = MockRepository.GenerateStrictMockWithRemoting<ITestInterface>();
			mockObj.Expect(x => x.AddService<IDisposable, SqlConnection>()).Return(mockObj);

			mockObj.AddService<IDisposable, SqlConnection>();

			mockObj.VerifyAllExpectations();
		}

		[Fact]
		public void MockInterfaceWithGenericMethodWithConstraints_WhenNotValid()
		{
			ITestInterface mockObj = MockRepository.GenerateStrictMockWithRemoting<ITestInterface>();
			mockObj.Expect(x => x.AddService<IDisposable, SqlConnection>()).Return(mockObj);

			var ex = Assert.Throws<ExpectationViolationException>(() => mockObj.VerifyAllExpectations());
			Assert.Equal("ITestInterface.AddService<System.IDisposable, System.Data.SqlClient.SqlConnection>(); Expected #1, Actual #0.", ex.Message);
		}

		[Fact]
		public void MockInterfaceWithGenericMethodWithConstraints_WhenNotValid_UsingDynamicMock()
		{
			ITestInterface mockObj = MockRepository.GenerateDynamicMockWithRemoting<ITestInterface>();
			mockObj.Expect(x => x.AddService<IDisposable, SqlConnection>()).Return(mockObj);

			var ex = Assert.Throws<ExpectationViolationException>(() => mockObj.VerifyAllExpectations());
			Assert.Equal("ITestInterface.AddService<System.IDisposable, System.Data.SqlClient.SqlConnection>(); Expected #1, Actual #0.", ex.Message);
		}

		[Fact]
		public void MockInterfaceWithGenericMethodWithConstraints_UsingDynamicMock()
		{
			ITestInterface mockObj = MockRepository.GenerateDynamicMockWithRemoting<ITestInterface>();

			mockObj.AddService<IDisposable, SqlConnection>();

			mockObj.VerifyAllExpectations();
		}
	}
}
