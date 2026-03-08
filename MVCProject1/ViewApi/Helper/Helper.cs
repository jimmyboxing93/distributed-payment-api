using System;
using System.Net.Http;


namespace ViewApi.Helper
{
    public class PaymentAPI
    {

		private readonly IConfiguration _configuration;

		public PaymentAPI(IConfiguration configuration) 
		{
			_configuration = configuration;
		}
		public HttpClient Initial()
        {
            var client = new HttpClient();
			// Use the injected URL from Docker, fallback to localhost for debugging
			var baseUrl = _configuration["ApiBaseUrl"] ?? "http://localhost:8080/";
			var apiKey = _configuration["ApiKey"];

			client.BaseAddress = new Uri("http://payment_api/");

			// You can even set the header here once so it's in every request
			var key = _configuration["ApiKey"];
			client.DefaultRequestHeaders.Add("ApiKey", key);

			return client;
        }
    }
}
