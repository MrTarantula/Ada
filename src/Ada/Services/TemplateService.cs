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

            nav.Append($"<div class=\"col-md-2\"><h1><a href=\"/\">{_settings.SiteName}</a></h1>");
            nav.Append("<div class=\"list-group\">");
            foreach (string category in categories)
            {
                nav.Append($"<a class=\"list-group-item\" href=\"/{category.Prettify()}\">{category}</a>");
                List<Document> pages = documents.Where(d => d.Category == category).OrderBy(d => d.Title).ToList();
                foreach (var page in pages)
                {
                    nav.Append($"<a class=\"list-group-item\" href=\"{page.RelativePath.Urlify()}\">&nbsp;&nbsp; - {page.Title}</a>");
                }
            }
            nav.Append("</div></div><div class=\"col-md-10\">");

            return nav.ToString();
        }

        public void CopyTemplateStyles()
        {
            DirectoryInfo templates = new DirectoryInfo(_settings.TemplatePath);
            DirectoryInfo assets = new DirectoryInfo(Path.Combine(_settings.OutputPath, "assets"));

            if (!assets.Exists)
            {
                Directory.CreateDirectory(Path.Combine(_settings.OutputPath, "assets"));
            }

            foreach (var file in templates.GetFiles("*.css"))
            {
                file.CopyTo(Path.Combine(assets.FullName, file.Name));
            }          
        }

        public void CopyTemplateJs()
        {
            DirectoryInfo templates = new DirectoryInfo(_settings.TemplatePath);
            DirectoryInfo assets = new DirectoryInfo(Path.Combine(_settings.OutputPath, "assets"));

            if (!assets.Exists)
            {
                Directory.CreateDirectory(Path.Combine(_settings.OutputPath, "assets"));
            }

            foreach (var file in templates.GetFiles("*.js"))
            {
                file.CopyTo(Path.Combine(assets.FullName, file.Name));
            }
        }
    }
}
