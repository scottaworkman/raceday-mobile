using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

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
            string[] lightColors = { "#FF9800", "#f57c00", "#03A9F4", "#0290d1", "#f57c00", "#FF9800", "#0290d1", "#03A9F4" };
            string[] darkColors =  { "#383838", "#525252", "#6c6c6c", "#818181", "#525252", "#383838", "#818181", "#6c6c6c" };
            //string[] darkColors =  { "#b36b00", "#994d00", "#0279b1", "#026897", "#994d00", "#b36b00", "#026897", "#0279b1" };

            if (Application.Current.UserAppTheme == OSAppTheme.Light)
               return lightColors[Math.Abs(value.GetHashCode()) % lightColors.Length];
            
            return darkColors[Math.Abs(value.GetHashCode()) % darkColors.Length];
        }
    }
}
