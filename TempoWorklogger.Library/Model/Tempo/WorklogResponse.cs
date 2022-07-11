#nullable enable
namespace TempoWorklogger.Library.Model.Tempo
{
    public record Result<T>(Metadata metadata, IEnumerable<T> results, string self);

    public record Metadata(
        int? Count,
        int? Limit,
        string? Next,
        int? Offset,
        string? Previous);

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
        int? startTime,
        int? tempoWorklogId,
        int? timeSpentSeconds,
        DateTime? updatedAt
   );

    public record Attributes(string? self, object[]? values);

    public record Author(string? accountId, string? self);

    public record Issue(int? id, string? key, string? self);
}
