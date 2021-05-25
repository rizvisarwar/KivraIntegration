using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentAdmin.ServicesIntegration.Kivra
{
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Newtonsoft.Json;
    using PaymentAdmin.ServicesIntegration.Kivra.Messages;

    public class KivraCommunicator : IKivraCommunicator
    {
        private readonly string _endpointUrl;
        private readonly string _clientId = "SampleClientId_1618233289e100509a894240a4b322cfcbe71039bc1";
        private readonly string _clientSecret = "SampleClientSecret_UF0RT8pPQpmzCWsITDmht5uzNrN4O83";
        private readonly string _encodedCliedIdAndSecret = "MTYxODIzMzI4OWUxMDA1MDlhODk0MjQwYTRiMzIyY2ZjYmU3MTAzOWJjOlVGMFJUOHBQUXBtekNXc0lURG1odDV1ek5yTjRPOA=="; //follow kivra api doc to generate it
        private readonly string _authorization = "auth";
        private readonly string _sendContent = "tenant/{tenantKey}/content";
        private readonly string _tenantKey = "SampleTenantKey_1618233185c0b0682538044d4fac9c84b5310d35bef";

        public KivraCommunicator(string endpointUrl)
        {
            _endpointUrl = endpointUrl;
        }

        public async Task<bool> SendContent(string data, string invoiceId)
        {
            //authenticate and get bearer token
            var tokenRequest = new TokenRequest() { grant_type = "client_credentials" };
            var token = await GetBearerToken(tokenRequest).ConfigureAwait(false);

            var fileArray = new List<FileRequest>();
            fileArray.Add(new FileRequest
            {
                name = $"invoice_{invoiceId}.pdf",
                data = data
            });
            var request = new ContentRequest
            {
                generated_at = DateTime.UtcNow,
                ssn = "194605092256", // sample SSN
                subject = $"Test invoice {invoiceId}",
                files = fileArray.ToArray(),
                type = "invoice"
            };
            await SendRequestAsync(request, token).ConfigureAwait(false);
            return true;
        }

        private async Task<string> GetBearerToken(TokenRequest request)
        {
            using (var handler = new HttpClientHandler())
            {
                using (var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(_endpointUrl)
                })
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    var requestUri = new Uri(_endpointUrl);
                    var message = new HttpRequestMessage(HttpMethod.Post, requestUri);
                    message.Headers.Host = new Uri(_endpointUrl).Host;

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", _encodedCliedIdAndSecret);

                    var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync("/v1/auth", stringContent).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var serializedResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            var deserializedObject = JsonConvert.DeserializeObject<AuthenticationResponse>(serializedResponse);
                            return deserializedObject.access_token;
                        }
                        return null;
                    }
                }
            }
        }

        private async Task<HttpResponseMessage> SendRequestAsync(ContentRequest request, string token)
        {
            using (var handler = new HttpClientHandler())
            {
                using (var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(_endpointUrl)
                })
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                    var requestUri = new Uri(_endpointUrl);
                    var message = new HttpRequestMessage(HttpMethod.Post, requestUri);
                    message.Headers.Host = new Uri(_endpointUrl).Host;

                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var stringContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                    using (var response = await httpClient.PostAsync($"/v1/tenant/{_tenantKey}/content", stringContent).ConfigureAwait(false))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            var serializedResponse = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                            return response;
                        }
                        return null;
                    }
                }
            }
        }
    }
}
