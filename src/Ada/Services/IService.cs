namespace Ada.Services
{
    public interface IService
    {
        bool ProcessDocuments();
        void ExtractFrontMatter();
        void Folderize();
    }
}
