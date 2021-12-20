using System;
using System.Net.Http;
using System.Text.Json;

namespace Maestro
{
    public class MaestroClient
    {
        private const string API_PREFIX = "/api/v1/";

        private HttpClient httpClient;
        private string id;

        public MaestroClient(string baseUrl, string authToken, string appId)
        {
            this.id = appId;
            HttpClient c = new HttpClient();
            c.Timeout = new TimeSpan(0, 0, 0, 10);
            c.BaseAddress = new Uri(baseUrl.TrimEnd('/') + API_PREFIX);
            c.DefaultRequestHeaders.Add("X-Registry-Token", authToken);
            this.httpClient = c;
        }

        public void Register(string appUrl)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "register");
            var reqBody = JsonSerializer.Serialize(new Registrant()
            {
                AppId = this.id,
                Address = appUrl,
            });
            req.Content = new StringContent(reqBody);
            var resp = this.httpClient.Send(req);
            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"registration request failed with status {resp.StatusCode}");
            }
        }

        public void Deregister()
        {
            var req = new HttpRequestMessage(HttpMethod.Delete, $"deregister?id={this.id}");
            var resp = this.httpClient.Send(req);
            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"deregistration request failed with status {resp.StatusCode}");
            }
        }

        public Registrant Query(string appId)
        {
            var req = new HttpRequestMessage(HttpMethod.Get, $"query?id={appId}");
            var resp = this.httpClient.Send(req);
            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"query request failed with status {resp.StatusCode}");
            }

            var body = resp.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            return JsonSerializer.Deserialize<Registrant>(body);
        }

        public void Ping()
        {
            var req = new HttpRequestMessage(HttpMethod.Put, $"ping?id={this.id}");
            var resp = this.httpClient.Send(req);
            if (!resp.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"ping request failed with status {resp.StatusCode}");
            }
        }

    }
}
