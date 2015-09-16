//New test for multiStub
using Xunit;

namespace Rhino.Mocks.Tests
{
    public class MultiStubWithAAA
    {
        [Fact]
        public void MultiStubShouldGenerateStubWithOnlyBaseClass()
        {
            var animal = (IAnimal)MockRepository.GenerateStub(typeof(IAnimal), null, null);

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