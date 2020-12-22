using System;
using System.Collections.Generic;
using System.Text;

namespace DrWndr.Utils
{
    static class ListExtensions
    {
        #region Private member 

        /// <summary>
        /// Random generator.
        /// </summary>
        private static readonly Random random = new Random();

        #endregion

        #region Public helper

        /// <summary>
        /// Shuffles given list in place.
        /// 
        /// Based on:
        ///     - https://stackoverflow.com/a/1262619/1613740
        /// </summary>
        /// <typeparam name="T">Typ of list items.</typeparam>
        /// <param name="list">List.</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        #endregion
    }
}
