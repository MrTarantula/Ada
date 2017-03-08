namespace Ada.Helpers
{
    public static class DocumentHelpers
    {
        public static string Prettify(this string str) => string.IsNullOrEmpty(str) ? "" : str.Replace(" ", "-").ToLower();

        public static string Urlify(this string str) => str.Replace("\\", "/");
    }
}
