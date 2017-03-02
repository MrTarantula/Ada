using Ada.Models;
using Ada.Services.Helpers;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Ada.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly AppSettings _settings;

        public TemplateService(IOptions<AppSettings> settings)
        {
            _settings = settings.Value;
        }

        public string GenerateFooter(IList<Document> documents)
        {
            try
            {
                using (var reader = File.OpenText(Path.Combine(_settings.TemplatePath, "footer.html")))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GenerateHeader(IList<Document> documents)
        {
            try
            {
                using (var reader = File.OpenText(Path.Combine(_settings.TemplatePath, "header.html")))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GenerateNav(IList<Document> documents)
        {
            StringBuilder nav = new StringBuilder();
            string[] categories = documents.Select(c => c.Category).Distinct().ToArray();

            nav.Append($"<ul><li><a href=\"/\"</a>{_settings.SiteName}</li>");
            foreach (string category in categories)
            {
                nav.Append($"<li><a href=\"/{category.Prettify()}\">{category}");
                List<Document> pages = documents.Where(d => d.Category == category).OrderBy(d => d.Title).ToList();
                nav.Append("<ul>");
                foreach (var page in pages)
                {
                    nav.Append($"<li><a href=\"{page.RelativePath.Urlify()}\">{page.Title}</a></li>");
                }
                nav.Append("</ul>");
            }
            nav.Append("</ul>");

            return nav.ToString();
        }
    }
}
