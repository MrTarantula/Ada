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

        public BaseEngine(IOptionsSnapshot<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public virtual void Generate(List<Document> documents)
        {
            string nav = TemplatizeNav(documents);

            //Create nonexistent documents if needed
            var nonexistentCats = documents
                .Where(c => documents.All(d => c.Category != d.Title))
                .Select(x => x.Category)
                .Distinct()
                .ToList();

            foreach (string category in nonexistentCats)
            {
                documents.Add(new Document
                {
                    Title = category,
                    Category = category
                });
            }

            var nonexistentSubcats = documents
                .Where(s => documents.All(d => s.Subcategory != d.Title) && s.Subcategory != null)
                .Select(x => new { Category = x.Category, Subcategory = x.Subcategory })
                .Distinct()
                .ToList();

            foreach (var subcategory in nonexistentSubcats)
            {
                documents.Add(new Document
                {
                    Title = subcategory.Subcategory,
                    Category = subcategory.Category,
                    Subcategory = subcategory.Subcategory
                });
            }

            if (documents.All(d => d.Category != null && d.Category != "Index"))
            {
                documents.Add(new Document
                {
                    Title = _settings.SiteName,
                    Category = "Index"
                });
            }

            //process existing documents
            foreach (var doc in documents)
            {
                //site Index page
                if (doc.Category == "Index")
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, "index.html")))
                    {
                        writer.Write(Templatize(doc, nav));
                    }
                }
                //category index page
                if (doc.Category == doc.Title)
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), "index.html")))
                    {
                        writer.Write(Templatize(doc, nav));
                    }
                }
                //subcategory index page, when it exists
                else if (doc.Subcategory == doc.Title)
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), doc.Subcategory.Prettify(), "index.html")))
                    {
                        //need children table
                        writer.Write(Templatize(doc, nav));
                    }
                }
                //document page
                else
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), doc.Subcategory.Prettify(), doc.Title.Prettify(), "index.html")))
                    {
                        writer.Write(Templatize(doc, nav));
                    }
                }
            }
        }

        public virtual string Templatize(Document document, string nav)
        {
            return document.ToString();
        }

        public virtual string TemplatizeNav(List<Document> documents)
        {
            return documents.ToString();
        }
    }
}
