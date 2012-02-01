namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System;
	using System.Data;
	using Exceptions;
	using Xunit;
	using Rhino.Mocks;

	public class FieldProblem_Andrew
	{
		[Fact]
		public void Will_get_unexpect_error()
		{
			var stubConnection = MockRepository.GenerateStub<IDbConnection>();
			var mockCommand = MockRepository.GenerateMock<IDbCommand>();
			mockCommand.Expect(c => c.Connection = stubConnection);
			mockCommand.Expect(c => c.Connection = null);
			mockCommand.Stub(c => c.ExecuteNonQuery()).Throw(new TestException());

			var executor = new Executor(stubConnection);
			try
			{
				executor.ExecuteNonQuery(mockCommand);
				Assert.False(true, "exception was expected");
			}
			catch (TestException)
			{
			}

			var ex = Assert.Throws<ExpectationViolationException>(() => mockCommand.VerifyAllExpectations());
			Assert.Equal("IDbCommand.set_Connection(null); Expected #1, Actual #0.", ex.Message);
		}
	}

	public class TestException : Exception
	{
	}


	public class Executor
	{
		private IDbConnection _connection;

		public Executor(IDbConnection connection)
		{
			this._connection = connection;
		}

		public int ExecuteNonQuery(IDbCommand command)
		{
			try
			{
				command.Connection = this._connection;
				return command.ExecuteNonQuery();
			}
			finally
			{
				//command.Connection = null;
				if (this._connection.State != ConnectionState.Closed) this._connection.Close();
			}
		}
	}
}