namespace Rhino.Mocks.Tests
{
	using System.IO;
	using Xunit;
	using Rhino.Mocks.Impl;

	public class TraceWriterWithStackTraceExpectationWriterFixture
	{
		[Fact]
		public void WillPrintLogInfoWithStackTrace()
		{
			TraceWriterWithStackTraceExpectationWriter expectationWriter = new TraceWriterWithStackTraceExpectationWriter();
			StringWriter writer = new StringWriter();
			expectationWriter.AlternativeWriter = writer;

			RhinoMocks.Logger = expectationWriter;

			IDemo mock = MockRepository.GenerateStrictMock<IDemo>();
			mock.Expect(x => x.VoidNoArgs());
			mock.VoidNoArgs();
			mock.VerifyAllExpectations();

			Assert.Contains("WillPrintLogInfoWithStackTrace", writer.GetStringBuilder().ToString());
		}
	}
}