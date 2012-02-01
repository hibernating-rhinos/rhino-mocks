namespace Rhino.Mocks.Tests.FieldsProblem
{
  using Xunit;

  /// <summary>
  /// Reproduces a problem with indexers reported by Ted.
  /// </summary>
  /// <remarks>
  /// See also http://groups.google.com/group/rhinomocks/browse_thread/thread/5c9d8fcf529d33e3.
  /// </remarks>
  public class FieldProblem_Ted
  {
    /// <summary>
    /// Just a simple constant object used as a reference during the tests.
    /// </summary>
    private static readonly object constant = new object();

    [Fact]
    public void AccessReadOnlyIndexerReturnsDefaultValue()
    {
      var item = MockRepository.GenerateStub<IReadOnlyIndexer>();
      Assert.Null(item[null]);
    }

    [Fact]
    public void AccessReadOnlyIndexerReturnsStubbedValue()
    {
      var item = MockRepository.GenerateStub<IReadOnlyIndexer>();
      item.Stub(x => x[null]).Return(constant);
      Assert.Equal(constant, item[null]);
    }

    [Fact]
    public void AccessIndexerWithNullArgumentReturnsDefaultValue()
    {
      var item = MockRepository.GenerateStub<IIndexer>();
      Assert.Null(item[null]);
    }

    [Fact]
    public void AccessIndexerWithNonNullArgumentReturnsDefaultValue()
    {
      var item = MockRepository.GenerateStub<IIndexer>();
      Assert.Null(item["name"]);
    }

    [Fact]
    public void IndexerShowsPropertyBehavior()
    {
      var item = MockRepository.GenerateStub<IIndexer>();
      Assert.Null(item[null]);
      Assert.Null(item["name"]);

      item[null] = constant;
      Assert.Equal(constant, item[null]);
    }

    public interface IReadOnlyIndexer
    {
      object this[string name] { get; }
    }

    public interface IIndexer
    {
      object this[string name] { get; set; }
    }
  }
}
