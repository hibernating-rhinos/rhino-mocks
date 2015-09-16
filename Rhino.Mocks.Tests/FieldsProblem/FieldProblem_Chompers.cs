using Xunit;
using System.IO;

namespace Rhino.Mocks.Tests.FieldsProblem
{
	public class FieldProblem_Chompers
	{
		[Fact]
		public void MockingPropertyThatReturnsStream()
		{
			IBaseMessagePart messagePart = MockRepository.GenerateStrictMock<IBaseMessagePart>();
			MemoryStream stream = new MemoryStream();
			messagePart.Expect(x => x.Data).Return(stream).Repeat.Any();
			messagePart.Data.WriteByte(127);
			stream.Seek(0, SeekOrigin.Begin);
			Assert.Equal(127,  stream.ReadByte());
		}
	}

	public interface IBaseMessagePart
	{
		Stream Data { get; set; }
	}
}
