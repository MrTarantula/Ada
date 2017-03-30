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
            //there's probably a better place for this
            string cat = "", subcat = "", ti = "";

            if (Category == "Index")
            {
                //fall through
            }
            else if (Title == Category && string.IsNullOrEmpty(Subcategory))
            {
                ti = Title;
            }
            else if (Title == Subcategory)
            {
                cat = Category;
                ti = Subcategory;
            }
            else
            {
                cat = Category;
                subcat = Subcategory;
                ti = Title;
            }

            return string.Concat(
                string.IsNullOrEmpty(cat) ? "" : $"/{cat}",
                string.IsNullOrEmpty(subcat) ? "" : $"/{subcat}",
                $"/{ti}").Prettify();
        }
    }
}
