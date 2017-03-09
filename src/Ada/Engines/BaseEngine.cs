using System.Collections.Generic;
using System.Linq;
using Ada.Models;
using Microsoft.Extensions.Options;
using System.IO;
using Ada.Helpers;

namespace Ada.Engines
{
    public class BaseEngine : IEngine
    {
        public readonly AppSettings _settings;

        public BaseEngine(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public void Generate(List<Document> documents)
        {
            //process nonexisting documents
            var nonexistentCats = documents
                .Where(c => documents.All(d => c.Category != d.Title))
                .Select(x => x.Category)
                .Distinct()
                .ToList();

            foreach (string category in nonexistentCats)
            {
                using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, category.Prettify(), "index.html")))
                {
                    writer.Write(Templatize(new Document { Title = category }));
                }
            }

            var nonexistentSubcats = documents
                .Where(s => documents.All(d => s.Subcategory != d.Title) && s.Subcategory != null)
                .Select(x => new { Category = x.Category, Subcategory = x.Subcategory })
                .Distinct()
                .ToList();

            foreach (var subcategory in nonexistentSubcats)
            {
                using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, subcategory.Category.Prettify(), subcategory.Subcategory.Prettify(), "index.html")))
                {
                    writer.Write(Templatize(new Document { Title = subcategory.Subcategory }));
                }
            }

            if (documents.All(d => d.Category != null))
            {
                using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, "index.html")))
                {
                    writer.Write(Templatize( new Document { Title = _settings.SiteName }));
                }
            }

            //process existing documents
            foreach (var doc in documents)
            {
                //site Index page
                if (doc.Category == null || doc.Category == "Index")
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, "index.html")))
                    {
                        writer.Write(Templatize(doc));
                    }
                }
                //category index page
                if (doc.Category == doc.Title)
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), "index.html")))
                    {
                        writer.Write(Templatize(doc));
                    }
                }
                //subcategory index page, when it exists
                else if (doc.Subcategory == doc.Title)
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), doc.Subcategory.Prettify(), "index.html")))
                    {
                        //need children table
                        writer.Write(Templatize(doc));
                    }
                }
                //document page
                else
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), doc.Subcategory.Prettify(), doc.Title.Prettify(), "index.html")))
                    {
                        writer.Write(Templatize(doc));
                    }
                }
            }
        }

        public virtual string Templatize(Document document)
        {
            return document.ToString();
        }

        public List<Category> ProcessNav(List<Document> documents)
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
