using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PracticeAPIFront.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace PracticeAPIFront.Controllers
{
    public class HomeController : Controller
    {
        string baseUrl = "https://localhost:7110/";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public async Task<ActionResult> Index()
        {
            List<Product> prds = new List<Product>();

            using(var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage res = await client.GetAsync("api/Products");

                if(res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    prds = JsonConvert.DeserializeObject<List<Product>>(response);
                }
            }
            
            return View(prds);
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(Product prd)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                StringContent content = new StringContent(JsonConvert.SerializeObject(prd), Encoding.UTF8, "application/json");

                using (var response = await client.PostAsync("api/Products", content))
                {
                    string res = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> UpdateProduct(int id)
        {
            Product prd = new Product();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                using (var response = await client.GetAsync($"api/Products/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    prd = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return View(prd);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateProduct(Product prd)
        {
            Product received = new Product();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                #region
                //var content = new MultipartFormDataContent();
                //content.Add(new StringContent(reservation.Empid.ToString()), "Empid");
                //content.Add(new StringContent(reservation.Name), "Name");
                //content.Add(new StringContent(reservation.Gender), "Gender");
                //content.Add(new StringContent(reservation.Newcity), "Newcity");
                //content.Add(new StringContent(reservation.Deptid.ToString()), "Deptid");
                #endregion
                int id = prd.Id;
                StringContent content = new StringContent(JsonConvert.SerializeObject(prd), Encoding.UTF8, "application/json");
                using (var response = await client.PutAsync($"api/Products/{id}", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    received = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return RedirectToAction("index");
        }

        public async Task<ActionResult> DeleteProduct(int Id)
        {
            Product prd = new Product();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                using (var response = await client.GetAsync($"api/Products/{Id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    prd = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return View(prd);
        }

        [HttpPost]
        [ActionName("DeleteProduct")]
        public async Task<ActionResult> DeleteProd(string p)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                using (var response = await client.DeleteAsync($"api/Products/"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ProductDetails(int id)
        {
            Product prd = new Product();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                using (var response = await client.GetAsync($"api/Products/{id}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    prd = JsonConvert.DeserializeObject<Product>(apiResponse);
                }
            }
            return View(prd);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}