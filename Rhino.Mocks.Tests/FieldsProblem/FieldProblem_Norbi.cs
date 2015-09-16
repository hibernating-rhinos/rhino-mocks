namespace Rhino.Mocks.Tests.FieldsProblem
{
  using System;
  using Xunit;

  /// <summary>
  /// Reproducing "Expect throws InvalidCastException with delegates"
  /// (https://github.com/ayende/rhino-mocks/issues/6).
  /// </summary>
  public class FieldProblem_Norbi
  {
    public interface ITest<T>
    {
      Func<T> Get(string text);
    }

    public interface IParam
    {
      string Text { get; }
    }

    public class Param : IParam
    {
      public string Text { get; set; }
    }

    [Fact]
    public void Test()
    {
      var mock = MockRepository.GenerateMock<ITest<IParam>>();
      mock.Expect(m => m.Get("Test1")).Return(() => new Param { Text = "ParamWithText1" }); // OK
      mock.Expect(m => m.Get("Test2")).Return(() => new Param { Text = "ParamWithText2" }); // Exception

      Assert.Equal("ParamWithText1", mock.Get("Test1")().Text);
      Assert.Equal("ParamWithText2", mock.Get("Test2")().Text);

      mock.VerifyAllExpectations();
    }
  }
}
