using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityHelper
{
    public static class KaosCombinatricsEx
    {



        /// <summary>
        /// rearranges the elements into a sequence of element sets where no set has the same elements as another
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="c"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> CombineDistinct<T>(this IEnumerable<T> c, int size)
        {
            return c.ReArrange(new Kaos.Combinatorics.Combination(c.Count(), size).EnumerateDistinct());
        }




        /// <summary>
        /// produces sequence of sets of elements where no set has the same elements as another
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<int>> EnumerateDistinct(this Kaos.Combinatorics.Combination c)
        {
            HashSet<int> hs = new HashSet<int>();
            foreach (var x in c.GetRows())
            {
                if (x.All(_ => hs.Add(_)))
                    yield return x;

            }
        }

        /// <summary>
        /// Rearranges the elements into a sequence according to their index and the index of sequence-sets in the given parameter
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ts"></param>
        /// <param name="enms"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> ReArrange<T>(this IEnumerable<T> ts, IEnumerable<IEnumerable<int>> enms)
        {
            return enms.Select(_ => _.Select(__ => Enumerable.ElementAt(ts, __)));
        }





        /// /////////////////

    }
}
