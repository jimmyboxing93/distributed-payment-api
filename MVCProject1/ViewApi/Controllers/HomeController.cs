using Microsoft.AspNetCore.Mvc;
using ViewApi.Models;
using ViewApi.Helper;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace MVCProject1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

		// 1. Define the private field
		private readonly PaymentAPI _api;


        public HomeController(PaymentAPI api) 
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var client = _api.Initial();

            var res = await client.GetAsync("api/Api");
            if (res.IsSuccessStatusCode)
            {

                var results = await res.Content.ReadAsStringAsync();

                var creditCards = JsonConvert.DeserializeObject<List<UserInfo>>(results);

                return View(creditCards);

            }
            return View(new List<UserInfo>());
        }

        public async Task<IActionResult> PaymentDetails(Guid id)
        {

            var client = _api.Initial();

            var res = await client.GetAsync($"api/Api/{id}");

            if (res.IsSuccessStatusCode)
            {
                var results = await res.Content.ReadAsStringAsync();

				var creditCards = JsonConvert.DeserializeObject<List<UserInfo>>(results);

				return View(creditCards);

			}

			return RedirectToAction("Index");
		}


        public ActionResult Payment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Payment(UserInfo model)
        {

            var client = _api.Initial();

            var result = await client.PostAsJsonAsync<UserInfo>("api/Api", model);

            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        public async Task<ActionResult> Edit(Guid id)
        {

            var client = _api.Initial();

            var res = await client.GetAsync($"api/Api/{id}");


            if (res.IsSuccessStatusCode)
            {

                var results = await res.Content.ReadAsStringAsync();
                
                var creditCard = JsonConvert.DeserializeObject<UserInfo>(results);

				return View(creditCard);

			}

            return RedirectToAction("Index");

        }

        [HttpPost]
        public async Task<ActionResult> Edit(UserInfo model, Guid id)
        {
            var client = _api.Initial();


            var result = await client.PutAsJsonAsync<UserInfo>($"api/API/{id}", model);

            if (result.IsSuccessStatusCode)

            {
                return RedirectToAction("Index");
            }
            

            return View(model);
        }
        
    
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _api.Initial();

            await client.DeleteAsync($"api/Api/{id}");

            return RedirectToAction("Index");
        }

    }

}