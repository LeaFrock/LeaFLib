namespace LeaFLib.Common.Core
{
    public static class NumberHelper
    {
        private const string Default32BaseAlphabet = "0123456789abcdefghjkmnpqrstuwxyz";

        public static string FormatInt32To32Base(int num, string? customAlphabet = default)
        {
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(num), "num is less than 0");
            }

            string alphabet = Default32BaseAlphabet;
            if (customAlphabet != null)
            {
                if (customAlphabet.Length != 32)
                {
                    throw new ArgumentException("The length of customAlphabet is not 32", nameof(customAlphabet));
                }
                alphabet = customAlphabet;
            }

            if (num < 32)
            {
                return alphabet[num].ToString();
            }

            Span<char> chars = stackalloc char[7];
            int temp = num & (1 << 30), offset;
            chars[0] = alphabet[temp >> 30];
            for (int i = 1; i < chars.Length; i++)
            {
                offset = 30 - i * 5;
                temp = num & (31 << offset);
                chars[i] = alphabet[temp >> offset];
            }

            if (chars[0] != alphabet[0])
            {
                return new string(chars);
            }
            for (int i = 1; i < chars.Length - 1; i++)
            {
                if (chars[i] != alphabet[0])
                {
                    return new string(chars[i..]);
                }
            }
            return chars[^1].ToString();
        }

        public static string FormatInt64To32Base(long num, string? customAlphabet = default)
        {
            if (num < 0L)
            {
                throw new ArgumentOutOfRangeException(nameof(num), "num is less than 0");
            }

            string alphabet = Default32BaseAlphabet;
            if (customAlphabet != null)
            {
                if (customAlphabet.Length != 32)
                {
                    throw new ArgumentException("The length of customAlphabet is not 32", nameof(customAlphabet));
                }
                alphabet = customAlphabet;
            }

            if (num < 32L)
            {
                return alphabet[(int)num].ToString();
            }

            Span<char> chars = stackalloc char[13];
            long temp = num & (7L << 60);
            chars[0] = alphabet[(int)(temp >> 60)];
            int offset;
            for (int i = 1; i < chars.Length; i++)
            {
                offset = 60 - i * 5;
                temp = num & (31L << offset);
                chars[i] = alphabet[(int)(temp >> offset)];
            }

            if (chars[0] != alphabet[0])
            {
                return new string(chars);
            }
            for (int i = 1; i < chars.Length - 1; i++)
            {
                if (chars[i] != alphabet[0])
                {
                    return new string(chars[i..]);
                }
            }
            return chars[^1].ToString();
        }
    }
}