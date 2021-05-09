using X.PagedList;

namespace KeywordsApp.Models.File
{
    public class FileListViewModel
    {
        public string Search { get; set; }
        public IPagedList<FileViewModel> Files { get; set; }
        public PagedListOptions PageListOptions { get { return new PagedListOptions(); } }

    }
}