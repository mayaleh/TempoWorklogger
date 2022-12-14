using FluentAssertions;
using Moq;
using TempoWorklogger.Library.Extensions;
using TempoWorklogger.Model;

namespace TempoWorklogger.Library.Tests.Extensions
{
    public class IntervalExtensionTests
    {
        [Theory]
        [InlineData(30, 20)]
        [InlineData(0, 20)]
        public void FormatTotalTime_ShouldReturnFormattedString_WhenInputIsValid(int hours, int minutes)
        {
            // Arrange
            var total = new TimeSpan(hours, minutes, 0);

            // Act
            var result = total.FormatTotalTime();

            // Assert
            result.Should().Be($"{hours}h {minutes}min");
        }

        [Fact]
        public void SummarizeIntervals_ShouldReturnZero_WhenGivenEmptyCollection()
        {
            // Arrange
            IEnumerable<IInterval> intervals = new List<IInterval>();

            // Act
            var result = intervals.SummarizeIntervals();

            // Assert
            result.Should().Be(TimeSpan.Zero);
        }

        [Fact]
        public void SummarizeIntervals_ShouldReturnCorrectDuration_WhenGivenSingleInterval()
        {
            // Arrange
            var interval = BuildInterval(new DateTime(2022, 12, 11, 8, 0, 0), new DateTime(2022, 12, 11, 9, 30, 0));

            IEnumerable<IInterval> intervals = new List<IInterval> { interval };

            // Act
            var result = intervals.SummarizeIntervals();

            // Assert
            result.Should().Be(new TimeSpan(0, 1, 30, 0));
        }

        [Fact]
        public void SummarizeIntervals_ShouldReturnCorrectDuration_WhenGivenMultipleIntervals()
        {
            // Arrange
            var interval1 = BuildInterval(new DateTime(2022, 12, 11, 8, 0, 0), new DateTime(2022, 12, 11, 9, 30, 0)); // 1h 30min
            var interval2 = BuildInterval(new DateTime(2022, 12, 11, 10, 0, 0), new DateTime(2022, 12, 11, 11, 30, 0)); // 1h 30min

            IEnumerable<IInterval> intervals = new List<IInterval> { interval1, interval2 };

            // Act
            var result = intervals.SummarizeIntervals(); ;

            // Assert
            result.Should().Be(new TimeSpan(0, 3, 0, 0));
        }

        [Fact]
        public void SummarizeIntervals_ShouldReturnCorrectDuration_WhenGivenMultipleIntervalsWithSomeEmptyDurations()
        {
            // Arrange
            var interval1 = BuildIntervalNullable(new DateTime(2022, 12, 11, 8, 0, 0), new DateTime(2022, 12, 11, 9, 30, 0)); // 1h 30min
            var interval2 = BuildIntervalNullable(new DateTime(2022, 12, 11, 10, 0, 0), new DateTime(2022, 12, 11, 11, 30, 0)); // 1h 30min
            var interval3 = BuildIntervalNullable(null, new DateTime(2022, 12, 11, 11, 30, 0)); // should be ignored
            var interval4 = BuildIntervalNullable(new DateTime(2022, 12, 11, 11, 30, 0), null); // should be ignored
            var interval5 = BuildIntervalNullable(null, null); // should be ignored

            IEnumerable<IIntervalNullable> intervals = new List<IIntervalNullable> { interval1, interval2, interval3, interval4, interval5 };

            // Act
            var result = intervals.SummarizeIntervals(); ;

            // Assert
            result.Should().Be(new TimeSpan(0, 3, 0, 0));
        }

        private static IInterval BuildInterval(DateTime stratTime, DateTime endTime)
        {
            var interval = new Mock<IInterval>();
            interval.Setup(i => i.StartTime).Returns(stratTime);
            interval.Setup(i => i.EndTime).Returns(endTime);

            return interval.Object;
        }

        private static IIntervalNullable BuildIntervalNullable(DateTime? stratTime, DateTime? endTime)
        {
            var interval = new Mock<IIntervalNullable>();
            interval.Setup(i => i.StartTime).Returns(stratTime);
            interval.Setup(i => i.EndTime).Returns(endTime);

            return interval.Object;
        }
    }
}
