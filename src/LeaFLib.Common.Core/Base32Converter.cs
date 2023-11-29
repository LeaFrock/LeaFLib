using System.Text;

namespace LeaFLib.Common.Core
{
    /// <summary>
    /// Converter for Base32.
    /// <para>See <see href="https://github.com/dotnet/aspnetcore/blob/main/src/Identity/Extensions.Core/src/Base32.cs"/></para>
    /// <para>Also see <seealso href="https://www.rfc-editor.org/rfc/rfc4648"/></para>
    /// </summary>
    public sealed class Base32Converter
    {
        public static Base32Converter Standard { get; } = new("ABCDEFGHIJKLMNOPQRSTUVWXYZ234567");

        public string Alphabet { get; }

        public Base32Converter(string alphabet)
        {
            ArgumentNullException.ThrowIfNull(alphabet);
            if (alphabet.Length != 32)
            {
                throw new ArgumentException("The length of alphabet is not 32");
            }

            Alphabet = alphabet;
        }

        public string ToBase32String(byte[] input)
        {
            ArgumentNullException.ThrowIfNull(input);

            StringBuilder sb = new(input.Length * 8);
            for (int offset = 0; offset < input.Length;)
            {
                int numCharsToOutput = GetNextGroup(input, ref offset, out var a, out var b, out var c, out var d, out var e, out var f, out var g, out var h);

                sb.Append((numCharsToOutput >= 1) ? Alphabet[a] : '=');
                sb.Append((numCharsToOutput >= 2) ? Alphabet[b] : '=');
                sb.Append((numCharsToOutput >= 3) ? Alphabet[c] : '=');
                sb.Append((numCharsToOutput >= 4) ? Alphabet[d] : '=');
                sb.Append((numCharsToOutput >= 5) ? Alphabet[e] : '=');
                sb.Append((numCharsToOutput >= 6) ? Alphabet[f] : '=');
                sb.Append((numCharsToOutput >= 7) ? Alphabet[g] : '=');
                sb.Append((numCharsToOutput >= 8) ? Alphabet[h] : '=');
            }
            return sb.ToString();
        }

        public byte[] FromBase32String(string input)
        {
            ArgumentNullException.ThrowIfNull(input);

            var trimmedInput = input.AsSpan().TrimEnd('=');
            if (trimmedInput.Length == 0)
            {
                return [];
            }

            var output = new byte[trimmedInput.Length * 5 / 8];
            var bitIndex = 0;
            var inputIndex = 0;
            var outputBits = 0;
            var outputIndex = 0;
            while (outputIndex < output.Length)
            {
                var byteIndex = Alphabet.IndexOf(char.ToUpperInvariant(trimmedInput[inputIndex]));
                if (byteIndex < 0)
                {
                    throw new FormatException();
                }

                var bits = Math.Min(5 - bitIndex, 8 - outputBits);
                output[outputIndex] <<= bits;
                output[outputIndex] |= (byte)(byteIndex >> (5 - (bitIndex + bits)));

                bitIndex += bits;
                if (bitIndex >= 5)
                {
                    inputIndex++;
                    bitIndex = 0;
                }

                outputBits += bits;
                if (outputBits >= 8)
                {
                    outputIndex++;
                    outputBits = 0;
                }
            }
            return output;
        }

        // returns the number of bytes that were output
        private static int GetNextGroup(byte[] input, ref int offset, out byte a, out byte b, out byte c, out byte d, out byte e, out byte f, out byte g, out byte h)
        {
            var retVal = (input.Length - offset) switch
            {
                1 => 2,
                2 => 4,
                3 => 5,
                4 => 7,
                _ => 8,
            };

            uint b1 = (offset < input.Length) ? input[offset++] : 0U;
            uint b2 = (offset < input.Length) ? input[offset++] : 0U;
            uint b3 = (offset < input.Length) ? input[offset++] : 0U;
            uint b4 = (offset < input.Length) ? input[offset++] : 0U;
            uint b5 = (offset < input.Length) ? input[offset++] : 0U;

            a = (byte)(b1 >> 3);
            b = (byte)(((b1 & 0x07) << 2) | (b2 >> 6));
            c = (byte)((b2 >> 1) & 0x1f);
            d = (byte)(((b2 & 0x01) << 4) | (b3 >> 4));
            e = (byte)(((b3 & 0x0f) << 1) | (b4 >> 7));
            f = (byte)((b4 >> 2) & 0x1f);
            g = (byte)(((b4 & 0x3) << 3) | (b5 >> 5));
            h = (byte)(b5 & 0x1f);

            return retVal;
        }
    }
}