using ConsumeWebAPIOne.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;

namespace ConsumeWebAPIOne.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        string baseUrl = "http://localhost:47077/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()//calls the WEBAPI and populates the view using Datatable
        {
            DataTable dt = new DataTable();
            using(var client = new HttpClient())
            {
                client.BaseAddress= new Uri(baseUrl);//setting the base address of the uri used when sending ie this http://localhost:47077/
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//addding new mediatype

                HttpResponseMessage getData = await client.GetAsync("Customer/GetAllCustomers");//the call where users is the method by sending a GET request with the spacified uri

                if(getData .IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;//fetching all the data from the api database
                    dt=JsonConvert.DeserializeObject<DataTable>(results);//the JSON returned is deserialized into a .net type ie DataTable format

                }
                else
                {
                    Console.WriteLine("Error calling web API");//exception
                }

                ViewData.Model= dt;

            }


            return View();
        }

        public async Task<IActionResult> Index2()//calls the WEBAPI and populates the view using Datatable
        {

            IList<UserEntity> users = new List<UserEntity>();


            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);//setting the base address of the uri used when sending ie this http://localhost:47077/
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//addding new mediatype

                HttpResponseMessage getData = await client.GetAsync("Customer/GetAllCustomers");//the call where users is the method by sending a GET request with the spacified uri

                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;//fetching all the data from the api database
                    users = JsonConvert.DeserializeObject<List<UserEntity>>(results);//the JSON returned is deserialized into a .net type ie DataTable format

                }
                else
                {
                    Console.WriteLine("Error calling web API");//exception
                }

                ViewData.Model = users;

            }


            return View();
        }

        public async Task<ActionResult<String>> AddCustomer(UserEntity user)
        {
            UserEntity obj = new UserEntity()
            {
                customerID = user.customerID,
                CustomerName = user.CustomerName,
                Email = user.Email,
                Mobile = user.Mobile
            };

            if (user.customerID != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl + "Customer/");//setting the base address of the uri used when sending ie this http://localhost:47077/
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));//addding new mediatype

                    HttpResponseMessage getData = await client.PostAsJsonAsync<UserEntity>("AddConsumer", obj);//the call where users is the method by sending a GET request with the spacified uri

                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index2", "Home");
                    }
                    else
                    {
                        Console.WriteLine("Error calling web API");//exception
                    }


                }
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}