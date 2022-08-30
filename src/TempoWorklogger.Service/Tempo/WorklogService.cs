using Maya.Ext.Rop;
using TempoWorklogger.Contract.Services.Tempo;
using TempoWorklogger.Model.Tempo;

namespace TempoWorklogger.Service.Tempo
{
    public class WorklogService : BaseTempoApiService, IWorklogService
    {
        public WorklogService(string accessToken) : base(accessToken)
        {
        }

        public async Task<Result<WorklogResponse, Exception>> Create(Worklog worklog)
        {
            try
            {
                var uriRequest = new Maya.AnyHttpClient.Model.UriRequest(new string[] { "worklogs" });

                return await this.HttpPost<WorklogResponse>(uriRequest, worklog)
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return Result<WorklogResponse, Exception>.Failed(e);
            }
        }
    }
}
