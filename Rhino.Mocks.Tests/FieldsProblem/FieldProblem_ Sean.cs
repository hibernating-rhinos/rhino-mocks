using System.Security.Permissions;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	
	public class FieldProblem__Sean
	{
		[Fact]
		public void CanMockMethodWithEnvironmentPermissions()
		{
			MockRepository mocks = new MockRepository();
			IEmployeeRepository employeeRepository = mocks.StrictMock<IEmployeeRepository>();
			IEmployee employee = mocks.StrictMock<IEmployee>();
			
			using (mocks.Record())
			{
				employeeRepository.GetEmployeeDetails("ayende");
				LastCall.Return(employee);
			}

			using(mocks.Playback())
			{
				IEmployee actual = employeeRepository.GetEmployeeDetails("ayende");
				Assert.Equal(employee, actual);
			}
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
