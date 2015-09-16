namespace Rhino.Mocks.Tests.FieldsProblem
{
    using Xunit;

    /// <summary>
    /// Problem reported by Steinkauz (https://groups.google.com/forum/?fromgroups=#!topic/RhinoMocks/gta6a6bHhT8).
    /// </summary>
    public class FieldProblem_Steinkauz
    {
        /// <summary>
        /// Just a interface for mocking.
        /// </summary>
        public interface IInterface
        {
            /// <summary>
            /// Some generic function.
            /// </summary>
            /// <typeparam name="T">A arbitrary type parameter.</typeparam>
            /// <param name="message">The message.</param>
            void SendMessage<T>(T message);
        }

        /// <summary>
        /// Expose problem with handling of generic methods.
        /// </summary>
        [Fact]
        public void Test()
        {
            var mock = MockRepository.GenerateMock<IInterface>();
            mock.SendMessage(5);

            mock.AssertWasCalled(x => x.SendMessage(Arg<object>.Is.Anything));
            mock.AssertWasCalled(x => x.SendMessage(Arg<int>.Is.Anything));
            mock.AssertWasNotCalled(x => x.SendMessage(Arg<string>.Is.Anything));
        }
    }
}
