namespace TempoWorklogger.Model
{
    /// <summary>
    /// Represents interval contract.
    /// </summary>
    public interface IInterval
    {
        /// <summary>
        /// Start date only.
        /// </summary>
        DateTime StartDate { get; }

        /// <summary>
        /// Start date time.
        /// </summary>
        DateTime StartTime { get; }

        /// <summary>
        /// End date only.
        /// </summary>
        DateTime EndDate { get; }

        /// <summary>
        /// End date time.
        /// </summary>
        DateTime EndTime { get; }
    }

    public interface IIntervalNullable
    {
        /// <summary>
        /// Start date time.
        /// </summary>
        DateTime? StartTime { get; }

        /// <summary>
        /// End date time.
        /// </summary>
        DateTime? EndTime { get; }
    }
}
