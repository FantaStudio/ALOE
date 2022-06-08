using System;
using System.Net.Http;

namespace ALOE.Helpers
{
    class ApiHelper
    {
        public static HttpClient CreateClient(string adress)
        {
            HttpClient client = new HttpClient() { BaseAddress = new Uri(adress) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}
