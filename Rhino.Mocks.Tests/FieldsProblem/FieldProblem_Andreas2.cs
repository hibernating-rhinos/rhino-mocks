namespace Rhino.Mocks.Tests.FieldsProblem
{
  using System;
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
  }

  public abstract class MyClass
  {
    protected internal abstract byte[] Configuration { get; set; }
  }
}
