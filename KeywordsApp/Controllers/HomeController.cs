using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KeywordsApp.Models;
using KeywordsApp.Data;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using HtmlAgilityPack;

namespace keywords.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly KeywordContext _dbContext;

        public HomeController(ILogger<HomeController> logger, KeywordContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }


        public async Task<IActionResult> Index()
        {
            const string GOOGLE_URL = "https://www.google.com/search";
            var searchResults = new List<KeyResult>();
            var queryList = new string[]{"bonjour", "palace","minister","outline",
                        "veteran","ethics","swing","inspiration",
                        "poem","wife","tired","tower",
                        "danger","stomach","embarrassment","flock",
                        "fire","thin","detective","item","owner",
                        "ignore","berry","gun","black",
                        "prefer","plan","dentist","jam",
                        "dictate","exposure","remind","hate",
                        "definition","monk","lid","demonstration",
                        "throat","war","kidnap","swipe",
                        "prosecute","expenditure","syndrome","fog",
                        "instrument","adjust","shame","day","constituency","domestic",
                        "glimpse","grind","expose","ivory","pierce","pan","correction",
                        "joystick","cafe","bet","east","grow","partnership",
                        "traffic","consolidate","poetry","fat","drift","democratic",
                        "owner","academy","labour","inflation",
                        "young","spring","negotiation","flow","adoption",
                        "visit","embark","surprise",
                        "tragedy","essential","cabin","survivor","monarch","trial",
                        "scramble","splurge","sink","gregarious","feel","football","satisfaction","flawed","reporter",
                        "deteriorate","move","excess","sanctuary"};

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(GOOGLE_URL);

                // Build Client header
                // The user agent describe to google which device is requesting.
                // It enable us to get Desktop-style UI instead of Mobile version (that does not include stats)
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("AppleWebKit/537.36 (KHTML, like Gecko)");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Chrome/90.0.4430.85");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Safari/537.36");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Edg/90.0.818.49");

                // Add an Accept header for JSON format.
                // These correspond to what a classic web browser would expose
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/apng"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/signed-exchange"));

                // Set language to english
                client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US");

                foreach (var keyword in queryList.Take(1))
                {


                    // List data response.
                    HttpResponseMessage respGoogle = client.GetAsync("?q=" + keyword).Result;
                    if (respGoogle.IsSuccessStatusCode)
                    {
                        respGoogle.EnsureSuccessStatusCode();
                        searchResults.Add(new KeyResult
                        {
                            Keyword = keyword,
                            PageContent = await respGoogle.Content.ReadAsStringAsync()
                        });
                        break; // No need to switch to another proxy;
                    }
                    // Too many requests - bot detected
                    else if ((int)respGoogle.StatusCode == 429)
                    {
                        Console.WriteLine("Google send '429 (Too many requests)");
                        Console.WriteLine("Trying another server");
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("Parsing Google unsufessful:");
                        Console.WriteLine("{0} ({1})", (int)respGoogle.StatusCode, respGoogle.ReasonPhrase);
                        continue;

                    }
                }
            }

            return View(searchResults);
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
