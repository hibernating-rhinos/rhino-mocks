
namespace Rhino.Mocks.Tests.FieldsProblem
{
	using Xunit;
	
	public class FieldProblem_Mithresh
	{
		[Fact]
		public void TestOutMethod()
		{
			ITest mockProxy = MockRepository.GenerateStrictMock<ITest>();

			int intTest = 0;

			mockProxy.Expect(x => x.Addnumber(out intTest)).OutRef(4);

			mockProxy.Addnumber(out intTest);
			Assert.Equal(4, intTest);

			mockProxy.VerifyAllExpectations();
		}

		public interface ITest
		{
			void Addnumber(out int Num);
		}
	}
}
