using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LegendsGenerator.Contracts
{
    internal static class Extensions
    {
        public static Collection<T> ToCollection<T>(this IEnumerable<T> obj)
        {
            return new Collection<T>(obj.ToList());
        }

        public static void AddRange<T>(this ICollection<T> obj, IEnumerable<T> input)
        {
            foreach (T item in input)
            {
                obj.Add(item);
            }
        }
    }
}
