using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace KeywordsApp.Models.File
{
    public class UploadFormViewModel
    {
        public List<string> Keywords { get; set; }
        public string ErrorMsg { get; set; }
        public bool HasError { get { return !string.IsNullOrEmpty(ErrorMsg); } }
        public UploadFormViewModel() { }
        public UploadFormViewModel(IFormFile csvFile)
        {
            if (validate(csvFile))
                parse(csvFile);
        }

        private bool validate(IFormFile csvFile)
        {
            if (csvFile == null)
                ErrorMsg = "File not found. Please try again with another file.";
            else if (csvFile.Length <= 0)
                ErrorMsg = "The file cannot be empty.";
            else if (csvFile.Length > 10000)
                ErrorMsg = "The file must be under 10Kb.";
            return string.IsNullOrEmpty(ErrorMsg);
        }

        private void parse(IFormFile csvFile)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
            var csvParser = new CsvParser<string[]>(csvParserOptions, new CsvStringArrayMapping());

            var records = csvParser.ReadFromStream(csvFile.OpenReadStream(), Encoding.UTF8);

            Keywords = records.Select(x => x.Result[0]).ToList();
        }

    }
}
