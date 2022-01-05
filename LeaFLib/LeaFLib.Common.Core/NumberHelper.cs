using System.Collections.Specialized;
using System.Text;

namespace LeaFLib.Common.Core
{
    public static class NumberHelper
    {
        private const string Bin32 = "0123456789abcdefghjkmnpqrstuwxyz";

        public static string ConvertToBin32(int num, string? template = default)
        {
            if (string.IsNullOrEmpty(template))
            {
                template = Bin32;
            }
            else if (template.Length < 32)
            {
                throw new ArgumentException("Length of template should be 32 at least");
            }

            BitVector32 bits = new(num);
            BitVector32.Section[] sections = new BitVector32.Section[7];
            sections[6] = BitVector32.CreateSection(31);
            sections[5] = BitVector32.CreateSection(31, sections[6]);
            sections[4] = BitVector32.CreateSection(31, sections[5]);
            sections[3] = BitVector32.CreateSection(31, sections[4]);
            sections[2] = BitVector32.CreateSection(31, sections[3]);
            sections[1] = BitVector32.CreateSection(31, sections[2]);
            sections[0] = BitVector32.CreateSection(1, sections[1]);
            StringBuilder sb = new();
            bool ignoreZero = true;
            foreach (var sec in sections)
            {
                int temp = bits[sec];
                if (temp == 0 && ignoreZero)
                {
                    continue;
                }
                sb.Append(template[temp]);
                ignoreZero = false;
            }
            return sb.ToString();
        }
    }
}