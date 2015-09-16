using Rhino.Mocks.Tests.Model;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_PropertyBehaviorOnInternalProperty
    {
        [Fact]
        public void CanUsePropertyBehaviorOnInternalProperty()
        {
            var mock = MockRepository.GenerateMock<Internal>();
            mock.Stub(m => m.Baz).PropertyBehavior();

            mock.Baz = "baz";

            Assert.Equal("baz", mock.Baz);
        }
    }
}
