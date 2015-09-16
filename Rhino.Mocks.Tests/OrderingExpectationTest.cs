using Rhino.Mocks.Exceptions;
using Xunit;

namespace Rhino.Mocks.Tests
{
  public class OrderingExpectationTest
  {
    public interface IBefore { void MethodBefore(); }

    public interface IAfter { void MethodAfter(); }

    private readonly IBefore mockBefore;
    private readonly IAfter mockAfter;

    public OrderingExpectationTest()
    {
      this.mockBefore = MockRepository.GenerateStub<IBefore>();
      this.mockAfter = MockRepository.GenerateStub<IAfter>();
    }

    [Fact]
    public void Before_and_After_succeeds_if_all_beforeCalls_occurred_before_afterCalls()
    {
      this.AllBeforeThenAllAfter();

      this.mockBefore.AssertWasCalled(b => b.MethodBefore())
          .Before(this.mockAfter.AssertWasCalled(a => a.MethodAfter()));

      this.mockAfter.AssertWasCalled(a => a.MethodAfter())
          .After(this.mockBefore.AssertWasCalled(b => b.MethodBefore()));
    }

    [Fact]
    public void Before_and_After_succeeds_if_before_occurred_before_after()
    {
      this.BeforeAfterInterlaced();

      this.mockBefore.AssertWasCalled(b => b.MethodBefore()).First()
          .Before(this.mockAfter.AssertWasCalled(a => a.MethodAfter()).First())
          .Before(this.mockBefore.AssertWasCalled(b => b.MethodBefore()).Last());

      this.mockAfter.AssertWasCalled(a => a.MethodAfter()).Last()
          .After(this.mockBefore.AssertWasCalled(b => b.MethodBefore()).Last())
          .After(this.mockAfter.AssertWasCalled(a => a.MethodAfter()).First());
    }

    [Fact]
    public void Before_and_After_chokes_if_one_of_beforeCalls_occurred_after_any_of_afterCalls()
    {
      this.BeforeAfterInterlaced();

      Throws.Exception<ExpectationViolationException>(
        () => this.mockBefore.AssertWasCalled(b => b.MethodBefore())
          .Before(this.mockAfter.AssertWasCalled(a => a.MethodAfter())));

      Throws.Exception<ExpectationViolationException>(
        () => this.mockAfter.AssertWasCalled(a => a.MethodAfter())
          .After(this.mockBefore.AssertWasCalled(b => b.MethodBefore())));
    }

    [Fact]
    public void Before_and_After_chokes_if_before_occurred_after_after()
    {
      this.BeforeAfterInterlaced();

      Throws.Exception<ExpectationViolationException>(
        () => this.mockBefore.AssertWasCalled(b => b.MethodBefore()).Last()
          .Before(this.mockAfter.AssertWasCalled(a => a.MethodAfter()).First()));

      Throws.Exception<ExpectationViolationException>(
        () => this.mockAfter.AssertWasCalled(a => a.MethodAfter()).First()
          .After(this.mockBefore.AssertWasCalled(b => b.MethodBefore()).Last()));
    }

    private void AllBeforeThenAllAfter()
    {
      this.mockBefore.MethodBefore();
      this.mockBefore.MethodBefore();
      this.mockAfter.MethodAfter();
      this.mockAfter.MethodAfter();
      this.mockAfter.MethodAfter();
    }

    private void BeforeAfterInterlaced()
    {
      this.mockBefore.MethodBefore();
      this.mockAfter.MethodAfter();
      this.mockBefore.MethodBefore();
      this.mockAfter.MethodAfter();
      this.mockAfter.MethodAfter();
    }
  }
}
