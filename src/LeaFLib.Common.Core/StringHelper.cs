namespace LeaFLib.Common.Core
{
    public static class StringHelper
    {
        /// <summary>
        /// Random a Base64 String
        /// </summary>
        /// <param name="length">The length of Base64 String</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">The length is less than 1.</exception>
        /// <exception cref="ArgumentException">The length is not a multiple of 4.</exception>
        public static string RandomBase64String(int length)
        {
            if (length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "The length is less than 1");
            }
            if (length % 4 != 0)
            {
                throw new ArgumentException("The length is not a multiple of 4", nameof(length));
            }

            int byteCount = length * 6 / 8;
            Span<byte> bytes = byteCount <= 512
                ? stackalloc byte[byteCount]
                : new byte[byteCount];
            Random.Shared.NextBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}