using System;
using System.Text.RegularExpressions;
using HtmlAgilityPack;


namespace KeywordsApp.Models.Keyword
{
    public class KeywordParserModel
    {
        public int FileId { get; set; }
        public int KeywordId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string RawHtmlContent { get; set; }
        public HtmlNode BodyNode { get; set; }

        private HtmlDocument _pageDocument;


        public KeywordResultParserModel ParseHtml()
        {
            var result = new KeywordResultParserModel();
            if (!LoadHtmlContent())
            {
                result.ErrorMsg = "Could not parse keyword. Search did not respond with a standard google result.";
                return result;
            }

            ParseRequestStats(result);

            return result;

        }
        private bool LoadHtmlContent()
        {
            _pageDocument = new HtmlDocument();
            _pageDocument.LoadHtml(RawHtmlContent);
            // Test if it is a google search page result based on "GSR" id presence:
            BodyNode = _pageDocument.DocumentNode.SelectSingleNode("//body[contains(@id,'gsr')][1]");
            return !string.IsNullOrEmpty(BodyNode?.InnerHtml);
        }
        private void ParseRequestStats(KeywordResultParserModel result)
        {
            // About 340,000,000 results (0.68 seconds) 
            // Khoảng 2.000.000.000 kết quả (0,78 giây) 
            // Результатов: примерно 23 800 (0,58 сек.) 
            // Environ 49 700 résultats (0,54 secondes) 
            // --> Numbers and ( ) are the only singularity.

            var requestStatsString = BodyNode.SelectSingleNode("//div[contains(@id,'result-stats')][1]")?.InnerHtml;
            if (string.IsNullOrEmpty(requestStatsString))
            {
                result.ErrorMsg = "Cannot find request stats string";
                return;
            }
            // (0,78 giay)
            var durationParenthesis = Regex.Matches(requestStatsString, @"\(.*\)");
            if (durationParenthesis.Count == 0)
            {
                result.ErrorMsg = string.Format("Cannot parse duration parenthesis from {0}", requestStatsString);
                return;
            }

            // (0 <-- Getting unit first
            var durationSeconds = Regex.Matches(durationParenthesis[0].Value, @"\({1}[0-9]+");
            if (durationSeconds.Count == 0)
            {
                result.ErrorMsg = string.Format("Cannot parse duration second from {0}", durationParenthesis[0].Value);
                return;
            }
            int seconds = 0;
            // 0
            var secondstring = durationSeconds[0].Value.Replace("(", "");
            if (!int.TryParse(secondstring, out seconds))
            {
                result.ErrorMsg = string.Format("Cannot parse secondString to int from {0}", secondstring);
                return;
            }

            // 78 giay) <-- Getting decimals then
            var durationDecimals = Regex.Matches(durationParenthesis[0].Value, @"[0-9]+[ ]{1}");
            if (durationDecimals.Count == 0)
            {
                result.ErrorMsg = string.Format("Cannot parse duration decimals from {0}", durationParenthesis[0].Value);
                return;
            }
            int decimals = 0;
            // 0
            var decimalsString = durationDecimals[0].Value.Replace(" ", "");
            if (!int.TryParse(decimalsString, out decimals))
            {
                result.ErrorMsg = string.Format("Cannot parse decimalsString to int from {0}", secondstring);
                return;
            }

            result.RequestDuration = seconds * 1000 + decimals * 10;
        }
    }


}