using System;
using System.Net.Http;


namespace ViewApi.Helper
{
    public class PaymentAPI
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000/");

            return client;
        }
    }
}
