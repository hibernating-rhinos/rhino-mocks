namespace Rhino.Mocks.Tests.FieldsProblem
{
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
  }

  public abstract class MyClass
  {
    protected internal abstract byte[] Configuration { get; set; }
  }
}
