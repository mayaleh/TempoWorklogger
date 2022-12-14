using TempoWorklogger.Model;

namespace TempoWorklogger.Library.Extensions
{
    public static class IntervalExtension
    {
        public static string GetSummarizedTotalTime(this IEnumerable<IInterval> intervals)
        {
            return intervals.SummarizeIntervals()
                .FormatTotalTime();
        }

        public static string GetSummarizedTotalTime(this IEnumerable<IIntervalNullable> intervals)
        {
            return intervals.SummarizeIntervals()
                .FormatTotalTime();
        }

        public static TimeSpan SummarizeIntervals(this IEnumerable<IInterval> intervals)
        {
            return intervals.Aggregate(TimeSpan.Zero, (total, interval) => total + (interval.EndTime - interval.StartTime));
        }

        public static TimeSpan SummarizeIntervals(this IEnumerable<IIntervalNullable> intervals)
        {
            return intervals.Where(x => x.StartTime.HasValue && x.EndTime.HasValue)
                    .Aggregate(TimeSpan.Zero, (total, interval) => total + (interval.EndTime.Value - interval.StartTime.Value));
        }

        public static string FormatTotalTime(this TimeSpan total)
        {
            int hours = (int)total.TotalHours;
            int minutes = total.Minutes;
            return $"{hours}h {minutes}min";
        }
    }
}
