namespace LeaFLib.Common.Core
{
    /// <summary>
    /// Convert a number to string with the 32 Base format
    /// </summary>
    /// <remarks>
    /// Similar to <strong>Demical to Hex</strong> or <strong>Demical to Binary</strong>.
    /// </remarks>
    public sealed class DemicalToBase32Converter
    {
        public static DemicalToBase32Converter Default { get; } = new("0123456789abcdefghijklmnopqrstuv");

        public string Alphabet { get; }

        public DemicalToBase32Converter(string alphabet)
        {
            ArgumentNullException.ThrowIfNull(alphabet);
            if (alphabet.Length != 32)
            {
                throw new ArgumentException("The length of alphabet is not 32");
            }

            Alphabet = alphabet;
        }

        public string ToBase32String(int num)
        {
            if (num < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(num), "num is less than 0");
            }

            if (num < 32)
            {
                return Alphabet[num].ToString();
            }

            Span<char> chars = stackalloc char[7];
            int temp = num & (1 << 30), offset;
            chars[0] = Alphabet[temp >> 30];
            for (int i = 1; i < chars.Length; i++)
            {
                offset = 30 - i * 5;
                temp = num & (31 << offset);
                chars[i] = Alphabet[temp >> offset];
            }

            if (chars[0] != Alphabet[0])
            {
                return new string(chars);
            }
            for (int i = 1; i < chars.Length - 1; i++)
            {
                if (chars[i] != Alphabet[0])
                {
                    return new string(chars[i..]);
                }
            }
            return chars[^1].ToString();
        }

        public string ToBase32String(long num)
        {
            if (num < 0L)
            {
                throw new ArgumentOutOfRangeException(nameof(num), "num is less than 0");
            }

            if (num < 32L)
            {
                return Alphabet[(int)num].ToString();
            }

            Span<char> chars = stackalloc char[13];
            long temp = num & (7L << 60);
            chars[0] = Alphabet[(int)(temp >> 60)];
            int offset;
            for (int i = 1; i < chars.Length; i++)
            {
                offset = 60 - i * 5;
                temp = num & (31L << offset);
                chars[i] = Alphabet[(int)(temp >> offset)];
            }

            if (chars[0] != Alphabet[0])
            {
                return new string(chars);
            }
            for (int i = 1; i < chars.Length - 1; i++)
            {
                if (chars[i] != Alphabet[0])
                {
                    return new string(chars[i..]);
                }
            }
            return chars[^1].ToString();
        }

        public int ToInt32(ReadOnlySpan<char> chars)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(string str) => ToInt32(str.AsSpan());

        public long ToInt64(ReadOnlySpan<char> chars)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(string str) => ToInt64(str.AsSpan());
    }
}