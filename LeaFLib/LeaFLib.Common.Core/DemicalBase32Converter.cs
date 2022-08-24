namespace LeaFLib.Common.Core
{
    /// <summary>
    /// Convert a number to string with the 32 Base format
    /// </summary>
    /// <remarks>
    /// Similar to <strong>Demical to Hex</strong> or <strong>Demical to Binary</strong>.
    /// </remarks>
    public sealed class DemicalBase32Converter
    {
        public static DemicalBase32Converter Default { get; } = new("0123456789abcdefghijklmnopqrstuv");

        public string Alphabet { get; }

        public DemicalBase32Converter(string alphabet)
        {
            ArgumentNullException.ThrowIfNull(alphabet);
            if (alphabet.Length != 32)
            {
                throw new ArgumentException("The length of alphabet is not 32");
            }

            Alphabet = alphabet;
        }

        public string ToString(int num)
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

        public string ToString(long num)
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

        public string ToString(DateTimeOffset dateTimeOffset, bool useMillisecond = false)
        {
            long num = useMillisecond
                ? dateTimeOffset.ToUnixTimeMilliseconds()
                : dateTimeOffset.ToUnixTimeSeconds();
            return ToString(num);
        }

        public int ToInt32(ReadOnlySpan<char> span)
        {
            const int header = 7;
            ValidateCharSpan(ref span, header, 1);

            int result = 0, index;
            for (int i = 1; i <= Math.Min(span.Length, header); i++)
            {
                index = Alphabet.IndexOf(span[^i]);
                if (index < 0)
                {
                    throw new ArgumentException($"Char[{span.Length - i}] not found in the alphabet", nameof(span));
                }
                result |= index << ((i - 1) * 5);
            }
            return result;
        }

        public int ToInt32(string str) => ToInt32(str.AsSpan());

        public bool TryToInt32(ReadOnlySpan<char> span, out int value)
        {
            const int header = 7;
            if (!TryValidateCharSpan(ref span, header, 1))
            {
                value = default;
                return false;
            }

            value = default;
            int index;
            for (int i = 1; i <= Math.Min(span.Length, header); i++)
            {
                index = Alphabet.IndexOf(span[^i]);
                if (index < 0)
                {
                    value = default;
                    return false;
                }
                value |= index << ((i - 1) * 5);
            }
            return true;
        }

        public bool TryToInt32(string str, out int value) => TryToInt32(str.AsSpan(), out value);

        public long ToInt64(ReadOnlySpan<char> span)
        {
            const int header = 13;
            ValidateCharSpan(ref span, header, 7);

            long result = 0, index;
            for (int i = 1; i <= Math.Min(span.Length, header); i++)
            {
                index = Alphabet.IndexOf(span[^i]);
                if (index < 0)
                {
                    throw new ArgumentException($"Char[{span.Length - i}] not found in the alphabet", nameof(span));
                }
                result |= index << ((i - 1) * 5);
            }
            return result;
        }

        public long ToInt64(string str) => ToInt64(str.AsSpan());

        public bool TryToInt64(ReadOnlySpan<char> span, out long value)
        {
            const int header = 13;
            if (!TryValidateCharSpan(ref span, header, 7))
            {
                value = default;
                return false;
            }

            value = default;
            long index;
            for (int i = 1; i <= Math.Min(span.Length, header); i++)
            {
                index = Alphabet.IndexOf(span[^i]);
                if (index < 0)
                {
                    value = default;
                    return false;
                }
                value |= index << ((i - 1) * 5);
            }
            return true;
        }

        public bool TryToInt64(string str, out long value) => TryToInt64(str.AsSpan(), out value);

        private void ValidateCharSpan(ref ReadOnlySpan<char> span, in int header, in int headerMaxValue)
        {
            if (span.Length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(span), "The length is less than 1");
            }
            if (span.Length >= header)
            {
                int index = Alphabet.IndexOf(span[^header]);
                if (index < 0)
                {
                    throw new ArgumentException($"Char[{span.Length - header}] not found in the alphabet", nameof(span));
                }
                if (index > headerMaxValue)
                {
                    throw new ArgumentException($"Char[{span.Length - header}] is invalid", nameof(span));
                }
                for (int i = 0; i < span.Length - header; i++)
                {
                    if (span[i] != Alphabet[0])
                    {
                        throw new ArgumentException($"Char[{i}] is not {Alphabet[0]}", nameof(span));
                    }
                }
            }
        }

        private bool TryValidateCharSpan(ref ReadOnlySpan<char> span, in int header, in int headerMaxValue)
        {
            if (span.Length < 1)
            {
                return false;
            }
            if (span.Length >= header)
            {
                int index = Alphabet.IndexOf(span[^header]);
                if (index < 0)
                {
                    return false;
                }
                if (index > headerMaxValue)
                {
                    return false;
                }
                for (int i = 0; i < span.Length - header; i++)
                {
                    if (span[i] != Alphabet[0])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}