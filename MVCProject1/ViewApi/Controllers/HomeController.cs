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

namespace MVCProject1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        PaymentAPI api = new PaymentAPI();

        public async Task<IActionResult> Index()
        {
            List<UserInfo> creditCards = new List<UserInfo>();

            HttpClient client = api.Initial();

            client.DefaultRequestHeaders.Add("ApiKey", "df008b2e24f947b1b873c94d8a3f2201");
            HttpResponseMessage res = await client.GetAsync("api/Api");
            if (res.IsSuccessStatusCode)
            {

                var results = res.Content.ReadAsStringAsync().Result;

                creditCards = JsonConvert.DeserializeObject<List<UserInfo>>(results);

            }
            return View(creditCards);
        }

        public async Task<IActionResult> PaymentDetails(Guid id)
        {
            var creditCard = new UserInfo();

            HttpClient client = api.Initial();

            client.DefaultRequestHeaders.Add("ApiKey", "df008b2e24f947b1b873c94d8a3f2201");
            HttpResponseMessage res = await client.GetAsync($"api/Api/{id}");

            if (res.IsSuccessStatusCode)
            {
                var results = res.Content.ReadAsStringAsync().Result;

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
        public IActionResult Payment(UserInfo model)
        {

            HttpClient client = api.Initial();

            client.DefaultRequestHeaders.Add("ApiKey", "df008b2e24f947b1b873c94d8a3f2201");
            var postTask = client.PostAsJsonAsync<UserInfo>("api/Api", model);
            postTask.Wait();

            var result = postTask.Result;

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(Guid id)
        {
            var creditCard = new UserInfo();

            HttpClient client = api.Initial();

            client.DefaultRequestHeaders.Add("ApiKey", "df008b2e24f947b1b873c94d8a3f2201");
            var res = client.GetAsync($"api/Api/{id}");
            res.Wait();

            var readData = res.Result;

            if (readData.IsSuccessStatusCode)
            {

                var results = readData.Content.ReadAsStringAsync().Result;
                
                creditCard = JsonConvert.DeserializeObject<UserInfo>(results);

            }

            return View(creditCard);

        }

        [HttpPost]
        public ActionResult Edit(UserInfo model, Guid id)
        {
            HttpClient client = api.Initial();

            client.DefaultRequestHeaders.Add("ApiKey", "df008b2e24f947b1b873c94d8a3f2201");
            var putTask = client.PutAsJsonAsync<UserInfo>($"api/API/{id}", model);
            putTask.Wait();

            var result = putTask.Result;

            if (result.IsSuccessStatusCode)

            {
                return RedirectToAction("Index");
            }
            

            return View(model);
        }
        
    

        public async Task<IActionResult> Delete(Guid id)
        {
            var creditCard = new UserInfo();

            HttpClient client = api.Initial();

            client.DefaultRequestHeaders.Add("ApiKey", "df008b2e24f947b1b873c94d8a3f2201");
            HttpResponseMessage res = await client.DeleteAsync($"api/Api/{id}");

            return RedirectToAction("Index");
        }

    }

}