using Xunit;

namespace Rhino.Mocks.Tests
{
	using Exceptions;

	public class DotNet35Tests
	{
		[Fact]
		public void NaturalSyntaxForCallingMethods()
		{
			IDemo demo = MockRepository.GenerateStrictMock<IDemo>();
			demo.Expect(x => x.VoidNoArgs());
			demo.VoidNoArgs();
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void NaturalSyntaxForCallingMethods_WithArguments()
		{
			IDemo demo = MockRepository.GenerateStrictMock<IDemo>();
			demo.Expect(x => x.VoidStringArg("blah"));
			demo.VoidStringArg("blah");
			demo.VerifyAllExpectations();
		}

		[Fact]
		public void NaturalSyntaxForCallingMethods_WithArguments_WhenNotCalled_WouldFailVerification()
		{
			IDemo demo = MockRepository.GenerateStrictMock<IDemo>();
			demo.Expect(x => x.VoidStringArg("blah"));

			Throws.Exception<ExpectationViolationException>("IDemo.VoidStringArg(\"blah\"); Expected #1, Actual #0.",demo.VerifyAllExpectations);
		}

		[Fact]
		public void NaturalSyntaxForCallingMethods_WithArguments_WhenCalledWithDifferentArgument()
		{
			IDemo demo = MockRepository.GenerateStrictMock<IDemo>();
			demo.Expect(x => x.VoidStringArg("blah"));

			Throws.Exception<ExpectationViolationException>(@"IDemo.VoidStringArg(""arg""); Expected #0, Actual #1.
IDemo.VoidStringArg(""blah""); Expected #1, Actual #0.", () => demo.VoidStringArg("arg"));
		}

		[Fact]
		public void CanCallMethodWithParameters_WithoutSpecifyingParameters_WillAcceptAnyParameter()
		{
			IDemo demo = MockRepository.GenerateStrictMock<IDemo>();
			demo.Expect(x => x.VoidStringArg("blah")).IgnoreArguments();
			demo.VoidStringArg("asd");
			demo.VerifyAllExpectations();
		}
	}
}
