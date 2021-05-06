using System;

namespace KeywordsApp.Models.File
{
    public class FileViewModel
    {
        public int FileId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public int KeywordsCount { get; set; }
    }
}