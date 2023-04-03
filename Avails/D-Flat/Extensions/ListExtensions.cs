using System;
using System.Collections.Generic;
using System.Linq;

namespace Avails.D_Flat.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// NOT TESTED!!!!
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="compareMethod"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <returns></returns>
        public static bool NotAny<TSource>(this IEnumerable<TSource> enumerable, Predicate<TSource> compareMethod)
        {
            return ! enumerable.Any(item => compareMethod(item));
        }
    }
}
