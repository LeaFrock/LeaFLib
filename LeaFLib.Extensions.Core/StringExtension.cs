namespace LeaFLib.Extensions.Core
{
    /// <summary>
    /// Extensions for <see cref="string"/>
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// Get the count of char value
        /// </summary>
        /// <param name="str">Source String</param>
        /// <param name="value">Search Char</param>
        /// <returns></returns>
        public static int CountOf(this string str, char value)
        {
            ArgumentNullException.ThrowIfNull(str);

            if (str.Length < 1)
            {
                return default;
            }

            int count = 0, index = str.IndexOf(value);
            while (index >= 0)
            {
                count++;
                index = str.IndexOf(value, index + 1);
            }
            return count;
        }

        /// <summary>
        /// Get the count of char value
        /// </summary>
        /// <param name="str">Source String</param>
        /// <param name="value">Search Char</param>
        /// <returns></returns>
        public static int CountOf(this ReadOnlySpan<char> str, char value)
        {
            if (str.Length < 1)
            {
                return default;
            }

            var temp = str;
            int count = 0, index = temp.IndexOf(value);
            while (index >= 0)
            {
                count++;
                if (index >= temp.Length - 1)
                {
                    break;
                }
                temp = temp[(index + 1)..];
                index = temp.IndexOf(value);
            }
            return count;
        }

        /// <summary>
        /// Get the count of substring
        /// </summary>
        /// <param name="str">Source String</param>
        /// <param name="subStr">Search String</param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static int CountOf(this string str, string subStr, StringComparison comparisonType = StringComparison.Ordinal)
        {
            ArgumentNullException.ThrowIfNull(str);
            ArgumentNullException.ThrowIfNull(subStr);

            if (str.Length < 1)
            {
                return default;
            }
            if (subStr.Length < 1)
            {
                return default;
            }

            int count = 0, index = str.IndexOf(subStr, comparisonType);
            while (index >= 0)
            {
                count++;
                index = str.IndexOf(subStr, index + subStr.Length, comparisonType);
            }
            return count;
        }

        /// <summary>
        /// Get the count of substring
        /// </summary>
        /// <param name="str">Source String</param>
        /// <param name="subStr">Search String</param>
        /// <param name="comparisonType"></param>
        /// <returns></returns>
        public static int CountOf(this ReadOnlySpan<char> str, string subStr, StringComparison comparisonType = StringComparison.Ordinal)
        {
            ArgumentNullException.ThrowIfNull(subStr);

            if (str.Length < 1)
            {
                return default;
            }
            if (subStr.Length < 1)
            {
                return default;
            }

            var temp = str;
            int count = 0, index = temp.IndexOf(subStr, comparisonType);
            while (index >= 0)
            {
                count++;
                if (index + subStr.Length >= str.Length - 1)
                {
                    break;
                }
                temp = temp[(index + subStr.Length)..];
                index = temp.IndexOf(subStr, comparisonType);
            }
            return count;
        }
    }
}