namespace Rhino.Mocks.Tests.FieldsProblem
{
	using Exceptions;
	using Xunit;

	public class FieldProblem_Shane
	{
		[Fact]
		public void WillMerge_UnorderedRecorder_WhenRecorderHasSingleRecorderInside()
		{
			ICustomer customer = MockRepository.GenerateStrictMock<ICustomer>();

			CustomerMapper mapper = new CustomerMapper();

			customer.Expect(x => x.Id).Return(0);
			customer.Expect(x => x.IsPreferred = true);

			mapper.MarkCustomerAsPreferred(customer);
			var ex = Assert.Throws<ExpectationViolationException>(() =>
				{
					customer.AssertWasCalled(x => x.IsPreferred = true).After(customer.AssertWasCalled(x => x.Id));
				});
			Assert.Equal("Expected that call Int32 get_Id() occurs before call Void set_IsPreferred(Boolean), but the expectation is not satisfied.", ex.Message);
		}
	}

	public interface ICustomer
	{
		int Id { get; }

		bool IsPreferred { get; set; }
	}

	public class CustomerMapper
	{
		public void MarkCustomerAsPreferred(ICustomer customer)
		{
			customer.IsPreferred = true;

			int id = customer.Id;
		}
	}
}