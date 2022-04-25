using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIClient
{
    public class ApiClient
    {
        private readonly string apiUrl;
        private readonly string username;
        private readonly string bearerToken;


        public ApiClient(IConfiguration config)
        {
            this.apiUrl = config.GetSection("api").GetValue<string>("ApiUrl");
            this.username = config.GetSection("api").GetValue<string>("Username");
            this.bearerToken = config.GetSection("api").GetValue<string>("BearerToken");
        }

        public async Task<string> ExecuteRequest(string jobNumber, CancellationToken cancellationToken)
        {
            using (HttpClient httpClient = InitializeHttpClient())
            {
                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, $"{this.apiUrl}/pc/rest/v1/quotation/{jobNumber}/document/package");
                //request.Headers.Add("Accept", "application/json");
                //request.Headers.Add("Authorization", string.Format("Bearer {0}", this.bearerToken));
                //request.Headers.Add("User", string.Format("Basic {0}", this.username));


                var response = await httpClient.SendAsync(request, cancellationToken);

                return await response.Content.ReadAsStringAsync(cancellationToken);
            }
        }

        private HttpClient InitializeHttpClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", string.Format("Bearer {0}", this.bearerToken));
            httpClient.DefaultRequestHeaders.Add("user", string.Format(this.username));

            return httpClient;
        }
    }
}
