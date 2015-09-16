using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Greg
    {
        [Fact]
        public void IgnoreArguments()
        {
            IFoo myFoo = MockRepository.GenerateStrictMock<IFoo>();
            IBar<int> myBar = MockRepository.GenerateStrictMock<IBar<int>>();

            myFoo.Expect(x => x.RunBar(myBar)).IgnoreArguments().Return(true);

            Example<int> myExample = new Example<int>(myFoo, myBar);
            bool success = myExample.ExampleMethod();
            Assert.True(success);

            myFoo.VerifyAllExpectations();
            myBar.VerifyAllExpectations();
        }
    }

    public class Example<T>
    {
        private readonly IBar<T> _bar;
        private readonly IFoo _foo;

        public Example(IFoo foo, IBar<T> bar)
        {
            _foo = foo;
            _bar = bar;
        }

        public bool ExampleMethod()
        {
            bool success = _foo.RunBar(_bar);
            return success;
        }
    }

    public interface IFoo
    {
        bool RunBar<T>(IBar<T> barObject);
    }

    public interface IBar<T>
    {
        void BarMethod(T paramToBarMethod);
    }

    public class Foo : IFoo
    {
        //When Foo is mocked, this method returns FALSE!!!

        #region IFoo Members

        public bool RunBar<T>(IBar<T> barObject)
        {
            return true;
        }

        #endregion
    }

    public class Bar<T> : IBar<T>
    {
        #region IBar<T> Members

        public void BarMethod(T paramToBarMethod)
        {
            //nothing important
        }

        #endregion
    }
}