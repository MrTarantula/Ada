using Ada.Models;
using System.IO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RazorLight;
using System.Collections.Generic;

namespace Ada.Engines
{
    public class RazorEngine : BaseEngine
    {
        private TemplateSettings _templateSettings;
        private IRazorLightEngine _engine;

        public RazorEngine(IOptionsSnapshot<AppSettings> settings) : base(settings)
        {
            _templateSettings = JsonConvert.DeserializeObject<TemplateSettings>(File.ReadAllText(Path.Combine(_settings.TemplatePath, "template.json")));
            _engine = EngineFactory.CreatePhysical(Path.Combine(_settings.TemplatePath));
        }

        public override string Templatize(Document document, string nav)
        {
            return _engine.Parse("template.cshtml", nav);
        }

        public override string TemplatizeNav(List<Document> documents)
        {
            return _engine.Parse("nav.cshtml", documents);
        }
    }
}
