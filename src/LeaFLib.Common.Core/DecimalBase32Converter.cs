namespace LeaFLib.Common.Core
{
    /// <summary>
    /// Convert a number to string with the 32 Base format
    /// </summary>
    /// <remarks>
    /// Similar to <strong>Demical to Hex</strong> or <strong>Demical to Binary</strong>.
    /// </remarks>
    public sealed class DecimalBase32Converter
    {
        public static DecimalBase32Converter Default { get; } = new("0123456789abcdefghijklmnopqrstuv");

        public string Alphabet { get; }

        public DecimalBase32Converter(string alphabet)
        {
            ArgumentNullException.ThrowIfNull(alphabet);
            ArgumentOutOfRangeException.ThrowIfNotEqual(alphabet.Length, 32);

            Alphabet = alphabet;
        }

        public string ToString(uint num)
        {
            if (num == 0)
            {
                return Alphabet[0].ToString();
            }

            const int length = (sizeof(uint) * 8 + 4) / 5;
            int head = 0;
            var n = num;
            Span<char> chars = stackalloc char[length];
            var a = Alphabet.AsSpan();
            for (int i = 0; i < chars.Length; i++)
            {
                chars[^(i + 1)] = a[(int)(n & 0x1Fu)];
                n >>= 5;
                if (n == 0)
                {
                    head = length - 1 - i;
                    break;
                }
            }
            return new string(chars[head..]);
        }

        public string ToString(ulong num)
        {
            if (num == 0)
            {
                return Alphabet[0].ToString();
            }

            const int length = (sizeof(ulong) * 8 + 4) / 5; ;
            int head = 0;
            var n = num;
            Span<char> chars = stackalloc char[length];
            var a = Alphabet.AsSpan();
            for (int i = 0; i < chars.Length; i++)
            {
                chars[^(i + 1)] = a[(int)(n & 0x1Ful)];
                n >>= 5;
                if (n == 0)
                {
                    head = length - 1 - i;
                    break;
                }
            }
            return new string(chars[head..]);
        }

        public string ToString(DateTimeOffset dateTimeOffset, bool useMillisecond = false)
        {
            long num = useMillisecond
                ? dateTimeOffset.ToUnixTimeMilliseconds()
                : dateTimeOffset.ToUnixTimeSeconds();
            return ToString((ulong)num);
        }

        public uint ToUInt32(ReadOnlySpan<char> span)
        {
            const int header = 7;
            ValidateCharSpan(ref span, header, 1);

            uint result = 0; int index;
            for (int i = 1; i <= Math.Min(span.Length, header); i++)
            {
                index = Alphabet.IndexOf(span[^i]);
                if (index < 0)
                {
                    throw new ArgumentException($"Char[{span.Length - i}] not found in the alphabet", nameof(span));
                }
                result |= ((uint)index) << ((i - 1) * 5);
            }
            return result;
        }

        public uint ToUInt32(string str) => ToUInt32(str.AsSpan());

        public bool TryToInt32(ReadOnlySpan<char> span, out uint value)
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
                value |= ((uint)index) << ((i - 1) * 5);
            }
            return true;
        }

        public bool TryToUInt32(string str, out uint value) => TryToInt32(str.AsSpan(), out value);

        public ulong ToUInt64(ReadOnlySpan<char> span)
        {
            const int header = 13;
            ValidateCharSpan(ref span, header, 7);

            ulong result = 0; int index;
            for (int i = 1; i <= Math.Min(span.Length, header); i++)
            {
                index = Alphabet.IndexOf(span[^i]);
                if (index < 0)
                {
                    throw new ArgumentException($"Char[{span.Length - i}] not found in the alphabet", nameof(span));
                }
                result |= ((ulong)index) << ((i - 1) * 5);
            }
            return result;
        }

        public ulong ToUInt64(string str) => ToUInt64(str.AsSpan());

        public bool TryToUInt64(ReadOnlySpan<char> span, out ulong value)
        {
            const int header = 13;
            if (!TryValidateCharSpan(ref span, header, 7))
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
                value |= ((ulong)index) << ((i - 1) * 5);
            }
            return true;
        }

        public bool TryToUInt64(string str, out ulong value) => TryToUInt64(str.AsSpan(), out value);

        private void ValidateCharSpan(ref ReadOnlySpan<char> span, in int header, in int headerMaxValue)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(span.Length, 1);

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