namespace ComLineCommon
{
    public static class StringTool
    {
        public static string Capitalize(this string s)
        {
            if (string.IsNullOrEmpty(s) || s[0] >= 'A' && s[0] <= 'Z') return s;
            return string.Concat(((char)(s[0] - 32)).ToString(), s.AsSpan(1));
        }
    }
}
