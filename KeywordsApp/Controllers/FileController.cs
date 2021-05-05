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
using KeywordsApp.Models.File;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace keywords.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly KeywordContext _dbContext;
        private readonly IConfiguration _config;

        public FileController(ILogger<FileController> logger, KeywordContext dbContext, IConfiguration config)
        {
            _logger = logger;
            _dbContext = dbContext;
            _config = config;
        }

        public IActionResult Index()
        {

            var userId = _dbContext.GetUserId(User.Identity.Name);

            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var model = _dbContext.Files.Where(x => x.CreatedByUserId == userId)
                .Select(x => new FileViewModel
                {
                    FileId = x.Id,
                    Name = x.Name,
                    CreatedDate = x.CreatedDate,
                    KeywordsCount = x.Keywords.Count()
                })
                .OrderByDescending(x => x.CreatedDate)
                .Take(10);
            return View(model);
        }

        [HttpPost]
        [Route("File/Upload")]
        [ValidateAntiForgeryToken]
        // Set the limit to 10 Kb
        [RequestFormLimits(MultipartBodyLengthLimit = 10000)]
        public async Task<IActionResult> UploadForm(IFormFile csvFile)
        {
            var uploadFormViewModel = new UploadFormViewModel(csvFile, _config);

            if (uploadFormViewModel.HasError)
                return PartialView("_UploadForm", uploadFormViewModel);

            var userId = _dbContext.GetUserId(User.Identity.Name);

            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var fileEntity = new FileEntity(userId, uploadFormViewModel.Keywords);
            _dbContext.Files.Add(fileEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                uploadFormViewModel.SuccessMsg = "File uploaded. Keyword are being processed.";
            }
            catch (DataException e)
            {
                _logger.LogError(0, e, "DataBase cannot persist a new File for user: " + userId);
                uploadFormViewModel.ErrorMsg = "The file could not be saved. Please try again.";
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Db Context failed to persist File for user: " + userId);
                uploadFormViewModel.ErrorMsg = "The file could not be saved. Please try again.";
            }

            return PartialView("_UploadForm", uploadFormViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
