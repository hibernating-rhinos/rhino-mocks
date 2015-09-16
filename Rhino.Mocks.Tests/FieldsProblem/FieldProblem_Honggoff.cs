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
      IDummy test = MockRepository.GenerateStrictMock<IDummy>();

      test.Expect(x => x.GetValue()).Return(true).Repeat.AtLeastOnce();

      Assert.True(test.GetValue());
      test.VerifyAllExpectations();

      test.BackToRecord(BackToRecordOptions.All);
      test.Expect(x => x.GetValue()).Return(false).Repeat.AtLeastOnce();
      test.Replay();

      Assert.False(test.GetValue());
      test.VerifyAllExpectations();
    }
  }
}
