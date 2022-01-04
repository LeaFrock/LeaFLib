namespace LeaFLib.Extensions.Core
{
    /// <summary>
    /// Extensions for <see cref="IList{T}"/>
    /// </summary>
    public static class IListExtension
    {
        /// <summary>
        /// Random a single element from items
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">Items</param>
        /// <returns></returns>
        public static T? SingleRandom<T>(this IList<T> items)
        {
            if (items.Count < 1)
            {
                return default;
            }
            if (items.Count == 1)
            {
                return items[0];
            }
            int index = Random.Shared.Next(0, items.Count);
            return items[index];
        }

        /// <summary>
        /// Random elements from items repeatly. Allow duplicated indexes random.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">Items</param>
        /// <param name="length">Length of result</param>
        /// <returns></returns>
        public static T[] RepeatRandom<T>(this IList<T> items, int length)
        {
            return items.Count switch
            {
                1 => NoRand(),
                > 1 => Rand(),
                _ => Array.Empty<T>()
            };

            T[] NoRand()
            {
                T[] result = new T[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = items[0];
                }
                return result;
            }

            T[] Rand()
            {
                T[] result = new T[length];
                int index;
                var rand = Random.Shared;
                for (int i = 0; i < length; i++)
                {
                    index = rand.Next(0, items.Count);
                    result[i] = items[index];
                }
                return result;
            }
        }

        /// <summary>
        /// Select random elements from items. Only unique indexes random.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">Items</param>
        /// <param name="length">Length of result</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static T?[] SelectRandom<T>(this IList<T> items, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sort randomly
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">Items</param>
        public static void Shuffle<T>(this IList<T> items)
        {
            int total = items.Count;
            if (total < 2)
            {
                return;
            }
            int randIndex, tailIndex = total - 1;
            var rand = Random.Shared;
            for (int i = 0; i < total - 1; i++)
            {
                randIndex = rand.Next(0, tailIndex + 1);
                if (randIndex != tailIndex)
                {
                    Swap(randIndex, tailIndex);
                }
                tailIndex--;
            }

            void Swap(int i1, int i2)
            {
                var temp = items[i1];
                items[i1] = items[i2];
                items[i2] = temp;
            }
        }

        /// <summary>
        /// Clone a new list and sort randomly
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="items">Items</param>
        /// <returns></returns>
        public static List<T> ShuffleClone<T>(this IList<T> items)
        {
            var list = new List<T>(items);
            list.Shuffle();
            return list;
        }
    }
}