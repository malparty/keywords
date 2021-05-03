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
using System.Threading;

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
            //
            //
            // IMPROVEMENT: can have 2 threads (one using proxy, the other one direct google?)
            //
            //
            const int MAX_RETRY = 10;
            const int INIT_SLEEP_TIME = 500; // ms
            const int MIN_RETRY_SLEEP_TIME = 250; //ms
            int sleepTime = INIT_SLEEP_TIME;

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


            var proxyServers = new string[]{"us13",
                            "eu2","us1","us2","us3","us4","us5","us6","us7","us8","us9","us10",
                            "us11","us12","us13","us14","us15","us16","us17","eu1","eu2","eu3","eu4","eu5","eu6",
                            "eu7","eu8","eu9","eu10","eu11","eu12","eu13","eu14","eu15"};
            HttpClient client = new HttpClient();
            try
            {
                var googleIndex = 0;
                foreach (var keyword in queryList.Take(100)) // .Take(1)
                {
                    Console.WriteLine("Parsing new keyword: " + keyword);
                    // foreach (var proxyServer in proxyServers)
                    // {
                    //     Console.WriteLine("Server: " + proxyServer);

                    // Call proxy with Server arg and Google query:
                    var googleUrls = new string[] {"https://www.google.com/search",
                    "https://www.google.com.vn/search"
                    }
                    ;
                    var queryGoogle = googleUrls[googleIndex] + "?q=" + keyword;
                    // var proxyUrl = "https://" + proxyServer + ".proxysite.com/includes/process.php?action=update";



                    // // Set proxy form submition content
                    // var formContent = new FormUrlEncodedContent(new[]
                    // {
                    //     // new KeyValuePair<string, string>("q", keyword), // google

                    //     new KeyValuePair<string, string>("d", queryGoogle), // proxy
                    //     new KeyValuePair<string, string>("server-option", proxyServer), // proxy
                    // });
                    // request.Content = formContent;

                    int retryCount = 0;
                    HttpResponseMessage respProxy = null;
                    do
                    {

                        // Build request:
                        var request = new HttpRequestMessage(HttpMethod.Get, queryGoogle);
                        // var request = new HttpRequestMessage(HttpMethod.Post, proxyUrl);

                        // Build Client header
                        request.Headers.Accept.Clear();
                        // The user agent describe to google which device is requesting.
                        // It enable us to get Desktop-style UI instead of Mobile version (that does not include stats)
                        request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                        request.Headers.UserAgent.ParseAdd("AppleWebKit/537.36 (KHTML, like Gecko)");
                        request.Headers.UserAgent.ParseAdd("Chrome/90.0.4430.85");
                        request.Headers.UserAgent.ParseAdd("Safari/537.36");
                        request.Headers.UserAgent.ParseAdd("Edg/90.0.818.49");

                        // Add an Accept header for JSON format.
                        // These correspond to what a classic web browser would expose
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("image/apng"));
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/signed-exchange"));

                        // Set language to english
                        request.Headers.AcceptLanguage.ParseAdd("en-US");

                        respProxy = client.SendAsync(request, cancellationToken: CancellationToken.None).Result;
                        // Preventing error 429 - Too many requests
                        Thread.Sleep(sleepTime);
                        retryCount++;

                        // Faster if Success, slower if failed
                        if (respProxy.IsSuccessStatusCode)
                            sleepTime = sleepTime / 2;
                        else if (sleepTime > 1100)
                        {
                            if (googleIndex == 0)
                            {
                                googleIndex = 1;
                                Console.WriteLine("Trying with Google Vietnam");
                                continue; // Retry
                            }
                            else
                            {
                                client.Dispose();
                                client = new HttpClient();
                                Console.WriteLine("Trying to dispose HttpClient");
                                continue; // Retry
                            }
                        }
                        else
                            sleepTime = sleepTime < MIN_RETRY_SLEEP_TIME ? MIN_RETRY_SLEEP_TIME : sleepTime * 2;
                        Console.WriteLine("New sleep time: " + sleepTime);
                    }
                    while (retryCount < MAX_RETRY && !respProxy.IsSuccessStatusCode);

                    if (respProxy.IsSuccessStatusCode)
                    {
                        searchResults.Add(new KeyResult
                        {
                            Keyword = keyword,
                            PageContent = await respProxy.Content.ReadAsStringAsync()
                        });
                        // break; // No need to try with another server
                        Console.WriteLine("GOOD!!");

                    }
                    // Too many requests - bot detected
                    else if ((int)respProxy.StatusCode == 429)
                    {
                        Console.WriteLine("Google send '429 (Too many requests)");
                        Console.WriteLine(respProxy.Content);
                        Console.WriteLine(respProxy.ReasonPhrase);
                        Console.WriteLine(respProxy.Headers);
                        Console.WriteLine("EXPERIMENTATION FAILED!!");
                    }
                    else
                    {
                        // Need to try with another server
                        Console.WriteLine("EXPERIMENTATION FAILED!!");
                        Console.WriteLine("Parsing Proxy unsufessful:");
                        Console.WriteLine("{0} ({1})", (int)respProxy.StatusCode, respProxy.ReasonPhrase);
                    }
                }
            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
                Console.Error.Write(e.Data);
                // Mark current object status as failed.
            }
            finally
            {
                client.Dispose();
            }
            // Persist current data changes

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
