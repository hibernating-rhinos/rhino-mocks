namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System.Threading;
	using Xunit;

	public class FieldProblem_Naraga
	{
		public interface IService
		{
			void Do(string msg);
		}

		[Fact]
		public void MultiThreadedReplay()
		{
			var service = MockRepository.GenerateStrictMock<IService>();
			for (int i = 0; i < 100; i++)
			{
				int i1 = i;

				service.Expect(x => x.Do("message" + i1));
			}

			int counter = 0;
			for (int i = 0; i < 100; i++)
			{
				var i1 = i;
				ThreadPool.QueueUserWorkItem(delegate
				{
					service.Do("message" + i1);
					Interlocked.Increment(ref counter);
				});
			}

			while (counter != 100)
				Thread.Sleep(100);

			service.VerifyAllExpectations();
		}
	}
}
