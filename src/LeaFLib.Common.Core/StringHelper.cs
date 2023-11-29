using System.Buffers;

namespace LeaFLib.Common.Core
{
    public static class StringHelper
    {
        /// <summary>
        /// Random a Base64 String
        /// </summary>
        /// <param name="length">The length of Base64 String</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">The length is less than 1, or not a multiple of 4.</exception>
        public static string RandomBase64String(int length)
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(length, 1);
            ArgumentOutOfRangeException.ThrowIfNotEqual(length % 4, 0);

            int byteCount = length * 6 / 8;
            if (byteCount <= 512)
            {
                Span<byte> shortBytes = stackalloc byte[byteCount];
                Random.Shared.NextBytes(shortBytes);
                return Convert.ToBase64String(shortBytes);
            }
            var rentBytes = ArrayPool<byte>.Shared.Rent(byteCount);
            var longBytes = rentBytes.AsSpan(0, byteCount);
            Random.Shared.NextBytes(longBytes);
            string result = Convert.ToBase64String(longBytes);
            ArrayPool<byte>.Shared.Return(rentBytes);
            return result;
        }
    }
}