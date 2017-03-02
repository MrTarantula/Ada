namespace Ada.Services.Helpers
{
    public static class DocExtensions
    {
        public static string Prettify(this string str) => str.Replace(".md", string.Empty).Replace(" ", "-").ToLower();

        public static string Urlify(this string str) => str.Replace("\\", "/");
    }
}
