using System.Security.Permissions;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem__Sean
	{
		[Fact]
		public void CanMockMethodWithEnvironmentPermissions()
		{
			IEmployeeRepository employeeRepository = MockRepository.GenerateStrictMock<IEmployeeRepository>();
			IEmployee employee = MockRepository.GenerateStrictMock<IEmployee>();
			
			employeeRepository.Expect(x => x.GetEmployeeDetails("ayende")).Return(employee);

			IEmployee actual = employeeRepository.GetEmployeeDetails("ayende");
			Assert.Equal(employee, actual);

			employee.VerifyAllExpectations();
			employeeRepository.VerifyAllExpectations();
		}
	}

	public interface IEmployeeRepository
	{
		[EnvironmentPermission(SecurityAction.LinkDemand)]
		IEmployee GetEmployeeDetails(string username);
	}

	public interface IEmployee
	{
	}
}
