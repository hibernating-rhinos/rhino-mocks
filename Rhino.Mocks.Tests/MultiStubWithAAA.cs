//New test for multiStub
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Rhino.Mocks.Tests
{
    public class MultiStubWithAAA
    {
        [Fact]
        public void MultiStubShouldGenerateStubWithMultiInterfaces()
        {
            MockRepository mockRepository = new MockRepository();
            var animal = (IAnimal)mockRepository.Stub(typeof(IAnimal), new Type[] { typeof(ICat) });
            var cat = (ICat) animal;

            Assert.NotNull(cat);
            Assert.NotNull(animal);
        }

        [Fact]
        public void MultiStubShouldGenerateStubWithOnlyBaseClass()
        {
            MockRepository mockRepository = new MockRepository();
            var animal = (IAnimal)mockRepository.Stub(typeof(IAnimal), null, null);

            Assert.NotNull(animal);
            Assert.Null(animal as ICat);
        }

        [Fact]
        public void StatciMultiStubMethodCanGenerateStubWithMultiInterfaces()
        {
            var animal = (IAnimal)MockRepository.GenerateStub(typeof(IAnimal), new Type[] { typeof(ICat) });
            var cat = (ICat) animal;

            Assert.NotNull(animal);
            Assert.NotNull(cat);
        }
    }
}