using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ada.Models;
using System.IO;
using Ada.Helpers;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RazorLight;

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
            var engine = EngineFactory.CreatePhysical(Path.Combine(_settings.TemplatePath));

            string test = engine.Parse("_nav.cshtml", ProcessNav(documents));
            Console.WriteLine(test);
            foreach (var doc in documents)
            {
                if (doc.Category == doc.Title)
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), "index.html")))
                    {
                        //need children table
                        writer.Write(engine.Parse("template.cshtml", doc));
                    }
                }
                else if (doc.Subcategory == doc.Title)
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), doc.Subcategory.Prettify(), "index.html")))
                    {
                        //need children table
                        writer.Write(engine.Parse("template.cshtml", doc));
                    }
                }
                else
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), doc.Subcategory.Prettify(), doc.Title.Prettify(), "index.html")))
                    {
                        writer.Write(engine.Parse("template.cshtml", doc));
                    }
                }
            }
        }

        private List<Category> ProcessNav(List<Document> documents)
        {
            List<Category> nav = new List<Category>();

            foreach (var cat in documents.Select(c => c.Category).Distinct())
            {
                nav.Add(new Category
                {
                    Name = cat,
                    Subcategories = documents
                            .Where(c => c.Category == cat && c.Subcategory != null)
                            .Select(s => new Category
                            {
                                Name = s.Subcategory,
                                Pages = documents.Where(p => p.Subcategory == s.Subcategory).Select(p => p.Title).ToList()
                            }).ToList(),
                    Pages = documents.Where(p => p.Category == cat && p.Subcategory == null).Select(p => p.Title).ToList()
                }
                );
            }

            return nav;
        }
    }
}
