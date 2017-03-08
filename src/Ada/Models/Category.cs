using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ada.Models
{
    public class Category
    {
        public string Name { get; set; }
        public List<Category> Subcategories { get; set; }
        public List<string> Pages { get; set; }
    }
}
