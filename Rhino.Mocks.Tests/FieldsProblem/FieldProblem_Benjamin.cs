using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_Benjamin
	{
		[Fact]
        public void ThisTestPasses()
        {
            var interfaceStub = MockRepository.GenerateStub<InterfaceINeedToStub>();

            interfaceStub.Stub(x => x.MyStringValue).Return("string");
            interfaceStub.MyIntValue = 4;

            Assert.Equal(4, interfaceStub.MyIntValue);
        }

        [Fact]
        public void ThisTestDoesNotPass()
        {
            var myInterface = MockRepository.GenerateStub<InterfaceINeedToStub>();

            // Changed order of property initialization
            myInterface.MyIntValue = 4;
            myInterface.Stub(x => x.MyStringValue).Return("string");

            Assert.Equal(4, myInterface.MyIntValue);
        }
	}

	public interface InterfaceINeedToStub
	{
		int MyIntValue { get; set; }
		string MyStringValue { get; }
	}
}
