using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LockScreen.Extension.Collections
{
    public static class Observable
    {
        public static void AddRangeSorted<T, TSort>(this ObservableCollection<T> collection, IEnumerable<T> toAdd)
        {
            var sortArr = Enumerable.Concat(collection, toAdd).Order().ToList();
            foreach (T obj in toAdd.OrderBy(sortArr.IndexOf).ToList())
            {
                collection.Insert(sortArr.IndexOf(obj), obj);
            }
        }

        public static void AddRangeSorted<T, TSort>(this ObservableCollection<T> collection, IEnumerable<T> toAdd, Func<T, TSort> sortSelector)
        {
            var sortArr = Enumerable.Concat(collection, toAdd).OrderBy(sortSelector).ToList();
            foreach (T obj in toAdd.OrderBy(sortArr.IndexOf).ToList())
            {
                collection.Insert(sortArr.IndexOf(obj), obj);
            }
        }

        public static void AddRangeSorted<T, TSort, TSort2>(this ObservableCollection<T> collection, IEnumerable<T> toAdd, Func<T, TSort> sortSelector, Func<T, TSort2> sortSelector2)
        {
            var sortArr = Enumerable.Concat(collection, toAdd).OrderBy(sortSelector).ThenBy(sortSelector2).ToList();
            foreach (T obj in toAdd.OrderBy(sortArr.IndexOf).ToList())
            {
                collection.Insert(sortArr.IndexOf(obj), obj);
            }
        }
    }
}
