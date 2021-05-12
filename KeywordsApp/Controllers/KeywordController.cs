using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KeywordsApp.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using X.PagedList;
using KeywordsApp.Models.Keyword;

namespace KeywordsApp.Controllers
{
    public class KeywordController : AuthorizedController
    {
        private readonly ILogger<FileController> _logger;
        private readonly KeywordContext _dbContext;
        private const int NBR_KEYWORD_PER_PAGE = 100;

        public KeywordController(ILogger<FileController> logger, KeywordContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index(int? page, KeywordOrderBy orderBy = KeywordOrderBy.NameAsc, int fileId = 0, string search = "", bool showResults = true)
        {
            var userId = _dbContext.GetUserId(User.Identity.Name);

            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var initQuery = _dbContext.Keywords.Where(x => x.File.CreatedByUserId == userId);

            if (!string.IsNullOrEmpty(search))
            {
                var searchLow = search.ToLower();
                initQuery = initQuery.Where(x => x.Name.ToLower().Contains(searchLow));
            }

            if (fileId > 0)
            {
                initQuery = initQuery.Where(x => x.FileId == fileId);
            }

            if (orderBy == KeywordOrderBy.StatusAsc)
            {
                initQuery = initQuery.OrderBy(x => x.ParsingStatus)
                    .ThenByDescending(x => x.ParsedDate)
                    .ThenBy(x => x.Name);
            }
            else if (orderBy == KeywordOrderBy.StatusDesc)
            {
                initQuery = initQuery.OrderByDescending(x => x.ParsingStatus)
                    .ThenByDescending(x => x.ParsedDate)
                    .ThenBy(x => x.Name);
            }
            else if (orderBy == KeywordOrderBy.NameAsc)
            {
                initQuery = initQuery.OrderBy(x => x.ParsingStatus)
                    .OrderBy(x => x.Name)
                    .ThenBy(x => x.ParsingStatus)
                    .ThenByDescending(x => x.ParsedDate);
            }
            else // KeywordOrderBy.NameDesc
            {
                initQuery = initQuery.OrderByDescending(x => x.Name)
                    .ThenBy(x => x.ParsingStatus)
                    .ThenByDescending(x => x.ParsedDate);
            }

            var model = new KeywordListViewModel
            {
                OrderBy = orderBy,
                Keywords = initQuery.ToPagedList(page ?? 1, NBR_KEYWORD_PER_PAGE),
                FileId = fileId,
                Search = search,
                IsShowResults = showResults
            };

            return View(model);
        }

        public async Task<IActionResult> LastParsed()
        {
            var userId = _dbContext.GetUserId(User.Identity.Name);

            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var model = await _dbContext.Keywords.Where(x => x.File.CreatedByUserId == userId && x.ParsingStatus == ParsingStatus.Succeed)
            .OrderByDescending(x => x.ParsedDate)
            .Take(8)
            .Select(x => new KeywordViewModel
            {
                KeywordId = x.Id,
                Name = x.Name,
                FileName = x.File.Name,
                AdWordsCount = x.AdWordsCount,
                LinkCount = x.LinkCount,
                RequestDuration = x.RequestDuration,
                TotalThouthandResultsCount = x.TotalThouthandResultsCount,
                FileId = x.FileId,
                ParsedDate = x.ParsedDate,
                ParsingStatus = x.ParsingStatus
            })
            .ToListAsync();
            return PartialView("_LastParsed", model);
        }

        [HttpPost]
        public IActionResult Details(int keywordId)
        {
            var userId = _dbContext.GetUserId(User.Identity.Name);

            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var model = _dbContext.Keywords.Where(x => x.File.CreatedByUserId == userId && x.Id == keywordId)
            .Select(x => new KeywordViewModel
            {
                KeywordId = x.Id,
                Name = x.Name,
                FileName = x.File.Name,
                AdWordsCount = x.AdWordsCount,
                LinkCount = x.LinkCount,
                RequestDuration = x.RequestDuration,
                TotalThouthandResultsCount = x.TotalThouthandResultsCount,
                FileId = x.FileId,
                ParsedDate = x.ParsedDate,
                ParsingStatus = x.ParsingStatus
            })
            .FirstOrDefault();
            if (model == null)
                return NotFound();

            return PartialView("_Details", model);
        }

        public IActionResult Cached(int keywordId)
        {
            var userId = _dbContext.GetUserId(User.Identity.Name);

            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var model = _dbContext.Keywords.Where(x => x.File.CreatedByUserId == userId && x.Id == keywordId)
            .FirstOrDefault();
            if (model == null)
                return NotFound();

            return View("Cached", model);
        }

    }
}
