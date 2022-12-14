using TempoWorklogger.Library.Helper;

namespace TempoWorklogger.Library.Tests.Helper
{
    public class ExcelHelperTests
    {
        [Theory]
        [InlineData("A", 0)]
        [InlineData("B", 1)]
        [InlineData("G", 6)]
        [InlineData("Z", 25)]
        [InlineData("AA", 26)]
        [InlineData("AN", 39)]
        [InlineData("BA", 52)]
        [InlineData("ZZ", 701)]
        [InlineData("AAA", 702)]
        public void GetColumnIndex_ShouldReturnIndexOfTheColumn_WhenColumnNameGiven(string column, int expectedIndex)
        {
            // arrange
            // actual
            var actualIndex = ExcelHelper.GetColumnIndex(column);

            // asserts
            Assert.Equal(expectedIndex, actualIndex);
        }
    }
}
