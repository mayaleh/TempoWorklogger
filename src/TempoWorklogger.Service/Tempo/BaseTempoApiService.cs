﻿using System.Text.Json;

namespace TempoWorklogger.Service.Tempo
{
    public abstract class BaseTempoApiService : Maya.AnyHttpClient.ApiService
    {
#if DEBUG
        //const string TempoUri = "https://localhost:7201";
        //const int TimeoutRequestSeconds = 600;
        //const string TempoUri = "https://localhost:44373/api";
        //const int TimeoutRequestSeconds = 600;

        const string TempoUri = "https://api.tempo.io/3";
        const int TimeoutRequestSeconds = 30;
#else
        const string TempoUri = "https://api.tempo.io/4";
        const int TimeoutRequestSeconds = 30;
#endif

        public BaseTempoApiService(Model.Db.IntegrationSettings integrationSettings) : base(new Maya.AnyHttpClient.Model.HttpClientConnector
        {
            Endpoint = integrationSettings.Endpoint,
            TimeoutSeconds = TimeoutRequestSeconds,
            AuthType = Maya.AnyHttpClient.Model.AuthTypeKinds.Bearer,
            Token = integrationSettings.AccessToken,
            CustomJsonSerializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }
        })
        {
        }
    }
}
