using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KeywordsApp.Models.File;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Xunit;

namespace KeywordsApp.Tests.File
{
    public class UploadFormViewModel_Tests
    {
        static IConfiguration CONFIG = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();
        public class UploadFormViewModel_ValidateMethod
        {
            private int _maxCsvFileSizeUpload;

            public UploadFormViewModel_ValidateMethod()
            {
                _maxCsvFileSizeUpload = int.Parse(UploadFormViewModel_Tests.CONFIG["MaxCsvFileSizeUpload"]);
            }


            [Fact]
            public void ValidateFile1_NullFile_GetError()
            {
                var uploadForm = new UploadFormViewModel(null, UploadFormViewModel_Tests.CONFIG);

                Assert.False(uploadForm.IsValid);
            }

            [Fact]
            public void ValidateFile2_FileToBig_GetError()
            {
                var stubFormFile = new StubFormFile();

                // override file length by 1 + maximum allowed
                stubFormFile.Length = _maxCsvFileSizeUpload + 1;

                var uploadForm = new UploadFormViewModel(stubFormFile, UploadFormViewModel_Tests.CONFIG);

                Assert.False(uploadForm.IsValid);
            }

            [Fact]
            public void ValidateFile3_FileEmpty_GetError()
            {
                var stubFormFile = new StubFormFile();

                // override file length by 0
                stubFormFile.Length = 0;

                var uploadForm = new UploadFormViewModel(stubFormFile, UploadFormViewModel_Tests.CONFIG);

                Assert.False(uploadForm.IsValid);
            }


            [Fact]
            public void ValidateFile4_FileEmpty_GetError()
            {
                var stubFormFile = new StubFormFile();

                // override file length by 0
                stubFormFile.Length = 0;

                var uploadForm = new UploadFormViewModel(stubFormFile, UploadFormViewModel_Tests.CONFIG);

                Assert.False(uploadForm.IsValid);
            }


            [Fact]
            public void ValidateFile5_NormalFileEmpty_GetNoError()
            {
                var stubFormFile = new StubFormFile();

                var uploadForm = new UploadFormViewModel(stubFormFile, UploadFormViewModel_Tests.CONFIG);

                Assert.True(uploadForm.IsValid);
            }
        }

        public class UploadFormViewModel_ParseMathod
        {
            [Fact]
            public void ParseMethod1_EmptyCsv_GetError()
            {
                var formFile = new StubFormFile("");
                var uploadForm = new UploadFormViewModel(formFile, UploadFormViewModel_Tests.CONFIG);

                Assert.False(uploadForm.IsValid);
            }

            [Theory]
            [InlineData("key<>word")]
            [InlineData("key[]word")]
            [InlineData("key{}word")]
            [InlineData("key=word")]
            [InlineData("key;word")]
            [InlineData("key:word")]
            [InlineData(":")]
            public void ParseMethod2_SpecialCharKeyword_GetError(string value)
            {
                var formFile = new StubFormFile(value);
                var uploadForm = new UploadFormViewModel(formFile, UploadFormViewModel_Tests.CONFIG);

                Assert.False(uploadForm.IsValid);
            }


            [Fact]
            public void ParseMethod3_CsvHasMoreThan100Keywords_GetError()
            {
                var keywords = "";
                for (int i = 0; i < 101; i++)
                {
                    keywords += i.ToString() + "\r";
                }
                var formFile = new StubFormFile(keywords);
                var uploadForm = new UploadFormViewModel(formFile, UploadFormViewModel_Tests.CONFIG);

                Assert.False(uploadForm.IsValid);
            }


            [Theory]
            [InlineData(" .csv")]
            [InlineData("[")]
            [InlineData("<")]
            [InlineData("<<.csv")]
            [InlineData(" |.csv")]
            public void ParseMethod4_BadCsvFileNaming_GetError(string fileName)
            {
                var formFile = new StubFormFile();
                // Overwrite a bad file name:
                formFile.FileName = fileName;
                var uploadForm = new UploadFormViewModel(formFile, UploadFormViewModel_Tests.CONFIG);

                Assert.False(uploadForm.IsValid);
            }
            [Fact]
            public void ParseMethod5_CsvFileSpecialCharNaming_GetNoError()
            {

                var formFile = new StubFormFile();
                // Overwrite a bad file name:
                formFile.FileName = "file{name}.csv";
                var uploadForm = new UploadFormViewModel(formFile, UploadFormViewModel_Tests.CONFIG);

                Assert.True(uploadForm.IsValid);
            }


            [Fact]
            public void ParseMethod6_CsvFileSpecialCharNaming_GetNoSpecialChar()
            {

                var formFile = new StubFormFile();
                // Overwrite a bad file name:
                formFile.FileName = "file{name}.csv";
                var uploadForm = new UploadFormViewModel(formFile, UploadFormViewModel_Tests.CONFIG);

                Assert.False(uploadForm.FileName.Contains('{'));
            }

            [Fact]
            public void ParseMethod7_NormalCsv_GetNoError()
            {
                var formFile = new StubFormFile("helloWorld");
                var uploadForm = new UploadFormViewModel(formFile, UploadFormViewModel_Tests.CONFIG);

                Assert.True(uploadForm.IsValid);
            }
        }
    }

    public class StubFormFile : IFormFile
    {
        public StubFormFile(string csvContent = "Hello,World,")
        {
            _fakeStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
            FileName = "normal.csv";
            Length = 1;
        }
        private Stream _fakeStream;
        public long Length { get; set; }
        public string ContentType => throw new NotImplementedException();

        public string ContentDisposition => throw new NotImplementedException();

        public IHeaderDictionary Headers => throw new NotImplementedException();


        public string Name => throw new NotImplementedException();

        public string FileName { get; set; }

        public void CopyTo(Stream target)
        {
            throw new NotImplementedException();
        }

        public Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Stream OpenReadStream()
        {
            return _fakeStream;
        }
    }
}
