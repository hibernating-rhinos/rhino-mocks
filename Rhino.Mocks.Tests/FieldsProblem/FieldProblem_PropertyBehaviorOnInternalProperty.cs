using Rhino.Mocks.Tests.Model;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_PropertyBehaviorOnInternalProperty
    {
        [Fact]
        public void CanUsePropertyBehaviorOnInternalProperty()
        {
            MockRepository mocks = new MockRepository();
            var mock = mocks.PartialMock<Internal>();
            mock.Stub(m => m.Baz).PropertyBehavior();

            mock.Baz = "baz";

            Assert.Equal("baz", mock.Baz);
        }
    }
}
