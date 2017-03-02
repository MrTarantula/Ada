using Ada.Models;
using System.Collections.Generic;

namespace Ada.Services
{
    public interface ITemplateService
    {
        string GenerateHeader(IList<Document> documents);

        string GenerateNav(IList<Document> documents);

        string GenerateFooter(IList<Document> documents);
    }
}
