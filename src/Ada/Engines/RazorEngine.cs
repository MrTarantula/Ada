using Ada.Models;
using System.IO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RazorLight;

namespace Ada.Engines
{
    public class RazorEngine : BaseEngine
    {
        private TemplateSettings _templateSettings;
        private IRazorLightEngine _engine;

        public RazorEngine(IOptions<AppSettings> settings) : base(settings)
        {
            _templateSettings = JsonConvert.DeserializeObject<TemplateSettings>(File.ReadAllText(Path.Combine(_settings.TemplatePath, "template.json")));
            _engine = EngineFactory.CreatePhysical(Path.Combine(_settings.TemplatePath));
        }

        public override string Templatize(Document document)
        {
            return _engine.Parse("template.cshtml", document);
        }
    }
}
