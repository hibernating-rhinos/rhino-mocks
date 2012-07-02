namespace Rhino.Mocks.Tests.FieldsProblem
{
  using Xunit;

  /// <summary>
  /// Problem discussed in google forum.
  /// </summary>
  /// <remarks>
  /// See https://groups.google.com/d/topic/rhinomocks/tMAbfs2qBec/discussion.
  /// </remarks>
  public class FieldProblem_Honggoff
  {
    /// <summary>
    /// Only a dummy interface used by the test.
    /// </summary>
    public interface IDummy
    {
      /// <summary>
      /// Dummy property.
      /// </summary>
      /// <returns>Whatever you like.</returns>
      bool GetValue();
    }

    /// <summary>
    /// Simple test illustrating the problem.
    /// </summary>
    [Fact]
    public void TestBackToRecordAll()
    {
      MockRepository mock = new MockRepository();
      IDummy test = mock.StrictMock<IDummy>();

      using (mock.Unordered())
      {
        Expect.Call(test.GetValue())
          .Return(true)
          .Repeat.AtLeastOnce();
      }

      mock.ReplayAll();

      Assert.True(test.GetValue());
      mock.VerifyAll();

      mock.BackToRecordAll(BackToRecordOptions.All);
      Expect.Call(test.GetValue())
        .Return(false)
        .Repeat.AtLeastOnce();

      mock.ReplayAll();

      Assert.False(test.GetValue());
      mock.VerifyAll();
    }
  }
}
