using Maya.Ext.Rop;
using TempoWorklogger.Library.Helper;
using TempoWorklogger.Model.Db;
using WorklogDb = TempoWorklogger.Model.Db.Worklog;
using WorklogTempo = TempoWorklogger.Model.Tempo.Worklog;

namespace TempoWorklogger.Library.Mapper.Tempo
{
    using worklogResult = Result<WorklogTempo, Exception>;
    public static class WorklogFromDbWorkog
    {
        public static worklogResult MapWorklogDbToWorklogTempo(this WorklogDb worklogDb, IntegrationSettings integrationSettings)
        {
            try
            {
                var tempoWorklog = new WorklogTempo()
                {
                    IssueKey = worklogDb.IssueKey,
                    Description = worklogDb.Description,
                    AuthorAccountId = integrationSettings.AuthorAccountId,
                    StartDate = DateOnly.FromDateTime(worklogDb.StartDate),
                    StartTime = TimeOnly.FromDateTime(worklogDb.StartTime),
                    TimeSpentSeconds = WorklogHelper.CalculateTimeSpentSeconds(worklogDb.TimeSpentSeconds, worklogDb.StartTime, worklogDb.EndTime),
                    Attributes = worklogDb.Attributes.Select(x => x.MapAttributeDbByToTempo()).ToList()
                };
                return worklogResult.Succeeded(tempoWorklog);
            }
            catch (Exception e)
            {
                return worklogResult.Failed(e);
            }
        }

        private static TempoWorklogger.Model.Tempo.AttributeKeyVal MapAttributeDbByToTempo(this CustomAttributeKeyVal attributeKeyValDb)
        {
            var tempoAttr = new Model.Tempo.AttributeKeyVal
            {
                Key = attributeKeyValDb.Key, // columnDefinition.Name[ModelsConstant.AttributePrefix.Length..]
                Value = attributeKeyValDb.Value
            };

            return tempoAttr;
        }
    }
}
