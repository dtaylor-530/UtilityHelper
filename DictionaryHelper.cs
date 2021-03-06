﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;


namespace UtilityHelper
{


    public static class ObjectToDictionaryHelper
    {
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
                AddPropertyToDictionary<T>(property, source, dictionary);
            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }

        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("source", "Unable to convert object to a dictionary. The source object is null.");
        }






    }



    public static class DictionaryExtension
    {
        /// <summary>
        /// Get a the value for a key. If the key does not exist, creates a new one
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dic">The dictionary to call this method on.</param>
        /// <param name="key">The key to look up.</param>
        /// <returns>The key value. null if this key is not in the dictionary.</returns>
        public static TValue GetValueOrNew<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key) where TValue : new()
        {
            TValue result;
            var x = dic.TryGetValue(key, out result) ? result : new TValue();

            dic[key] = x;

            return x;
        }

        /// <summary>
        /// Get a the value for a key. If the key does not exist, creates a new one
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dic">The dictionary to call this method on.</param>
        /// <param name="key">The key to look up.</param>
        /// <returns>The key value. null if this key is not in the dictionary.</returns>
        public static TValue GetValueOrNew<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value) 
        {
            TValue result;
            var x = dic.TryGetValue(key, out result) ? result :value;

            dic[key] = x;

            return x;
        }


        public static void AddRange<T, K, V>(this IDictionary<K, V> me, params IDictionary<K, V>[] others)
        {

            foreach (IDictionary<K, V> src in others)
            {
                foreach (KeyValuePair<K, V> p in src)
                {
                    //if(typeof(IDictionary).IsAssignableFrom(typeof(V)e()))
                    me[p.Key] = p.Value;
                }
            }

        }
    }

    public class DictionaryHelper
    { 


        #region Dictionary
        /// <summary>
        /// Unionise two dictionaries of generic types.
        /// Duplicates take their value from the leftmost dictionary.
        /// </summary>
        /// <typeparam name="T1">Generic key type</typeparam>
        /// <typeparam name="T2">Generic value type</typeparam>
        /// <param name="D1">Dictionary 1</param>
        /// <param name="D2">Dictionary 2</param>
        /// <returns>The combined dictionaries.</returns>
        public static Dictionary<T1, T2> UnionDictionaries<T1, T2>(Dictionary<T1, T2> D1, Dictionary<T1, T2> D2)
        {
            Dictionary<T1, T2> rd = new Dictionary<T1, T2>(D1);
            foreach (var key in D2.Keys)
            {
                if (!rd.ContainsKey(key))
                    rd.Add(key, D2[key]);
                else if (rd[key].GetType().IsGenericType)
                {
                    if (rd[key].GetType().GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    {
                        var mBase = MethodBase.GetCurrentMethod();
                        MethodInfo info = mBase is MethodInfo ? (MethodInfo)mBase : typeof(DictionaryHelper).GetMethod("UnionDictionaries", BindingFlags.Public | BindingFlags.Static);
                        var genericMethod = info.MakeGenericMethod(rd[key].GetType().GetGenericArguments()[0], rd[key].GetType().GetGenericArguments()[1]);
                        var invocationResult = genericMethod.Invoke(null, new object[] { rd[key], D2[key] });
                        rd[key] = (T2)invocationResult;
                    }
                }
            }
            return rd;
        }
        #endregion
    }



}
