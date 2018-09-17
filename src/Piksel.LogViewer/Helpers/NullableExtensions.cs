using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Piksel.LogViewer.Helpers
{
    public static class NullableExtensions
    {
        public static void HasValue<T>(this T? nullable, Action<T> action) where T : struct
        {
            if (nullable.HasValue) action(nullable.Value);
        }

        public static T? HasValue<T>(this T? nullable, Func<T, T?> function) where T : struct
            => nullable.HasValue ? function(nullable.Value) : nullable;
        
    }
}
