// <copyright file="StringExtention.cs" company="PlaceholderCompany">
//     Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HexaFramework.Extensions
{
    using System.Globalization;

    public static class StringExtention
    {
        public static int ToInt(this string str) => int.Parse(str, NumberStyles.Any, CultureInfo.CurrentCulture);

        public static float ToFloat(this string str) => float.Parse(str, NumberStyles.Any, CultureInfo.CurrentCulture);

        public static string ToString(this string[] strings, string separator) => string.Join(separator, strings);
    }
}