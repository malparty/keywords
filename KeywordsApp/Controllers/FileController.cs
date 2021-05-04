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
using Microsoft.AspNetCore.Http;
using System.IO;
using KeywordsApp.Models.File;

namespace keywords.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly KeywordContext _dbContext;

        public FileController(ILogger<FileController> logger, KeywordContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("File/Upload")]
        // [ValidateAntiForgeryToken]
        // Set the limit to 10 Kb
        [RequestFormLimits(MultipartBodyLengthLimit = 10000)]
        public async Task<IActionResult> UploadForm(IFormFile csvFile)
        {
            var uploadFormViewModel = new UploadFormViewModel(csvFile);

            return PartialView("_UploadForm", uploadFormViewModel);
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
