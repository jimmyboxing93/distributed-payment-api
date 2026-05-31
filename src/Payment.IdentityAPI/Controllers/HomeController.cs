using Microsoft.AspNetCore.Mvc;
using ViewApi.Models;
using ViewApi.Helper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;

namespace PaymentGateway.API.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        PaymentAPI _api = new PaymentAPI();
        private readonly string _apiKey;

        public HomeController(IConfiguration configuration) 
        {
            _apiKey = configuration["API_KEY"] ?? throw new ArgumentNullException("API_KEY variable is not set.");
        }

        public async Task<IActionResult> Index()
        {
            List<UserInfo> creditCards = new List<UserInfo>();
            var client = GetConfiguredClient();

             HttpResponseMessage res = await client.GetAsync("api/Api");
            if (res.IsSuccessStatusCode)
            {

                var results = await res.Content.ReadAsStringAsync();

                creditCards = JsonConvert.DeserializeObject<List<UserInfo>>(results);

            }
            return View(creditCards);
        }

        public async Task<IActionResult> PaymentDetails(Guid id)
        {
            var creditCard = new UserInfo();
			var client = GetConfiguredClient();

            HttpResponseMessage res = await client.GetAsync($"api/Api/{id}");

            if (res.IsSuccessStatusCode)
            {
                var results = await res.Content.ReadAsStringAsync();

                creditCard = JsonConvert.DeserializeObject<UserInfo>(results);

            }

            return View(creditCard);
        }


        public ActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(UserInfo model)
        {

			var client = GetConfiguredClient();

            var result = await client.PostAsJsonAsync("api/Api", model);


			if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var creditCard = new UserInfo();

			var client = GetConfiguredClient();
			var res = await client.GetAsync($"api/Api/{id}");
            

            if (res.IsSuccessStatusCode)
            {

                var results = await res.Content.ReadAsStringAsync();
                
                creditCard = JsonConvert.DeserializeObject<UserInfo>(results);

            }

            return View(creditCard);

        }

        [HttpPost]
        public  async Task<ActionResult> Edit(UserInfo model, Guid id)
        {

			var client = GetConfiguredClient();
			var result = await client.PutAsJsonAsync<UserInfo>($"api/API/{id}", model);
            

            if (result.IsSuccessStatusCode)

            {
                return RedirectToAction("Index");
            }
            

            return View(model);
        }
        
    

        public async Task<IActionResult> Delete(Guid id)
        {
			var client = GetConfiguredClient();
			await client.DeleteAsync($"api/Api/{id}");

            return RedirectToAction("Index");
        }

        private HttpClient GetConfiguredClient() 
        {
            HttpClient client = _api.Initial();
            client.DefaultRequestHeaders.Add("API_KEY", _apiKey);
            return client;
        }

    }

}