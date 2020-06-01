namespace ArkSavegameToolkitNet.Domain.Internal
{
    public static class StringExtensions
    {
        public static string SubstringAfterLast(this string self, char value)
        {
            if (self == null) return null;

            var i = self.LastIndexOf(value);
            if (i >= 0 && i + 1 < self.Length) return self.Substring(i + 1);

            return null;
        }
    }
}
