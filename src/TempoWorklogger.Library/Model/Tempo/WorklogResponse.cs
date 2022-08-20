#nullable enable
namespace TempoWorklogger.Library.Model.Tempo
{
    [Obsolete]
    public record Result<T>(Metadata metadata, IEnumerable<T> results, string self);

    public record Metadata(
        int? Count,
        int? Limit,
        string? Next,
        int? Offset,
        string? Previous);
    [Obsolete]
    public record WorklogResponse(
        Attributes? attributes,
        Author? author,
        int? billableSeconds,
        DateTime? createdAt,
        string? description,
        Issue? issue,
        int? jiraWorklogId,
        string? self,
        string? startDate,
        string? startTime,
        int? tempoWorklogId,
        int? timeSpentSeconds,
        DateTime? updatedAt
   );
    [Obsolete]
    public record Attributes(string? self, object[]? values);
    [Obsolete]
    public record Author(string? accountId, string? self);
    [Obsolete]
    public record Issue(int? id, string? key, string? self);
}
