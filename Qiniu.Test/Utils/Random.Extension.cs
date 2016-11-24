using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qiniu.Test.Utils
{
    public static partial class RandomExtension
    {
        public static T RandomGetFromArray<T>(this IEnumerable<T> array)
        {
            var count = array.Count();
            var index = 0;
            var target = new Random().Next(0, count - 1);
            var enumerator = array.GetEnumerator();
            var value = default(T);
            while (enumerator.MoveNext())
            {
                value = enumerator.Current;
                if (index++ == target)
                {
                    break;
                }
            }
            return value;
        }
    }
}
