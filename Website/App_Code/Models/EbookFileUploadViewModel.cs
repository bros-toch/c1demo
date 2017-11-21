using System;
using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// Summary description for EbookFileUploadViewModel
    /// </summary>
    public class EbookFileUploadViewModel
    {
        
    }

    public class FileUploadListViewModel
    {
        public IEnumerable<FileUploadListItemViewModel> Items { get; set; }
    }

    public class FileUploadListItemViewModel
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
    }

    public class BookDetailViewModel
    {
        public string Name { get; set; }
    }
}