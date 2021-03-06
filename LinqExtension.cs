﻿using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityHelper
{
    public static class LinqExtension
    {
        public static IEnumerable<TResult> Zip<TResult>(Func<object[], TResult> resultSelector,
params System.Collections.IEnumerable[] itemCollections)
        {
            System.Collections.IEnumerator[] enumerators = itemCollections.Select(i => i.GetEnumerator()).ToArray();

            Func<bool> MoveNext = () =>
            {
                for (int i = 0; i < enumerators.Length; i++)
                {
                    if (!enumerators[i].MoveNext())
                    {
                        return false;
                    }
                }
                return true;
            };

            while (MoveNext())
            {
                yield return resultSelector(enumerators.Select(e => e.Current).ToArray());
            }


        }



        public static void RemoveLast<T>(this ICollection<T> collection, int n)
        {
            var x = collection.TakeLast(n);
            foreach (var y in x)
            {
                collection.Remove(y);
            }
        }



        public static void RemoveFirst<T>(this ICollection<T> collection, int n)
        {
            var x = collection.Take(n);
            foreach (var y in x)
            {
                collection.Remove(y);
            }
        }


        public static IEnumerable<U> Map<T, U>(this IEnumerable<T> s, Func<T, U> f)
        {
            foreach (var item in s)
                yield return f(item);
        }






        public static IEnumerable<double> SelectDifferences(this double[] sequence)
        {
            for (int i = 0; i < sequence.Length - 1; i++)
            {
                yield return sequence[i + 1] - sequence[i];

            }
        }
        public static IEnumerable<double> SelectDifferences(this List<double> sequence)
        {
            for (int i = 0; i < sequence.Count() - 1; i++)
            {
                yield return sequence[i + 1] - sequence[i];

            }
        }


        public static void AddOrReplaceBy<TSource, TKey>(this ICollection<TSource> source, Func<TSource, TKey> keySelector, TSource replacement)
        {

            RemoveBy(source,keySelector, keySelector(replacement));
            source.Add(replacement);

        }



        public static void RemoveBy<TSource, TKey>(this ICollection<TSource> source, Func<TSource, TKey> keySelector, TKey key)
        {

            source.ActionBy(keySelector, key, (a, b) => a.Remove(b));

        }


        public static void ActionBy<TSource, TKey>(this ICollection<TSource> source, Func<TSource, TKey> keySelector, TKey key, Action<ICollection<TSource>, TSource> action)
        {

            if (!source.IsEmpty())
                foreach (TSource element in source.ToList())
                    if (key?.Equals(keySelector(element))?? false)
                    {
                        action(source, element);
                    }

        }



        public static Boolean IsEmpty<T>(this IEnumerable<T> source)
        {
            if (source == null)
                return true; // or throw an exception
            return !source.Any();
        }



        //public static Boolean IsEmpty(this System.Collections.IList source)
        //{
        //    if (source == null)
        //        return true; // or throw an exception
        //    return source.Count == 0;
        //}




        public static IEnumerable<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
          Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, null);

        }

        // From MoreLinq
        public static IEnumerable<TSource> MaxBy<TSource, TKey>(this IEnumerable<TSource> source,
       Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            comparer = comparer ?? Comparer<TKey>.Default;
            return ExtremaBy(source, selector, (x, y) => comparer.Compare(x, y));
        }



        // > In mathematical analysis, the maxima and minima (the respective
        // > plurals of maximum and minimum) of a function, known collectively
        // > as extrema (the plural of extremum), ...
        // >
        // > - https://en.wikipedia.org/wiki/Maxima_and_minima

        static IEnumerable<TSource> ExtremaBy<TSource, TKey>(IEnumerable<TSource> source,
            Func<TSource, TKey> selector, Func<TKey, TKey, int> comparer)
        {
            foreach (var item in Extrema())
                yield return item;

            IEnumerable<TSource> Extrema()
            {
                using (var e = source.GetEnumerator())
                {
                    if (!e.MoveNext())
                        return new List<TSource>();

                    var extrema = new List<TSource> { e.Current };
                    var extremaKey = selector(e.Current);

                    while (e.MoveNext())
                    {
                        var item = e.Current;
                        var key = selector(item);
                        var comparison = comparer(key, extremaKey);
                        if (comparison > 0)
                        {
                            extrema = new List<TSource> { item };
                            extremaKey = key;
                        }
                        else if (comparison == 0)
                        {
                            extrema.Add(item);
                        }
                    }

                    return extrema;
                }
            }
        }





        //.Aggregate((total, nextCode) => total ^ nextCode);




        public static double WeightedAverage<T>(this IEnumerable<T> records, Func<T, double> value, Func<T, double> weight,double control=0)
        {
            double weightedValueSum = records.Sum(x => (value(x)-control) * weight(x));
            double weightSum = records.Sum(x => weight(x));

            if (weightSum != 0)
                return weightedValueSum / weightSum;
            else
                throw new DivideByZeroException("Your message here");
        }


        public static List<double> MovingWeightedAverage<T>(this IEnumerable<T> series, int period, Func<T, double> value, Func<T, double> weight)
        {
            return series.Skip(period - 1).Aggregate(
       new
       {
           Result = new List<double>(),
           Working = new Queue<T>(series.Take(period - 1))
       },
      (list, item) =>
      {
          list.Working.Enqueue(item);
          list.Result.Add(list.Working.WeightedAverage(value, weight));
          list.Working.Dequeue();
          return list;
      }
    ).Result;
        }





        public static SortedList<DateTime, double> MovingAverage(this SortedList<DateTime, double> series, int period)
        {
            return new SortedList<DateTime, double>(series.Skip(period - 1).Scan(new SortedList<DateTime, double>(),

                 (list, item) =>   { list.Add(item.Key, item.Value);  return list;  })
                .Select(_ => new KeyValuePair<DateTime, double>(_.Last().Key, _.Select(__ => __.Value).Average()))
                .ToDictionary(_ => _.Key, _ => _.Value));
        }





        public static List<double> MovingAverage(this List<double> series, int period)
        {
            return series.Skip(period - 1).Aggregate(
       new
       {
           Result = new List<double>(),
           Working = new Queue<double>(series.Take(period - 1).Select(item => item))
       },
      (list, item) =>
      {
          list.Working.Enqueue(item);
          list.Result.Add(list.Working.Average());
          list.Working.Dequeue();
          return list;
      }
    ).Result;
        }





            public static IEnumerable<TResult> TakeIfNotNull<TResult>(this IEnumerable<TResult> source, int? count)
            {
                return !count.HasValue ? source : source.Take(count.Value);
            }


            public static IEnumerable<TResult> TakeAllIfNull<TResult>(this IEnumerable<TResult> source, int? count)
            {

                if (count == null)
                    return source;
                else
                    return source.Take(count.Value);
            }
        
    }



}
