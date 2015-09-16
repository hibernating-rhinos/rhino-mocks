namespace Rhino.Mocks.Tests.FieldsProblem
{
  using System;
  using Rhino.Mocks.Exceptions;
  using Xunit;

  public class FieldProblem_Andreas2
  {
    [Fact]
    public void AbstractProtectedProperty()
    {
      var sut = MockRepository.GenerateStub<MyClass>();
      var configuration = new byte[] { 1, 2, 3 };

      sut.Configuration = configuration;

      Assert.Equal(configuration, sut.Configuration);
    }

    [Fact]
    public void DuplicatedReturns()
    {
      var sut = MockRepository.GenerateStrictMock<IAnimal>();
      Assert.Throws<InvalidOperationException>(() => sut.Expect(x => x.GetMood()).Return("good").Return("exception"));
      sut.Expect(x => x.GetMood()).Return("bad");

      Assert.Equal("good", sut.GetMood());
      Assert.Equal("bad", sut.GetMood());
      sut.VerifyAllExpectations();
    }

    [Fact]
    public void AnyArgsExpectationMightLeadToStrangeResults()
    {
      var sut = MockRepository.GenerateMock<MyClass>();
      sut.Expect(x => x.DoSomething(10)).IgnoreArguments();

      sut.DoSomething("call with a string");
      
      var ex = Assert.Throws<ExpectationViolationException>(() => sut.AssertWasCalled(x => x.DoSomething(default(int)), o => o.IgnoreArguments()));
      Assert.Equal("MyClass.DoSomething<System.Int32>(any); Expected #1, Actual #0.", ex.Message);
    }
  }

  public abstract class MyClass
  {
    protected internal abstract byte[] Configuration { get; set; }
    public abstract void DoSomething<T>(T value);
  }
}
