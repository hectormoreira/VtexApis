using ApisVtex.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RestSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace ApisVtex.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly IWebHostEnvironment _environment;



        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }


        public async Task<IActionResult> Index()
        {
            try
            {
                string account = Environment.GetEnvironmentVariable("account") ?? "";
                string appKey = Environment.GetEnvironmentVariable("x-vtex-api-appkey") ?? "";
                string appToken = Environment.GetEnvironmentVariable("x-vtex-api-apptoken") ?? "";
                
                string fullpathFile = Path.Combine(_environment.ContentRootPath, @"files\addSinIva.xls");

                var options = new RestClientOptions($"https://{account}.vtexcommercestable.com.br")
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);
                var request = new RestRequest("/api/catalog/pvt/collection/137/stockkeepingunit/importinsert", Method.Post);
                request.AddHeader("accept", "application/json");
                request.AddHeader("content-type", "multipart/form-data");
                request.AddHeader("x-vtex-api-appkey", appKey);
                request.AddHeader("x-vtex-api-apptoken", appToken);
                //request.AddHeader("cache-control", "no-cache");
                request.AlwaysMultipartFormData = true;

                request.AddFile("file", fullpathFile);

                RestResponse response = await client.ExecutePostAsync(request);

                ViewBag.message = response.Content;

            }
            catch (Exception e)
            {
                ViewBag.message = e.Message;
                throw;
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