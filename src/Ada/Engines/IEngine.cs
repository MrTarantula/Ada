using Ada.Models;
using System.Collections.Generic;

namespace Ada.Engines
{
    public interface IEngine
    {
        void Templatize(List<Document> documents);
    }
}
