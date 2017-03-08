using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using Ada.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Ada.Helpers;
using System.Linq;
using Ada.Engines;

namespace Ada.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly AppSettings _settings;
        private readonly IEngine _engine;
        List<Document> _documents = new List<Document>();

        MarkdownPipeline _pipeline = new MarkdownPipelineBuilder().UseGridTables().UsePipeTables().UseBootstrap().UseDiagrams().UseMathematics().UseFigures().Build();

        public DocumentService(IOptions<AppSettings> settings, IEngine engine)
        {
            _settings = settings.Value;
            _engine = engine;
        }

        /// <summary>
        /// Entry point for document service
        /// </summary>
        /// <returns></returns>
        public bool ProcessDocuments()
        {
            try
            {
                ExtractFrontMatter();
                Folderize();

                _engine.Templatize(_documents);
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
        public void ExtractFrontMatter()
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

        /// <summary>
        /// Creates category folders, subcategory folders, and file folders (for nice urls)
        /// </summary>
        public void Folderize()
        {
            //delete and create ouptut directory
            if (Directory.Exists(_settings.OutputPath))
            {
                Directory.Delete(_settings.OutputPath, true);
            }

            Directory.CreateDirectory(_settings.OutputPath);

            List<string> categories = _documents.Select(c => c.Category).Distinct().ToList();
            foreach (string category in categories)
            {
                List<string> subcategories = _documents
                    .Where(c => c.Category == category)
                    .Select(s => s.Subcategory).Distinct()
                    .ToList();

                List<string> files = _documents
                    .Where(f => f.Category == category)
                    .Where(f => f.Title != category)
                    .Where(f => f.Subcategory == null)
                    .Select(s => s.Title)
                    .ToList();

                //generate folder for each category
                Directory.CreateDirectory(Path.Combine(_settings.OutputPath, category.Prettify()));

                foreach (string subcategory in subcategories)
                {
                    //generate folder for each subcategory in category
                    Directory.CreateDirectory(Path.Combine(_settings.OutputPath, category.Prettify(), subcategory.Prettify()));

                    List<string> subFiles = _documents
                        .Where(f => f.Category == category)
                        .Where(f => f.Title != category)
                        .Where(f => f.Title != subcategory)
                        .Where(f => f.Subcategory == subcategory)
                        .Select(s => s.Title)
                        .ToList();

                    foreach (string subFile in subFiles)
                    {
                        //generate folder for each file in subcategory
                        Directory.CreateDirectory(Path.Combine(_settings.OutputPath, category.Prettify(), subcategory.Prettify(), subFile.Prettify()));
                    }
                }

                //generate folder for each file in category
                foreach (string file in files)
                {
                    Directory.CreateDirectory(Path.Combine(_settings.OutputPath, category.Prettify(), file.Prettify()));
                }
            }
        }
    }
}