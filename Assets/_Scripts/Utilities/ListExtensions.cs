using System;
using System.Collections.Generic;

namespace Utilities
{
    public static class ListExtensions
    {
        // Extension method to shuffle a list
        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        // Extension method to get a random element from a list
        public static T GetRandomElement<T>(this IList<T> list)
        {
            if (list == null || list.Count == 0)
                throw new InvalidOperationException("Cannot get a random element from an empty list.");

            Random rng = new Random();
            int index = rng.Next(list.Count);
            return list[index];
        }
    }
}