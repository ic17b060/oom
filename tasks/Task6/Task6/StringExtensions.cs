using System;
namespace Task6
{
        public static class StringExtensions
        {
            public static string Truncate(this string s, int maxLength) => (s == null || s.Length <= maxLength) ? s : s.Substring(0, maxLength);
        }
}

