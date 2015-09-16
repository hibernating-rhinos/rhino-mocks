namespace Rhino.Mocks.Tests.FieldsProblem
{
	using System;
	using Xunit;

	public class FieldProblem_Lopez
	{
		public interface GenericContainer<T>
		{
			T Item { get; set; }
		}


		[Fact]
		public void PropertyBehaviorForSinglePropertyTypeOfString()
		{
			GenericContainer<string> stringContainer = MockRepository.GenerateStrictMock<GenericContainer<string>>();

			stringContainer.Expect(x => x.Item).PropertyBehavior();

			for (int i = 1; i < 49; ++i)
			{
				string newItem = i.ToString();

				stringContainer.Item = newItem;

				Assert.Equal(newItem, stringContainer.Item);
			}

			stringContainer.VerifyAllExpectations();
		}


		[Fact]
		public void PropertyBehaviourForSinglePropertyTypeOfDateTime()
		{
			GenericContainer<DateTime> dateTimeContainer = MockRepository.GenerateStrictMock<GenericContainer<DateTime>>();

			dateTimeContainer.Expect(x => x.Item).PropertyBehavior();

			for (int i = 1; i < 12; i++)
			{
				DateTime date = new DateTime(2007, i, i);

				dateTimeContainer.Item = date;

				Assert.Equal(date, dateTimeContainer.Item);
			}

			dateTimeContainer.VerifyAllExpectations();
		}


		[Fact]
		public void PropertyBehaviourForSinglePropertyTypeOfInteger()
		{
			GenericContainer<int> dateTimeContainer = MockRepository.GenerateStrictMock<GenericContainer<int>>();

			dateTimeContainer.Expect(x => x.Item).PropertyBehavior();

			for (int i = 1; i < 49; i++)
			{
				dateTimeContainer.Item = i;

				Assert.Equal(i, dateTimeContainer.Item);
			}

			dateTimeContainer.VerifyAllExpectations();
		}
	}
}