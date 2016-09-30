

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var baseAddress = "http://localhost:3636";
            using (var client = new HttpClient())
            {
                var form = new Dictionary<string, string>    
               {    
                   {"grant_type", "password"},    
                   {"username", "quiteimran@yahoo.co.uk"},    
                   {"password", "Pav1l1on!"},    
               };
                var tokenResponse = client.PostAsync(baseAddress + "/Token", new FormUrlEncodedContent(form)).Result;
                var token = tokenResponse.Content.ReadAsStringAsync().Result;  
                //var token = tokenResponse.Content.ReadAsAsync<Token>(new[] { new JsonMediaTypeFormatter() }).Result;
                Console.WriteLine(string.IsNullOrEmpty(token) ? "Token issued is: {0}" : "Error : {0}", token);

                using (var httpClient1 = new HttpClient())
                {
                    httpClient1.BaseAddress = new Uri(baseAddress);
                    httpClient1.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
                    var response = httpClient1.GetAsync("api/values/Jest").Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Success");
                    }
                    var message = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("URL responese : " + message);
                } 
            } 
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }

    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("token_type")]
        public string TokenType { get; set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonProperty("error")]
        public string Error { get; set; }
    }  
}