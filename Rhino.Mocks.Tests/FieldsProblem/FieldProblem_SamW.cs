using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_SamW
    {
        [Fact]
        public void UsingArrayAndOutParam()
        {
            ITest test = MockRepository.GenerateStrictMock<ITest>();
            string b;
            test.Expect(x => x.ArrayWithOut(new string[] { "data" }, out b)).Return("SuccessWithOut1").OutRef("SuccessWithOut2");

            Assert.Equal("SuccessWithOut1", test.ArrayWithOut(new string[] { "data" }, out b));
            Assert.Equal("SuccessWithOut2", b);
        }

        public interface ITest
        {
            string ArrayWithOut(string[] a, out string b);
        }
    }
}
