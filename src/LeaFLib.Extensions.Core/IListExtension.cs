﻿using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
        /// <param name="source">Items</param>
        /// <returns></returns>
        public static T? RandomSingle<T>(this IList<T> source)
        {
            if (source.Count < 1)
            {
                return default;
            }
            if (source.Count == 1)
            {
                return source[0];
            }
            int index = Random.Shared.Next(0, source.Count);
            return source[index];
        }

        /// <summary>
        /// Random elements from items. Allow repeated indexes random.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">Items</param>
        /// <param name="length">Length of result</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T[] RandomManyRepeatable<T>(this IList<T> source, int length)
        {
            if (length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "The length cannot be less than 1.");
            }
            return source.Count switch
            {
                1 => NoRand(),
                > 1 => Rand(),
                _ => []
            };

            T[] NoRand()
            {
                T[] result = new T[length];
                for (int i = 0; i < length; i++)
                {
                    result[i] = source[0];
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
                    index = rand.Next(0, source.Count);
                    result[i] = source[index];
                }
                return result;
            }
        }

        /// <summary>
        /// Random elements from items. Only unique indexes random.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">Items</param>
        /// <param name="length">Length of result</param>
        /// <param name="algorithm">The algorithm of picking</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static T[] RandomManyUnique<T>(this IList<T> source, int length, RandomManyUniqueAlgorithm algorithm)
        {
            if (source.Count < 1)
            {
                return [];
            }

            if (length < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "The length is less than 1.");
            }
            if (length > source.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "The length is greater than source count.");
            }

            if (length == 1)
            {
                return [source.RandomSingle()!];
            }
            if (length == source.Count)
            {
                var clone = new T[length];
                source.CopyTo(clone, 0);
                source.Shuffle();
                return clone;
            }

            return algorithm switch
            {
                RandomManyUniqueAlgorithm.ReservoirSampling => source.RandomManyUniqueByReservoirSampling(length),
                RandomManyUniqueAlgorithm.SelectionSampling => source.RandomManyUniqueBySelectionSampling(length),
                RandomManyUniqueAlgorithm.Shuffle => source.RandomManyUniqueByShuffle(length),
                RandomManyUniqueAlgorithm.SkipDictionary => source.RandomManyUniqueBySkipDictionary(length),
                RandomManyUniqueAlgorithm.HashSet => source.RandomManyUniqueByHashSet(length),

                _ => throw new NotImplementedException($"Invalid algorithm[{algorithm}]")
            };
        }

        /// <summary>
        /// Sort randomly
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">Items</param>
        public static void Shuffle<T>(this IList<T> source)
        {
            source.ShuffleCore(source.Count);
        }

        /// <summary>
        /// Clone a new list and sort randomly
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">Items</param>
        /// <returns></returns>
        public static List<T> ShuffleClone<T>(this IList<T> source)
        {
            var list = new List<T>(source);
            list.Shuffle();
            return list;
        }

        internal static void ShuffleCore<T>(this IList<T> source, int opTimes)
        {
            if (opTimes < 1 || source.Count < 2)
            {
                return;
            }

            int total = source.Count;
            int randIndex, tailIndex = total - 1;
            var rand = Random.Shared;
            for (int i = 0; i < Math.Min(opTimes, total - 1); i++)
            {
                randIndex = rand.Next(0, tailIndex + 1);
                if (randIndex != tailIndex)
                {
                    (source[randIndex], source[tailIndex]) = (source[tailIndex], source[randIndex]);
                }
                tailIndex--;
            }
        }

        private static T[] RandomManyUniqueByReservoirSampling<T>(this IList<T> source, int length)
        {
            var result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = source[i];
            }
            var rand = Random.Shared;
            for (int i = length; i < source.Count; i++)
            {
                int idx = rand.Next(i + 1);
                if (idx < length)
                {
                    result[idx] = source[i];
                }
            }
            return result;
        }

        private static T[] RandomManyUniqueBySelectionSampling<T>(this IList<T> source, int length)
        {
            var result = new T[length];
            int a = length, b = source.Count;
            var rand = Random.Shared;
            for (int i = 0; i < source.Count; i++)
            {
                if (rand.Next(b) < a)
                {
                    result[^a] = source[i];
                    if (a <= 1)
                    {
                        break;
                    }
                    a--;
                }
                b--;
            }
            return result;
        }

        private static T[] RandomManyUniqueByShuffle<T>(this IList<T> source, int length)
        {
            source.ShuffleCore(length);
            var result = new T[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = source[^(i + 1)];
            }
            return result;
        }

        private static T[] RandomManyUniqueBySkipDictionary<T>(this IList<T> source, int length)
        {
            var skipDict = new Dictionary<int, int>(length);
            T[] result = new T[length];
            int currentIdx = 0;
            var rand = Random.Shared;
            for (int i = 0; i < length; i++)
            {
                int randIdx = rand.Next(source.Count - i);
                ref int actualIndex = ref CollectionsMarshal.GetValueRefOrAddDefault(skipDict, randIdx, out bool exisits);
                result[currentIdx] = exisits
                    ? source[actualIndex]
                    : source[randIdx];
                int lastIdx = source.Count - 1 - i;
                ref int actualIdx4Last = ref CollectionsMarshal.GetValueRefOrNullRef(skipDict, lastIdx);
                actualIndex = Unsafe.IsNullRef(ref actualIdx4Last)
                    ? lastIdx
                    : actualIdx4Last;
                if (++currentIdx >= length)
                {
                    break;
                }
            }
            return result;
        }

        private static T[] RandomManyUniqueByHashSet<T>(this IList<T> source, int length)
        {
            T[] result = new T[length];
            int randomCount = Math.Min(source.Count - length, length);
            var indexSet = new HashSet<int>(randomCount);
            var rand = Random.Shared;
            while (indexSet.Count < randomCount)
            {
                indexSet.Add(rand.Next(source.Count));
            }
            int index = 0;
            if (randomCount == length)
            {
                foreach (var idx in indexSet)
                {
                    result[index++] = source[idx];
                }
            }
            else
            {
                for (int i = 0; i < source.Count; i++)
                {
                    if (!indexSet.Contains(i))
                    {
                        result[index++] = source[i];
                    }
                }
            }
            return result;
        }
    }

    /// <summary>
    /// Algorithms about picking random elements from <see cref="IList{T}"/>
    /// </summary>
    public enum RandomManyUniqueAlgorithm
    {
        /// <summary>
        /// <para>Time: O(N), N is the length of source.</para>
        /// <para>Space: O(1)</para>
        /// </summary>
        /// <remarks>
        /// The elements' order of result has an association with the source, especially when the result length is close to the source count.
        /// If you want to get an totally unordered result, you can shuffle it then.
        /// </remarks>
        ReservoirSampling = 1,

        /// <summary>
        /// A special case of <strong>Reservoir Sampling</strong>.
        /// <para>Time: O(N), N is the length of source.</para>
        /// <para>Space: O(1)</para>
        /// </summary>
        /// <remarks>
        /// The elements of result will keep in order as same as the original source.
        /// If you want to get an unordered result, you can shuffle it then.
        /// <para>Also see <seealso href="https://stackoverflow.com/questions/48087/select-n-random-elements-from-a-listt-in-c-sharp"/>.</para>
        /// </remarks>
        SelectionSampling = 2,

        /// <summary>
        /// <para>Time: O(M), M is the length of result.</para>
        /// <para>Space: O(1)</para>
        /// </summary>
        /// <remarks>
        /// It will change the elements' order of original source.
        /// If you don't want to modify the source, you should create a new list of source,
        /// such as <c>new <see cref="List{T}"/>(source)</c>.
        /// </remarks>
        Shuffle = 3,

        /// <summary>
        /// <para>Time: O(M), M is the length of result.</para>
        /// <para>Space: O(M), M is the length of result.</para>
        /// </summary>
        SkipDictionary = 4,

        /// <summary>
        /// <para>Time: O(M) when M is less than N/2; O(N) when M is greater than N/2</para>
        /// <para>Space: O(M) when M is less than N/2; O(N-M) when M is greater than N/2</para>
        /// </summary>
        /// <remarks>
        /// If M is less than N/2, the result is unordered; otherwise the result is in order.
        /// </remarks>
        HashSet = 5,
    }
}