namespace TempoWorklogger.Library.Helper
{
    public static class WorklogHelper
    {
        public static int CalculateTimeSpentSeconds(int actualSeconds, DateTime startTime, DateTime endTime)
        {
            if (actualSeconds == CommonConstants.Zero && startTime.Ticks != endTime.Ticks && endTime.Year != CommonConstants.Zero && startTime.Year != CommonConstants.Zero)
            {
                return (int)(endTime - startTime).TotalSeconds;
            }

            return actualSeconds;
        }
    }
}
