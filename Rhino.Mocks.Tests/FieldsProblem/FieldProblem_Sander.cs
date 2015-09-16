using System;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Sander
    {
        [Fact]
        public void CanUseOutIntPtr()
        {
            IFooWithOutIntPtr mock = MockRepository.GenerateStrictMock<IFooWithOutIntPtr>();
            IntPtr parameter;
            mock.Expect(x => x.GetBar(out parameter)).IgnoreArguments().Return(5).OutRef(new IntPtr(3));
            Assert.Equal(5, mock.GetBar(out parameter));
            Assert.Equal(new IntPtr(3), parameter);
            mock.VerifyAllExpectations();
        }
    }

    public interface IFooWithOutIntPtr
    {
        int GetBar(out IntPtr parameter);
    }
}