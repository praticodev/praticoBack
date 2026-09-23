using Comtele.Sdk.Core.Resources;
using Microsoft.Extensions.Configuration;
using Pratico.Business.Configuration;
using Pratico.Dominio.Intefaces.Service;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Utils
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configuration;

        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private ServiceResult<object> EnviaSMS(string content, params string[] receivers)
        {
            var appSettingsSection = _configuration.GetSection("SMS");
            var smsSettings = appSettingsSection.Get<SmsConfiguration>();

            var restClient = new RestClient(smsSettings.UrlBase);
            var restRequest = new RestRequest("send", Method.POST);

            restRequest.AddHeader("auth-key", smsSettings.Key);
            restRequest.AddJsonBody(new
            {
                smsSettings.Id,
                content,
                receivers = string.Join(",", receivers)
            });

            var restResponse = restClient.Execute<ServiceResult<object>>(restRequest);

            return restResponse.Data;
        }

        public async Task<ServiceResult<object>> SendAsync(string content, params string[] receivers)
        {
            return await Task.Run(() => EnviaSMS(content, receivers));
        }
    }
}
