using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UtilityHelper
{
    public static class UIHelper
    {
        /// <summary>
        /// slow method: consider using IProgress to update UI or batch update 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="item"></param>
        //https://stackoverflow.com/questions/2091988/how-do-i-update-an-observablecollection-via-a-worker-thread
        //The reality, however, is that this solution will likely bog down under heavy load 
        //because of all the cross-thread activity. 
        public static void AddOnUI<T>(this ICollection<T> collection, T item)
        {
            Action<T> addMethod = collection.Add;
            Application.Current.Dispatcher.BeginInvoke(addMethod,item);
        }


        // A more efficient solution to updating UI: batches up items and post them to the UI
        // thread periodically to avoid calling across threads for each item.
        public static void AddRangeOnUI<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            Action<IEnumerable<T>> addRangeMethod = collection.AddRange;
            Application.Current.Dispatcher.BeginInvoke(addRangeMethod, items);
        }





        public static void DoEvents()
        {
            if (Application.Current == null)
                return;
            Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Background, new Action(() => { }));
        }



    }



}
