using Maya.Ext.Rop;
using System.Net;
using System.Text;
using TempoWorklogger.Library.Model.Tempo;
using static System.Net.Mime.MediaTypeNames;

namespace TempoWorklogger.Library.Service
{
    interface ITempoService
    {
        Task<Result<WorklogResponse, Exception>> CreateWorklog(Worklog worklog);
    }

    internal class TempoService : Maya.AnyHttpClient.ApiService, ITempoService
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
        ///           "authorAccountId": "62975e0e658d580068890373",
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
            //todo Date format to YYYY-MM-DD
            var uriRequest = new Maya.AnyHttpClient.Model.UriRequest(new string[] { "worklogs" });

            var result = await this.HttpPost<WorklogResponse>(uriRequest, worklog)
                .ConfigureAwait(false);

            return result;
        }
    }
}
