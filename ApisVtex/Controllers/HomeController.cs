using ApisVtex.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using RestSharp;
using RestSharp.Serializers;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Reflection;
using System.Text;

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


        //public ActionResult Index()
        public async Task<IActionResult> Index()
        {
            try
            {
                string account = Environment.GetEnvironmentVariable("account") ?? "";
                string appKey = Environment.GetEnvironmentVariable("x-vtex-api-appkey") ?? "";
                string appToken = Environment.GetEnvironmentVariable("x-vtex-api-apptoken") ?? "";

                string pathCombine = Path.Combine(_environment.ContentRootPath, @"files\addSinIva.xls");
                string fullpathFile = pathCombine.Replace("\\", "/");

                var options = new RestClientOptions($"https://{account}.vtexcommercestable.com.br")
                {
                    MaxTimeout = -1,
                };

                var client = new RestClient(options);
                var request = new RestRequest("/api/catalog/pvt/collection/137/stockkeepingunit/importinsert", Method.Post);
                request.AddHeader(KnownHeaders.Accept, "application/json");
                request.AddHeader(KnownHeaders.ContentType, "multipart/form-data");
                request.AddHeader(KnownHeaders.ContentDisposition, "multipart/form-data");

                request.AddHeader("x-vtex-api-appkey", appKey);
                request.AddHeader("x-vtex-api-apptoken", appToken);
                //request.AddHeader("cache-control", "no-cache");
                request.Method = Method.Post;
                request.AlwaysMultipartFormData = true;

                request.AddFile("file", fullpathFile);

                RestResponse response = await client.ExecutePostAsync(request);



                //Otra prueba

                //var options = new RestClientOptions("https://imagetechdev.vtexcommercestable.com.br")
                //{
                //    MaxTimeout = -1,
                //    Encoding = Encoding.UTF8,
                //};
                //var client = new RestClient(options);
                //var request = new RestRequest("/api/catalog/pvt/collection/137/stockkeepingunit/importinsert");
                ////request.AddHeader("Content-Type", "multipart/form-data");
                //request.AddHeader("Accept", "application/json");
                //request.AddHeader("x-vtex-api-appkey", "");
                //request.AddHeader("x-vtex-api-apptoken", "");

                //request.AlwaysMultipartFormData = true;
                //request.Method = Method.Post;

                //request.AddFile("file", "D:/Desarrollo_01/Proyect/ApisVtex/ApisVtex/Files/addSinIva.xls", "multipart/form-data");

                //RestResponse response = client.ExecutePost(request);

                //fin









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