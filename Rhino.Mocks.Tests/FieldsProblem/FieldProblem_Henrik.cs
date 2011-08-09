#if DOTNET35
namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System;
	using System.Globalization;
	using System.Threading;
	using Xunit;
	
	public class FieldProblem_Henrik
	{
		[Fact]
		public void Trying_to_mock_null_instance_should_fail_with_descriptive_error_message()
		{
			var culture = Thread.CurrentThread.CurrentUICulture;
			try
			{
				Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
				var ex = Assert.Throws<ArgumentNullException>(() => RhinoMocksExtensions.Expect<object>(null, x => x.ToString()));
				Assert.Equal("You cannot mock a null instance\r\nParameter name: mock", ex.Message);
			}
			finally
			{
				Thread.CurrentThread.CurrentUICulture = culture;
			}
		}
	}
}
#endif