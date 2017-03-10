using Markdig;
using System;
using System.IO;
using Ada.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Ada.Engines;

namespace Ada.Services
{
    public class MarkdownService : BaseService
    {
        MarkdownPipeline _pipeline = new MarkdownPipelineBuilder().UseGridTables().UsePipeTables().UseBootstrap().UseDiagrams().UseMathematics().UseFigures().Build();

        public MarkdownService(IOptions<AppSettings> settings, IEngine engine) : base(settings, engine)
        {
        }

        public override bool ProcessDocuments()
        {
            try
            {
                ExtractFrontMatter();
                Folderize();

                _engine.Generate(_documents);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Extracts front matter from each document and converts body to html
        /// </summary>
        public override void ExtractFrontMatter()
        {
            var files = Directory.EnumerateFiles(_settings.InputPath, "*.md", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                string fileString = "";
                string pattern = "(-){3,}(\\r\\n)?";

                using (var reader = File.OpenText(file))
                {
                    //read until line is at least 3 dashes
                    while (!Regex.IsMatch(fileString, pattern))
                    {
                        fileString += reader.ReadLine() + Environment.NewLine;
                    }

                    string sanitizedYaml = Regex.Replace(fileString, pattern, "");

                    //parse everything so far as YAML
                    var doc = new DeserializerBuilder()
                        .WithNamingConvention(new PascalCaseNamingConvention())
                        .Build()
                        .Deserialize<Document>(sanitizedYaml);

                    //read the rest of the md file
                    doc.Body = reader.ReadToEnd();

                    //convert to HTML 
                    doc.Body = Markdown.ToHtml(doc.Body, _pipeline);

                    _documents.Add(doc);
                }
            }
        }
    }
}