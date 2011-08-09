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
            var mockRepository = new MockRepository();
            var animal = (IAnimal)mockRepository.Stub(typeof(IAnimal), new[] { typeof(ICat) });
            var cat = (ICat)animal;

            Assert.NotNull(cat);
            Assert.NotNull(animal);
        }

        [Fact]
        public void MultiStubShouldGenerateStubWithOnlyBaseClass()
        {
            var mockRepository = new MockRepository();
            var animal = (IAnimal)mockRepository.Stub(typeof(IAnimal), null, null);

            Assert.NotNull(animal);
            Assert.Null(animal as ICat);
        }

        [Fact]
        public void StaticMultiStubMethodCanGenerateStubWithMultiInterfaces()
        {
            var animal = (IAnimal)MockRepository.GenerateStub(typeof(IAnimal), new[] { typeof(ICat) });
            var cat = (ICat)animal;

            Assert.NotNull(animal);
            Assert.NotNull(cat);
        }

        [Fact]
        public void StaticGenericMultiStubMethodCanGenerateStubWithMultiInterfaces()
        {
          var animal = MockRepository.GenerateStub<IAnimal, ICat>();
          var cat = (ICat)animal;

          Assert.NotNull(animal);
          Assert.NotNull(cat);
        }
    }
}