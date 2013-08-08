using System;

namespace Rhino.Mocks.Tests.Model
{
	internal class Internal
	{
		internal virtual string Bar()
		{
			return "abc";
		}

		internal virtual string Foo()
		{
			return Bar();
		}

		internal virtual object Baz
		{
			get { throw new Exception("exception thrown from baz property getter"); }
			set { throw new Exception("exception thrown from baz property setter"); }
		}
	}
}