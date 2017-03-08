using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ada.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.IO;
using HandlebarsDotNet;
using Ada.Helpers;

namespace Ada.Engines
{
    /// <summary>
    /// THIS IS VERY BROKEN
    /// </summary>
    public class HandlebarsEngine : IEngine
    {
        private readonly AppSettings _settings;
        private TemplateSettings _templateSettings;

        public HandlebarsEngine(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
            _templateSettings = JsonConvert.DeserializeObject<TemplateSettings>(File.ReadAllText(Path.Combine(_settings.TemplatePath, "template.json")));
        }

        /// <summary>
        /// Builds each file from template and places into the correct folder
        /// </summary>
        /// <param name="documents">List of documents to be processed</param>
        public void Templatize(List<Document> documents)
        {
            var navList = ProcessNav(documents);


            Handlebars.RegisterTemplate("nav", File.ReadAllText(Path.Combine(_settings.TemplatePath, _templateSettings.NavPartial)));

            string temp = File.ReadAllText(Path.Combine(_settings.TemplatePath, _templateSettings.Template));
            var template = Handlebars.Compile(temp);

            foreach (var doc in documents)
            {
                if (doc.Category == doc.Title || doc.Subcategory == doc.Title)
                {
                    if (doc.Category == doc.Title)
                    {
                        using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), "index.html")))
                        {
                            //need children table
                            writer.Write(template(doc));
                        }
                    }
                    //how do I account for different parent categories?
                    else if (doc.Subcategory == doc.Title)
                    {
                        using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), doc.Subcategory.Prettify(), "index.html")))
                        {
                            //need children table
                            writer.Write(template(doc));
                        }
                    }
                }
                else
                {
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.Category.Prettify(), doc.Subcategory.Prettify(), doc.Title.Prettify(), "index.html")))
                    {
                        writer.Write(template(doc));
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
