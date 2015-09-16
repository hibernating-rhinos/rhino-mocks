using System;
using Xunit;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Harley
    {
        public delegate void ChangeTestEvent(bool value);
        public interface ClassToMock
        {
            event ChangeTestEvent ChangeTestProperty;
        }

        [Fact]
        public void TestSampleMatrixChanged()
        {
            var mockTestClass = MockRepository.GenerateMock<ClassToMock>();
            var fireChangeTestProperty = mockTestClass.Expect(x => x.ChangeTestProperty += null).IgnoreArguments().GetEventRaiser();
            new ClassRaisingException(mockTestClass);

            Assert.Throws<ArgumentOutOfRangeException>(() => fireChangeTestProperty.Raise(true));
        }
    }

    public class ClassRaisingException
    {
        public ClassRaisingException(FieldProblem_Harley.ClassToMock eventRaisingClass)
        {
            eventRaisingClass.ChangeTestProperty += handleTestEvent;
        }

        public void handleTestEvent(bool value)
        {
            if (value)
                throw new ArgumentOutOfRangeException();
        }
    }
}
