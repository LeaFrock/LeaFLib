namespace LeaFLib.Extensions.Core
{
    /// <summary>
    /// Extensions for <see cref="Array"/>
    /// </summary>
    public static class ArrayExtension
    {
        /// <summary>
        /// Return all matched elements in <see cref="ArraySegment{T}"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        /// <remarks>This method may change the order of elements</remarks>
        public static ArraySegment<T> MatchAll<T>(this T[] array, Predicate<T> match)
        {
            ArgumentNullException.ThrowIfNull(nameof(match));

            if (array is null)
            {
                return ArraySegment<T>.Empty;
            }

            if (array.Length == 0)
            {
                return new(array);
            }

            int cursor = 0, tail = array.Length - 1;
            while (cursor < tail)
            {
                if (match(array[cursor]))
                {
                    cursor++;
                }
                else
                {
                    (array[cursor], array[tail]) = (array[tail], array[cursor]);
                    tail--;
                }
            }
            if (!match(array[cursor])) // when cursor == tail
            {
                tail--;
            }
            return new(array, 0, tail + 1);
        }
    }
}