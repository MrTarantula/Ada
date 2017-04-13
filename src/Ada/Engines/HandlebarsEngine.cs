using Ada.Models;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;
using System.IO;
using HandlebarsDotNet;

namespace Ada.Engines
{
    public class HandlebarsEngine : BaseEngine
    {
        private TemplateSettings _templateSettings;

        public HandlebarsEngine(IOptionsSnapshot<AppSettings> settings) : base(settings)
        {
            _templateSettings = JsonConvert.DeserializeObject<TemplateSettings>(File.ReadAllText(Path.Combine(_settings.TemplatePath, "template.json")));
            Handlebars.RegisterTemplate("nav", File.ReadAllText(Path.Combine(_settings.TemplatePath, _templateSettings.NavPartial)));
        }

        public override string Templatize(Document document, string nav)
        {
            string temp = File.ReadAllText(Path.Combine(_settings.TemplatePath, _templateSettings.Template));
            var template = Handlebars.Compile(temp);

            return template(document);
        }
    }
}
