using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace KeywordsApp.Models.File
{
    public class UploadFormViewModel
    {
        const int NAME_MAX_LENGTH = 256;
        public int PreviousFileId { get; set; }

        public string FileName { get; internal set; }
        public string ErrorMsg { get; set; }
        public string SuccessMsg { get; set; }

        public bool IsValid { get { return string.IsNullOrEmpty(ErrorMsg); } }

        public List<string> Keywords { get; set; }

        private int _maxCsvFileSizeUpload;


        public UploadFormViewModel(IFormFile csvFile, IConfiguration config)
        {
            initConfig(config);

            if (!validate(csvFile))
                return;

            if (!parseUntrustedFileName(csvFile.FileName))
                return;

            parse(csvFile);
        }

        private void initConfig(IConfiguration config)
        {
            _maxCsvFileSizeUpload = int.Parse(config["MaxCsvFileSizeUpload"]);
        }

        private bool validate(IFormFile csvFile)
        {
            if (csvFile == null)
                ErrorMsg = "File not found. Please try again with another file.";
            else if (csvFile.Length <= 0)
                ErrorMsg = "The file cannot be empty.";
            else if (csvFile.Length > _maxCsvFileSizeUpload)
                ErrorMsg = "The file must be under 10Kb.";
            return IsValid;
        }

        private void parse(IFormFile csvFile)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(false, ',');
            var csvParser = new CsvParser<string[]>(csvParserOptions, new CsvStringArrayMapping());

            var records = csvParser.ReadFromStream(csvFile.OpenReadStream(), Encoding.UTF8);

            Keywords = records.Select(x => x.Result[0]).ToList();

            if (Keywords.Count <= 0)
            {
                ErrorMsg = "No keyword found in the provided csv file.";
                return;
            }

            if (Keywords.Count > 100)
            {
                ErrorMsg = "The file cannot contain more than 100 keywords.";
                return;
            }

            var keywordRegex = new Regex(@"^[^<>=:;.\{\}\[\]]+$");
            if (Keywords.Any(kw => !keywordRegex.IsMatch(kw)))
                ErrorMsg = "Keywords with specical chars (<>=:.{}[]...) are not allowed.";

        }

        private bool parseUntrustedFileName(string untrustedFileName)
        {
            string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", @"<>:\\\{\}\|\?\*\[\]\+");

            FileName = System.Text.RegularExpressions.Regex.Replace(untrustedFileName, invalidRegStr, " ");

            FileName = FileName.Replace(".csv", "");
            if (FileName.Length > NAME_MAX_LENGTH)
                FileName = FileName.Substring(0, NAME_MAX_LENGTH);

            if (string.IsNullOrWhiteSpace(FileName))
                ErrorMsg = "Your file name is not valid.";

            return IsValid;
        }
    }
}
