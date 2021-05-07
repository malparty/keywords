using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KeywordsApp.Models.File;
using KeywordsApp.Models.Keyword;
using Xunit;

namespace KeywordsApp.Tests.Keyword
{
    public class KeywordParserModel_Tests
    {
        public class KeywordParserModel_ParseStatsMethod
        {
            [Fact]
            public void ParseStatsMethod1_ParsingNormalPage_IsValid()
            {
                var parser = new KeywordParserFake();
                var result = parser.ParseHtml();
                Assert.True(result.IsValid);
            }

            [Fact]
            public void ParseStatsMethod2_CannotFindStatString_IsNotValid()
            {
                var parser = new KeywordParserFake();
                parser.RawHtmlContent = parser.RawHtmlContent.Replace("result-stats", "another-id");
                var result = parser.ParseHtml();
                Assert.False(result.IsValid);
            }


            [Theory]
            [InlineData("(0.68 seconds")]
            [InlineData("(A.68 seconds)")]
            [InlineData("(0. seconds)")]
            public void ParseMethod3_CannotParseRequestDuration_IsNotValid(string value)
            {
                var parser = new KeywordParserFake();
                parser.RawHtmlContent = parser.RawHtmlContent.Replace("(0.68 seconds)", value);
                var result = parser.ParseHtml();
                Assert.False(result.IsValid);
            }

        }
    }

    public class KeywordParserFake : KeywordParserModel
    {
        public KeywordParserFake()
        {
            KeywordId = 1;
            Name = "hello";
            RawHtmlContent = "<html itemscope=\"\" itemtype=\"http://schema.org/SearchResultsPage\" lang=\"en-VN\"><head><title>hello world - Google Search</title></head><body jsmodel=\"TvHxbe\" class=\"srp wf-b vasq\" jscontroller=\"aCZVp\" marginheight=\"3\" topmargin=\"3\" id=\"gsr\" ><div class=\"main\" id=\"main\"><div id=\"cnt\" class=\"big\"><div jscontroller=\"qik19b\" jsdata=\"Z1JpA;_;CHz8xI\" jsaction=\"rcuQ6b:npT2md\" class=\"gke0pe\" id=\"top_nav\" ></div><div class=\"appbar\" id=\"appbar\"><div id=\"extabar\"><div style=\"position: relative\"><div class=\"WE0UJf\" id=\"slim_appbar\"><div class=\"LHJvCe\"><div id=\"result-stats\"> About 1,780,000,000 results<nobr> (0.68 seconds)&nbsp;</nobr ></div></div></div></div></div></div><div class=\"GyAeWb\" id=\"rcnt\"><div class=\"D6j0vc\"><div id=\"center_col\"><div id=\"taw\"><div></div><div></div><div id=\"tvcap\"></div></div><div class=\"eqAnXb\" id=\"res\" role=\"main\"><div id=\"topstuff\"></div><div id=\"search\"><div data-hveid=\"CAMQLw\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQGnoECAMQLw\" ><h1 class=\"Uo8X3b\">Search Results</h1><div eid=\"YMaUYLyLCcvT-QbA5pYg\" data-async-context=\"query:hello%20world\" id=\"rso\" ><div class=\"hlcw0c\"><div class=\"g\"><h2 class=\"Uo8X3b\">Web results</h2><div data-hveid=\"CAYQAA\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFSgAMAB6BAgGEAA\" ><div class=\"tF2Cxc\"><div class=\"yuRUbf\"> <a href=\"https://en.wikipedia.org/wiki/%22Hello,_World!%22_program\" data-jsarwt=\"1\" data-usg=\"AOvVaw3amFkG8-Aw6a06ezWAoWQr\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFjAAegQIBhAD\" ><br /><h3 class=\"LC20lb DKV0Md\"> \"Hello, World!\" program - Wikipedia</h3><div class=\"TbwUpd NJjxre\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://en.wikipedia.org<span class=\"dyjrff qzEoUe\" > › wiki › \"Hello,_World!\"_prog...</span ></cite ></div></a ><div class=\"B6fmyf\"><div class=\"TbwUpd\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://en.wikipedia.org<span class=\"dyjrff qzEoUe\" > › wiki › \"Hello,_World!\"_prog...</span ></cite ></div><div class=\"eFM0qc\"> <span ><div jscontroller=\"hiU8Ie\" class=\"action-menu\" > <a class=\"GHDvEf\" href=\"#\" aria-label=\"Result options\" aria-expanded=\"false\" aria-haspopup=\"true\" role=\"button\" jsaction=\"PZcoEd;keydown:wU6FVd;keypress:uWmNaf\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ7B0wAHoECAYQBg\" ><span class=\"gTl8xb\"></span ></a><ol class=\"action-menu-panel\" role=\"menu\" tabindex=\"-1\" jsaction=\"keydown:Xiq7wd;mouseover:pKPowd;mouseout:O9bKS\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQqR8wAHoECAYQBw\" ><li class=\"action-menu-item\" role=\"menuitem\" > <a class=\"fl\" href=\"https://webcache.googleusercontent.com/search?q=cache:UInCiDfJyeUJ:https://en.wikipedia.org/wiki/%2522Hello,_World!%2522_program+&amp;cd=1&amp;hl=en&amp;ct=clnk&amp;gl=vn\" data-jsarwt=\"1\" data-usg=\"AOvVaw3ax_eUWfYc2IsYk9VqN8VM\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQIDAAegQIBhAI\" ><span>Cached</span></a ></li><li class=\"action-menu-item\" role=\"menuitem\" > <a class=\"fl\" href=\"/search?q=related:https://en.wikipedia.org/wiki/%2522Hello,_World!%2522_program+hello+world&amp;sa=X&amp;ved=2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQHzAAegQIBhAJ\" ><span>Similar</span></a ></li></ol></div></span ></div></div></div><div class=\"IsZvec\"> <span class=\"aCOpRe\" ><span >A \"<em>Hello</em>, <em>World</em>!\" program generally is a computer program that outputs or displays the message \"<em>Hello</em>, <em>World</em>!\". Such a program is very simple in most programming languages, and is often used to illustrate the basic syntax of a programming language. It is often the first program written by people learning to code.</span ></span ><div class=\"jYOxx\"><div class=\"HiHjCd\"> &lrm;<a href=\"https://en.wikipedia.org/wiki/%22Hello,_World!%22_program#History\" data-jsarwt=\"1\" data-usg=\"AOvVaw3amFkG8-Aw6a06ezWAoWQr\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ0gIoAHoECAUQAQ\" >History</a > · &lrm;<a href=\"https://en.wikipedia.org/wiki/%22Hello,_World!%22_program#Variations\" data-jsarwt=\"1\" data-usg=\"AOvVaw3amFkG8-Aw6a06ezWAoWQr\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ0gIoAXoECAUQAg\" >Variations</a > · &lrm;<a href=\"https://en.wikipedia.org/wiki/%22Hello,_World!%22_program#Time_to_Hello_World\" data-jsarwt=\"1\" data-usg=\"AOvVaw3amFkG8-Aw6a06ezWAoWQr\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ0gIoAnoECAUQAw\" >Time to Hello World</a ></div></div></div><div jscontroller=\"m6a0l\" id=\"eob_14\" jsdata=\"fxg5tf;_;CHz8xY\" jsaction=\"rcuQ6b:npT2md\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ2Z0BMAB6BAgGEAo\" ><div jsname=\"UTgHCf\" class=\"AUiS2\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQx40DegQINRAA\" ><div jsname=\"d3PE6e\" style=\"display: none\"><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAB6BAg1EAE\" > hello world mal</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAF6BAg1EAI\" > hello world full movie</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAJ6BAg1EAM\" > hello world myanimelist</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAN6BAg1EAQ\" > time to hello world</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAR6BAg1EAU\" > hello world c++</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAV6BAg1EAY\" > hello world code</div></div><div><div jsname=\"l1CLDf\" class=\"d8lLoc\"><h4 jsname=\"IaVMje\" class=\"eJ7tvc\"> People also search for</h4> <span jsname=\"ZnuYW\" class=\"XCKyNd\" jsaction=\"ornU0b\" aria-label=\"Dismiss suggested follow ups\" role=\"button\" tabindex=\"0\" ></span><div jsname=\"CeevUc\" class=\"hYkSRb\" style=\"display: none\" ></div></div></div></div></div></div></div></div></div><div class=\"hlcw0c\"><div class=\"g\"><h2 class=\"Uo8X3b\">Web results</h2><div data-hveid=\"CAQQAA\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFSgAMAt6BAgEEAA\" ><div class=\"tF2Cxc\"><div class=\"yuRUbf\"> <a href=\"https://vi.wikipedia.org/wiki/%22Hello,_World!%22_(ch%C6%B0%C6%A1ng_tr%C3%ACnh_m%C3%A1y_t%C3%ADnh)\" data-jsarwt=\"1\" data-usg=\"AOvVaw3tPmttZoIAOrwjau_lkcfU\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFjALegQIBBAD\" ><br /><h3 class=\"LC20lb DKV0Md\"> \"Hello, World!\" (chương trình máy tính) – Wikipedia tiếng Việt</h3><div class=\"TbwUpd NJjxre\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://vi.wikipedia.org<span class=\"dyjrff qzEoUe\" > › wiki › \"Hel...</span ></cite ></div></a ><div class=\"B6fmyf\"><div class=\"TbwUpd\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://vi.wikipedia.org<span class=\"dyjrff qzEoUe\" > › wiki › \"Hel...</span ></cite ></div></div></div><div class=\"IsZvec\"> <span class=\"aCOpRe\" ><span >Trong các thiết bị không hiển thị thông điệp, một chương trình đơn giản là phát sinh tín hiện, như bật đèn LED sáng để thay thế cho dòng chữ \"<em>Hello world</em>\" như là&nbsp;...</span ></span ></div><div jscontroller=\"m6a0l\" id=\"eob_13\" jsdata=\"fxg5tf;_;CHz8xU\" jsaction=\"rcuQ6b:npT2md\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ2Z0BMAt6BAgEEAs\" ><div jsname=\"UTgHCf\" class=\"AUiS2\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQx40DegQIPRAA\" ><div jsname=\"d3PE6e\" style=\"display: none\"><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAB6BAg9EAE\" > hello world mal</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAF6BAg9EAI\" > hello world full movie</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAJ6BAg9EAM\" > hello world myanimelist</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAN6BAg9EAQ\" > time to hello world</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAR6BAg9EAU\" > hello world c++</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAV6BAg9EAY\" > hello world code</div></div><div><div jsname=\"l1CLDf\" class=\"d8lLoc\"><h4 jsname=\"IaVMje\" class=\"eJ7tvc\"> People also search for</h4> <span jsname=\"ZnuYW\" class=\"XCKyNd\" jsaction=\"ornU0b\" aria-label=\"Dismiss suggested follow ups\" role=\"button\" tabindex=\"0\" ></span><div jsname=\"CeevUc\" class=\"hYkSRb\" style=\"display: none\" ></div></div></div></div></div></div></div></div></div><div class=\"ULSxyf\"><span id=\"fld\"></span></div><div class=\"ULSxyf\"></div><div class=\"g\"><h2 class=\"Uo8X3b\">Web results</h2><div data-hveid=\"CB4QAA\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFSgAMBd6BAgeEAA\" ><div class=\"tF2Cxc\"><div class=\"yuRUbf\"> <a href=\"https://blog.hackerrank.com/the-history-of-hello-world/\" data-jsarwt=\"1\" data-usg=\"AOvVaw0f0x8oEm-e4U4M43nw6B2E\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFjAXegQIHhAD\" ><br /><h3 class=\"LC20lb DKV0Md\"> The History of 'Hello, World' - HackerRank Blog</h3><div class=\"TbwUpd NJjxre\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://blog.hackerrank.com<span class=\"dyjrff qzEoUe\" > › the-history-of-hello-world</span ></cite ></div></a ><div class=\"B6fmyf\"><div class=\"TbwUpd\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://blog.hackerrank.com<span class=\"dyjrff qzEoUe\" > › the-history-of-hello-world</span ></cite ></div><div class=\"eFM0qc\"> <span ><div jscontroller=\"hiU8Ie\" class=\"action-menu\" > <a class=\"GHDvEf\" href=\"#\" aria-label=\"Result options\" aria-expanded=\"false\" aria-haspopup=\"true\" role=\"button\" jsaction=\"PZcoEd;keydown:wU6FVd;keypress:uWmNaf\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ7B0wF3oECB4QBg\" ><span class=\"gTl8xb\"></span ></a><ol class=\"action-menu-panel\" role=\"menu\" tabindex=\"-1\" jsaction=\"keydown:Xiq7wd;mouseover:pKPowd;mouseout:O9bKS\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQqR8wF3oECB4QBw\" ><li class=\"action-menu-item\" role=\"menuitem\" > <a class=\"fl\" href=\"https://webcache.googleusercontent.com/search?q=cache:8aToCD1-a0QJ:https://blog.hackerrank.com/the-history-of-hello-world/+&amp;cd=24&amp;hl=en&amp;ct=clnk&amp;gl=vn\" data-jsarwt=\"1\" data-usg=\"AOvVaw0VOj2680HWTeBO16lqOIjT\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQIDAXegQIHhAI\" ><span>Cached</span></a > ";
            RawHtmlContent += " </li></ol></div></span ></div></div></div><div class=\"IsZvec\"> <span class=\"aCOpRe\" ><span class=\"f\">Apr 21, 2015 — </span ><span >As a function, the computer program simply tells the computer to display the words “<em>Hello</em>, <em>World</em>!” Traditionally, it's the first program developers&nbsp;...</span ></span ></div><div jscontroller=\"m6a0l\" id=\"eob_110\" jsdata=\"fxg5tf;_;CHz8zA\" jsaction=\"rcuQ6b:npT2md\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ2Z0BMBd6BAgeEAs\" ><div jsname=\"UTgHCf\" class=\"AUiS2\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQx40DegQIPBAA\" ><div jsname=\"d3PE6e\" style=\"display: none\"><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAB6BAg8EAE\" > hello world mal</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAF6BAg8EAI\" > hello world full movie</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAJ6BAg8EAM\" > hello world myanimelist</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAN6BAg8EAQ\" > time to hello world</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAR6BAg8EAU\" > hello world c++</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAV6BAg8EAY\" > hello world code</div></div><div><div jsname=\"l1CLDf\" class=\"d8lLoc\"><h4 jsname=\"IaVMje\" class=\"eJ7tvc\"> People also search for</h4> <span jsname=\"ZnuYW\" class=\"XCKyNd\" jsaction=\"ornU0b\" aria-label=\"Dismiss suggested follow ups\" role=\"button\" tabindex=\"0\" ></span><div jsname=\"CeevUc\" class=\"hYkSRb\" style=\"display: none\" ></div></div></div></div></div></div></div></div><div class=\"g\"><div data-hveid=\"CB8QAA\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFSgAMBh6BAgfEAA\" ><div class=\"tF2Cxc\"><div class=\"yuRUbf\"> <a href=\"https://www.learnpython.org/en/Hello,_World!\" data-jsarwt=\"1\" data-usg=\"AOvVaw3vCbuoGv4HBM8EznJDGvUG\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFjAYegQIHxAD\" ><br /><h3 class=\"LC20lb DKV0Md\"> Hello, World! - Learn Python - Free Interactive Python Tutorial</h3><div class=\"TbwUpd NJjxre\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://www.learnpython.org<span class=\"dyjrff qzEoUe\" > › Hello,_World!</span ></cite ></div></a ><div class=\"B6fmyf\"><div class=\"TbwUpd\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://www.learnpython.org<span class=\"dyjrff qzEoUe\" > › Hello,_World!</span ></cite ></div><div class=\"eFM0qc\"> <span ><div jscontroller=\"hiU8Ie\" class=\"action-menu\" > <a class=\"GHDvEf\" href=\"#\" aria-label=\"Result options\" aria-expanded=\"false\" aria-haspopup=\"true\" role=\"button\" jsaction=\"PZcoEd;keydown:wU6FVd;keypress:uWmNaf\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ7B0wGHoECB8QBg\" ><span class=\"gTl8xb\"></span ></a><ol class=\"action-menu-panel\" role=\"menu\" tabindex=\"-1\" jsaction=\"keydown:Xiq7wd;mouseover:pKPowd;mouseout:O9bKS\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQqR8wGHoECB8QBw\" ><li class=\"action-menu-item\" role=\"menuitem\" > <a class=\"fl\" href=\"https://webcache.googleusercontent.com/search?q=cache:DUlbPV2A5dIJ:https://www.learnpython.org/en/Hello,_World!+&amp;cd=25&amp;hl=en&amp;ct=clnk&amp;gl=vn\" data-jsarwt=\"1\" data-usg=\"AOvVaw3I99LzKwPw7dGgW_DXyIPQ\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQIDAYegQIHxAI\" ><span>Cached</span></a ></li></ol></div></span ></div></div></div><div class=\"IsZvec\"> <span class=\"aCOpRe\" ><span ><em>Hello</em>, <em>World</em>! Python is a very simple language, and has a very straightforward syntax. It encourages programmers to program without boilerplate (prepared)&nbsp;...</span ></span ></div><div jscontroller=\"m6a0l\" id=\"eob_113\" jsdata=\"fxg5tf;_;CHz8zE\" jsaction=\"rcuQ6b:npT2md\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ2Z0BMBh6BAgfEAk\" ><div jsname=\"UTgHCf\" class=\"AUiS2\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQx40DegQINhAA\" ><div jsname=\"d3PE6e\" style=\"display: none\"><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAB6BAg2EAE\" > hello world mal</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAF6BAg2EAI\" > hello world full movie</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAJ6BAg2EAM\" > hello world myanimelist</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAN6BAg2EAQ\" > time to hello world</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAR6BAg2EAU\" > hello world c++</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAV6BAg2EAY\" > hello world code</div></div><div><div jsname=\"l1CLDf\" class=\"d8lLoc\"><h4 jsname=\"IaVMje\" class=\"eJ7tvc\"> People also search for</h4> <span jsname=\"ZnuYW\" class=\"XCKyNd\" jsaction=\"ornU0b\" aria-label=\"Dismiss suggested follow ups\" role=\"button\" tabindex=\"0\" ></span><div jsname=\"CeevUc\" class=\"hYkSRb\" style=\"display: none\" ></div></div></div></div></div></div></div></div><div class=\"g\"><div data-hveid=\"CBYQAA\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFSgAMBl6BAgWEAA\" ><div class=\"tF2Cxc\"><div class=\"yuRUbf\"> <a href=\"https://www.programiz.com/c-programming/examples/print-sentence\" data-jsarwt=\"1\" data-usg=\"AOvVaw2nQLUEsDjzJwUL8cmrj221\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFjAZegQIFhAD\" ><br /><h3 class=\"LC20lb DKV0Md\"> C \"Hello, World!\" Program</h3><div class=\"TbwUpd NJjxre\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://www.programiz.com<span class=\"dyjrff qzEoUe\" > › examples › print-sentence</span ></cite ></div></a ><div class=\"B6fmyf\"><div class=\"TbwUpd\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://www.programiz.com<span class=\"dyjrff qzEoUe\" > › examples › print-sentence</span ></cite ></div><div class=\"eFM0qc\"> <span ><div jscontroller=\"hiU8Ie\" class=\"action-menu\" > <a class=\"GHDvEf\" href=\"#\" aria-label=\"Result options\" aria-expanded=\"false\" aria-haspopup=\"true\" role=\"button\" jsaction=\"PZcoEd;keydown:wU6FVd;keypress:uWmNaf\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ7B0wGXoECBYQBg\" ><span class=\"gTl8xb\"></span ></a><ol class=\"action-menu-panel\" role=\"menu\" tabindex=\"-1\" jsaction=\"keydown:Xiq7wd;mouseover:pKPowd;mouseout:O9bKS\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQqR8wGXoECBYQBw\" ><li class=\"action-menu-item\" role=\"menuitem\" > <a class=\"fl\" href=\"https://webcache.googleusercontent.com/search?q=cache:bnpvF-rnYjUJ:https://www.programiz.com/c-programming/examples/print-sentence+&amp;cd=26&amp;hl=en&amp;ct=clnk&amp;gl=vn\" data-jsarwt=\"1\" data-usg=\"AOvVaw0VXwKQPCptzdNpciGmBpdc\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQIDAZegQIFhAI\" ><span>Cached</span></a ></li><li class=\"action-menu-item\" role=\"menuitem\" > <a class=\"fl\" href=\"/search?q=related:https://www.programiz.com/c-programming/examples/print-sentence+hello+world&amp;sa=X&amp;ved=2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQHzAZegQIFhAJ\" ><span>Similar</span></a ></li></ol></div></span ></div></div></div><div class=\"IsZvec\"> <span class=\"aCOpRe\" ><span >In this example, you will learn to print \"<em>Hello</em>, <em>World</em>!\" on the screen in C programming. A \"<em>Hello</em>, <em>World</em>!\" is a simple program to display \"<em>Hello</em>, <em>World</em>!\" on the&nbsp;...</span ></span ></div><div jscontroller=\"m6a0l\" id=\"eob_89\" jsdata=\"fxg5tf;_;CHz8yw\" jsaction=\"rcuQ6b:npT2md\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ2Z0BMBl6BAgWEAo\" ><div jsname=\"UTgHCf\" class=\"AUiS2\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQx40DegQIOhAA\" ><div jsname=\"d3PE6e\" style=\"display: none\"><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAB6BAg6EAE\" > hello world mal</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAF6BAg6EAI\" > hello world full movie</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAJ6BAg6EAM\" > hello world myanimelist</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAN6BAg6EAQ\" > time to hello world</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAR6BAg6EAU\" > hello world c++</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAV6BAg6EAY\" > hello world code</div></div><div><div jsname=\"l1CLDf\" class=\"d8lLoc\"><h4 jsname=\"IaVMje\" class=\"eJ7tvc\"> People also search for</h4> <span jsname=\"ZnuYW\" class=\"XCKyNd\" jsaction=\"ornU0b\" aria-label=\"Dismiss suggested follow ups\" role=\"button\" tabindex=\"0\" ></span><div jsname=\"CeevUc\" class=\"hYkSRb\" style=\"display: none\" ></div></div></div></div></div></div></div></div><div class=\"g\"><div data-hveid=\"CAoQAA\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFSgAMBp6BAgKEAA\" ><div class=\"tF2Cxc\"><div class=\"yuRUbf\"> <a href=\"https://guides.github.com/activities/hello-world/\" data-jsarwt=\"1\" data-usg=\"AOvVaw0OomplqyFlv7mIgod-m50J\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFjAaegQIChAD\" ><br /><h3 class=\"LC20lb DKV0Md\"> Hello World · GitHub Guides</h3><div class=\"TbwUpd NJjxre\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://guides.github.com<span class=\"dyjrff qzEoUe\" > › activities › hello-world</span ></cite ></div></a ><div class=\"B6fmyf\"><div class=\"TbwUpd\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://guides.github.com<span class=\"dyjrff qzEoUe\" > › activities › hello-world</span ></cite ></div><div class=\"eFM0qc\"> <span ><div jscontroller=\"hiU8Ie\" class=\"action-menu\" > <a class=\"GHDvEf\" href=\"#\" aria-label=\"Result options\" aria-expanded=\"false\" aria-haspopup=\"true\" role=\"button\" jsaction=\"PZcoEd;keydown:wU6FVd;keypress:uWmNaf\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ7B0wGnoECAoQBg\" ><span class=\"gTl8xb\"></span ></a><ol class=\"action-menu-panel\" role=\"menu\" tabindex=\"-1\" jsaction=\"keydown:Xiq7wd;mouseover:pKPowd;mouseout:O9bKS\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQqR8wGnoECAoQBw\" ><li class=\"action-menu-item\" role=\"menuitem\" > <a class=\"fl\" href=\"https://webcache.googleusercontent.com/search?q=cache:V5qh2gVcfskJ:https://guides.github.com/activities/hello-world/+&amp;cd=27&amp;hl=en&amp;ct=clnk&amp;gl=vn\" data-jsarwt=\"1\" ";
            RawHtmlContent += " data-usg=\"AOvVaw0wp-Jza5yXCF_vZ4JklhJO\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQIDAaegQIChAI\" ><span>Cached</span></a ></li><li class=\"action-menu-item\" role=\"menuitem\" > <a class=\"fl\" href=\"/search?q=related:https://guides.github.com/activities/hello-world/+hello+world&amp;sa=X&amp;ved=2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQHzAaegQIChAJ\" ><span>Similar</span></a ></li></ol></div></span ></div></div></div><div class=\"IsZvec\"> <span class=\"aCOpRe\" ><span class=\"f\">Jul 24, 2020 — </span ><span >You'll create your own <em>Hello World</em> repository and learn GitHub's Pull Request workflow, a popular way to create and review code. No coding&nbsp;...</span ></span ></div><div jscontroller=\"m6a0l\" id=\"eob_61\" jsdata=\"fxg5tf;_;CHz8yU\" jsaction=\"rcuQ6b:npT2md\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ2Z0BMBp6BAgKEAs\" ><div jsname=\"UTgHCf\" class=\"AUiS2\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQx40DegQIOBAA\" ><div jsname=\"d3PE6e\" style=\"display: none\"><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAB6BAg4EAE\" > hello world mal</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAF6BAg4EAI\" > hello world full movie</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAJ6BAg4EAM\" > hello world myanimelist</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAN6BAg4EAQ\" > time to hello world</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAR6BAg4EAU\" > hello world c++</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAV6BAg4EAY\" > hello world code</div></div><div><div jsname=\"l1CLDf\" class=\"d8lLoc\"><h4 jsname=\"IaVMje\" class=\"eJ7tvc\"> People also search for</h4> <span jsname=\"ZnuYW\" class=\"XCKyNd\" jsaction=\"ornU0b\" aria-label=\"Dismiss suggested follow ups\" role=\"button\" tabindex=\"0\" ></span><div jsname=\"CeevUc\" class=\"hYkSRb\" style=\"display: none\" ></div></div></div></div></div></div></div></div><div class=\"g\"><div data-hveid=\"CAkQAA\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFSgAMBt6BAgJEAA\" ><div class=\"tF2Cxc\"><div class=\"yuRUbf\"> <a href=\"https://reactjs.org/docs/hello-world.html\" data-jsarwt=\"1\" data-usg=\"AOvVaw1QCIuiJ7CqY8Vi40tbkOgv\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFjAbegQICRAD\" ><br /><h3 class=\"LC20lb DKV0Md\"> Hello World – React</h3><div class=\"TbwUpd NJjxre\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://reactjs.org<span class=\"dyjrff qzEoUe\" > › docs › hello-world</span ></cite ></div></a ><div class=\"B6fmyf\"><div class=\"TbwUpd\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://reactjs.org<span class=\"dyjrff qzEoUe\" > › docs › hello-world</span ></cite ></div><div class=\"eFM0qc\"> <span ><div jscontroller=\"hiU8Ie\" class=\"action-menu\" > <a class=\"GHDvEf\" href=\"#\" aria-label=\"Result options\" aria-expanded=\"false\" aria-haspopup=\"true\" role=\"button\" jsaction=\"PZcoEd;keydown:wU6FVd;keypress:uWmNaf\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ7B0wG3oECAkQBg\" ><span class=\"gTl8xb\"></span ></a><ol class=\"action-menu-panel\" role=\"menu\" tabindex=\"-1\" jsaction=\"keydown:Xiq7wd;mouseover:pKPowd;mouseout:O9bKS\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQqR8wG3oECAkQBw\" ><li class=\"action-menu-item\" role=\"menuitem\" > <a class=\"fl\" href=\"/url?sa=t&amp;rct=j&amp;q=&amp;esrc=s&amp;source=web&amp;cd=&amp;cad=rja&amp;uact=8&amp;ved=2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQIDAbegQICRAI&amp;url=https%3A%2F%2Fwebcache.googleusercontent.com%2Fsearch%3Fq%3Dcache%3AWe7zpakyqG8J%3Ahttps%3A%2F%2Freactjs.org%2Fdocs%2Fhello-world.html%2B%26cd%3D28%26hl%3Den%26ct%3Dclnk%26gl%3Dvn&amp;usg=AOvVaw0Zt8ZFIikHz6q8pDG8tnR1\" data-jsarwt=\"1\" data-usg=\"AOvVaw0Zt8ZFIikHz6q8pDG8tnR1\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQIDAbegQICRAI\" data-ctbtn=\"2\" data-cthref=\"/url?sa=t&amp;rct=j&amp;q=&amp;esrc=s&amp;source=web&amp;cd=&amp;cad=rja&amp;uact=8&amp;ved=2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQIDAbegQICRAI&amp;url=https%3A%2F%2Fwebcache.googleusercontent.com%2Fsearch%3Fq%3Dcache%3AWe7zpakyqG8J%3Ahttps%3A%2F%2Freactjs.org%2Fdocs%2Fhello-world.html%2B%26cd%3D28%26hl%3Den%26ct%3Dclnk%26gl%3Dvn&amp;usg=AOvVaw0Zt8ZFIikHz6q8pDG8tnR1\" data-jrwt=\"1\" ></a></li></ol></div ></span></div></div></div><div class=\"IsZvec\"> <span class=\"aCOpRe\" ><span ><em>Hello World</em>. The smallest React example looks like this: ReactDOM.render( &lt;h1&gt;<wbr /><em>Hello</em>, <em>world</em>!&lt;/h1&gt;, document.getElementById('root') );. It displays a heading&nbsp;...</span ></span ></div><div jscontroller=\"m6a0l\" id=\"eob_60\" jsdata=\"fxg5tf;_;CHz8yQ\" jsaction=\"rcuQ6b:npT2md\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ2Z0BMBt6BAgJEAk\" ><div jsname=\"UTgHCf\" class=\"AUiS2\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQx40DegQINxAA\" ><div jsname=\"d3PE6e\" style=\"display: none\"><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAB6BAg3EAE\" > hello world mal</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAF6BAg3EAI\" > hello world full movie</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAJ6BAg3EAM\" > hello world myanimelist</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAN6BAg3EAQ\" > time to hello world</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAR6BAg3EAU\" > hello world c++</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAV6BAg3EAY\" > hello world code</div></div><div><div jsname=\"l1CLDf\" class=\"d8lLoc\"><h4 jsname=\"IaVMje\" class=\"eJ7tvc\"> People also search for</h4> <span jsname=\"ZnuYW\" class=\"XCKyNd\" jsaction=\"ornU0b\" aria-label=\"Dismiss suggested follow ups\" role=\"button\" tabindex=\"0\" ></span><div jsname=\"CeevUc\" class=\"hYkSRb\" style=\"display: none\" ></div></div></div></div></div></div></div></div><div class=\"g\"><div data-hveid=\"CAgQAA\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFSgAMBx6BAgIEAA\" ><div class=\"tF2Cxc\"><div class=\"yuRUbf\"> <a href=\"https://www.hello-world.com/\" data-jsarwt=\"1\" data-usg=\"AOvVaw1PmK88NvDQUa7W4kDtnm-d\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFjAcegQICBAD\" ><br /><h3 class=\"LC20lb DKV0Md\"> Total immersion, Serious fun! with Hello-World!</h3><div class=\"TbwUpd NJjxre\"><cite class=\"iUh30 Zu0yb tjvcx\" >https://www.hello-world.com</cite ></div></a ></div><div class=\"IsZvec\"> <span class=\"aCOpRe\" ><span >Main index for <em>hello</em>-<em>world</em>: links to login and all of the languages.</span ></span ></div><div jscontroller=\"m6a0l\" id=\"eob_59\" jsdata=\"fxg5tf;_;CHz8yM\" jsaction=\"rcuQ6b:npT2md\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ2Z0BMBx6BAgIEAc\" ><div jsname=\"UTgHCf\" class=\"AUiS2\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQx40DegQIORAA\" ><div jsname=\"d3PE6e\" style=\"display: none\"><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAB6BAg5EAE\" > hello world mal</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAF6BAg5EAI\" > hello world full movie</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAJ6BAg5EAM\" > hello world myanimelist</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAN6BAg5EAQ\" > time to hello world</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAR6BAg5EAU\" > hello world c++</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAV6BAg5EAY\" > hello world code</div></div><div><div jsname=\"l1CLDf\" class=\"d8lLoc\"><h4 jsname=\"IaVMje\" class=\"eJ7tvc\"> People also search for</h4> <span jsname=\"ZnuYW\" class=\"XCKyNd\" jsaction=\"ornU0b\" aria-label=\"Dismiss suggested follow ups\" role=\"button\" tabindex=\"0\" ></span><div jsname=\"CeevUc\" class=\"hYkSRb\" style=\"display: none\" ></div></div></div></div></div></div></div></div><div class=\"hlcw0c\"><div class=\"g\"><div data-hveid=\"CB0QAA\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFSgAMB16BAgdEAA\" ><div class=\"tF2Cxc\"><div class=\"yuRUbf\"> <a href=\"https://hub.docker.com/_/hello-world\" data-jsarwt=\"1\" data-usg=\"AOvVaw09GLYA3Z6MERUA2_v8BB6n\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQFjAdegQIHRAD\" ><br /><h3 class=\"LC20lb DKV0Md\"> hello-world - Docker Hub</h3><div class=\"TbwUpd NJjxre\"><cite class=\"iUh30 Zu0yb qLRx3b tjvcx\" >https://hub.docker.com<span class=\"dyjrff qzEoUe\" > › hello-world</span ></cite ></div></a ></div><div class=\"IsZvec\"> <span class=\"aCOpRe\" ><span >Example output. $ docker run <em>hello</em>-<em>world</em> Hello from Docker! This message shows that your installation appears to be working correctly. To generate this message,&nbsp;...</span ></span ></div><div jscontroller=\"m6a0l\" id=\"eob_108\" jsdata=\"fxg5tf;_;CHz8y8\" jsaction=\"rcuQ6b:npT2md\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQ2Z0BMB16BAgdEAk\" ><div jsname=\"UTgHCf\" class=\"AUiS2\" data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQx40DegQIOxAA\" ><div jsname=\"d3PE6e\" style=\"display: none\"><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAB6BAg7EAE\" > hello world mal</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAF6BAg7EAI\" > hello world full movie</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAJ6BAg7EAM\" > hello world myanimelist</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAN6BAg7EAQ\" > time to hello world</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAR6BAg7EAU\" > hello world c++</div><div data-ved=\"2ahUKEwi8u-L_4bbwAhXLad4KHUCzBQQQsKwBKAV6BAg7EAY\" > hello world code</div></div><div><div jsname=\"l1CLDf\" class=\"d8lLoc\"><h4 jsname=\"IaVMje\" class=\"eJ7tvc\"> People also search for</h4> <span jsname=\"ZnuYW\" class=\"XCKyNd\" jsaction=\"ornU0b\" aria-label=\"Dismiss suggested follow ups\" role=\"button\" tabindex=\"0\" ></span><div jsname=\"CeevUc\" class=\"hYkSRb\" style=\"display: none\" ></div></div></div></div></div></div></div></div></div></div></div></div></div></div></div></div></div></div></body></html>";

        }
    }

}