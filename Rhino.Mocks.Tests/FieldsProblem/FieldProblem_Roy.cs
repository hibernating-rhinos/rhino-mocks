using System;
using Xunit;
using Rhino.Mocks.Constraints;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Roy
    {
        [Fact]
        public void StubNeverFailsTheTest()
        {
            IGetResults resultGetter = MockRepository.GenerateStub<IGetResults>();
            resultGetter.Expect(x => x.GetSomeNumber("a")).Return(1);

            int result = resultGetter.GetSomeNumber("b");
            Assert.Equal(0, result);
            resultGetter.VerifyAllExpectations(); //<- should not fail the test methinks
        }

        [Fact]
        public void CanGetSetupResultFromStub()
        {
            IGetResults resultGetter = MockRepository.GenerateStub<IGetResults>();
            resultGetter.Expect(x => x.GetSomeNumber("a")).Return(1);

            int result = resultGetter.GetSomeNumber("a");
            Assert.Equal(1, result);
            resultGetter.VerifyAllExpectations(); 
        }

        [Fact]
        public void CannotCallLastCallConstraintsMoreThanOnce()
        {
            IGetResults resultGetter = MockRepository.GenerateStub<IGetResults>();
            var ex = Assert.Throws<InvalidOperationException>(() =>
              {
                resultGetter.Expect(x => x.GetSomeNumber("a"))
                  .Constraints(Text.Contains("b"))
                  .Constraints(Text.Contains("a"));
                });

            Assert.Equal("You have already specified constraints for this method. (IGetResults.GetSomeNumber(contains \"b\");)", ex.Message);
        }
    }

    public interface IGetResults
    {
        int GetSomeNumber(string s);
    }
}