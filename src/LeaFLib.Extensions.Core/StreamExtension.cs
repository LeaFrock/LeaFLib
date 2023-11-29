using System.Text;

namespace LeaFLib.Extensions.Core
{
    /// <summary>
    /// Extensions for <see cref="Stream"/>
    /// </summary>
    public static class StreamExtension
    {
        /// <summary>
        /// Get current encoding of the stream by reading in sync
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Encoding? GetEncodingSync(this Stream stream)
        {
            try
            {
                var pos = stream.Position;
                using var reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true, bufferSize: 128, leaveOpen: true);
                if (reader.Peek() > -1)
                {
                    _ = reader.Read();
                    stream.Position = pos;
                    return reader.CurrentEncoding;
                }
            }
            catch (Exception)
            {
                // Just ignore it
            }
            return default;
        }

        /// <summary>
        /// Get current encoding of the stream by reading in async
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task<Encoding?> GetEncodingAsync(this Stream stream)
        {
            try
            {
                var pos = stream.Position;
                using var reader = new StreamReader(stream, detectEncodingFromByteOrderMarks: true, bufferSize: 128, leaveOpen: true);
                if (reader.Peek() > -1)
                {
                    _ = await reader.ReadAsync(new char[1]).ConfigureAwait(false);
                    stream.Position = pos;
                    return reader.CurrentEncoding;
                }
            }
            catch (Exception)
            {
                // Just ignore it
            }
            return default;
        }
    }
}