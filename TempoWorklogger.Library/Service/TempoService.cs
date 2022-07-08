using Maya.Ext.Rop;
using NPOI.SS.Formula.Functions;
using System.Net;
using System.Text;
using TempoWorklogger.Library.Model.Tempo;
using static System.Net.Mime.MediaTypeNames;

namespace TempoWorklogger.Library.Service
{
    public class TempoService : Maya.AnyHttpClient.ApiService, ITempoService
    {
        const string TempoUri = "https://api.tempo.io/4";
        const int TimeoutRequestSeconds = 30;

        public TempoService(string accessToken) : base(new Maya.AnyHttpClient.Model.HttpClientConnector
        {
            Endpoint = TempoUri,
            TimeoutSeconds = TimeoutRequestSeconds,
            AuthType = Maya.AnyHttpClient.Model.AuthTypeKinds.Bearer,
            Token = accessToken
        })
        {

        }

        /// <summary>
        ///     Worklogs
        ///         POST
        ///         {{ baseUrl}}/ worklogs
        ///         Authorization: Bearer {{ apiTokenTempo}}
        ///         Accept: application / json
        ///         Accept - Encoding: gzip, deflate, br
        ///         Accept-Language: cs,en; q = 0.9,en - GB; q = 0.8,en - US; q = 0.7
        ///         Content - Type: application / json
        /// 
        ///         {
        ///             "attributes": [
        ///               {
        ///                 "key": "_Account_",
        ///               "value": "DODODEFAUL"
        ///               }
        ///           ],
        ///           "authorAccountId": "<account id>",
        ///           "description": "implement",
        ///           "issueKey": "IDODOEC-8121",
        ///           "startDate": "2022-08-06",
        ///           "startTime": "6:45:00",
        ///           "timeSpentSeconds": 1800
        ///         }
        /// </summary>
        /// <param name="worklog"></param>
        /// <returns></returns>
        public async Task<Result<WorklogResponse, Exception>> CreateWorklog(Worklog worklog)
        {
            try
            {
                var uriRequest = new Maya.AnyHttpClient.Model.UriRequest(new string[] { "worklogs" });

                return await this.HttpPost<Result<WorklogResponse>>(uriRequest, worklog)
                    .MapAsync(success => Task.FromResult(success.Results.First()))
                    .ConfigureAwait(false);
            }
            catch (Exception e)
            {
                return Result<WorklogResponse, Exception>.Failed(e);
            }
        }
    }
}
