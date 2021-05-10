using System;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;


namespace KeywordsApp.Models.Keyword
{
    public class KeywordParserModel
    {
        public int FileId { get; set; }
        public int KeywordId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string RawHtmlContent { get; set; }
        public HtmlNode BodyNode { get; set; }

        private HtmlDocument _pageDocument;
        private KeywordResultParserModel _keywordResult;


        public KeywordResultParserModel ParseHtml()
        {
            _keywordResult = new KeywordResultParserModel();
            if (!LoadHtmlContent())
            {
                _keywordResult.ErrorMsg = "Could not parse keyword. Search did not respond with a standard google result.";
                return _keywordResult;
            }

            ParseRequestStats();
            if (!_keywordResult.IsValid)
                return _keywordResult;

            ParseRequestLinkCount();
            if (!_keywordResult.IsValid)
                return _keywordResult;

            ParseRequestAdWordsCount();
            return _keywordResult;

        }
        private bool LoadHtmlContent()
        {
            _pageDocument = new HtmlDocument();
            _pageDocument.LoadHtml(RawHtmlContent);
            // Test if it is a google search page result based on "GSR" id presence:
            BodyNode = _pageDocument.DocumentNode.SelectSingleNode("//body[contains(@id,'gsr')][1]");
            if (string.IsNullOrEmpty(BodyNode?.InnerHtml))
            {
                return false;
            }
            _keywordResult.HtmlCode = RawHtmlContent;
            return true;
        }
        private void ParseRequestAdWordsCount()
        {
            var countMerchantAds = BodyNode.SelectNodes("//a")
            .Select(x => x.GetAttributeValue("data-offer-id", ""))
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .Count();
            var countBasicAds = BodyNode.SelectNodes("//div[@data-text-ad=\"1\"]")?.Count ?? 0;

            _keywordResult.AdWordsCount = countMerchantAds + countBasicAds;

        }
        private void ParseRequestLinkCount()
        {
            _keywordResult.LinkCount = BodyNode.SelectNodes("//a").Count;
        }
        private void ParseRequestStats()
        {
            // About 340,000,000 results (0.68 seconds) 
            // Khoảng 2.000.000.000 kết quả (0,78 giây) 
            // Результатов: примерно 23 800 (0,58 сек.) 
            // Environ 49 700 résultats (0,54 secondes) 
            // --> Numbers and ( ) are the only singularity.

            var requestStatsString = BodyNode.SelectSingleNode("//div[contains(@id,'result-stats')][1]")?.InnerHtml;
            if (string.IsNullOrEmpty(requestStatsString))
            {
                _keywordResult.ErrorMsg = "Cannot find request stats string";
                return;
            }
            ParseRequestStatsDuration(requestStatsString);
            if (!_keywordResult.IsValid)
                return;
            ParseRequestStatsResultCount(requestStatsString);

        }
        private void ParseRequestStatsDuration(string requestStatsString)
        {
            // (0,78 giay)
            var durationParenthesis = Regex.Matches(requestStatsString, @"\(.*\)");
            if (durationParenthesis.Count == 0)
            {
                _keywordResult.ErrorMsg = string.Format("Cannot parse duration parenthesis from {0}", requestStatsString);
                return;
            }

            // (0 <-- Getting unit first
            var durationSeconds = Regex.Matches(durationParenthesis[0].Value, @"\({1}[0-9]+");
            if (durationSeconds.Count == 0)
            {
                _keywordResult.ErrorMsg = string.Format("Cannot parse duration second from {0}", durationParenthesis[0].Value);
                return;
            }
            int seconds = 0;
            // 0
            var secondstring = durationSeconds[0].Value.Replace("(", "");
            if (!int.TryParse(secondstring, out seconds))
            {
                _keywordResult.ErrorMsg = string.Format("Cannot parse secondString to int from {0}", secondstring);
                return;
            }

            // 78 giay) <-- Getting decimals then
            var durationDecimals = Regex.Matches(durationParenthesis[0].Value, @"[0-9]+[ ]{1}");
            if (durationDecimals.Count == 0)
            {
                _keywordResult.ErrorMsg = string.Format("Cannot parse duration decimals from {0}", durationParenthesis[0].Value);
                return;
            }
            int decimals = 0;
            // 0
            var decimalsString = durationDecimals[0].Value.Replace(" ", "");
            if (!int.TryParse(decimalsString, out decimals))
            {
                _keywordResult.ErrorMsg = string.Format("Cannot parse decimalsString to int from {0}", secondstring);
                return;
            }

            _keywordResult.RequestDuration = seconds * 1000 + decimals * 10;
        }
        private void ParseRequestStatsResultCount(string requestStatsString)
        {
            // About 340,000,000 results (0.68 seconds)
            // ==>  340,000,000 
            var requestCountMatches = Regex.Matches(requestStatsString, @"[ ][0-9,\. ]+");
            if (requestCountMatches.Count == 0)
            {
                _keywordResult.ErrorMsg = string.Format("Cannot parse results count from {0}", requestStatsString);
                return;
            }
            // ==>340000000 
            var requestCountString = requestCountMatches[0].Value.Replace(" ", "");
            requestCountString = requestCountString.Replace(",", "");
            requestCountString = requestCountString.Replace(".", "");
            double resultCount = 0;
            if (!double.TryParse(requestCountString, out resultCount))
            {
                _keywordResult.ErrorMsg = string.Format("Cannot parse resultCount to double from {0}", resultCount);
                return;
            }
            _keywordResult.TotalThouthandResultsCount = resultCount / 1000;
        }
    }


}