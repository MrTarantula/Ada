namespace Ada.Services
{
    public interface IDocumentService
    {
        bool ProcessDocuments();
        void ExtractFrontMatter();
        void Folderize();
    }
}
