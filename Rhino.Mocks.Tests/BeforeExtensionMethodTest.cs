using Rhino.Mocks.Exceptions;
using Xunit;

namespace Rhino.Mocks.Tests
{
  public class BeforeExtensionMethodTest
  {
    public interface IBefore
    {
      void MethodBefore();
    }

    public interface IAfter
    {
      void MethodAfter();
    }

    [Fact]
    public void Before_succeeds_if_beforeCalls_occured_before_afterCalls()
    {
      var mockBefore = MockRepository.GenerateStub<IBefore>();
      var mockAfter = MockRepository.GenerateStub<IAfter>();
      mockBefore.MethodBefore();
      mockBefore.MethodBefore();
      mockAfter.MethodAfter();
      mockAfter.MethodAfter();
      mockAfter.MethodAfter();
      mockBefore.AssertWasCalled(b => b.MethodBefore())
        .Before(mockAfter.AssertWasCalled(a => a.MethodAfter()));
    }

    [Fact]
    public void Before_chokes_if_one_of_beforeCalls_occured_after_any_of_afterCalls()
    {
      var mockBefore = MockRepository.GenerateStub<IBefore>();
      var mockAfter = MockRepository.GenerateStub<IAfter>();
      mockBefore.MethodBefore();
      mockAfter.MethodAfter();
      mockBefore.MethodBefore();
      mockAfter.MethodAfter();
      mockAfter.MethodAfter();
      Assert.Throws<ExpectationViolationException>(
        () => mockBefore.AssertWasCalled(b => b.MethodBefore())
          .Before(mockAfter.AssertWasCalled(a => a.MethodAfter())));
    }
  }
}
