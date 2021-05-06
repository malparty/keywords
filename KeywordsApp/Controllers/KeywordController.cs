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
using System.Data;
using X.PagedList;
using KeywordsApp.Models.Keyword;

namespace keywords.Controllers
{
    [Authorize]
    public class KeywordController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly KeywordContext _dbContext;
        private const int NBR_KEYWORD_PER_PAGE = 100;

        public KeywordController(ILogger<FileController> logger, KeywordContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index(int? page, KeywordOrderBy orderBy = KeywordOrderBy.Status, int fileId = 0)
        {
            var userId = _dbContext.GetUserId(User.Identity.Name);

            if (string.IsNullOrEmpty(userId))
                return NotFound();

            var initQuery = _dbContext.Keywords.Where(x => x.File.CreatedByUserId == userId);
            if (fileId > 0)
            {
                initQuery = initQuery.Where(x => x.FileId == fileId);
            }

            if (orderBy == KeywordOrderBy.Status) // default
            {
                initQuery = initQuery.OrderBy(x => x.ParsingStatus)
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

            var model = new KeywordViewModel
            {
                OrderBy = orderBy,
                Keywords = initQuery.ToPagedList(page ?? 1, NBR_KEYWORD_PER_PAGE),
                FileId = fileId
            };

            return View(model);
        }

    }
}
