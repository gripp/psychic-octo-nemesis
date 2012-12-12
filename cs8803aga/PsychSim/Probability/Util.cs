using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGAI.Probability
{
    class Util
    {
        public readonly static String NO = "No";
        public readonly static String YES = "Yes";
        //
        private static Random _r = new Random();

        public static Dictionary<K, V> create<K, V>(HashSet<K> keys, V value)
        {
            Dictionary<K, V> map = new Dictionary<K, V>();

            foreach (K k in keys)
            {
                map.Add(k, value);
            }

            return map;
        }

        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(Dictionary<TKey, TValue> dict1, Dictionary<TKey, TValue> dict2)
        {
            foreach (var x in dict2)
                dict1[x.Key] = x.Value;

            return dict1;
        }

        public static HashSet<TKey> MergeSet<TKey>(HashSet<TKey> set1, HashSet<TKey> set2)
        {
            foreach (var x in set2)
                set1.Add(x);

            return set1;
        }

        public static  T selectRandomlyFromList<T>(List<T> l) 
        {
		    return l[_r.Next(l.Count())];
	    }
    }
}
