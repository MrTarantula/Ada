using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ada.Models;
using System.IO;
using Ada.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Ada.Engines
{
    public class RazorEngine : IEngine
    {
        private readonly AppSettings _settings;
        private TemplateSettings _templateSettings;

        public RazorEngine(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
            _templateSettings = JsonConvert.DeserializeObject<TemplateSettings>(File.ReadAllText(Path.Combine(_settings.TemplatePath, "template.json")));
        }

        public void Templatize(List<Document> documents)
        {

            foreach (var doc in documents)
            {
                if (doc.Category == doc.Title)
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), "index.html")))
                    {
                        //need children table
                        writer.Write("Category Index exists");
                    }
                }
                else if (doc.Subcategory == doc.Title)
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), doc.Subcategory.Prettify(), "index.html")))
                    {
                        //need children table
                        writer.Write("Subcategory Index exists");
                    }
                }
                else
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), doc.Subcategory.Prettify(), doc.Title.Prettify(), "index.html")))
                    {
                        writer.Write("This is a page");
                    }
                }
            }
        }
    }
}
