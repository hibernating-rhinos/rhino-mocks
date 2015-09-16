using System;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public interface IBase
    {
        String BaseString { get; set; }
    }

    public interface IChild : IBase
    {
        String ChildString { get; set; }
    }

    public class StubDemoTestFixture : IDisposable
    {
        private IBase _mockBase;
        private IChild _mockChild;

        public StubDemoTestFixture()
        {
            _mockBase = MockRepository.GenerateStub<IBase>();
            _mockChild = MockRepository.GenerateStub<IChild>();
        }

        public void Dispose()
        {
            _mockBase.VerifyAllExpectations();
            _mockChild.VerifyAllExpectations();
        }

        [Fact]
        public void BaseStubSetsBasePropertiesCorrectly()
        {
            String str = "Base stub";

            _mockBase.BaseString = str;

            Assert.Equal(str, _mockBase.BaseString);
        }

        [Fact]
        public void ChildStubSetsChildPropertiesCorrectly()
        {
            String str = "Child stub";

            _mockChild.ChildString = str;

            Assert.Equal(str, _mockChild.ChildString);
        }

        [Fact]
        public void ChildStubSetsBasePropertiesCorrectly()
        {
            String str = "Child's base stub";

            _mockChild.BaseString = str;

            Assert.Equal(str, _mockChild.BaseString);
        }
    }
}
