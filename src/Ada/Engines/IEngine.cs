using Ada.Models;
using System.Collections.Generic;

namespace Ada.Engines
{
    public interface IEngine
    {
        void Generate(List<Document> documents);
        string Templatize(Document document);
    }
}
