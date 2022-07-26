using Maya.AnyHttpClient;
using Maya.AnyHttpClient.Model;
using Maya.Ext.Rop;
using Microsoft.Extensions.Logging;
using NPOI.POIFS.Crypt.Dsig;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TempoWorklogger.Library.Model.Tempo;

namespace TempoWorklogger.Library.Service
{
    public class TempoService : ITempoService //Maya.AnyHttpClient.ApiService, ITempoService
    {
#if DEBUG
        //const string TempoUri = "https://localhost:7201";
        //const int TimeoutRequestSeconds = 600;
        
        const string TempoUri = "https://api.tempo.io/4";
        const int TimeoutRequestSeconds = 30;
#else
        const string TempoUri = "https://api.tempo.io/4";
        const int TimeoutRequestSeconds = 30;
#endif

        private readonly string accessToken;
        //public TempoService(string accessToken) : base(new Maya.AnyHttpClient.Model.HttpClientConnector
        //{
        //    Endpoint = TempoUri,
        //    TimeoutSeconds = TimeoutRequestSeconds,
        //    AuthType = Maya.AnyHttpClient.Model.AuthTypeKinds.Bearer,
        //    Token = accessToken
        //})
        //{
        //}
        public TempoService(string accessToken)
        {
            this.accessToken = accessToken;
        }

        private async Task<Result<T, Exception>> HttpPost<T>(UriRequest uriRequest, object data, Func<HttpResponseMessage, Exception>? onError = null)
        {
            var logAction = $"{nameof(TempoService)}.{nameof(TempoService.HttpPost)}";
            var content = "";
            try
            {
                if (uriRequest == null)
                {
                    return Result<T, Exception>.Failed(new ArgumentNullException(nameof(uriRequest)));
                }

                Uri? uri = null;

                var isValidUri = uriRequest.TryGetUri(TempoUri, out uri);

                if (isValidUri == false)
                {
                    return Result<T, Exception>.Failed(new Exception("Invalid uriRequest"));
                }

                var httpClientHandler = new HttpClientHandler();

                using (var client = new HttpClient(httpClientHandler)
                {
                    Timeout = TimeSpan.FromSeconds(TimeoutRequestSeconds)
                })
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", this.accessToken);
                    
                    var bodyContent = JsonSerializer.Serialize(data,
                        new JsonSerializerOptions
                        {
                            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                    using (var message = new HttpRequestMessage(HttpMethod.Post, uri)
                    {
                        Content = new System.Net.Http.StringContent(
                            bodyContent,
                            Encoding.UTF8,
                            "application/json")
                    })
                    {
                        var httpResponseMessage = await client.SendAsync(message);

                        if (httpResponseMessage.IsSuccessStatusCode)
                        {
                            if (typeof(T) == typeof(Maya.Ext.Unit)) // void is not value type, this is for response, that has not any body response
                            {
                                return Result<T, Exception>.Succeeded((T)Convert.ChangeType(Maya.Ext.Unit.Default, typeof(T)));
                            }

                            if (typeof(T) == typeof(byte[]))
                            {
                                var result = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                                return Result<T, Exception>.Succeeded((T)Convert.ChangeType(result, typeof(T)));
                            }

                            content = await httpResponseMessage.Content.ReadAsStringAsync();

                            if (typeof(T) == typeof(string))
                            {
                                return Result<T, Exception>.Succeeded((T)Convert.ChangeType(content, typeof(T)));
                            }

                            T reusultData = JsonSerializer.Deserialize<T>(content);

                            return Result<T, Exception>.Succeeded(reusultData);
                        }

                        if (onError != null)
                        {
                            var ownException = onError.Invoke(httpResponseMessage);

                            return Result<T, Exception>.Failed(ownException);
                        }

                        return Result<T, Exception>.Failed(new Exception(await httpResponseMessage.Content.ReadAsStringAsync()));
                    }
                }
            }
            catch (TaskCanceledException ex)
            {
                if (ex.CancellationToken.IsCancellationRequested)
                {
                    return Result<T, Exception>.Failed(ex);
                }
                return Result<T, Exception>.Failed(ex);
            }
            catch (Exception e)
            {
                return Result<T, Exception>.Failed(e);
            }
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
