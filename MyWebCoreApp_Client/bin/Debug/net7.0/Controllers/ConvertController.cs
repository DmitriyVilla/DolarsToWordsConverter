using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text;
using MyWebCoreApp_Client.Models;

namespace MyWebCoreApp_Client.Controllers
{
    
    public class ConvertController : Controller
    {
        // URL des Servers
        private static string url = "http://localhost:5000/api/convert";

        private readonly HttpClient httpClient;

        public ConvertController()
        {
            httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConvertToWords(string amount)
        {
            // Daten vorbereiten
            var requestData = new StringContent(amount, Encoding.UTF8, "text/plain");

            // Anfrage an den Server senden
            var response = await httpClient.PostAsync(url, requestData);

            if (response.IsSuccessStatusCode)
            {
                // Antwort des Servers lesen
                string result = await response.Content.ReadAsStringAsync();
                ViewBag.Result = result;
            }
            else
            {
                ViewBag.Result = "Error";
            }

            ViewBag.Amount = amount;

            //return View("Index");

            return View("~/Views/Home/Index.cshtml");

        }


    }
}
