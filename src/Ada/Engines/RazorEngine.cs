using Ada.Models;
using System.IO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RazorLight;
using System.Collections.Generic;
using System;

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

        //temporary because I don't write tests
        public override void Generate(List<Document> documents)
        {
            try
            {
                using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, "index.html")))
                {
                    writer.Write(_engine.Parse("template.cshtml", documents));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override string Templatize(Document document)
        {
            throw new NotImplementedException();
        }
    }
}
