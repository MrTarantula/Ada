using System;

namespace Ada.Models
{
    public class Document
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime Published { get; set; }
        public string RelativePath { get; set; }
        public string Body { get; set; }
    }
}
