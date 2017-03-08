using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ada.Models
{
    public class TemplateSettings
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Version { get; set; }
        public string Template { get; set; }
        public string NavPartial { get; set; }
        public string[] Assets { get; set; }
    }
}
