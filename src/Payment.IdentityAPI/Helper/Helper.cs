using System;
using System.Net.Http;


namespace ViewApi.Helper
{
    public class PaymentAPI
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
			client.BaseAddress = new Uri("http://payment_api/");

			return client;
        }
    }
}
