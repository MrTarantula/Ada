using Ada.Helpers;
using System;

namespace Ada.Models
{
    public class Document
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public string Subcategory { get; set; }
        public string Description { get; set; }
        public string Path
        {
            get { return GetPath(); }
        }
        public History[] History { get; set; }
        public string Body { get; set; }

        private string GetPath()
        {
            
            if (Category == "Index")
            {
                return "/";
            }
            else if (Title == Category && string.IsNullOrEmpty(Subcategory))
            {
                return $"/{Title.Prettify()}";
            }
            else if (Title == Subcategory)
            {
                return $"/{Category.Prettify()}/{Subcategory.Prettify()}";
            }
            else
            {
                return string.Concat(
                    string.IsNullOrEmpty(Category) ? "" : $"/{Category.Prettify()}",
                    string.IsNullOrEmpty(Subcategory) ? "" : $"/{Subcategory.Prettify()}",
                    $"/{Title.Prettify()}");
            }
        }
    }
}
