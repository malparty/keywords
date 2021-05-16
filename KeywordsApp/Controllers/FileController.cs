using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KeywordsApp.Models;
using KeywordsApp.Data;
using Microsoft.AspNetCore.Http;
using KeywordsApp.Models.File;
using Microsoft.Extensions.Configuration;
using System.Data;
using KeywordsApp.Models.Keyword;
using X.PagedList;

namespace KeywordsApp.Controllers
{
    public class FileController : AuthorizedController
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
            var model = _dbContext.Files.Where(x => x.CreatedByUserId == UserId)
                .Select(
                    x => new FileViewModel
                    {
                        FileId = x.Id,
                        Name = x.Name,
                        ShowProgressBar = true,
                        CreatedDate = x.CreatedDate,
                        TotalKeywordsCount = x.Keywords.Count(),
                        ParsedKeywordsCount = x.Keywords
                            .Where(
                                y => y.ParsingStatus == ParsingStatus.Succeed
                            )
                            .Count()
                    })
                .OrderByDescending(x => x.CreatedDate)
                .Take(4)
                .ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult Details(int fileId)
        {
            var model = _dbContext.Files.Where(x => x.CreatedByUserId == UserId && x.Id == fileId)
                .Select(
                    x => new FileViewModel
                    {
                        FileId = x.Id,
                        Name = x.Name,
                        ShowProgressBar = true,
                        ParsedKeywordsCount = x.Keywords.Count(
                            y => y.ParsingStatus == ParsingStatus.Succeed
                        ),
                        TotalKeywordsCount = x.Keywords.Count(),
                        CreatedDate = x.CreatedDate
                    })
                .FirstOrDefault();

            if (model == null)
                return NotFound();

            return PartialView("_Details", model);
        }

        public IActionResult HeaderIntro(int fileId = 0)
        {
            var model = _dbContext.Files.FirstOrDefault(x => x.CreatedByUserId == UserId && x.Id == fileId);

            if (model == null)
                return NotFound();

            return PartialView("_HeaderIntro", model);
        }

        [HttpPost]
        [Route("File/Upload")]
        [ValidateAntiForgeryToken]
        // Set the limit to 10 Kb
        [RequestFormLimits(MultipartBodyLengthLimit = 10000)]
        public async Task<IActionResult> UploadForm(IFormFile csvFile)
        {
            var uploadFormViewModel = new UploadFormViewModel(csvFile, _config);

            if (!uploadFormViewModel.IsValid)
                return PartialView("_UploadForm", uploadFormViewModel);

            var fileEntity = new FileEntity(UserId, uploadFormViewModel.Keywords, uploadFormViewModel.FileName);
            _dbContext.Files.Add(fileEntity);
            try
            {
                await _dbContext.SaveChangesAsync();
                uploadFormViewModel.SuccessMsg = "File uploaded. Keyword are being processed.";
            }
            catch (DataException e)
            {
                _logger.LogError(0, e, "DataBase cannot persist a new File for user: " + UserId);
                uploadFormViewModel.ErrorMsg = "The file could not be saved. Please try again.";
            }
            catch (Exception e)
            {
                _logger.LogError(0, e, "Db Context failed to persist File for user: " + UserId);
                uploadFormViewModel.ErrorMsg = "The file could not be saved. Please try again.";
            }

            // Enable UI to get back newly created File
            uploadFormViewModel.PreviousFileId = fileEntity.Id;

            return PartialView("_UploadForm", uploadFormViewModel);
        }

        public IActionResult Search(int page = 1, string search = null)
        {
            var query = _dbContext.Files.Where(x => x.CreatedByUserId == UserId);
            if (!string.IsNullOrEmpty(search))
            {
                var searchLow = search.ToLower();
                query = query.Where(x => x.Name.ToLower().Contains(searchLow));
            }
            query = query.OrderByDescending(x => x.CreatedDate).ThenBy(x => x.Name);
            var model = new FileListViewModel
            {
                Search = search,
            };
            model.Files = query
                .Select(
                    x => new FileViewModel
                    {
                        CreatedDate = x.CreatedDate,
                        FileId = x.Id,
                        Name = x.Name,
                        ShowProgressBar = false,
                        ParsedKeywordsCount = x.Keywords.Where(
                            k => k.ParsingStatus == ParsingStatus.Succeed
                        )
                        .Count(),
                        TotalKeywordsCount = x.Keywords.Count()
                    }
                )
                .ToPagedList(page, 20);

            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
