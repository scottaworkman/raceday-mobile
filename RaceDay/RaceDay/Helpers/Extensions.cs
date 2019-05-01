using System;
using System.Collections.Generic;
using System.Text;

namespace RaceDay.Helpers
{
    public static class Extensions
    {
        public static string Left(this string value, int length)
        {
            if (String.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Length < length)
                return string.Empty;

            return value.Substring(0, length);
        }

        public static string ToColor(this string value)
        {
            string[] colors = { "#FF9800", "#f57c00", "#03A9F4", "#0290d1", "#f57c00", "#FF9800", "#0290d1", "#03A9F4" };

            return colors[Math.Abs(value.GetHashCode()) % colors.Length];
        }
    }
}
