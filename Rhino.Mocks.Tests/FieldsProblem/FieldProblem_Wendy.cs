using Xunit;
using Rhino.Mocks.Exceptions;

namespace Rhino.Mocks.Tests.FieldsProblem
{
    public class FieldProblem_Wendy
    {
        private ISearchPatternBuilder _searchPatternBuilder;
        private ImageFinder _imageFinder;

		public FieldProblem_Wendy()
        {
            _searchPatternBuilder = MockRepository.GenerateMock<ISearchPatternBuilder>();
            _imageFinder = new ImageFinder(_searchPatternBuilder);
        }

        [Fact]
        public void SendingNullParamsValueShouldNotThrowNullReferenceException()
        {
            _searchPatternBuilder.Expect(x => x.CreateFromExtensions(ImageFinder.ImageExtensions)).Return(null);
            _imageFinder.FindImagePath();
        	var ex = Assert.Throws<ExpectationViolationException>(() => this.Verify(this._searchPatternBuilder));
        	Assert.Equal("ISearchPatternBuilder.CreateFromExtensions([\"png\", \"gif\", \"jpg\", \"bmp\"]); Expected #1, Actual #0.", ex.Message);
        }

		[Fact]
		public void VerifyShouldFailIfDynamicMockWasCalledWithRepeatNever()
		{
			_searchPatternBuilder.Expect(x => x.CreateFromExtensions()).Repeat.Never();
			try
			{
				_searchPatternBuilder.CreateFromExtensions();
			}
			catch 
			{
				
			}
			var ex = Assert.Throws<ExpectationViolationException>(() => this.Verify(this._searchPatternBuilder));
			Assert.Equal("ISearchPatternBuilder.CreateFromExtensions([]); Expected #0, Actual #1.", ex.Message);
		}

        private void Verify(ISearchPatternBuilder builder)
        {
            builder.VerifyAllExpectations();
        }

        public string FindImagePath(string directoryToSearch)
        {
            _searchPatternBuilder.CreateFromExtensions(null);
            return null;
        }

        public interface ISearchPatternBuilder
        {
            string CreateFromExtensions(params string[] extensions);
        }

        public class ImageFinder
        {
            private readonly ISearchPatternBuilder builder;
            public static string[] ImageExtensions = { "png", "gif", "jpg", "bmp" };

            public ImageFinder(ISearchPatternBuilder builder)
            {
                this.builder = builder;
            }

            public void FindImagePath()
            {
                builder.CreateFromExtensions(null);
            }
        }
    }
}
