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
        private IConfiguration _config;
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

                Assert.True(uploadForm.HasError);
            }

            [Fact]
            public void ValidateFile2_FileToBig_GetError()
            {
                var stubFormFile = new StubFormFile();

                // override file length by 1 + maximum allowed
                stubFormFile.Length = _maxCsvFileSizeUpload + 1;

                var uploadForm = new UploadFormViewModel(stubFormFile, UploadFormViewModel_Tests.CONFIG);

                Assert.True(uploadForm.HasError);
            }

            [Fact]
            public void ValidateFile3_FileEmpty_GetError()
            {
                var stubFormFile = new StubFormFile();

                // override file length by 0
                stubFormFile.Length = 0;

                var uploadForm = new UploadFormViewModel(stubFormFile, UploadFormViewModel_Tests.CONFIG);

                Assert.True(uploadForm.HasError);
            }


            [Fact]
            public void ValidateFile4_FileEmpty_GetError()
            {
                var stubFormFile = new StubFormFile();

                // override file length by 0
                stubFormFile.Length = 0;

                var uploadForm = new UploadFormViewModel(stubFormFile, UploadFormViewModel_Tests.CONFIG);

                Assert.True(uploadForm.HasError);
            }
        }

        public class UploadFormViewModel_ParseMathod
        {
            [Fact]
            public void ParseMethod_EmptyCsv_GetError()
            {
                var formFile = new StubFormFile("");
                var uploadForm = new UploadFormViewModel(formFile, UploadFormViewModel_Tests.CONFIG);

                Assert.True(uploadForm.HasError);
            }

            // [Fact]
            // public void ParseMethod_EmptyCsv_GetError()
            // {
            //     var formFile = new StubFormFile("");
            //     var uploadForm = new UploadFormViewModel(formFile, UploadFormViewModel_Tests.CONFIG);

            //     Assert.True(uploadForm.HasError);
            // }
        }
    }

    public class StubFormFile : IFormFile
    {
        public StubFormFile(string csvContent = "Hello,World,")
        {
            _fakeStream = new MemoryStream(Encoding.UTF8.GetBytes(csvContent));
        }
        private Stream _fakeStream;
        public long Length { get; set; }
        public string ContentType => throw new NotImplementedException();

        public string ContentDisposition => throw new NotImplementedException();

        public IHeaderDictionary Headers => throw new NotImplementedException();


        public string Name => throw new NotImplementedException();

        public string FileName => throw new NotImplementedException();

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
