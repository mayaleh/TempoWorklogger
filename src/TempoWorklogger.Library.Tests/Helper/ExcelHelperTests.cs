using TempoWorklogger.Library.Helper;

namespace TempoWorklogger.Library.Tests.Helper
{
    public class ExcelHelperTests
    {
        [Theory]
        [InlineData("A", 0)]
        [InlineData("G", 6)]
        [InlineData("Z", 25)]
        //[InlineData("AA", 26)]
        //[InlineData("AB", 27)]
        //[InlineData("AZ", 51)]
        //[InlineData("BA", 52)]
        //[InlineData("ABD", 731)]
        //[InlineData("ACR", 771)]
        public void GetColumnIndex_ShouldReturnIndexOfTheColumn(string column, int expectedIndex)
        {
            // arrange
            // actual
            var actualIndex = ExcelHelper.GetColumnIndex(column);

            // asserts
            Assert.Equal(expectedIndex, actualIndex);
        }
    }
}
