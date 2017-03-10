using Ada.Engines;
using Ada.Helpers;
using Ada.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ada.Services
{
    public class BaseService : IService
    {
        public readonly AppSettings _settings;
        public readonly IEngine _engine;
        public List<Document> _documents = new List<Document>();

        public BaseService(IOptions<AppSettings> settings, IEngine engine)
        {
            _settings = settings.Value;
            _engine = engine;
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

        public virtual bool ProcessDocuments()
        {
            throw new NotImplementedException();
        }

        public virtual void ExtractFrontMatter()
        {
            throw new NotImplementedException();
        }
    }
}
