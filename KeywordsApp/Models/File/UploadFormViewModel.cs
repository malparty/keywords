using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TinyCsvParser;
using TinyCsvParser.Mapping;

namespace KeywordsApp.Models.File
{
    public class UploadFormViewModel
    {
        private int _maxCsvFileSizeUpload;
        public List<string> Keywords { get; set; }
        public string ErrorMsg { get; set; }
        public bool HasError { get { return !string.IsNullOrEmpty(ErrorMsg); } }
        public UploadFormViewModel(IFormFile csvFile, IConfiguration config)
        {
            initConfig(config);
            if (validate(csvFile))
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
            return string.IsNullOrEmpty(ErrorMsg);
        }

        private void parse(IFormFile csvFile)
        {
            CsvParserOptions csvParserOptions = new CsvParserOptions(false, ',');
            var csvParser = new CsvParser<string[]>(csvParserOptions, new CsvStringArrayMapping());

            var records = csvParser.ReadFromStream(csvFile.OpenReadStream(), Encoding.UTF8);

            Keywords = records.Select(x => x.Result[0]).ToList();

            if (Keywords.Count <= 0)
                ErrorMsg = "No keyword found in the provided csv file.";
        }

    }
}
