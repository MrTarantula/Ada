using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using Ada.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Ada.Services.Helpers;

namespace Ada.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly AppSettings _settings;
        private readonly ITemplateService _templateService;
        List<Document> _documents = new List<Document>();

        public DocumentService(IOptions<AppSettings> settings, ITemplateService templateService)
        {
            _settings = settings.Value;
            _templateService = templateService;
        }

        public void ReadDocuments()
        {
            var files = Directory.EnumerateFiles(_settings.InputPath, "*.md", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                string fileString = "";
                string pattern = "(-){3,}(\\r\\n)?";

                using (var reader = File.OpenText(file))
                {

                    while (!Regex.IsMatch(fileString, pattern))
                    {
                        fileString += reader.ReadLine() + Environment.NewLine;
                    }

                    string sanitizedYaml = Regex.Replace(fileString, pattern, "");

                    var deserializerBuilder = new DeserializerBuilder().WithNamingConvention(new PascalCaseNamingConvention());

                    var deserializer = deserializerBuilder.Build();

                    var doc = deserializer.Deserialize<Document>(sanitizedYaml);

                    doc.RelativePath = file.Replace(_settings.InputPath, string.Empty).Prettify();
                    doc.Body = reader.ReadToEnd();

                    _documents.Add(doc);

                }
            }

            Generate();
        }

        public void Templatize()
        {
            string header = _templateService.GenerateHeader(_documents);
            string nav = _templateService.GenerateNav(_documents);
            string footer = _templateService.GenerateFooter(_documents);

            var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();

            foreach (var doc in _documents)
            {
                try
                {
                    string paths = Path.Combine(_settings.OutputPath, doc.RelativePath.Substring(1), "index.html");
                    using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, doc.RelativePath.Substring(1), "index.html")))
                    {
                        writer.Write(header);
                        writer.Write(nav);
                        writer.Write(Markdown.ToHtml(doc.Body));
                        writer.Write(footer);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void Generate()
        {
            if (Directory.Exists(_settings.OutputPath))
            {
                Directory.Delete(_settings.OutputPath, true);
            }

            Directory.CreateDirectory(_settings.OutputPath);

            string header = _templateService.GenerateHeader(_documents);
            string nav = _templateService.GenerateNav(_documents);
            string footer = _templateService.GenerateFooter(_documents);

            using (var writer = File.CreateText(Path.Combine(_settings.OutputPath, "index.html")))
            {
                writer.Write(header);
                writer.Write(nav);
                writer.Write($"<h2>{_settings.OutputPath.Prettify()}</h2>");
                writer.Write(footer);
            }
            MakeDirs(new DirectoryInfo(_settings.InputPath), new DirectoryInfo(_settings.OutputPath));

            Templatize();
        }

        private void MakeDirs(DirectoryInfo input, DirectoryInfo output)
        {
            try
            {
                foreach (var inDir in input.GetDirectories())
                {
                    var outDir = output.CreateSubdirectory(inDir.Name.Prettify());
                    GenerateIndex(outDir);
                    MakeDirs(inDir, outDir);
                }

                foreach (var file in input.GetFiles("*.md"))
                {
                    output.CreateSubdirectory(file.Name.Prettify());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void GenerateIndex(DirectoryInfo dir)
        {
            string header = _templateService.GenerateHeader(_documents);
            string nav = _templateService.GenerateNav(_documents);
            string footer = _templateService.GenerateFooter(_documents);

            using (var writer = File.CreateText(Path.Combine(dir.FullName, "index.html")))
            {
                writer.Write(header);
                writer.Write(nav);
                writer.Write($"<h2>{dir.Name.Prettify()}</h2>");
                writer.Write(footer);
            }
        }
    }
}