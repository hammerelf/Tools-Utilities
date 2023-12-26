using System;
using System.Collections.Generic;

namespace HammerElf.Tools.Utilities
{
    public static class IEnumberableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (T i in ie)
            {
                action(i);
            }
        }
    }
}
