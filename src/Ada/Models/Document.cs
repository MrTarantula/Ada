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
            //WIP
            string cat;
            string subcat;
            string ti;

            if (Category == "Index")
            {
                cat = "/";
                subcat = "";
                ti = "";
            }
            else if (Title == Category && string.IsNullOrEmpty(Category))
            {
                cat = Category;
                subcat = "";
                ti = "";
            }
            else if (Title == Subcategory)
            {
                cat = $"/{Category}";
                subcat = "";
            }
            return string.Concat(
                string.IsNullOrEmpty(Category) ? "" : $"/{Category}",
                string.IsNullOrEmpty(Subcategory) ? "" : $"/{Subcategory}",
                Title);
        }
    }
}
